using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Token.Encryption.AES
{
    public class AESECBDecryptMechanismCommand : AESDecryptMechanismCommand
    {
        public override Pkcs11Mechanism MechanismType => Pkcs11Mechanism.AES_ECB;

        protected override CipherMode CipherMode => CipherMode.ECB;

        public override void InitialiseContext(IContext contextObject, IMechanismOptions initialisationData, out ExecutionResultCode resultCode)
        {
            base.InitialiseContext(contextObject, initialisationData, out resultCode);

            if (resultCode != ExecutionResultCode.OK) return;

            DecryptionContext encryptionContext = contextObject as DecryptionContext;
            IAesMechanismOptions options = initialisationData as IAesMechanismOptions;

            if (options.IV.Length != 16)
            {
                resultCode = ExecutionResultCode.MECHANISM_PARAM_INVALID;
                return;
            }

            encryptionContext.IV = options.IV;
        }
    }
}
