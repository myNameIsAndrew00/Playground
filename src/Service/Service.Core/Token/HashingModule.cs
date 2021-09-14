using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Hashing;
using Service.Core.Storage.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Token
{
    /// <summary>
    /// Default implementation for hashing handler interface
    /// </summary>
    internal class HashingModule : IHashingModule
    {
        public IMemoryObject Context => throw new NotImplementedException();
    }
}
