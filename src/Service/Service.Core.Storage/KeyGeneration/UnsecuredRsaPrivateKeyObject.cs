using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Storage.KeyGeneration
{
    internal class UnsecuredRsaPrivateKeyObject : IUnsecuredKey
    {
        private byte[] raw;
         

        public UnsecuredRsaPrivateKeyObject(RSA rsaKey, IEnumerable<IDataContainer<Pkcs11Attribute>> attributes)
        {
            this.raw = rsaKey.ExportRSAPrivateKey();
            this.Attributes = attributes;
        }

        public IEnumerable<IDataContainer<Pkcs11Attribute>> Attributes { get; set; }

        public byte[] Raw => raw;

        public void Dispose()
        { 
        }
    }
}
