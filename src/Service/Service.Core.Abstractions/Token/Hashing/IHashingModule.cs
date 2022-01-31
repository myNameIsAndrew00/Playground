using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token.Hashing
{  
    /// <summary>
   /// Implements method which must be implemented by classes used to handle signing.
   /// </summary>
    public interface IHashingModule : ITokenModule 
    {
        /// <summary>
        /// Use this method to initialise the handler context with certain attributes and mechanism.
        /// </summary>
        /// <param name="mechanism">Mechanism specified for encryption</param>
        /// <param name="executionResultCode">Result code. Ok code is returned if handler was initialised with success</param>
        /// <returns>An object representing the context which can be used to do hashing</returns>
        IDigestContext Initialise<MechanismContainer>(MechanismContainer mechanism, out ExecutionResultCode executionResultCode)
            where MechanismContainer : IMechanismOptions;

        /// <summary>
        /// Use this method to hash data.
        /// </summary>
        /// <param name="hashingData">Data which must be hashed</param>
        /// <param name="isPartOperation">A boolean which specify if the operation will be succeed by further calls</param>
        /// <param name="executionResultCode">Result code. Ok code is returned if decryption was done with success</param>
        /// <returns>Result of the hashing process</returns>
        byte[] Hash(byte[] hashingData, bool isPartOperation, out ExecutionResultCode executionResultCode);

        /// <summary>
        /// Use this method to finalise hashing. This method will return the final digest.
        /// </summary>
        /// <param name="executionResultCode">Result code. Ok code is returned if decryption finalise was done with success.</param>
        /// <returns>Result of the hashing process</returns>
        byte[] HashFinalise(out ExecutionResultCode executionResultCode);
    }
}
