using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.DefinedTypes;
using Service.Core.Infrastructure.Communication.Structures;
using Service.Core.Infrastructure.Storage.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Token.Encryption
{
    /// <summary>
    /// Represents a base class for aes mechanism command objects
    /// </summary>
    internal abstract class AESMechanismCommand : IMechanismCommand
    {
        public abstract Pkcs11Mechanism MechanismType { get; }

        public abstract byte[] Execute(Pkcs11ContextObject contextObject, byte[] data, out ExecutionResultCode resultCode);

        public abstract void InitialiseContext(Pkcs11ContextObject contextObject, byte[] initialisationBytes, out ExecutionResultCode resultCode);
    }
}
