using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token.Structures
{
    /// <summary>
    /// Represents an handle to keep data of a attribute tlv structure
    /// </summary>
    public struct Pkcs11DataContainer<EnumDataType> 
        where EnumDataType : Enum
    {
        public EnumDataType Type { get; set; }

        public byte[] Value { get; set; }
    }
}
