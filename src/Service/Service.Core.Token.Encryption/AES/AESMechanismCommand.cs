using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Service.Core.Token.Encryption.AES
{
    /// <summary>
    /// Represents a base class for aes mechanism command objects
    /// </summary>
    public abstract class AESMechanismCommand : IMechanismCommand
    {        

        protected readonly HashSet<uint> AllowedKeySizes = new HashSet<uint> { 128, 192, 256 };

        protected readonly int EncryptionDataBlockSize = 16;

        protected readonly PaddingMode PaddingMode = PaddingMode.PKCS7;

        protected abstract CipherMode CipherMode { get; }


        public abstract Pkcs11Mechanism MechanismType { get; }


        public abstract byte[] Execute(IContext contextObject, byte[] data, out ExecutionResultCode resultCode);

        /// <summary>
        /// Initialise a context object which may be used for aes
        /// </summary>
        /// <param name="contextObject"></param>
        /// <param name="initialisationData">Mechanism options</param>
        /// <param name="resultCode"></param>
        public virtual void InitialiseContext(IContext contextObject, IMechanismOptions initialisationData, out ExecutionResultCode resultCode)
        {
            KeyContext keyContext = contextObject as KeyContext;

            if (keyContext is null || keyContext.Key is null || !AllowedKeySizes.Contains(keyContext.KeyLength ?? 0))
            {
                resultCode = ExecutionResultCode.KEY_HANDLE_INVALID;
                return;
            }

            if (keyContext.KeyType != Pkcs11KeyType.AES)
            {
                resultCode = ExecutionResultCode.KEY_TYPE_INCONSISTENT;
                return;
            } 

            resultCode = ExecutionResultCode.OK;
        }

        /// <summary>
        /// Creates an aes managed object using a key context
        /// </summary>
        /// <param name="keyContext"></param>
        /// <returns></returns>
        internal AesManaged GetAesContext(KeyContext keyContext) => new AesManaged
        {
            Padding = PaddingMode,
            Mode = CipherMode,
            KeySize = (int)keyContext.KeyLength.Value,
            IV = keyContext.IV,
            Key = keyContext.Key
        };


    }
}
