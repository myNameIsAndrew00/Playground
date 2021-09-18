using Service.Core.Abstractions.Communication;
using Service.Core.Abstractions.Execution;
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

namespace Service.Core
{
    /// <summary>
    /// A class which can handle received messages from pkcs11 clients
    /// </summary>
    public abstract class Server<DispatchResultType, SessionType> : IPkcs11Server<DispatchResultType, SessionType>
        where DispatchResultType : IDispatchResult<SessionType>
        where SessionType : ISession
    {
        private bool disposed;
        private Task listeningTask;

        private ModuleFactory moduleCollection;

        private CancellationTokenSource listenTaskTokenSource;

        private ITokenStorage tokenStorage;
         

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

            listenTaskTokenSource = new CancellationTokenSource();
           
            listeningTask = Resolver.Listen(listenTaskTokenSource.Token);
        }

        public void Stop()
        {
            listenTaskTokenSource.Cancel(); 
        }

        public IPkcs11Server SetStorage(ITokenStorage storage) { this.tokenStorage = storage; return this; }

        public IPkcs11Server RegisterModule<ModuleType, ImplementationType>()
            where ModuleType : ITokenModule 
            where ImplementationType : ITokenModule 
        {
            moduleCollection.RegisterModule(typeof(ModuleType), typeof(ImplementationType));
            return this;
        }

        public IPkcs11Server RegisterEncryptionModule<EncryptionModuleType>(Func<IMemoryObject, EncryptionModuleType> implementationFactory = null) where EncryptionModuleType : IEncryptionModule
        {
            moduleCollection.RegisterModule(typeof(IEncryptionModule), typeof(EncryptionModuleType), (builderParameter) => implementationFactory(builderParameter as IMemoryObject));
            return this;
        }

        public IPkcs11Server RegisterHashingModule<HashingModuleType>(Func<IMemoryObject, HashingModuleType> implementationFactory = null) where HashingModuleType : IHashingModule
        {
            moduleCollection.RegisterModule(typeof(IHashingModule), typeof(HashingModuleType), (builderParameter) => implementationFactory(builderParameter as IMemoryObject));
            return this;
        }
        public IPkcs11Server  RegisterSigningModule<SigningModuleType>(Func<IMemoryObject, SigningModuleType> implementationFactory = null) where SigningModuleType : ISigningModule
        {
            moduleCollection.RegisterModule(typeof(ISigningModule), typeof(SigningModuleType), (builderParameter) => implementationFactory(builderParameter as IMemoryObject));
            return this;
        }

        public void Dispose()
        {
            if (disposed) return;
            
            this.Resolver.Dispose();
            disposed = true;
        }



        #region Private

        private IExecutionResult onCommunicationCreated(DispatchResultType dispatchResult)
        {
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

                //if the dispatcher doesn't closed the session in current request and the session is already closed
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
