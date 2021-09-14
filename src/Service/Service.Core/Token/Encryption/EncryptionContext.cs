using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.DefinedTypes;
using Service.Core.Storage.Memory;
using Service.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Token.Encryption
{
    /// <summary>
    /// Represents an context object to handle encryption functionality
    /// </summary>
    public class EncryptionContext : ContextDecorator
    {
        public EncryptionContext(IMechanismCommand mechanismCommand, IMemoryObject objectHandler) : base(objectHandler)
        {
            this.MechanismCommand = mechanismCommand;
        }

        public IMechanismCommand MechanismCommand { get; }

        public byte[] Key => this[Pkcs11Attribute.VALUE]?.Value;

        public uint? KeyLength => this[Pkcs11Attribute.VALUE_LEN]?.Value.ToUInt32();

        public Pkcs11KeyType? KeyType
        {
            get
            {
                UInt64? value = this[Pkcs11Attribute.KEY_TYPE]?.Value.ToULong();
                return value is not null ? (Pkcs11KeyType)value : null;
            }
        }

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
