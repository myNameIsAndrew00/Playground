using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Service.Core.Token.Encryption.AES
{
    /// <summary>
    /// Represents a mechanism command object to handle AES with OFB mode
    /// </summary>
    public class AESOFBEncryptMechanismCommand : AESEncryptMechanismCommand
    {
        public override Pkcs11Mechanism MechanismType => Pkcs11Mechanism.AES_OFB;

        protected override CipherMode CipherMode => CipherMode.OFB;


        public override void InitialiseContext(IContext contextObject, IMechanismOptions initialisationData, out ExecutionResultCode resultCode)
        {
            base.InitialiseContext(contextObject, initialisationData, out resultCode);

            if (resultCode != ExecutionResultCode.OK) return;

            EncryptionContext encryptionContext = contextObject as EncryptionContext;
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
