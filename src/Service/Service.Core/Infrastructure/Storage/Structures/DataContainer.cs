using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Storage.Structures
{
    /// <summary>
    /// Represents an handler to keep data of a attribute tlv structure. Type and length value have 4 bytes each (uint data type)
    /// </summary>
    public class DataContainer
    {
        public uint Type { get; set; }

        /// <summary>
        /// Represents the value of container
        /// </summary>
        public byte[] Value { get; set; }

        /// <summary>
        /// Represents the size of raw container in bytes
        /// </summary>
        public int Size => sizeof(uint) /*size of type*/ + sizeof(uint) /*size of length*/ + Value.Length;
    }

    /// <summary>
    /// Represents an generic handler to keep data of a attribute tlv structure. Type and length value have 8 bytes each (long data type)
    /// </summary>
    public class DataContainer<EnumDataType> : DataContainer
        where EnumDataType : Enum
    {
        /// <summary>
        /// Represents the type of the container
        /// </summary>
        public new EnumDataType Type => (EnumDataType)(object)base.Type;

    }
}
