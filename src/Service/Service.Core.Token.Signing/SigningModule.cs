using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token.Signing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Token.Signing
{
    /// <summary>
    /// Default implementation for signing handler interface
    /// </summary>
    public class SigningModule : ISigningModule
    {
        public IContext Context => throw new NotImplementedException();
    }
}
