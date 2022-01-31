using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Token.Hashing.SHA
{
    public class SHA384MechanismCommand : SHAMechanismCommand
    {
        public override Pkcs11Mechanism MechanismType => Pkcs11Mechanism.SHA384;

        protected override HashAlgorithmName HashAlgorithmName => HashAlgorithmName.SHA384;
    }
}
