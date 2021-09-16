using Service.Core.Abstractions.Storage;
using Service.Core.Storage.Memory;
using Service.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Storage
{
    /// <summary>
    /// Use this class to create data containers from bytes
    /// </summary>
    internal class PayloadDataParser
    {
        /// <summary>
        /// Optain a set of pkcs11 attributes container from an array of bytes 
        /// </summary>
        /// <param name="bytes">Bytes used to provide data</param>
        /// <returns></returns>
        public  IEnumerable<IDataContainer<Type>> ToPkcs11DataContainerCollection<Type>( IEnumerable<byte> bytes)
            where Type : Enum
        {
            return ToPkcs11DataContainerCollection(bytes, typeof(Type)) as IEnumerable<IDataContainer<Type>>;
        }

        public  IDataContainer<Type> ToPkcs11DataContainer<Type>( IEnumerable<byte> bytes)
           where Type : Enum
        {
            return ToPkcs11DataContainer(bytes, typeof(Type)) as IDataContainer<Type>;
        }

        /// <summary>
        /// Optain a pkcs11 attribute from an array of bytes
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public  object ToPkcs11DataContainer( IEnumerable<byte> bytes, Type enumType)
        {
            TryParsePkcs11DataContainer(bytes, enumType, out object output);

            return output;
        }

        public  object ToPkcs11DataContainerCollection( IEnumerable<byte> bytes, Type enumType)
        {
            TryParsePkcs11DataContainerCollection(bytes, enumType, out object output);

            return output;
        }

        public  int TryParsePkcs11DataContainer( IEnumerable<byte> bytes, Type enumType, out object output)
        {
            output = null;

            if (bytes == null) return 0;

            int cursor = 0;

            try
            {
                Type containerType = enumType == null ? typeof(DataContainer) : typeof(DataContainer<>).MakeGenericType(enumType);
                output = parseContainer(containerType, bytes, ref cursor);

                return cursor;
            }
            catch
            {
                return cursor;
            }
        }

        public  int TryParsePkcs11DataContainerCollection( IEnumerable<byte> bytes, Type enumType, out object output)
        {
            output = null;
            if (bytes == null) return 0;

            int cursor = 0;
            try
            {
                Type containerType = enumType == null ? typeof(DataContainer) : typeof(DataContainer<>).MakeGenericType(enumType);

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

        private  object parseContainer(Type type, IEnumerable<byte> bytes, ref int cursor)
        {
            IDataContainer container = (IDataContainer)Activator.CreateInstance(type);

            // parse the type.
            container.Type = bytes.Skip(cursor).ToULong();
            cursor += sizeof(ulong);

            // parse the value.
            uint dataLength = bytes.Skip(cursor).ToUInt32();
            cursor += sizeof(uint);

            // parse the data.
            container.Value = new byte[dataLength];
            byte[] _bytes = bytes.Skip(cursor).Take((int)dataLength).ToArray();
            _bytes.CopyTo(container.Value, 0);
            cursor += (int)dataLength;

            return container;
        }

        #endregion
    }
}
