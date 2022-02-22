using Service.Core.Abstractions.Communication;
using Service.Core.Abstractions.Execution;
using Service.Core.Abstractions.Logging;
using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.Abstractions.Token.Hashing;
using Service.Core.Abstractions.Token.Signing;
using Service.Core.Communication.Infrastructure;
using Service.Core.DefinedTypes;
using Service.Core.Execution;
using Service.Core.Storage;
using Service.Core.Storage.Memory;
using Service.Core.Token.Encryption;
using Service.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using static Service.Core.Abstractions.Logging.IAllowLogging;

namespace Service.Core.Client
{
    /// <summary>
    /// Main executor used by service.
    /// Methods for commands execution are implemented on this class. Payload format for methods are different foreach method.
    /// Payload parameters are raw bytes, simple types are passed without any encoding (just consecutive bytes), composed types use TLV encoding
    /// </summary>
    public class TlvServiceExecutor : IServiceExecutor<DispatchResult, Session>
    {
        private IDispatchResult<Session> dispatchResult;

        private IModuleFactory moduleCollection;

        private ITokenStorage tokenStorage;

        private List<LogData> logs;

        public TlvServiceExecutor()
        {
            logs = new List<LogData>();
        }




        public IServiceExecutorModelBinder<DispatchResult, Session> ModelBinder => new TlvServiceExecutorModelBinder();

        public LogSection LogSection => LogSection.COMMUNICATION_EXECUTOR;

        public IReadOnlyCollection<LogData> Logs => logs;

        public void ClearLogs() => logs.Clear();


        public void SetDispatcherResult(DispatchResult dispatchResult) => this.dispatchResult = dispatchResult;

        public void SetModuleFactory(IModuleFactory moduleCollection) => this.moduleCollection = moduleCollection;

        public void SetStorage(ITokenStorage storage) => this.tokenStorage = storage;


        public IExecutionResult GetEmptySessionResult(ExecutionResultCode code)
        {
            return new BytesResult(code);
        }

        /// <summary>
        /// Begin session. No parameters required
        /// </summary>
        /// <returns>Bytes representing data. Payload represents 4 bytes for handler id</returns>
        public virtual IExecutionResult BeginSession()
        {
            //todo: better handling for codes
            return new BytesResult(dispatchResult.Session.Id.GetBytes(), ExecutionResultCode.OK);
        }


        /// <summary>
        /// End session. No parameters required
        /// </summary>
        /// <returns></returns>
        public virtual IExecutionResult EndSession()
        {
            //todo: better handling for codes
            return new BytesResult(ExecutionResultCode.OK);
        }

        /// <summary>
        /// Authenticate a session. Require a tlv structure representing the user type and password value
        /// </summary>
        /// <returns></returns>
        public virtual IExecutionResult Authenticate(IDataContainer<Pkcs11UserType> authenticationData)
        {

            bool authenticationResult =
                this.dispatchResult.Session.Authenticate(authenticationData.Type, ASCIIEncoding.UTF8.GetString(authenticationData.Value));

            //todo: better handling for codes
            return new BytesResult(ExecutionResultCode.OK);
        }

        /// <summary>
        /// Create a object. Require a tlv structure array representing the attributes used for creation
        /// </summary>
        /// <returns>Bytes representing data. Payload represents 4 bytes for handler id</returns>
        public virtual IExecutionResult CreateObject(IEnumerable<IDataContainer<Pkcs11Attribute>> attributes)
        {
            if (attributes == null) return new BytesResult(ExecutionResultCode.ARGUMENTS_BAD);

            // for now, only memory objects can be created and stored.
            if (!tokenStorage.CreateInMemoryObject(attributes, out IMemoryObject @object, out ExecutionResultCode creationResultCode))
            {
                return new BytesResult(creationResultCode);
            }

            ulong handlerId = this.dispatchResult.Session.AddSesionObject(@object);

            return new BytesResult(handlerId.GetBytes(), ExecutionResultCode.OK);
        }


        public virtual IExecutionResult EncryptInit(ulong keyIdentifier, IDataContainer<Pkcs11Mechanism> mechanism)
        {
            IMemoryObject keyHandler = this.dispatchResult.Session.GetSessionObject(keyIdentifier);

            if (keyHandler == null || mechanism == null) return new BytesResult(ExecutionResultCode.ARGUMENTS_BAD);

            IEncryptionModule encryptionHandler = moduleCollection.GetEncryptionModule(null);

            keyHandler = encryptionHandler.Initialise(
                contextData: keyHandler,
                mechanism: this.ModelBinder.CreateMechanismModel(mechanism, tokenStorage),
                out ExecutionResultCode executionResultCode);

            if (keyHandler is not null)
                this.dispatchResult.Session.UpdateAndRegisterSesionObject(keyIdentifier, keyHandler);

            return new BytesResult(executionResultCode);
        }


