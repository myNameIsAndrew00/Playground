using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using Service.Core.Storage.Memory;
using Service.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Storage.KeyGeneration
{
    internal class KeyGenerator
    {
        private IEnumerable<IDataContainer<Pkcs11Attribute>> publicKeyAttributes;
        private IEnumerable<IDataContainer<Pkcs11Attribute>> privateKeyAttributes;
        private Pkcs11Mechanism mechanism;

        private ulong modulusLength;

        public KeyGenerator(IEnumerable<IDataContainer<Pkcs11Attribute>> publicKeyAttributes, IEnumerable<IDataContainer<Pkcs11Attribute>> privateKeyAttributes)
        {
            this.privateKeyAttributes = privateKeyAttributes;
            this.publicKeyAttributes = publicKeyAttributes;
        }

        public bool Initialise(Pkcs11Mechanism mechanism, out ExecutionResultCode resultCode)
        {
            // for now, the only one mechanism allowed is RSA
            if (mechanism != Pkcs11Mechanism.RSA_PKCS_KEY_PAIR_GEN)
            {
                resultCode = ExecutionResultCode.MECHANISM_INVALID;
                return false;
            }

            // for now, only modulus length is used from attribues
            modulusLength = publicKeyAttributes.Where(attribute => attribute.Type == Pkcs11Attribute.MODULUS_BITS).FirstOrDefault()?.Value.ToULong() ?? 1024;

            resultCode = ExecutionResultCode.OK;

            return true;
        }


        public void Generate(out MemoryObject rsaPrivateKey, out MemoryObject rsaPublicKey, out ExecutionResultCode resultCode)
        {
            try
            {
                RSA keyObject = RSA.Create((int)modulusLength);

                rsaPrivateKey = new RsaKeyObject(privateKeyAttributes, keyObject, true);
                rsaPublicKey = new RsaKeyObject(publicKeyAttributes, keyObject, false);

                resultCode = ExecutionResultCode.OK;

            }
            catch
            {
                rsaPrivateKey = null;
                rsaPublicKey = null;

                resultCode = ExecutionResultCode.FUNCTION_FAILED;
            }

        }
    }
}
