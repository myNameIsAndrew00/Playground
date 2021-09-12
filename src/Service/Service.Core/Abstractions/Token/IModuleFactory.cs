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
        /// Get a parameterless module from the collection. If module requires parameters for construction, an exception will be thrown.
        /// </summary>
        /// <typeparam name="ModuleType"></typeparam>
        /// <returns></returns>
        ModuleType GetModule<ModuleType>() where ModuleType : class, ITokenModule;

        //todo: replace pkcs11object handler
        ISigningModule GetSigningModule(Pkcs11ObjectHandler objectHandler);

        //todo: replace pkcs11object handler
        IHashingModule GetHashingModule(Pkcs11ObjectHandler objectHandler);

        /// <summary>
        /// Get the encryption module from the collection
        /// </summary>
        /// <param name="objectHandler"></param>
        /// <returns></returns>
        IEncryptionModule GetEncryptionModule(EncryptionObjectHandler objectHandler);
    }
}
