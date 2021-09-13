using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Hashing;
using Service.Core.Infrastructure.Storage.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Token
{
    /// <summary>
    /// Default implementation for hashing handler interface
    /// </summary>
    internal class HashingModule : IHashingModule
    {
        public Pkcs11ContextObject Context => throw new NotImplementedException();
    }
}
