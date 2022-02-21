using Service.Core.Abstractions.Configuration;
using Service.Core.Abstractions.Execution;
using Service.Core.Abstractions.Logging;
using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.Abstractions.Token.Hashing;
using Service.Core.Abstractions.Token.Signing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Communication
{
    public interface IPkcs11Server : IDisposable 
    {
        /// <summary>
        /// Enable server instance to wait for listening client requests.
        /// </summary>
        void Start();

        /// <summary>
        /// Triger server instance to stop listening
        /// </summary>
        void Stop();

        /// <summary>
        /// Set the server storage module
        /// </summary>
        /// <param name="storage"></param>
        /// <returns></returns>
        IPkcs11Server SetStorage(ITokenStorage storage);

        /// <summary>
        /// Set the server logging module
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        IPkcs11Server SetLogger(ILogger logger);

        /// <summary>
        /// Set the service configuration api
        /// </summary>
        /// <param name="configurationApi"></param>
        /// <returns></returns>
        IPkcs11Server SetConfigurationAPI(Func<IConfigurablePkcs11Server, IConfigurationAPIProxy> configurationApiFactory);

        /// <summary>
        /// Register an module to the server which may be used by executor to handle the request invoked.
        /// </summary>
        /// <typeparam name="ModuleType"></typeparam>
        /// <returns>Returns an updated version of this instance</returns>
        IPkcs11Server RegisterModule<ModuleType, ImplementationType>()
            where ModuleType : ITokenModule
            where ImplementationType : ITokenModule;

        /// <summary>
        /// Register an encryption module to the server which may be used by executor to handle encryption operations.
        /// </summary>
        /// <returns>Returns an updated version of this instance</returns>
        IPkcs11Server RegisterEncryptionModule<EncryptionModuleType>(Func<IContext, EncryptionModuleType> implementationFactory = null)
            where EncryptionModuleType : IEncryptionModule;

        /// <summary>
        /// Register an encryption module to the server which may be used by executor to handle encryption operations.
        /// </summary>
        /// <returns>Returns an updated version of this instance</returns>
        IPkcs11Server RegisterDecryptionModule<DecryptionModuleType>(Func<IContext, DecryptionModuleType> implementationFactory = null)
            where DecryptionModuleType : IDecryptionModule;

        /// <summary>
        /// Register an encryption module to the server which may be used by executor to handle hashing operations.
        /// </summary>
        /// <returns>Returns an updated version of this instance</returns>
        IPkcs11Server RegisterHashingModule<HashingModuleType>(Func<IContext, HashingModuleType> implementationFactory = null)
            where HashingModuleType : IHashingModule;

        /// <summary>
        /// Register an encryption module to the server which may be used by executor to handle signing operations.
        /// </summary>
        /// <returns>Returns an updated version of this instance</returns>
        IPkcs11Server RegisterSigningModule<SigningModuleType>(Func<IContext, SigningModuleType> implementationFactory = null)
            where SigningModuleType : ISigningModule;

        /// <summary>
        /// Register an verifying module to the server which may be used by executor to handle signed data verifying operations.
        /// </summary>
        /// <returns>Returns an updated version of this intance.</returns>
        IPkcs11Server RegisterVerifyingModule<VerifyingModuleType>(Func<IContext, VerifyingModuleType> implementationFactory = null)
            where VerifyingModuleType : IVerifyModule;
    }

    /// <summary>
    /// Expose methods which describe a PKCS 11 service behavior.
    /// </summary> 
    public interface IPkcs11Server<DispatchResultType, SessionType> : IPkcs11Server 
        where DispatchResultType : IDispatchResult<SessionType>
        where SessionType : ISession
    {

        /// <summary>
        /// The instance which will initialise the communication and handle messages between server and client.
        /// </summary>
        IServiceCommunicationResolver<DispatchResultType, SessionType> Resolver { get; }

        /// <summary>
        /// A method which will create the instance to execute the desired action from client.
        /// </summary>
        /// <returns></returns>
        IServiceExecutor<DispatchResultType, SessionType> CreateExecutor();

    }
}
