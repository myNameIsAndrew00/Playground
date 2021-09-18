using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.DefinedTypes;
using Service.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Service.Core.Token.Encryption.AES
{
    /// <summary>
    /// Represents a base class for aes encrypt mechanism command objects
    /// </summary>
    public abstract class AESEncryptMechanismCommand : AESMechanismCommand
    {

        /// <summary>
        /// Execute the aes encryption. Expects an encryption context as parameter.
        /// </summary>
        /// <param name="contextObject"></param>
        /// <param name="data"></param>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public override byte[] Execute(IContext contextObject, byte[] data, out ExecutionResultCode resultCode)
        {
            EncryptionContext encryptionContext = contextObject as EncryptionContext;
            
            //validate that an encryption context is received
            if (encryptionContext is null)
            {
                resultCode = ExecutionResultCode.OPERATION_NOT_INITIALIZED;
                return null;
            }

            resultCode = ExecutionResultCode.OK;

            if (data is null || (data.Length == 0 && !encryptionContext.AddPadding))
            {
                resultCode = ExecutionResultCode.DATA_INVALID;
                return null;
            }

            byte[] processsedData = preprocessExecutionData(encryptionContext, data);

            if (processsedData.Length == 0 && !encryptionContext.AddPadding) return processsedData;

            //execute the encryption operation
            using (var aesContext = GetAesContext(encryptionContext))
            {
                if (!encryptionContext.AddPadding) aesContext.Padding = PaddingMode.None;
                
                return aesContext.CreateEncryptor().Apply(processsedData);
            }
        }

        #region Private

        private byte[] preprocessExecutionData(EncryptionContext encryptionContext, byte[] data)
        {
            //in case of an operation which not require padding, block size must be fixed for input
            
            if (!encryptionContext.AddPadding && (data.Length % EncryptionDataBlockSize != 0))
            {
                int unprocessedBytesCount = data.Length % EncryptionDataBlockSize;
                int processedBlocksCount = data.Length / EncryptionDataBlockSize;

                encryptionContext.UnprocessedData = encryptionContext.UnprocessedData is not null ? 
                    encryptionContext.UnprocessedData.Concat(new byte[data.Length % EncryptionDataBlockSize]) :
                    new byte[data.Length % EncryptionDataBlockSize];

                Array.Copy(data, processedBlocksCount * EncryptionDataBlockSize, encryptionContext.UnprocessedData, 0, unprocessedBytesCount);

                data = data.Take(processedBlocksCount * EncryptionDataBlockSize).ToArray();
            }

            return data;
        }

        #endregion

    }
}
