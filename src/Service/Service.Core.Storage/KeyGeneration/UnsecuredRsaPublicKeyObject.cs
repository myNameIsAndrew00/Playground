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
    internal class UnsecuredRsaPublicKeyObject : IUnsecuredKey
    {
        private byte[] raw;

        public UnsecuredRsaPublicKeyObject(RSA rsaParameters, IEnumerable<IDataContainer<Pkcs11Attribute>> attributes)
        {
            this.raw = rsaParameters.ExportRSAPublicKey();
            this.Attributes = attributes;
        }

        public byte[] Raw => raw;

        public IEnumerable<IDataContainer<Pkcs11Attribute>> Attributes { get; set; }

        public void Dispose()
        { 
        }
    }
}
