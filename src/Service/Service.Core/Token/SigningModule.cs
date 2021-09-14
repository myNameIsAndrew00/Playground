using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Signing;
using Service.Core.Storage.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Token
{
    /// <summary>
    /// Default implementation for signing handler interface
    /// </summary>
    internal class SigningModule : ISigningModule
    {
        public IMemoryObject Context => throw new NotImplementedException();
    }
}
