using Service.Core.Infrastructure.Token.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Storage.Structures
{
    /// <summary>
    /// Represents an model of a pcks11 object. A pkcs11 object represents an inmemory storage class which may be associated with a context object.
    /// It is used to handle internal operations for different modules.
    /// </summary>
    public abstract class Pkcs11ObjectHandler : IDisposable
    {       

        internal Pkcs11ObjectHandler(IEnumerable<Pkcs11DataContainer<Pkcs11Attribute>> attributes)
        {
            this.Attributes = attributes;
        }

        public long Id { get; }

        public IEnumerable<Pkcs11DataContainer<Pkcs11Attribute>> Attributes { get; set; }

        public abstract void Dispose();
    }
}
