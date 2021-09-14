using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Service.Core.Token.Encryption
{
    /// <summary>
    /// Represents a mechanism command object to handle AES with CFB mode
    /// </summary>
    internal class AESCFBEncryptMechanismCommand : AESEncryptMechanismCommand
    {
        public override Pkcs11Mechanism MechanismType => Pkcs11Mechanism.AES_CFB1;

        protected override CipherMode CipherMode => CipherMode.CFB;

    }
}
