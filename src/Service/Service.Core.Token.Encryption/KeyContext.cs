﻿using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.DefinedTypes;
using Service.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Token.Encryption
{
    /// <summary>
    /// Represents a context class for keys (used on encryption modules)
    /// </summary>
    internal abstract class KeyContext : IKeyContext
    {
        public IMemoryObject MemoryObject { get; }

        internal KeyContext(Pkcs11Mechanism mechanism, IMemoryObject objectHandler)
        {
            this.MemoryObject = objectHandler;
            this.Mechanism = mechanism;
        }

        public Pkcs11Mechanism Mechanism { get; }

        public byte[] Key => (this as IContext)[Pkcs11Attribute.VALUE]?.Value;

        public uint? KeyLength => (this as IContext)[Pkcs11Attribute.VALUE_LEN]?.Value.ToUInt32();

        public abstract bool EncryptionUsage { get; }

        public Pkcs11KeyType? KeyType
        {
            get
            {
                UInt64? value = (this as IContext)[Pkcs11Attribute.KEY_TYPE]?.Value.ToULong();
                return value is not null ? (Pkcs11KeyType)value : null;
            }
        }

        public IUnsecuredMemoryObject Unsecure() => MemoryObject?.Unsecure();

        /// <summary>
        /// IV which may be used for block encryption/decryption.
        /// </summary>
        public byte[] IV { get; set; }
      
        public bool LengthRequest { get; set; }

        /// <summary>
        /// Store data which was not processed by an encryption/decryption mechanism. The most common usecase is for multi part encryption/decryption.
        /// </summary>
        public byte[] UnprocessedData { get; set; }

        
    }
}
