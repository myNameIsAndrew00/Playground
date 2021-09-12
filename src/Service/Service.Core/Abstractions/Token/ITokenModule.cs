using Service.Core.Infrastructure.Storage.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token
{
    /// <summary>
    /// Implements methods required by a token module
    /// </summary>
    public interface ITokenModule
    {
        /// <summary>
        /// Represent the context used by this module
        /// </summary>
        Pkcs11ContextObject Context { get; }
    }
}
