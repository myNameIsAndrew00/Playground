using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Runtime
{
    public static class EncryptionExtensions
    {
        /// <summary>
        /// Use this extension method to apply a transform operation to a given array of bytes
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Apply(this ICryptoTransform transform, byte[] data)
        {
            if (data == null) return null;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data);
                    cryptoStream.Flush();
                }

                memoryStream.Flush();
                return memoryStream.ToArray();
            }
        }
    }
}
