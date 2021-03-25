using Service.Core.Abstractions.Communication.Structures;
using Service.Core.Abstractions.Token.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token.Interfaces
{
    /// <summary>
    /// Implements method which must be implemented by classes used to handle encryption
    /// </summary>
    public interface IEncryptionHandler
    {
        /// <summary>
        /// Use this method to initialise the handler with certain attributes and mechanism
        /// </summary>
        /// <param name="attributes">Attributes used to do encrypion</param>
        /// <param name="mechanism">Mechanism specified for encryption</param>
        /// <param name="executionResultCode">Result code. Ok code is returned if handler was initialised with success</param>
        /// <returns>A boolean which is true if initialisation was done with success, false otherwise</returns>
        bool Initialise(IEnumerable<Pkcs11DataContainer<Pkcs11Attribute>> attributes, Pkcs11DataContainer<Pkcs11Mechanism> mechanism, out ExecutionResultCode executionResultCode);

        /// <summary>
        /// Use this method to encrypt data
        /// </summary>
        /// <param name="plainData">Data which must be encrypted</param>
        /// <param name="encryptedData">Data which was encrypted</param>
        /// <param name="executionResultCode">Result code. Ok code is returned if encryption was done with success</param>
        /// <returns>A boolean which is true if data was encrypted with success</returns>
        bool Encrypt(byte[] plainData, out byte[] encryptedData, out ExecutionResultCode executionResultCode);

        /// <summary>
        /// Use this method to finalise encryption. This method can return encrypted data for some mechanisms.
        /// </summary>
        /// <param name="encryptedData">Data which was encrypted</param>
        /// <param name="executionResultCode">Result code. Ok code is returned if encryption finalise was done with success</param>
        /// <returns>A boolean which is true if encryption was finalised with success</returns>
        bool EncryptFinalise(out byte[] encryptedData, out ExecutionResultCode executionResultCode);
    }
}
