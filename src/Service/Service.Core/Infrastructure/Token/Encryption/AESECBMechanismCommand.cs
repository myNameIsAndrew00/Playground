using Service.Core.Abstractions.Token.DefinedTypes;
using Service.Core.Infrastructure.Communication.Structures;
using Service.Core.Infrastructure.Storage.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Token.Encryption
{
    /// <summary>
    /// Represents a mechanism command object to handle AES with ECB mode
    /// </summary>
    internal class AESECBMechanismCommand : AESMechanismCommand
    {
        public override Pkcs11Mechanism MechanismType => Pkcs11Mechanism.AES_ECB;

        public override byte[] Execute(Pkcs11ContextObject contextObject, byte[] data, out ExecutionResultCode resultCode)
        {
            throw new NotImplementedException();
        }

        public override void InitialiseContext(Pkcs11ContextObject contextObject, byte[] initialisationBytes, out ExecutionResultCode resultCode)
        {
            throw new NotImplementedException();
        }
    }
}
