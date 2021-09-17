using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Service.Core.Token.Encryption
{
    /// <summary>
    /// Represents a mechanism command object to handle AES with ECB mode
    /// </summary>
    public class AESECBEncryptMechanismCommand : AESEncryptMechanismCommand
    {
        public override Pkcs11Mechanism MechanismType => Pkcs11Mechanism.AES_ECB;

        protected override CipherMode CipherMode => CipherMode.ECB;


        public override void InitialiseContext(IMemoryObject contextObject, IMechanismOptions initialisationData, out ExecutionResultCode resultCode)
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
