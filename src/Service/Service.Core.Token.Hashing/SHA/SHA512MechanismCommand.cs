﻿using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Token.Hashing.SHA
{
    public class SHA512MechanismCommand : SHAMechanismCommand
    {
        public override Pkcs11Mechanism MechanismType => Pkcs11Mechanism.SHA512;

        protected override HashAlgorithmName HashAlgorithmName => HashAlgorithmName.SHA512;
    }
}
