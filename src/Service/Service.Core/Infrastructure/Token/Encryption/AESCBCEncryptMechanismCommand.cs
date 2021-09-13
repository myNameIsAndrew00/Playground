using Service.Core.Abstractions.Token.DefinedTypes;
using Service.Core.Infrastructure.Communication.Structures;
using Service.Core.Infrastructure.Storage.Structures;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Service.Core.Infrastructure.Token.Encryption
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
