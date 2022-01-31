using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Token.Encryption
{
    /// <summary>
    /// Represents an context object to handle decryption functionality
    /// </summary>
    internal class DecryptionContext : KeyContext
    {
        internal DecryptionContext(Pkcs11Mechanism mechanism, IMemoryObject memoryObject) : base(mechanism, memoryObject)
        {
        }

        public override bool EncryptionUsage => false;
    }
}
