using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Interfaces
{
    /// <summary>
    /// Expose methods which describe a PKCS 11 service behavior
    /// </summary> 
    public interface IPkcs11Server
    {
        /// <summary>
        /// Enable server instance to wait for listening client requests
        /// </summary>
        void Start();
    }
}
