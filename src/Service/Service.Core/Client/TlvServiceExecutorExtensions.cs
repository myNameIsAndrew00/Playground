using Service.Core.Abstractions.Token.Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Service.Core.Client
{
    /// <summary>
    /// Contains a list of ex
    /// </summary>
    public static class TlvServiceExecutorExtensions
    {
        /// <summary>
        /// Optain a set of pkcs11 attributes container from an array of bytes 
        /// </summary>
        /// <param name="bytes">Bytes used to provide data</param>
        /// <returns></returns>
        public static IEnumerable<Pkcs11DataContainer<Type>> ToPkcs11DataContainerCollection<Type>(this IEnumerable<byte> bytes)
            where Type : Enum
        {
            return ToPkcs11DataContainerCollection(bytes, typeof(Type)) as IEnumerable<Pkcs11DataContainer<Type>>;
        }

        /// <summary>
        /// Optain a pkcs11 attribute from an array of bytes
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Pkcs11DataContainer<Type> ToPkcs11DataContainer<Type>(this IEnumerable<byte> bytes)
           where Type : Enum
        {
            return ToPkcs11DataContainer(bytes, typeof(Type)) as Pkcs11DataContainer<Type>;
        }

        public static object ToPkcs11DataContainer(this IEnumerable<byte> bytes, Type enumType)
        {
            TryParsePkcs11DataContainer(bytes, enumType, out object output);
            
            return output;
        }

        public static object ToPkcs11DataContainerCollection(this IEnumerable<byte> bytes, Type enumType)
        {
            TryParsePkcs11DataContainerCollection(bytes, enumType, out object output);

            return output;
        }

        public static int TryParsePkcs11DataContainer(this IEnumerable<byte> bytes, Type enumType, out object output)
        {
            output = null;

            if (bytes == null) return 0;

            int cursor = 0;

            try
            {
                Type containerType = enumType == null ? typeof(Pkcs11DataContainer) : typeof(Pkcs11DataContainer<>).MakeGenericType(enumType);
                output = parseContainer(containerType, bytes, ref cursor);

                return cursor;
            }
            catch
            {
                return cursor;
            }
        }

        public static int TryParsePkcs11DataContainerCollection(this IEnumerable<byte> bytes, Type enumType, out object output)
        {
            output = null;
            if (bytes == null) return 0;

            int cursor = 0;
            try
            {
                Type containerType = enumType == null ? typeof(Pkcs11DataContainer) : typeof(Pkcs11DataContainer<>).MakeGenericType(enumType);

                Type resultType = typeof(List<>).MakeGenericType(containerType);

                var result = (IList)Activator.CreateInstance(resultType);

                while (cursor < bytes.Count())
                    result.Add(parseContainer(containerType, bytes, ref cursor));

                output = result;
                return cursor;
            }
            catch
            {
                return cursor;
            }
        }



        #region Private

        private static object parseContainer(Type type, IEnumerable<byte> bytes, ref int cursor) 
        {
            byte[] _bytes = bytes.ToArray();
            Pkcs11DataContainer container = (Pkcs11DataContainer) Activator.CreateInstance(type);

            container.Type = BitConverter.ToInt64(_bytes, cursor);
            cursor += sizeof(long);

            long dataLength = BitConverter.ToInt64(_bytes, cursor);
            cursor += sizeof(long);

            container.Value = new byte[dataLength];
            _bytes.CopyTo(container.Value, cursor);
            cursor += (int)dataLength;

            return container;
        }

        #endregion
    }
}