        public virtual IExecutionResult Encrypt(bool lengthRequest, IDataContainer dataToEncrypt)
        {
            IKeyContext context = this.dispatchResult.Session.RegisteredEncryptionContext;

            if (context is not null) context.LengthRequest = lengthRequest;

            IEncryptionModule encryptionHandler = moduleCollection.GetEncryptionModule(context);

            byte[] encryptedData = encryptionHandler.Encrypt(
                plainData: dataToEncrypt.Value,
                isPartOperation: false,
                out ExecutionResultCode executionResultCode);

            if (!lengthRequest)
                this.dispatchResult.Session.ResetRegisteredEncryptionContext();

            return new BytesResult(encryptedData, executionResultCode);
        }


        public virtual IExecutionResult EncryptUpdate(bool lengthRequest, IDataContainer dataToEncrypt)
        {
            IKeyContext context = this.dispatchResult.Session.RegisteredEncryptionContext;

            if (context is not null) context.LengthRequest = lengthRequest;

            IEncryptionModule encryptionHandler = moduleCollection.GetEncryptionModule(context);

            byte[] encryptedData = encryptionHandler.Encrypt(
                plainData: dataToEncrypt.Value,
                isPartOperation: true,
                out ExecutionResultCode executionResultCode);

            return new BytesResult(encryptedData, executionResultCode);
        }


        public virtual IExecutionResult EncryptFinal(bool lengthRequest)
        {
            IKeyContext context = this.dispatchResult.Session.RegisteredEncryptionContext;

            if (context is not null) context.LengthRequest = lengthRequest;

            IEncryptionModule encryptionHandler = moduleCollection.GetEncryptionModule(context);

            byte[] encryptedData = encryptionHandler.EncryptFinalise(out ExecutionResultCode executionResultCode);

            if (!lengthRequest)
                this.dispatchResult.Session.ResetRegisteredEncryptionContext();

            return new BytesResult(encryptedData, executionResultCode);
        }

        public IExecutionResult DecryptInit(ulong keyIdentifier, IDataContainer<Pkcs11Mechanism> mechanism)
        {
            IMemoryObject keyHandler = this.dispatchResult.Session.GetSessionObject(keyIdentifier);

            if (keyHandler == null || mechanism == null) return new BytesResult(ExecutionResultCode.ARGUMENTS_BAD);

            IDecryptionModule decryptionHandler = moduleCollection.GetDecryptionModule(null);

            keyHandler = decryptionHandler.Initialise(
                contextData: keyHandler,
                mechanism: this.ModelBinder.CreateMechanismModel(mechanism, tokenStorage),
                out ExecutionResultCode executionResultCode);

            if (keyHandler is not null)
                this.dispatchResult.Session.UpdateAndRegisterSesionObject(keyIdentifier, keyHandler);

            return new BytesResult(executionResultCode);
        }

        public IExecutionResult Decrypt(bool lengthRequest, IDataContainer dataToDecrypt)
        {
            IKeyContext context = this.dispatchResult.Session.RegisteredDecryptionContext;

            if (context is not null) context.LengthRequest = lengthRequest;

            IDecryptionModule decryptionHandler = moduleCollection.GetDecryptionModule(context);

            byte[] decryptedData = decryptionHandler.Decrypt(
                encryptedData: dataToDecrypt.Value,
                isPartOperation: false,
                out ExecutionResultCode executionResultCode);

            if (!lengthRequest)
                this.dispatchResult.Session.ResetRegisteredDecryptionContext();

            return new BytesResult(decryptedData, executionResultCode);
        }

        public IExecutionResult DecryptUpdate(bool lengthRequest, IDataContainer dataToDecrypt)
        {
            IKeyContext context = this.dispatchResult.Session.RegisteredDecryptionContext;

            if (context is not null) context.LengthRequest = lengthRequest;

            IDecryptionModule decryptionHandler = moduleCollection.GetDecryptionModule(context);

            byte[] decryptedData = decryptionHandler.Decrypt(
                encryptedData: dataToDecrypt.Value,
                isPartOperation: true,
                out ExecutionResultCode executionResultCode);

            return new BytesResult(decryptedData, executionResultCode);
        }

