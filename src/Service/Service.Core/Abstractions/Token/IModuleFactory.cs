using Service.Core.Infrastructure.Storage.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token
{
    /// <summary>
    /// Implements method to receive modules registered to a pkcs11 server
    /// </summary>
    public interface IModuleFactory
    {
        /// <summary>
        /// Get a parameterless module from the collection (without context initialisation).
        /// If module requires context for construction, an exception will be thrown.
        /// </summary>
        /// <typeparam name="ModuleType"></typeparam>
        /// <returns></returns>
        ModuleType GetModule<ModuleType>() where ModuleType : class, ITokenModule;

        /// <summary>
        /// Get the signing module from the collection
        /// </summary>
        /// <param name="context">Context used to initialise the module</param>
        /// <returns></returns>
        ISigningModule GetSigningModule(Pkcs11ContextObject context);
     
        /// <summary>
        /// Get the hashing module from the collection
        /// </summary>
        /// <param name="context">Context used to initialise the module</param>
        /// <returns></returns>
        IHashingModule GetHashingModule(Pkcs11ContextObject context);

        /// <summary>
        /// Get the encryption module from the collection
        /// </summary>
        /// <param name="context">Context used to initialise the module</param>
        /// <returns></returns>
        IEncryptionModule GetEncryptionModule(Pkcs11ContextObject context);
    }
}
