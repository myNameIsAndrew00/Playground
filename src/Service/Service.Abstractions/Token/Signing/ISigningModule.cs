using Service.Core.Abstractions.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token.Signing
{
    /// <summary>
    /// Implements method which must be implemented by classes used to handle signing
    /// </summary>
    public interface ISigningModule : ITokenModule 
    {
    }
}
