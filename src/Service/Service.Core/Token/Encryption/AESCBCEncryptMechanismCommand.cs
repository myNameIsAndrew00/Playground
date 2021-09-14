using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Service.Core.Token.Encryption
{
    /// <summary>
    /// Represents a mechanism command object to handle AES with CBC mode
    /// </summary>
    internal class AESCBCEncryptMechanismCommand : AESEncryptMechanismCommand
    {
        public override Pkcs11Mechanism MechanismType => Pkcs11Mechanism.AES_CBC;

        protected override CipherMode CipherMode => CipherMode.CBC;

      
    }
}
