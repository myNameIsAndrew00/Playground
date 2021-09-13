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
    /// Represents a base class for aes encrypt mechanism command objects
    /// </summary>
    internal abstract class AESEncryptMechanismCommand : AESMechanismCommand
    {
        protected override Func<AesManaged, ICryptoTransform> GetCryptor => (aesContext) => aesContext.CreateEncryptor();

        protected override byte[] PreprocessExecutionData(EncryptionContext encryptionContext, byte[] data)
        {
            //in case of an operation which not require padding, block size must be fixed for input
            if (!encryptionContext.AddPadding && (data.Length % encryptionDataBlockSize != 0))
            {
                int unprocessedBytesCount = data.Length % encryptionDataBlockSize;
                int processedBlocksCount = data.Length / encryptionDataBlockSize;

                encryptionContext.UnprocessedData = new byte[data.Length % encryptionDataBlockSize];
                Array.Copy(data, processedBlocksCount * encryptionDataBlockSize, encryptionContext.UnprocessedData, 0, unprocessedBytesCount);

                data = data.Take(processedBlocksCount * encryptionDataBlockSize).ToArray();
            }

            return data;
        }
    }
}
