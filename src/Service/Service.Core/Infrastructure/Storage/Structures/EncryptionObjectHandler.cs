using Service.Core.Infrastructure.Token.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Storage.Structures
{
    /// <summary>
    /// Represents an context object to handle encryption
    /// </summary>
    public class EncryptionObjectHandler : Pkcs11ObjectHandler
    {
        internal EncryptionObjectHandler(IEnumerable<Pkcs11DataContainer<Pkcs11Attribute>> attributes) : base(attributes)
        {
        }

        public override void Dispose()
        { 
        }
    }
}
