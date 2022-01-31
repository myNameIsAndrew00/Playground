using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.DefinedTypes;
using Service.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Token.Encryption
{
    /// <summary>
    /// Represents an context object to handle encryption functionality
    /// </summary>
    internal class EncryptionContext : KeyContext
    {
        internal EncryptionContext(Pkcs11Mechanism mechanism, IMemoryObject memoryObject) : base(mechanism, memoryObject)
        {
        }

        /// <summary>
        /// A boolean which specify if padding should be added in the current context.
        /// </summary>
        public bool AddPadding { get; set; } = false;

        public override bool EncryptionUsage => true;
    }
}