        public IExecutionResult DecryptFinal(bool lengthRequest)
        {
            IKeyContext context = this.dispatchResult.Session.RegisteredDecryptionContext;

            if (context is not null) context.LengthRequest = lengthRequest;

            IDecryptionModule decryptionHandler = moduleCollection.GetDecryptionModule(context);

            byte[] decryptedData = decryptionHandler.DecryptFinalise(out ExecutionResultCode executionResultCode);

            if (!lengthRequest)
                this.dispatchResult.Session.ResetRegisteredDecryptionContext();

            return new BytesResult(decryptedData, executionResultCode);
        }

        public IExecutionResult DigestInit(IDataContainer<Pkcs11Mechanism> mechanism)
        {
            IHashingModule hashingHandler = moduleCollection.GetHashingModule(null);

            var digestHandler = hashingHandler.Initialise(
                mechanism: this.ModelBinder.CreateMechanismModel(mechanism, tokenStorage),
                out ExecutionResultCode executionResultCode);

            if (digestHandler is not null)
                this.dispatchResult.Session.RegisterContext(digestHandler);

            return new BytesResult(executionResultCode);

        }

        public IExecutionResult Digest(bool lengthRequest, IDataContainer dataToDigest)
        {
            IDigestContext context = this.dispatchResult.Session.RegisteredDigestContext;

            if (context is not null) context.LengthRequest = lengthRequest;

            IHashingModule hashingHandler = moduleCollection.GetHashingModule(context);

            byte[] digest = hashingHandler.Hash(dataToDigest.Value, false, out ExecutionResultCode executionResultCode);

            if (!lengthRequest)
                this.dispatchResult.Session.ResetRegisteredDigestContext();

            return new BytesResult(digest, executionResultCode);
        }

        public IExecutionResult DigestUpdate(IDataContainer dataToDigest)
        {
            IDigestContext context = this.dispatchResult.Session.RegisteredDigestContext;

            IHashingModule hashingHandler = moduleCollection.GetHashingModule(context);

            hashingHandler.Hash(dataToDigest.Value, true, out ExecutionResultCode executionResultCode);

            return new BytesResult(executionResultCode);
        }

        public IExecutionResult DigestFinal(bool lengthRequest)
        {
            IDigestContext context = this.dispatchResult.Session.RegisteredDigestContext;

            IHashingModule hashingHandler = moduleCollection.GetHashingModule(context);

            byte[] digest = hashingHandler.HashFinalise(out ExecutionResultCode executionResultCode);

            if (!lengthRequest)
                this.dispatchResult.Session.ResetRegisteredDigestContext();

            return new BytesResult(digest, executionResultCode);
        }

        public IExecutionResult GenerateKeyPair(IDataContainer<Pkcs11Mechanism> mechanism, IEnumerable<IDataContainer<Pkcs11Attribute>> publicKeyAttributes, IEnumerable<IDataContainer<Pkcs11Attribute>> privateKeyAttributes)
        {
            if (publicKeyAttributes is null || privateKeyAttributes is null)
                return new BytesResult(ExecutionResultCode.ARGUMENTS_BAD);

            // create keys in memory
            if (!tokenStorage.CreateKeys(
                publicKeyAttributes,
                privateKeyAttributes,
                this.ModelBinder.CreateMechanismModel(mechanism, tokenStorage),
                out IMemoryObject publicKey,
                out IMemoryObject privateKey,
                out ExecutionResultCode resultCode
                ))
                return new BytesResult(resultCode);

            // for now, only register them in this session
            ulong privateKeyHandlerId = this.dispatchResult.Session.AddSesionObject(publicKey);
            ulong publicKeyHandlerId = this.dispatchResult.Session.AddSesionObject(privateKey);

            // return handlers for private and public key
            return new BytesResult(privateKeyHandlerId.GetBytes()
                           .Concat(publicKeyHandlerId.GetBytes()), ExecutionResultCode.OK);
        }

        public IExecutionResult SignInit(ulong privateKeyIdentifier, IDataContainer<Pkcs11Mechanism> mechanism)
        {
            IMemoryObject keyHandler = this.dispatchResult.Session.GetSessionObject(privateKeyIdentifier);

            if (keyHandler == null || mechanism == null) return new BytesResult(ExecutionResultCode.ARGUMENTS_BAD);

            ISigningModule signingModule = moduleCollection.GetSigningModule(null);

            keyHandler = signingModule.Initialise(
                privateKey: keyHandler,
                mechanism: this.ModelBinder.CreateMechanismModel(mechanism, tokenStorage),
                out ExecutionResultCode executionResultCode);

            if (keyHandler is not null)
                this.dispatchResult.Session.UpdateAndRegisterSesionObject(privateKeyIdentifier, keyHandler as ISigningContext);

            return new BytesResult(executionResultCode);
        }

