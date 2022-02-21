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

        /// <summary>
        /// Generate a pair of private/public keys using mechanism specified.
        /// </summary>
        /// <param name="publickKeyAttributes">Attributes which will be used to generate public key.</param>
        /// <param name="privateKeyAttributes">Attributes which will be used to generate private key.</param>
        /// <param name="mechanism">Mechanism which specifies to algorithm used to generate.</param>
        /// <param name="createdPublicKey">Public key object.</param>
        /// <param name="createdPrivateKey">Private key object.</param>
        /// <param name="code">Execution result code. Ok code is returned if keypair was created with success.</param>
        /// <returns>A boolean which is true if keypair was created with success, false otherwise</returns>
        bool CreateKeys(
            IEnumerable<IDataContainer<Pkcs11Attribute>> publickKeyAttributes,
            IEnumerable<IDataContainer<Pkcs11Attribute>> privateKeyAttributes,
            IMechanismOptions mechanism,
            out IMemoryObject createdPublicKey,
            out IMemoryObject createdPrivateKey,
            out ExecutionResultCode code);
    }
}
