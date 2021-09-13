using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.DefinedTypes;
using Service.Core.Infrastructure.Communication.Structures;
using Service.Core.Infrastructure.Storage.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Service.Core.Infrastructure.Token.Encryption
{
    /// <summary>
    /// Represents a base class for aes mechanism command objects
    /// </summary>
    internal abstract class AESMechanismCommand : IMechanismCommand
    {        

        protected readonly HashSet<int> allowedKeySizes = new HashSet<int> { 16, 24, 32 };

        protected readonly int encryptionDataBlockSize = 16;

        protected abstract CipherMode CipherMode { get; }

        protected abstract Func<AesManaged, ICryptoTransform> GetCryptor { get; }


        public abstract Pkcs11Mechanism MechanismType { get; }

        /// <summary>
        /// Execute the aes encryption.
        /// </summary>
        /// <param name="contextObject"></param>
        /// <param name="data"></param>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public byte[] Execute(Pkcs11ContextObject contextObject, byte[] data, out ExecutionResultCode resultCode)
        {
            EncryptionContext encryptionContext = contextObject as EncryptionContext;

            resultCode = ExecutionResultCode.OK;

            if (data is null || (data.Length == 0 && !encryptionContext.AddPadding))
            {
                resultCode = ExecutionResultCode.DATA_INVALID;
                return null;
            }

            data = PreprocessExecutionData(encryptionContext, data);

            if (data.Length == 0 && !encryptionContext.AddPadding) return data;

            //execute the encryption operation
            using (var aesContext = new AesManaged
            {
                Padding = encryptionContext.AddPadding ? PaddingMode.PKCS7 : PaddingMode.None,
                Mode = CipherMode,
                KeySize = encryptionContext.Key.Length * 8,
                IV = encryptionContext.IV,
                Key = encryptionContext.Key
            })
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, GetCryptor(aesContext), CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(cryptoStream))
                        {
                            writer.Write(data);
                        }
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Preprocess data which is executed by this command object.
        /// </summary>
        /// <param name="encryptionContext"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract byte[] PreprocessExecutionData(EncryptionContext encryptionContext, byte[] data);
      

        /// <summary>
        /// Initialise a context object which may be used for aes encryption
        /// </summary>
        /// <param name="contextObject"></param>
        /// <param name="initialisationBytes">An array of bytes representing the IV. Must be of length 16</param>
        /// <param name="resultCode"></param>
        public void InitialiseContext(Pkcs11ContextObject contextObject, byte[] initialisationBytes, out ExecutionResultCode resultCode)
        {
            EncryptionContext encryptionContext = contextObject as EncryptionContext;

            if (encryptionContext is null || encryptionContext.Key is null || !allowedKeySizes.Contains(encryptionContext.Key.Length))
            {
                resultCode = ExecutionResultCode.KEY_HANDLE_INVALID;
                return;
            }

            if (encryptionContext.KeyType != Pkcs11KeyType.AES)
            {
                resultCode = ExecutionResultCode.KEY_TYPE_INCONSISTENT;
                return;
            }

            // iv must be of 16 bytes length
            if (initialisationBytes.Length != 16)
            {
                resultCode = ExecutionResultCode.MECHANISM_PARAM_INVALID;
                return;
            }

            encryptionContext.IV = initialisationBytes;

            resultCode = ExecutionResultCode.OK;
        }
    }
}
