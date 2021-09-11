using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token.Structures
{
    /// <summary>
    /// Represents an handler to keep data of a attribute tlv structure. Type and length value have 8 bytes each (long data type)
    /// </summary>
    public class Pkcs11DataContainer
    {
        public long Type { get; set; }

        /// <summary>
        /// Represents the value of container
        /// </summary>
        public byte[] Value { get; set; }

        /// <summary>
        /// Represents the size of raw container in bytes
        /// </summary>
        public int Size => sizeof(long) /*size of type*/ + sizeof(long) /*size of length*/ + Value.Length;
    }

    /// <summary>
    /// Represents an generic handler to keep data of a attribute tlv structure. Type and length value have 8 bytes each (long data type)
    /// </summary>
    public class Pkcs11DataContainer<EnumDataType> : Pkcs11DataContainer
        where EnumDataType : Enum
    {
        /// <summary>
        /// Represents the type of the container
        /// </summary>
        public new EnumDataType Type { get; set; }

    }
}
