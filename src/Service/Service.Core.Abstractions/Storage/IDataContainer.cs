using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Storage
{
    /// <summary>
    /// Represents an handler to keep data of a attribute tlv structure. Type and length value have 4 bytes each (uint data type)
    /// </summary>
    public interface IDataContainer
    {
        public ulong Type { get; set; }

        /// <summary>
        /// Represents the value of container
        /// </summary>
        public byte[] Value { get; set; }

        /// <summary>
        /// Represents the size of raw container in bytes
        /// </summary>
        public int Size => sizeof(ulong) /*size of type*/ + sizeof(uint) /*size of length*/ + Value.Length;
    }

    /// <summary>
    /// Represents an generic handler to keep data of a attribute tlv structure. Type and length value have 8 bytes each (long data type)
    /// </summary>
    public interface IDataContainer<EnumDataType> : IDataContainer
        where EnumDataType : Enum
    {
        /// <summary>
        /// Represents the type of the container
        /// </summary>
        public new EnumDataType Type { get; set; }
    }

    public interface IPkcs11AttributeDataContainer : IDataContainer<Pkcs11Attribute> { }
}
