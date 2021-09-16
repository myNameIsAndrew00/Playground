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
    internal class DataContainer : IDataContainer
    {
        public ulong Type { get; set; }

        /// <summary>
        /// Represents the value of container
        /// </summary>
        public byte[] Value { get; set; }
 
    }

    /// <summary>
    /// Represents an generic handler to keep data of a attribute tlv structure. Type and length value have 8 bytes each (long data type)
    /// </summary>
    internal class DataContainer<EnumDataType> : DataContainer, IDataContainer<EnumDataType>
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
                base.Type = (ulong)(object)value;
            }
        }
    }

    /// <summary>
    /// Represents handler to keep pkcs11 attribute data.
    /// </summary>
    internal class Pkcs11AttributeContainer : DataContainer<Pkcs11Attribute>, IPkcs11AttributeDataContainer
    {
    }

    /// <summary>
    /// Represents handler to keep pkcs11 mechanism data.
    /// </summary>
    internal class Pkcs11MechanismContainer : DataContainer<Pkcs11Mechanism>
    {
    }
}
