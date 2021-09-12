using Service.Core.Infrastructure.Token.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Storage.Structures
{
    /// <summary>
    /// Represents an context object to handle encryption functionality
    /// </summary>
    public class EncryptionContext : Pkcs11ContextObjectDecorator
    {
        public EncryptionContext(Pkcs11ContextObject objectHandler) : base(objectHandler)
        {
        }
    }
}
