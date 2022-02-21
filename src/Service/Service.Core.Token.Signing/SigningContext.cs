using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token.Signing;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Token.Signing
{
    internal class SigningContext : ISigningContext
    {
        public IMemoryObject Key { get; }

        public IMemoryObject MemoryObject => Key;

        public Pkcs11Mechanism Mechanism { get; }

        internal SigningContext(Pkcs11Mechanism mechanism, IMemoryObject memoryPublicKey)
        {
            this.Mechanism = mechanism;
            this.Key = memoryPublicKey;
        }

        public bool LengthRequest { get; set; }

        public IUnsecuredKey Unsecure() => Key?.Unsecure() as IUnsecuredKey;

        IUnsecuredMemoryObject IMemoryObject.Unsecure() => Key?.Unsecure();

        /// <summary>
        /// Store data which was not processed by an signing mechanism. The most common usecase is for multi part signing.
        /// </summary>
        public byte[] UnprocessedData { get; set; }

    }
}
