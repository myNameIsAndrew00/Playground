using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token.Structures
{
    /// <summary>
    /// Represents an handle to keep data of a tlv structure
    /// </summary>
    internal struct Pkcs11AttributeContainer
    {
        public Pkcs11Attribute Attribute { get; set; }

        public byte[] Value { get; set; }
    }
}
