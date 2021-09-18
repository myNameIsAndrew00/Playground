using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Token.Encryption
{
    /// <summary>
    /// Implements method which must be implemented by classes used to handle decryption
    /// </summary>
    public interface IDecryptionModule : ITokenModule, IAllowMechanism
    {
        /// <summary>
        /// Use this method to initialise the handler context with certain attributes and mechanism
        /// </summary>
        /// <param name="contextData">Base object which is used to create the context</param>
        /// <param name="mechanism">Mechanism specified for decryption</param>
        /// <param name="executionResultCode">Result code. Ok code is returned if handler was initialised with success</param>
        /// <returns>An object representing the context which can be used to do decryption</returns>
        IKeyContext Initialise<MechanismContainer>(IMemoryObject contextData, MechanismContainer mechanism, out ExecutionResultCode executionResultCode)
            where MechanismContainer : IMechanismOptions;

        /// <summary>
        /// Use this method to decrypt data
        /// </summary>
        /// <param name="encryptedData">Data which must be decrypted</param>
        /// <param name="isPartOperation">A boolean which specify if the operation will be succeed by further calls</param>
        /// <param name="executionResultCode">Result code. Ok code is returned if decryption was done with success</param>
        /// <returns>Result of the decryption process</returns>
        byte[] Decrypt(byte[] encryptedData, bool isPartOperation, out ExecutionResultCode executionResultCode);

        /// <summary>
        /// Use this method to finalise deryption. This method can return encrypted data for some mechanisms.
        /// </summary>
        /// <param name="executionResultCode">Result code. Ok code is returned if decryption finalise was done with success</param>
        /// <returns>Result of the decryption process</returns>
        byte[] DecryptFinalise(out ExecutionResultCode executionResultCode);
    }
}
