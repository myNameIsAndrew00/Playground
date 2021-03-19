using Service.Core.Abstractions.Token.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Token
{
    internal static class Extensions
    {
        /// <summary>
        /// Optain a set of pkcs11 attributes container from an array of bytes 
        /// </summary>
        /// <param name="bytes">Bytes used to provide data</param>
        /// <returns></returns>
        public static IEnumerable<Pkcs11AttributeContainer> ToPkcs11Container(this byte[] bytes)
        {
            if (bytes == null) return null;

            try
            {
                List<Pkcs11AttributeContainer> result = new List<Pkcs11AttributeContainer>();
                int cursor = 0;

                while (cursor < bytes.Length)
                {
                    Pkcs11AttributeContainer container = new Pkcs11AttributeContainer();

                    container.Attribute = (Pkcs11Attribute)BitConverter.ToInt64(bytes, cursor);
                    cursor += sizeof(long);

                    long dataLength = BitConverter.ToInt64(bytes, cursor);
                    cursor += sizeof(long);

                    container.Value = new byte[dataLength];
                    bytes.CopyTo(container.Value, cursor);
                    cursor += (int)dataLength;
                }

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
