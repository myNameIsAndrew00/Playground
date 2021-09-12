using Service.Core.Infrastructure.Communication.Structures;
using Service.Core.Infrastructure.Storage.Structures;
using Service.Core.Infrastructure.Token.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token
{
    /// <summary>
    /// Implements method which must be implemented by classes used to handle encryption
    /// </summary>
    public interface IEncryptionModule : ITokenModule
    {
        /// <summary>
        /// Use this method to initialise the handler context with certain attributes and mechanism
        /// </summary>
        /// <param name="mechanism">Mechanism specified for encryption</param>
        /// <param name="executionResultCode">Result code. Ok code is returned if handler was initialised with success</param>
        /// <returns>An object representing the key which can be used to do encryption</returns>
        EncryptionContext Initialise(DataContainer<Pkcs11Mechanism> mechanism, out ExecutionResultCode executionResultCode);

        /// <summary>
        /// Use this method to encrypt data
        /// </summary>
        /// <param name="plainData">Data which must be encrypted</param>
        /// <param name="executionResultCode">Result code. Ok code is returned if encryption was done with success</param>
        /// <returns>A boolean which is true if data was encrypted with success</returns>
        bool Encrypt(byte[] plainData, out ExecutionResultCode executionResultCode);

        /// <summary>
        /// Use this method to finalise encryption. This method can return encrypted data for some mechanisms.
        /// </summary>
        /// <param name="executionResultCode">Result code. Ok code is returned if encryption finalise was done with success</param>
        /// <returns>A boolean which is true if encryption was finalised with success</returns>
        bool EncryptFinalise(out ExecutionResultCode executionResultCode);
    }
}
