using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Service.Core.Token.Encryption
{
    /// <summary>
    /// Represents a mechanism command object to handle AES with OFB mode
    /// </summary>
    internal class AESOFBEncryptMechanismCommand : AESEncryptMechanismCommand
    {
        public override Pkcs11Mechanism MechanismType => Pkcs11Mechanism.AES_OFB;

        protected override CipherMode CipherMode => CipherMode.OFB;
    }
}
