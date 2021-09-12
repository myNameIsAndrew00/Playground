using Service.Core.Abstractions.Token;
using Service.Core.Infrastructure.Storage.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Token
{
    /// <summary>
    /// Default implementation for signing handler interface
    /// </summary>
    internal class SigningModule : ISigningModule
    {
        public Pkcs11ContextObject Context => throw new NotImplementedException();
    }
}
