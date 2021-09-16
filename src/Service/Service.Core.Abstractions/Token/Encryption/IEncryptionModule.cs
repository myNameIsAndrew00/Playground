using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token.Encryption
{
    /// <summary>
    /// Implements method which must be implemented by classes used to handle encryption
    /// </summary>
    public interface IEncryptionModule : ITokenModule, IAllowMechanism 
    {
        /// <summary>
        /// Use this method to initialise the handler context with certain attributes and mechanism
        /// </summary>
        /// <param name="mechanism">Mechanism specified for encryption</param>
        /// <param name="executionResultCode">Result code. Ok code is returned if handler was initialised with success</param>
        /// <returns>An object representing the context which can be used to do encryption</returns>
        IMemoryObject Initialise<MechanismContainer>(MechanismContainer mechanism, out ExecutionResultCode executionResultCode)
            where MechanismContainer : IDataContainer<Pkcs11Mechanism>;

        /// <summary>
        /// Use this method to encrypt data
        /// </summary>
        /// <param name="plainData">Data which must be encrypted</param>
        /// <param name="isPartOperation">A boolean which specify if the operation will be succeed by further calls</param>
        /// <param name="executionResultCode">Result code. Ok code is returned if encryption was done with success</param>
        /// <returns>Result of the encryption process</returns>
        byte[] Encrypt(byte[] plainData, bool isPartOperation, out ExecutionResultCode executionResultCode);

        /// <summary>
        /// Use this method to finalise encryption. This method can return encrypted data for some mechanisms.
        /// </summary>
        /// <param name="executionResultCode">Result code. Ok code is returned if encryption finalise was done with success</param>
        /// <returns>Result of the encryption process</returns>
        byte[] EncryptFinalise(out ExecutionResultCode executionResultCode);


 
    }
}
