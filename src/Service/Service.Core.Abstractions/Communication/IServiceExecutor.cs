using Service.Core.Abstractions.Execution;
using Service.Core.Abstractions.Logging;
using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Communication
{
    /// <summary>
    /// This interface provide methods to execute service actions.
    /// An executor instance must handle and parse the parameters received after the request dispatch.
    /// 
    /// Optionaly, other methods which have names of values contained by ServiceActionCode enum, can be implemented.
    /// If a optional method is called, but it is not implemented by executor, service will return not implemented error code.
    /// </summary>
    public interface IServiceExecutor<DispatchResultType, SessionType> : IAllowLogging
        where DispatchResultType : IDispatchResult<SessionType>
        where SessionType : ISession
    {
        /// <summary>
        /// Set the dispatch result for executor. A dispatch result can be consider as the main context of the executor.
        /// </summary>
        /// <param name="dispatchResult">Value which will be set</param>
        void SetDispatcherResult(DispatchResultType dispatchResult);

        /// <summary>
        /// Set the module collection used by this executor in request handling.
        /// </summary>
        /// <param name="moduleCollection"></param>
        void SetModuleFactory(IModuleFactory moduleCollection);

        /// <summary>
        /// Set the executor storage which can be used to handle token internal storage.
        /// </summary>
        /// <param name="storage"></param>
        void SetStorage(ITokenStorage storage);

        /// <summary>
        /// Returns a empty session result
        /// </summary>
        /// <returns></returns>
        IExecutionResult GetEmptySessionResult(ExecutionResultCode code);

        /// <summary>
        /// Represents an object which may be used by server to parse parameters to ServiceExecutor invoked method.
        /// </summary>
        IServiceExecutorModelBinder<DispatchResultType, SessionType> ModelBinder { get; }

        #region Service actions

        /* This region of code contains methods implemented by executor for handling crypto actions.
        * Name of the methods are taken from ServiceActionCode enum */

        /// <summary>
        /// Begin session. No parameters required
        /// </summary>
        /// <returns>Bytes representing data. Payload represents 8 bytes for handler id</returns>
        IExecutionResult BeginSession();

        /// <summary>
        /// End session. No parameters required
        /// </summary>
        /// <returns></returns>
        IExecutionResult EndSession();

        /// <summary>
        /// Authenticate a session. Require a tlv structure representing the user type and password value.
        /// </summary>
        /// <returns></returns>
        IExecutionResult Authenticate(IDataContainer<Pkcs11UserType> authenticationData);

        /// <summary>
        /// Create a object.
        /// </summary>
        /// <returns>Bytes representing data. Payload represents 8 bytes for handler id</returns>
        IExecutionResult CreateObject(IEnumerable<IDataContainer<Pkcs11Attribute>> attributes);

        /// <summary>
        /// Function associated with encrypt init.
        /// </summary>
        /// <returns></returns>
        IExecutionResult EncryptInit(ulong keyIdentifier, IDataContainer<Pkcs11Mechanism> mechanism);

        /// <summary>
        /// Function associated with encrypt.
        /// </summary>
        /// <param name="dataToEncrypt">A data container which encapsulates data to be encrypted</param>
        /// <param name="lengthRequest">A bool which specify if request is made to receive the length of encrypted data</param>
        /// <returns></returns>
        IExecutionResult Encrypt(bool lengthRequest, IDataContainer dataToEncrypt);

        /// <summary>
        /// Function associated with PKCS11 encrypt update.   
        /// </summary>
        /// <param name="dataToEncrypt">A data container which encapsulates data to be encrypted</param>
        /// <param name="lengthRequest">A bool which specify if request is made to receive the length of encrypted data</param>
        /// <returns></returns>
        IExecutionResult EncryptUpdate(bool lengthRequest, IDataContainer dataToEncrypt);

        /// <summary>
        /// Function associated with PKCS11 encrypt final.
        /// </summary>
        /// <param name="lengthRequest">A bool which specify if request is made to receive the length of encrypted data</param> 
        /// <returns></returns>
        IExecutionResult EncryptFinal(bool lengthRequest);

        /// <summary>
        /// Function associated with decrypt.
        /// </summary>
        /// <param name="keyIdentifier"></param>
        /// <param name="mechanism"></param>
        /// <returns></returns>
        IExecutionResult DecryptInit(ulong keyIdentifier, IDataContainer<Pkcs11Mechanism> mechanism);

        /// <summary>
        /// Function associated with decrypt.
        /// </summary>
        /// <param name="lengthRequest"></param>
        /// <param name="dataToDecrypt"></param>
        /// <returns></returns>
        IExecutionResult Decrypt(bool lengthRequest, IDataContainer dataToDecrypt);

        /// <summary>
        /// Function associated with PKCS11 decrypt update.
        /// </summary>
        /// <param name="lengthRequest"></param>
        /// <param name="dataToDecrypt"></param>
        /// <returns></returns>
        IExecutionResult DecryptUpdate(bool lengthRequest, IDataContainer dataToDecrypt);

        /// <summary>
        /// Function associated with PKCS11 decrypt final.
        /// </summary>
        /// <param name="lengthRequest"></param>
        /// <returns></returns>
        IExecutionResult DecryptFinal(bool lengthRequest);

        /// <summary>
        /// Function associated with PKCS11 digest init.
        /// </summary>
        /// <param name="mechanism"></param>
        /// <returns></returns>
        IExecutionResult DigestInit(IDataContainer<Pkcs11Mechanism> mechanism);

        /// <summary>
        /// Function associated with PKCS11 digest.
        /// </summary>
        /// <param name="lengthRequest"></param>
        /// <param name="dataToDigest"></param>
        /// <returns></returns>
        IExecutionResult Digest(bool lengthRequest, IDataContainer dataToDigest);

        /// <summary>
        /// Function associated with PKCS11 digest update.
        /// </summary>
        /// <param name="dataToDigest"></param>
        /// <returns></returns>
        IExecutionResult DigestUpdate(IDataContainer dataToDigest);

        /// <summary>
        /// Function associated with PKCS11 digest final.
        /// </summary>
        /// <param name="lengthRequest"></param>
        /// <returns></returns>
        IExecutionResult DigestFinal(bool lengthRequest);

        /// <summary>
        /// Function associated with PKCS11 generate key pair.
        /// </summary>
        /// <param name="mechanism"></param>
        /// <param name="publicKeyAttributes"></param>
        /// <param name="privateKeyAttributes"></param>
        /// <returns></returns>
        IExecutionResult GenerateKeyPair(IDataContainer<Pkcs11Mechanism> mechanism, IEnumerable<IDataContainer<Pkcs11Attribute>> publicKeyAttributes, IEnumerable<IDataContainer<Pkcs11Attribute>> privateKeyAttributes);

        /// <summary>
        /// Function associated with PKCS11 sign init.
        /// </summary>
        /// <param name="publicKeyIdentifier">Identifier used for publick key</param>
        /// <param name="mechanism"></param>
        /// <returns></returns>
        IExecutionResult SignInit(ulong publicKeyIdentifier, IDataContainer<Pkcs11Mechanism> mechanism);

        /// <summary>
        /// Function associated with PKCS11 sign.
        /// </summary>
        /// <param name="lengthRequest"></param>
        /// <param name="dataToSign"></param>
        /// <returns></returns>
        IExecutionResult Sign(bool lengthRequest, IDataContainer dataToSign);

        /// <summary>
        /// Function associated with PKCS11 sign update.
        /// </summary>
        /// <param name="dataToSign"></param>
        /// <returns></returns>
        IExecutionResult SignUpdate(IDataContainer dataToSign);

        /// <summary>
        /// Function associated with PKCS11 sign final.
        /// </summary>
        /// <param name="lengthRequest"></param>
        /// <returns></returns>
        IExecutionResult SignFinal(bool lengthRequest);

        /// <summary>
        /// Function associated with PKCS11 verify init.
        /// </summary>
        /// <param name="privateKeyIdentifier">Identifier used for publick key</param>
        /// <param name="mechanism"></param>
        /// <returns></returns>
        IExecutionResult VerifyInit(ulong privateKeyIdentifier, IDataContainer<Pkcs11Mechanism> mechanism);

        /// <summary>
        /// Function associated with PKCS11 verify.
        /// </summary>
        /// <param name="lengthRequest"></param>
        /// <param name="dataToVerify"></param>
        /// <returns></returns>
        IExecutionResult Verify(bool lengthRequest, IDataContainer dataToVerify);

        /// <summary>
        /// Function associated with PKCS11 verify update.
        /// </summary>
        /// <param name="dataToVerify"></param>
        /// <returns></returns>
        IExecutionResult VerifyUpdate(IDataContainer dataToVerify);

        /// <summary>
        /// Function associated with PKCS11 verify final.
        /// </summary>
        /// <param name="lengthRequest"></param>
        /// <returns></returns>
        IExecutionResult VerifyFinal(bool lengthRequest);

        #endregion

    }
}
