using Service.Core.Abstractions.Communication.Interfaces;
using Service.Core.Abstractions.Communication.Structures;
using Service.Core.Abstractions.Storage.Structures;
using Service.Core.Abstractions.Token.Interfaces;
using Service.Core.Abstractions.Token.Structures;
using Service.Core.Communication.Infrastructure;
using Service.Core.Infrastructure.Token;
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
    /// </summary>
    public class ServiceExecutor : IServiceExecutor
    {
        private DispatchResult dispatchResult;

        public IEncryptionHandler EncryptionHandler => new EncryptionHandler();

        public ISigningHandler SigningHandler => new SigningHandler();

        public IHashingHandler HashingHandler => new HashingHandler();

        public void SetDispatcherResult(DispatchResult dispatchResult)
        {
            this.dispatchResult = dispatchResult;
        }

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
            return new BytesResult(BitConverter.GetBytes(dispatchResult.Session.Id), ExecutionResultCode.OK);
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
        public virtual IExecutionResult Authenticate()
        {
            Pkcs11DataContainer<Pkcs11UserType> authenticationData = this.dispatchResult.Payload.ToPkcs11DataContainer<Pkcs11UserType>().FirstOrDefault();

            bool authenticationResult =
                this.dispatchResult.Session.Authenticate(authenticationData.Type, ASCIIEncoding.UTF8.GetString(authenticationData.Value));

            //todo: better handling for codes
            return new BytesResult(ExecutionResultCode.OK);
        }

        /// <summary>
        /// Create a object. Require a tlv structure array representing the attributes used for creation
        /// </summary>
        /// <returns>Bytes representing data. Payload represents 4 bytes for handler id</returns>
        public virtual IExecutionResult CreateObject()
        {
            IEnumerable<Pkcs11DataContainer<Pkcs11Attribute>> attributes = this.dispatchResult.Payload.ToPkcs11DataContainer<Pkcs11Attribute>();
            //todo: better handling for codes

            if( ! Pkcs11Object.Create(attributes, out Pkcs11Object @object, out ExecutionResultCode creationResultCode)){
                return new BytesResult(BitConverter.GetBytes(-1L), creationResultCode);
            }

            long handlerId = this.dispatchResult.Session.AddSesionObject(@object);

            return new BytesResult(BitConverter.GetBytes(handlerId), ExecutionResultCode.OK);
        }

        public virtual IExecutionResult EncryptInit()
        {
            //todo: implement
            return null;
        }

        public virtual IExecutionResult Encrypt()
        {
            //todo: implement
            return null;
        }

        public virtual IExecutionResult EncryptFinal()
        {
            //todo: implement
            return null;
        }
       

    }
}
