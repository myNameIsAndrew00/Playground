using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Token.Signing.RSA
{
    public class RSASigningMechanismCommand : IMechanismCommand
    {
        public Pkcs11Mechanism MechanismType => Pkcs11Mechanism.RSA_PKCS;
        
        public void InitialiseContext(IContext contextObject, IMechanismOptions options, out ExecutionResultCode resultCode)
        {
            var signContext = contextObject as SigningContext;

            // try to unsecure the context key to validate that it is a valid rsa key.
            try
            {
                using var rsaKey = System.Security.Cryptography.RSA.Create();

                rsaKey.ImportRSAPrivateKey((signContext.Key.Unsecure() as IUnsecuredKey).Raw, out int bytesRead);                     
            }
            catch(Exception exception)
            {
                resultCode = ExecutionResultCode.KEY_TYPE_INCONSISTENT;
            }

            resultCode = ExecutionResultCode.OK;
        }

        public byte[] Execute(IContext contextObject, byte[] data, out ExecutionResultCode resultCode)
        {
            // Cast the context to a signing context
            var signContext = contextObject as SigningContext;
            
            resultCode = ExecutionResultCode.OK;

            // Try to unwrap the key and sign data.
            try
            {
                using var rsaKey = System.Security.Cryptography.RSA.Create();

                rsaKey.ImportRSAPrivateKey((signContext.Key.Unsecure() as IUnsecuredKey).Raw, out int bytesRead);

                return rsaKey.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            catch(Exception exception)
            {
                resultCode = ExecutionResultCode.FUNCTION_FAILED;
            }

            return null;
        }

        
    }
}
