using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token.Signing
{
    /// <summary>
    /// Implements method which must be implemented by classes used to handle signing validation.
    /// </summary>
    public interface IVerifyModule : ITokenModule, IAllowMechanism
    {
        /// <summary>
        /// Use this method to initialise the handler context with certain attributes and mechanism.
        /// </summary>
        /// <param name="mechanism">Mechanism specified for signing</param>
        /// <param name="executionResultCode">Result code. Ok code is returned if handler was initialised with success</param>
        /// <returns>An object representing the context which can be used to do verifying</returns>
        ISigningContext Initialise<MechanismContainer>(IMemoryObject publicKey, MechanismContainer mechanism, out ExecutionResultCode executionResultCode)
            where MechanismContainer : IMechanismOptions;

        /// <summary>
        /// Use this method to verify data.
        /// </summary>
        /// <param name="verifyingData">Data which must be verified</param>
        /// <param name="isPartOperation">A boolean which specify if the operation will be succeed by further calls</param>
        /// <param name="executionResultCode">Result code. Ok code is returned if verifying was done with success</param>
        /// <returns>Result of the verifying process</returns>
        byte[] Verify(byte[] verifyingData, bool isPartOperation, out ExecutionResultCode executionResultCode);

        /// <summary>
        /// Use this method to finalise verifying. This method will return the final data verified.
        /// </summary>
        /// <param name="executionResultCode">Result code. Ok code is returned if verifying was done with success.</param>
        /// <returns>Result of the verifying process</returns>
        byte[] VerifyFinalise(out ExecutionResultCode executionResultCode);
    }
}
