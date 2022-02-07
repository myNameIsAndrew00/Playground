using Service.Core.Abstractions.Logging;
using Service.Core.Abstractions.Token;
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
    public interface ITokenStorage : IDataContainerBuilder, IMechanismOptionsBuilder, IAllowLogging
    {
        /// <summary>
        /// Use this method to create an in memory object using the given attributes.
        /// </summary>
        /// <param name="attributes">Attributes used for object creation</param>
        /// <param name="createdObject">Object created</param>
        /// <param name="code">Result code. Ok code is returned if object was created with success</param>
        /// <returns>A boolean which is true if object was created with success, false otherwise</returns>
        bool CreateInMemoryObject(IEnumerable<IDataContainer<Pkcs11Attribute>> attributes, out IMemoryObject createdObject, out ExecutionResultCode code);


        bool CreateTokenObject(IEnumerable<IDataContainer<Pkcs11Attribute>> attributes, out ITokenObject tokenObject, out ExecutionResultCode code);
    }
}
