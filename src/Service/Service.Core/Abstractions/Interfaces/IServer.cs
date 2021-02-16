using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Interfaces
{
    /// <summary>
    /// Expose methods which describe a service behavior
    /// </summary> 
    public interface IServer
    {
        /// <summary>
        /// Enable server instance to wait for listening client requests
        /// </summary>
        void Start();
    }
}
