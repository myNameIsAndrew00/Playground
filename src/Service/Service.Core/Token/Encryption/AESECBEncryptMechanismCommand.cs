﻿using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Service.Core.Token.Encryption
{
    /// <summary>
    /// Represents a mechanism command object to handle AES with ECB mode
    /// </summary>
    internal class AESECBEncryptMechanismCommand : AESEncryptMechanismCommand
    {
        public override Pkcs11Mechanism MechanismType => Pkcs11Mechanism.AES_ECB;

        protected override CipherMode CipherMode => CipherMode.ECB;

      
    }
}