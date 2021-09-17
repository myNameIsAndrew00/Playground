using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Storage
{
    /// <summary>
    /// Represents a class which handles in memory or persistent storage operations
    /// </summary>
    public interface ITokenStorage : IDataContainerBuilder, IMechanismDataContainerBuilder
    {
        /// <summary>
        /// Use this method to create a pkcs11 object
        /// </summary>
        /// <param name="attributes">Attributes used for object creation</param>
        /// <param name="createdObject">Object created</param>
        /// <param name="code">Result code. Ok code is returned if object was created with success</param>
        /// <returns>A boolean which is true if object was created with success, false otherwise</returns>
        bool CreateInMemoryObject(IEnumerable<IDataContainer<Pkcs11Attribute>> attributes, out IMemoryObject createdObject, out ExecutionResultCode code);
    }
}
