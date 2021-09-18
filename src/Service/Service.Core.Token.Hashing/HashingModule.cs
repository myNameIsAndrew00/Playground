
using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token.Hashing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Token.Hashing
{
    /// <summary>
    /// Default implementation for hashing handler interface
    /// </summary>
    public class HashingModule : IHashingModule
    {
        public IContext Context => throw new NotImplementedException();
    }
}
