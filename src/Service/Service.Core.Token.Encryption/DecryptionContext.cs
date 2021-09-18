using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Token.Encryption
{
    /// <summary>
    /// Represents an context object to handle decryption functionality
    /// </summary>
    internal class DecryptionContext : KeyContext
    {
        internal DecryptionContext(IMechanismCommand mechanismCommand, IMemoryObject memoryObject) : base(mechanismCommand, memoryObject)
        {
        }

    }
}
