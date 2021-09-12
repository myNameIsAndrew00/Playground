using Service.Core.Infrastructure.Token.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Storage.Structures
{
    /// <summary>
    /// Represents an model of a pcks11 object
    /// </summary>
    public class Pkcs11Object
    {       

        public Pkcs11Object(IEnumerable<Pkcs11DataContainer<Pkcs11Attribute>> attributes)
        {
            this.Attributes = attributes;
        }

        public long Id { get; set; }

        public IEnumerable<Pkcs11DataContainer<Pkcs11Attribute>> Attributes { get; set; }
    }
}
