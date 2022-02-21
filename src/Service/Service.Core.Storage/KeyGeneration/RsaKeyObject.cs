using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using Service.Core.Storage.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Storage.KeyGeneration
{
    internal class RsaKeyObject : MemoryObject
    {
        private RSA rsaKey;
        private bool @private;

        internal RsaKeyObject(IEnumerable<IDataContainer<Pkcs11Attribute>> attributes, RSA keyObject, bool @private) : base(attributes)
        {
            this.rsaKey = keyObject;
            this.@private = @private;
        }

        public override IUnsecuredMemoryObject Unsecure()
        {
            return @private ?
                new UnsecuredRsaPrivateKeyObject(rsaKey, attributes) :
                new UnsecuredRsaPublicKeyObject(rsaKey, attributes);
        }
    }
}
