﻿using Service.Core.Abstractions.Communication;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.Abstractions.Token.Hashing;
using Service.Core.Abstractions.Token.Signing;
using Service.Core.Infrastructure.Communication.Structures;
using Service.Core.Infrastructure.Storage;
using Service.Core.Infrastructure.Storage.Structures;
using Service.Core.Infrastructure.Token;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Reflection;
using System.Text;

namespace Service.Core.Infrastructure
{
    /// <summary>
    /// A class which can handle received messages from pkcs11 clients
    /// </summary>
    public abstract class Server : IPkcs11Server
    {
        private ModuleFactory moduleCollection;

        private IServiceCommunicationResolver resolver;

        public IServiceCommunicationResolver Resolver => resolver;

        public abstract IServiceExecutor CreateExecutor();

        internal Server(IServiceCommunicationResolver resolver)
        {
            this.resolver = resolver;
            moduleCollection = new ModuleFactory();
        }

        public void Start()
        {
            resolver.OnCommunicationCreated += onCommunicationCreated;
            resolver.OnClientConnectionError += onClientConnectionError;
            resolver.OnRequestHandlingError += onRequestHandlingError;

            resolver.Listen();
        }

        public IPkcs11Server RegisterModule<ModuleType, ImplementationType>()
            where ModuleType : ITokenModule
            where ImplementationType : ITokenModule
        {
            moduleCollection.RegisterModule(typeof(ModuleType), typeof(ImplementationType));
            return this;
        }

        public IPkcs11Server RegisterEncryptionModule<EncryptionModuleType>(Func<Pkcs11ContextObject, EncryptionModuleType> implementationFactory = null) where EncryptionModuleType : IEncryptionModule
        {
            moduleCollection.RegisterModule(typeof(IEncryptionModule), typeof(EncryptionModuleType), (builderParameter) => implementationFactory(builderParameter as Pkcs11ContextObject));
            return this;
        }

        public IPkcs11Server RegisterHashingModule<HashingModuleType>(Func<Pkcs11ContextObject, HashingModuleType> implementationFactory = null) where HashingModuleType : IHashingModule
        {
            moduleCollection.RegisterModule(typeof(IHashingModule), typeof(HashingModuleType), (builderParameter) => implementationFactory(builderParameter as Pkcs11ContextObject));
            return this;
        }
        public IPkcs11Server RegisterSigningModule<SigningModuleType>(Func<Pkcs11ContextObject, SigningModuleType> implementationFactory = null) where SigningModuleType : ISigningModule
        {
            moduleCollection.RegisterModule(typeof(ISigningModule), typeof(SigningModuleType), (builderParameter) => implementationFactory(builderParameter as Pkcs11ContextObject));
            return this;
        }



        #region Private

        private IExecutionResult onCommunicationCreated(DispatchResult dispatchResult)
        {
            // create an instance of the executor
            IServiceExecutor executor = CreateExecutor();

            // initialise the executor
            executor.SetDispatcherResult(dispatchResult);
            executor.SetModuleCollection(this.moduleCollection);

            try
            {
                // check session
                if (!dispatchResult.SessionCheckPassed)
                    return executor.GetEmptySessionResult(ExecutionResultCode.ARGUMENTS_BAD);

                // initialise the invoked method and bind the parameters using the binder, if exists
                MethodInfo method = executor.GetType().GetMethod(
                    name: dispatchResult.DispatchedAction.ToString(),
                    bindingAttr: BindingFlags.Public | BindingFlags.Instance);

                object[] methodParameters = executor.ModelBinder?.GetMethodParameters(method, dispatchResult);

                if (method == null)
                    return executor.GetEmptySessionResult(ExecutionResultCode.FUNCTION_NOT_SUPPORTED);

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
    internal class Server<Executor> : Server
        where Executor : IServiceExecutor, new()
    {
        internal Server(IServiceCommunicationResolver resolver) : base(resolver)
        {
        }

        public override IServiceExecutor CreateExecutor() => new Executor();
    }
}