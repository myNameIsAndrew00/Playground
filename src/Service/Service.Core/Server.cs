using Service.Core.Abstractions.Communication;
using Service.Core.Abstractions.Configuration;
using Service.Core.Abstractions.Execution;
using Service.Core.Abstractions.Logging;
using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.Abstractions.Token.Hashing;
using Service.Core.Abstractions.Token.Signing;
using Service.Core.DefinedTypes;
using Service.Core.Execution;
using Service.Core.Storage.Memory;
using Service.Core.Token;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Service.Core.Abstractions.Logging.IAllowLogging;

namespace Service.Core
{
    /// <summary>
    /// A class which can handle received messages from pkcs11 clients
    /// </summary>
    public abstract class Server<DispatchResultType, SessionType> : IPkcs11Server<DispatchResultType, SessionType>, IConfigurablePkcs11Server
        where DispatchResultType : IDispatchResult<SessionType>
        where SessionType : ISession
    {
        private bool disposed;
        private Task listeningTask;

        private ModuleFactory moduleCollection;

        private CancellationTokenSource listenTaskTokenSource;

        private ITokenStorage tokenStorage;

        private IConfigurationAPIProxy configurationAPI;

        private ILogger logger;

        public IServiceCommunicationResolver<DispatchResultType, SessionType> Resolver { get; }

        public abstract IServiceExecutor<DispatchResultType, SessionType> CreateExecutor();

        internal Server(IServiceCommunicationResolver<DispatchResultType, SessionType> resolver)
        {
            this.disposed = false;
            this.Resolver = resolver;
            moduleCollection = new ModuleFactory();

            Resolver.OnCommunicationCreated += onCommunicationCreated;
            Resolver.OnClientConnectionError += onClientConnectionError;
            Resolver.OnRequestHandlingError += onRequestHandlingError;
        }

        public void Start()
        {
            if (disposed) return;

            // log application is starting
            this.logger?.Create(LogSection.MAIN_SERVER, new LogData("Starting server...", null, LogLevel.Info));

            listenTaskTokenSource = new CancellationTokenSource();

            listeningTask = Resolver.Listen(listenTaskTokenSource.Token);


            if (this.configurationAPI is not null)
            {
                // log that configuration api is starting
                this.logger?.Create(LogSection.MAIN_SERVER, new LogData("Starting configuration API...", null, LogLevel.Info));
                this.configurationAPI.Launch();
            }

            // log that server started successufuly
            this.logger?.Create(LogSection.MAIN_SERVER, new LogData("Server started...", null, LogLevel.Info));

        }

        public void Stop()
        {
            this.logger?.Create(LogSection.MAIN_SERVER, new LogData("Server is shuting down...", null, LogLevel.Info));

            listenTaskTokenSource.Cancel();

            if (this.configurationAPI is not null)
            {
                this.logger?.Create(LogSection.MAIN_SERVER, new LogData("Configuration API shutting down", null, LogLevel.Info));
                this.configurationAPI.Stop();
            }
        }

        public IEnumerable<IReadOnlySession> GetSessions() => this.Resolver.Dispatcher.GetSessions();

        public IEnumerable<ILogMessage> GetLogs() => this.logger.GetMessages(300);

        public IPkcs11Server SetStorage(ITokenStorage storage) { this.tokenStorage = storage; return this; }

        public IPkcs11Server SetLogger(ILogger logger) { this.logger = logger; return this; }

        public IPkcs11Server SetConfigurationAPI(Func<IConfigurablePkcs11Server, IConfigurationAPIProxy> configurationApiFactory)
        {
            this.configurationAPI = configurationApiFactory(this);
            return this;
        }


        public IPkcs11Server RegisterModule<ModuleType, ImplementationType>()
            where ModuleType : ITokenModule
            where ImplementationType : ITokenModule
        {
            moduleCollection.RegisterModule(typeof(ModuleType), typeof(ImplementationType));
            return this;
        }

        public IPkcs11Server RegisterEncryptionModule<EncryptionModuleType>(Func<IContext, EncryptionModuleType> implementationFactory = null) where EncryptionModuleType : IEncryptionModule
        {
            moduleCollection.RegisterModule(typeof(IEncryptionModule), typeof(EncryptionModuleType), (builderParameter) => implementationFactory(builderParameter as IContext));
            return this;
        }

        public IPkcs11Server RegisterDecryptionModule<DecryptionModuleType>(Func<IContext, DecryptionModuleType> implementationFactory = null) where DecryptionModuleType : IDecryptionModule
        {
            moduleCollection.RegisterModule(typeof(IDecryptionModule), typeof(DecryptionModuleType), (builderParameter) => implementationFactory(builderParameter as IContext));
            return this;
        }

        public IPkcs11Server RegisterHashingModule<HashingModuleType>(Func<IContext, HashingModuleType> implementationFactory = null) where HashingModuleType : IHashingModule
        {
            moduleCollection.RegisterModule(typeof(IHashingModule), typeof(HashingModuleType), (builderParameter) => implementationFactory(builderParameter as IContext));
            return this;
        }
        public IPkcs11Server RegisterSigningModule<SigningModuleType>(Func<IContext, SigningModuleType> implementationFactory = null) where SigningModuleType : ISigningModule
        {
            moduleCollection.RegisterModule(typeof(ISigningModule), typeof(SigningModuleType), (builderParameter) => implementationFactory(builderParameter as IContext));
            return this;
        }

        public IPkcs11Server RegisterVerifyingModule<VerifyingModuleType>(Func<IContext, VerifyingModuleType> implementationFactory = null) where VerifyingModuleType : IVerifyModule
        {
            moduleCollection.RegisterModule(typeof(IVerifyModule), typeof(VerifyingModuleType), (builderParameter) => implementationFactory(builderParameter as IContext));
            return this;
        }

        public void Dispose()
        {
            if (disposed) return;

            this.Resolver.Dispose();

            if (this.configurationAPI is not null)
                this.configurationAPI.Dispose();

            if (this.logger is not null)
                this.logger.Dispose();

            disposed = true;
        }



        #region Private

        private IExecutionResult onCommunicationCreated(DispatchResultType dispatchResult)
        {
            // log dispatcher logs
            LogActivity(this.Resolver);
            LogActivity(this.Resolver.Dispatcher);

            // create an instance of the executor
            IServiceExecutor<DispatchResultType, SessionType> executor = CreateExecutor();

            // initialise the executor
            executor.SetDispatcherResult(dispatchResult);
            executor.SetModuleFactory(this.moduleCollection);
            executor.SetStorage(this.tokenStorage);

            try
            {
                // check session
                if (!dispatchResult.SessionCheckPassed)
                    return executor.GetEmptySessionResult(ExecutionResultCode.SESSION_HANDLE_INVALID);

                //if the dispatcher doesn't close the session in current request and the session is already closed
                if (!dispatchResult.ClosedSession && dispatchResult.Session.Closed)
                    return executor.GetEmptySessionResult(ExecutionResultCode.SESSION_CLOSED);

                // initialise the invoked method and bind the parameters using the binder, if exists
                MethodInfo method = executor.GetType().GetMethod(
                    name: dispatchResult.DispatchedAction.ToString(),
                    bindingAttr: BindingFlags.Public | BindingFlags.Instance);

                if (method == null)
                    return executor.GetEmptySessionResult(ExecutionResultCode.FUNCTION_NOT_SUPPORTED);

                object[] methodParameters = executor.ModelBinder?.GetMethodParametersModels(method, dispatchResult, tokenStorage);

                // invoke the method and get the execution result
                IExecutionResult executionResult = (IExecutionResult)method.Invoke(executor, methodParameters);

                return executionResult;
            }
            catch
            {
                return executor.GetEmptySessionResult(ExecutionResultCode.GENERAL_ERROR);
            }
            finally
            {
                LogActivity(executor);
                LogActivity(this.tokenStorage);
            }
        }

        private void onClientConnectionError(Exception exception)
        {
            //todo: handle the exception
            Debug.WriteLine(exception);
        }

        private void onRequestHandlingError(Exception exception)
        {
            //todo: handle the exception
            Debug.WriteLine(exception);
        }

        /// <summary>
        /// Logs data using the logger for given logging instance.
        /// </summary>
        /// <param name="loggingInstance"></param>
        private void LogActivity(IAllowLogging loggingInstance)
        {
            if (this.logger is not null)
                logger.Create(loggingInstance.LogSection, loggingInstance.Logs);

            loggingInstance.ClearLogs();
        }

     

        #endregion
    }

    /// <summary>
    /// A class which can handle received messages from pkcs11 clients
    /// </summary>
    /// <typeparam name="Executor">Executor which handle client requests. For now only one executor is allowed</typeparam>
    internal class Server<Executor, DispatchResultType, SessionType> : Server<DispatchResultType, SessionType>
        where Executor : IServiceExecutor<DispatchResultType, SessionType>, new()
        where DispatchResultType : IDispatchResult<SessionType>
        where SessionType : ISession
    {
        internal Server(IServiceCommunicationResolver<DispatchResultType, SessionType> resolver) : base(resolver)
        {
        }

        public override IServiceExecutor<DispatchResultType, SessionType> CreateExecutor() => new Executor();
    }
}
