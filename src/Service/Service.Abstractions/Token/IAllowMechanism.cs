using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token
{
    /// <summary>
    /// This interface should be extended by classes which use mechanisms in their implementation
    /// </summary>
    public interface IAllowMechanism 
    {
        /// <summary>
        /// Retrieve a list of allowed mechanisms
        /// </summary>
        /// <returns>A list of mechanisms allowed by this instance</returns>
        IEnumerable<Pkcs11Mechanism> GetMechanisms();

        /// <summary>
        /// Add or update a mechanism command object to this mechanism container
        /// </summary>
        /// <param name="mechanismCommand"></param>
        /// <returns>An updated version of this instance</returns>
        IAllowMechanism SetMechanism(IMechanismCommand mechanismCommand);
    }
}
