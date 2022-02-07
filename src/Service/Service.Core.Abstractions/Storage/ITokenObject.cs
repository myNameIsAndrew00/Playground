using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Storage
{
    /// <summary>
    /// Represents and model for token objects. Implements a secure environment for storing memory objects into hard disk.
    /// </summary>
    public interface ITokenObject : IMemoryObject
    {
    }
}
