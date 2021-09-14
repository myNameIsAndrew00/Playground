using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Storage.Memory
{
    /// <summary>
    /// Represents an handler to keep data of a attribute tlv structure. Type and length value have 4 bytes each (uint data type)
    /// </summary>
    public class DataContainer : IDataContainer
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
    public class DataContainer<EnumDataType> : DataContainer, IDataContainer<EnumDataType>
        where EnumDataType : Enum
    {
        /// <summary>
        /// Represents the type of the container
        /// </summary>
        public new EnumDataType Type
        {
            get
            {
                return (EnumDataType)(object)base.Type;
            }
            set
            {
                base.Type = (uint)(object)value;
            }
        }
    }

    /// <summary>
    /// Represents handler to keep pkcs11 attribute data.
    /// </summary>
    public class Pkcs11AttributeContainer : DataContainer<Pkcs11Attribute>, IPkcs11AttributeDataContainer
    {
    }

    /// <summary>
    /// Represents handler to keep pkcs11 mechanism data.
    /// </summary>
    public class Pkcs11MechanismContainer : DataContainer<Pkcs11Mechanism>
    {
    }
}
