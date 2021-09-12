using Service.Core.Abstractions.Token;
using Service.Core.Infrastructure.Storage.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Communication
{
    /// <summary>
    /// Expose methods which describe a PKCS 11 service behavior.
    /// </summary> 
    public interface IPkcs11Server
    {
        /// <summary>
        /// Enable server instance to wait for listening client requests.
        /// </summary>
        void Start();

        /// <summary>
        /// The instance which will initialise the communication and handle messages between server and client.
        /// </summary>
        IServiceCommunicationResolver Resolver { get; }

        /// <summary>
        /// A method which will create the instance to execute the desired action from client.
        /// </summary>
        /// <returns></returns>
        IServiceExecutor CreateExecutor();

        /// <summary>
        /// Register an module to the server which may be used by executor to handle the request invoked.
        /// </summary>
        /// <typeparam name="ModuleType"></typeparam>
        /// <returns></returns>
        void RegisterModule<ModuleType, ImplementationType>() 
            where ModuleType : ITokenModule
            where ImplementationType : ITokenModule;

        /// <summary>
        /// Register an encryption module to the server which may be used by executor to handle encryption operations.
        /// </summary>
        /// <returns></returns>
        void RegisterEncryptionModule<EncryptionModuleType>(Func<EncryptionObjectHandler, EncryptionModuleType> implementationFactory = null) 
            where EncryptionModuleType : IEncryptionModule;

        /// <summary>
        /// Register an encryption module to the server which may be used by executor to handle hashing operations.
        /// </summary>
        /// <returns></returns>
        void RegisterHashingModule<HashingModuleType>(Func<Pkcs11ObjectHandler, HashingModuleType> implementationFactory = null) where HashingModuleType : IHashingModule;

        /// <summary>
        /// Register an encryption module to the server which may be used by executor to handle signing operations.
        /// </summary>
        /// <returns></returns>
        void RegisterSigningModule<SigningModuleType>(Func<Pkcs11ObjectHandler, SigningModuleType> implementationFactory = null) where SigningModuleType : ISigningModule;

    }
}
