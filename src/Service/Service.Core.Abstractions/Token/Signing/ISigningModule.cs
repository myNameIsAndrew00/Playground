using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token.Signing
{
    /// <summary>
    /// Implements method which must be implemented by classes used to handle signing
    /// </summary>
    public interface ISigningModule : ITokenModule, IAllowMechanism
    {
        /// <summary>
        /// Use this method to initialise the handler context with certain attributes and mechanism.
        /// </summary>
        /// <param name="mechanism">Mechanism specified for signing</param>
        /// <param name="executionResultCode">Result code. Ok code is returned if handler was initialised with success</param>
        /// <returns>An object representing the context which can be used to do signing</returns>
        ISigningContext Initialise<MechanismContainer>(IMemoryObject privateKey, MechanismContainer mechanism, out ExecutionResultCode executionResultCode)
            where MechanismContainer : IMechanismOptions;

        /// <summary>
        /// Use this method to sign data.
        /// </summary>
        /// <param name="signingData">Data which must be signed</param>
        /// <param name="isPartOperation">A boolean which specify if the operation will be succeed by further calls</param>
        /// <param name="executionResultCode">Result code. Ok code is returned if signing was done with success</param>
        /// <returns>Result of the signing process</returns>
        byte[] Sign(byte[] signingData, bool isPartOperation, out ExecutionResultCode executionResultCode);

        /// <summary>
        /// Use this method to finalise signing. This method will return the final data signed.
        /// </summary>
        /// <param name="executionResultCode">Result code. Ok code is returned if signing was done with success.</param>
        /// <returns>Result of the signing process</returns>
        byte[] SignFinalise(out ExecutionResultCode executionResultCode);
    }
}
