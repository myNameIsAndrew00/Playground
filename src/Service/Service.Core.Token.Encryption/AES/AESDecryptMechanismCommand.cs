using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using Service.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Token.Encryption.AES
{
    public abstract class AESDecryptMechanismCommand : AESMechanismCommand
    {
        public override byte[] Execute(IContext contextObject, byte[] data, out ExecutionResultCode resultCode)
        {
            DecryptionContext decryptionContext = contextObject as DecryptionContext;

            //validate that an decryption context is received
            if (decryptionContext is null)
            {
                resultCode = ExecutionResultCode.OPERATION_NOT_INITIALIZED;
                return null;
            }

            resultCode = ExecutionResultCode.OK;

            //validate that data provided is not null and have a valid length
            if (data is null || data.Length == 0)
            {
                resultCode = ExecutionResultCode.ENCRYPTED_DATA_INVALID;
                return null;
            }

            //check bytes which must be finaly decrypted
            byte[] processedData = preprocessExecutionData(decryptionContext, data);
            
            //if there is more unprocessed data available, that means that input block is still not of a valid length.
            //Still, is processedData is not empty, decryption operation can continue.
            if (decryptionContext.UnprocessedData is not null) resultCode = ExecutionResultCode.ENCRYPTED_DATA_LEN_RANGE;

            //If processedData is empty, stop the decryption operation.
            if (processedData.Length == 0) return null;

            using (var aesContext = GetAesContext(decryptionContext))
            {
                return aesContext.CreateDecryptor().Apply(processedData);
            }

        }

        #region Private

        private byte[] preprocessExecutionData(DecryptionContext decryptionContext, byte[] data)
        {
            //if length is not of a block size, reinitialise the unprocessed data
            //this means that input length is still not large enough

            if (data.Length % EncryptionDataBlockSize != 0)
            {
                int unprocessedBytesCount = data.Length % EncryptionDataBlockSize;
                int processedBlocksCount = data.Length / EncryptionDataBlockSize;

                decryptionContext.UnprocessedData = new byte[unprocessedBytesCount];

                Array.Copy(data, processedBlocksCount * EncryptionDataBlockSize, decryptionContext.UnprocessedData, 0, unprocessedBytesCount);
                data = data.Take(processedBlocksCount * EncryptionDataBlockSize).ToArray();
            }

            return data;
        }

        #endregion
    }
}
