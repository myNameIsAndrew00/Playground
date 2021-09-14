using Service.Core.Abstractions.Communication;
using Service.Core.Abstractions.Execution;
using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Encryption;
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

        public TlvServiceExecutor()
        {
        }




        public IServiceExecutorModelBinder<DispatchResult, Session> ModelBinder => new TlvServiceExecutorModelBinder();

        public void SetDispatcherResult(DispatchResult dispatchResult) => this.dispatchResult = dispatchResult;

        public void SetModuleFactory(IModuleFactory moduleCollection) => this.moduleCollection = moduleCollection;



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
        public virtual IExecutionResult Authenticate(DataContainer<Pkcs11UserType> authenticationData)
        {
            //Pkcs11DataContainer<Pkcs11UserType> authenticationData = this.dispatchResult.Payload.ToPkcs11DataContainer<Pkcs11UserType>();

            bool authenticationResult =
                this.dispatchResult.Session.Authenticate(authenticationData.Type, ASCIIEncoding.UTF8.GetString(authenticationData.Value));

            //todo: better handling for codes
            return new BytesResult(ExecutionResultCode.OK);
        }

        /// <summary>
        /// Create a object. Require a tlv structure array representing the attributes used for creation
        /// </summary>
        /// <returns>Bytes representing data. Payload represents 4 bytes for handler id</returns>
        public virtual IExecutionResult CreateObject(IEnumerable<DataContainer<Pkcs11Attribute>> attributes)
        { 
            if (attributes == null) return new BytesResult(ExecutionResultCode.ARGUMENTS_BAD);

            //todo: inject the builder into server
            if (!MemoryObjectsBuilder.Instance.Get(attributes.Select( item => item as IPkcs11AttributeDataContainer), out IMemoryObject @object, out ExecutionResultCode creationResultCode))
            {
                return new BytesResult(creationResultCode);
            }

            uint handlerId = this.dispatchResult.Session.AddSesionObject(@object);
            
            return new BytesResult( handlerId.GetBytes(), ExecutionResultCode.OK);
        }

        /// <summary>
        /// Function associated with encrypt init
        /// </summary>
        /// <returns></returns>
        public virtual IExecutionResult EncryptInit(uint keyIdentifier, DataContainer<Pkcs11Mechanism> mechanism)
        {
            //todo: handle null and edge cases
            IMemoryObject keyHandler = this.dispatchResult.Session.GetSessionObject(keyIdentifier);

            if (keyHandler == null || mechanism == null) return new BytesResult(ExecutionResultCode.ARGUMENTS_BAD);

            IEncryptionModule encryptionHandler = moduleCollection.GetEncryptionModule(keyHandler);

            //todo: better handling for codes
            keyHandler = encryptionHandler.Initialise(mechanism, out ExecutionResultCode executionResultCode);

            if (keyHandler is not null)
                this.dispatchResult.Session.UpdateAndRegisterSesionObject(keyIdentifier, keyHandler);

            return new BytesResult(executionResultCode);
        }

        /// <summary>
        /// Function associated with encrypt
        /// </summary>
        /// <returns></returns>
        public virtual IExecutionResult Encrypt(DataContainer dataToEncrypt)
        {
            EncryptionContext context = this.dispatchResult.Session.RegisteredEncryptionContext;

            IEncryptionModule encryptionHandler = moduleCollection.GetEncryptionModule(context);

            byte[] encryptedData = encryptionHandler.Encrypt(
                plainData: dataToEncrypt.Value,
                isPartOperation: false, 
                out ExecutionResultCode executionResultCode);

            this.dispatchResult.Session.ResetRegisteredEncryptionObject();

            return new BytesResult(encryptedData, executionResultCode);
        }

        /// <summary>
        /// Function associated with encrypt update
        /// </summary>
        /// <returns></returns>
        public virtual IExecutionResult EncryptUpdate(DataContainer dataToEncrypt)
        {
            EncryptionContext context = this.dispatchResult.Session.RegisteredEncryptionContext;

            IEncryptionModule encryptionHandler = moduleCollection.GetEncryptionModule(context);

            byte[] encryptedData = encryptionHandler.Encrypt(
                plainData: dataToEncrypt.Value,
                isPartOperation: true,
                out ExecutionResultCode executionResultCode);

            return new BytesResult(encryptedData, executionResultCode);
        }

        /// <summary>
        /// Function associated with encrypt final
        /// </summary>
        /// <returns></returns>
        public virtual IExecutionResult EncryptFinal()
        {
            EncryptionContext context = this.dispatchResult.Session.RegisteredEncryptionContext;

            IEncryptionModule encryptionHandler = moduleCollection.GetEncryptionModule(context);

            byte[] encryptedData = encryptionHandler.EncryptFinalise(out ExecutionResultCode executionResultCode);
            
            this.dispatchResult.Session.ResetRegisteredEncryptionObject();

            return new BytesResult(encryptedData, executionResultCode);
        }



    }
}
