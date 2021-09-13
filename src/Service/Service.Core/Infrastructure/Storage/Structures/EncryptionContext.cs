using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.DefinedTypes;
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
        public EncryptionContext(IMechanismCommand mechanismCommand, Pkcs11ContextObject objectHandler) : base(objectHandler)
        {
            this.MechanismCommand = mechanismCommand;
        }

        public IMechanismCommand MechanismCommand { get; }

        public byte[] Key => this[Pkcs11Attribute.VALUE].Value;

        public Pkcs11KeyType KeyType => (Pkcs11KeyType)this[Pkcs11Attribute.KEY_TYPE].Value.ToUInt32();

        /// <summary>
        /// IV which may be used for block encryption.
        /// </summary>
        public byte[] IV { get; set; }

        /// <summary>
        /// A boolean which specify if padding should be added in the current context.
        /// </summary>
        public bool AddPadding { get; set; } = false;

        /// <summary>
        /// Store data which was not processed by an encryption mechanism. The most common usecase is for multi part encryption.
        /// </summary>
        public byte[] UnprocessedData { get; set; }

    }
}
