using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token.Hashing;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Token.Hashing
{
    internal class DigestContext : IDigestContext
    {
        public DigestContext(Pkcs11Mechanism mechanism)
        {
            this.Mechanism = mechanism;
        }
         
        public Pkcs11Mechanism Mechanism { get; }

        /// <summary>
        /// Store data which was not processed by. The most common usecase is for multi part digest.
        /// </summary>
        public byte[] UnprocessedData { get; set; }

        public IMemoryObject MemoryObject => null;

        public IUnsecuredMemoryObject Unsecure() => null;

        public bool LengthRequest { get; set; }

    }
}
