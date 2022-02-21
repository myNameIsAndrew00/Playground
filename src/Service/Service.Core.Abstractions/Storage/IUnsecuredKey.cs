using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Storage
{
    /// <summary>
    /// Represents an unsecured key (private or public) in memory.
    /// </summary>
    public interface IUnsecuredKey : IUnsecuredMemoryObject
    {
        /// <summary>
        /// Represents raw bytes of key. Format is module defined.
        /// </summary>
        public byte[] Raw { get; }
    }
}