        public IExecutionResult Sign(bool lengthRequest, IDataContainer dataToSign)
        {
            ISigningContext context = this.dispatchResult.Session.RegisteredSigningContext;

            if (context is not null) context.LengthRequest = lengthRequest;

            ISigningModule signingModule = moduleCollection.GetSigningModule(context);

            byte[] signedData = signingModule.Sign(
                signingData: dataToSign.Value,
                isPartOperation: false,
                out ExecutionResultCode executionResultCode);

            if (!lengthRequest)
                this.dispatchResult.Session.ResetRegisteredSigningContext();

            return new BytesResult(signedData, executionResultCode);
        }

        public IExecutionResult SignUpdate(IDataContainer dataToSign)
        {
            ISigningContext context = this.dispatchResult.Session.RegisteredSigningContext;

            ISigningModule signingModule = moduleCollection.GetSigningModule(context);

            byte[] signedData = signingModule.Sign(
                signingData: dataToSign.Value,
                isPartOperation: true,
                out ExecutionResultCode executionResultCode);

            return new BytesResult(signedData, executionResultCode);
        }

        public IExecutionResult SignFinal(bool lengthRequest)
        {
            ISigningContext context = this.dispatchResult.Session.RegisteredSigningContext;

            ISigningModule signingModule = moduleCollection.GetSigningModule(context);

            byte[] signedData = signingModule.SignFinalise(out ExecutionResultCode executionResultCode);

            if (!lengthRequest)
                this.dispatchResult.Session.ResetRegisteredSigningContext();

            return new BytesResult(signedData, executionResultCode);
        }

        public IExecutionResult VerifyInit(ulong publicKeyIdentifier, IDataContainer<Pkcs11Mechanism> mechanism)
        {
            IMemoryObject keyHandler = this.dispatchResult.Session.GetSessionObject(publicKeyIdentifier);

            if (keyHandler == null || mechanism == null) return new BytesResult(ExecutionResultCode.ARGUMENTS_BAD);

            IVerifyModule signingModule = moduleCollection.GetVerifyModule(null);

            keyHandler = signingModule.Initialise(
                publicKey: keyHandler,
                mechanism: this.ModelBinder.CreateMechanismModel(mechanism, tokenStorage),
                out ExecutionResultCode executionResultCode);

            if (keyHandler is not null)
                this.dispatchResult.Session.UpdateAndRegisterSesionObject(publicKeyIdentifier, keyHandler as IVerifyContext);

            return new BytesResult(executionResultCode);
        }

        public IExecutionResult Verify(bool lengthRequest, IDataContainer dataToVerify, IDataContainer signedData)
        {
            IVerifyContext context = this.dispatchResult.Session.RegisteredVerifyContext;

            if (context is not null) context.LengthRequest = lengthRequest;

            IVerifyModule verifyModule = moduleCollection.GetVerifyModule(context);

            bool dataIsValid = verifyModule.Verify(
                verifyingData: dataToVerify.Value,
                signedData: signedData.Value,
                isPartOperation: false,
                out ExecutionResultCode executionResultCode);

            if (!lengthRequest)
                this.dispatchResult.Session.ResetRegisteredVerifyContext();

            return new BytesResult(new[] { Convert.ToByte(dataIsValid) }, executionResultCode);
        }

        public IExecutionResult VerifyUpdate(IDataContainer dataToVerify)
        {
            IVerifyContext context = this.dispatchResult.Session.RegisteredVerifyContext;

            IVerifyModule verifyModule = moduleCollection.GetVerifyModule(context);

            bool dataIsValid = verifyModule.Verify(
                 verifyingData: dataToVerify.Value,
                 signedData: null,
                 isPartOperation: true,
                 out ExecutionResultCode executionResultCode);

            return new BytesResult(new[] { Convert.ToByte(dataIsValid) }, executionResultCode);
        }

        public IExecutionResult VerifyFinal(bool lengthRequest, IDataContainer signedData)
        {
            IVerifyContext context = this.dispatchResult.Session.RegisteredVerifyContext;

            IVerifyModule signingModule = moduleCollection.GetVerifyModule(context);

            bool dataIsValid = signingModule.VerifyFinalise(signedData.Value, out ExecutionResultCode executionResultCode);

            if (!lengthRequest)
                this.dispatchResult.Session.ResetRegisteredVerifyContext();

            return new BytesResult(new[] { Convert.ToByte(dataIsValid) }, executionResultCode);
        }
    }
}
