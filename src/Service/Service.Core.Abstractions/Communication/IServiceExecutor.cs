using Service.Core.Abstractions.Execution;
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
    public interface IServiceExecutor<DispatchResultType, SessionType> 
        where DispatchResultType : IDispatchResult<SessionType>
        where SessionType : ISession
    {
        /// <summary>
        /// Set the dispatch result for executor. A dispatch result can be consider as the main context of the executor
        /// </summary>
        /// <param name="dispatchResult">Value which will be set</param>
        void SetDispatcherResult(DispatchResultType dispatchResult);

        /// <summary>
        /// Set the module collection used by this executor in request handling.
        /// </summary>
        /// <param name="moduleCollection"></param>
        void SetModuleFactory(IModuleFactory moduleCollection);

        /// <summary>
        /// Set the executor storage which can be used to handle token internal storage
        /// </summary>
        /// <param name="storage"></param>
        void SetStorage(ITokenStorage storage);

        /// <summary>
        /// Returns a empty session result
        /// </summary>
        /// <returns></returns>
        IExecutionResult GetEmptySessionResult(ExecutionResultCode code);

        /// <summary>
        /// Represents an object which may be used by server to parse parameters to ServiceExecutor invoked method
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
        /// Authenticate a session. Require a tlv structure representing the user type and password value
        /// </summary>
        /// <returns></returns>
        IExecutionResult Authenticate(IDataContainer<Pkcs11UserType> authenticationData);

        /// <summary>
        /// Create a object.
        /// </summary>
        /// <returns>Bytes representing data. Payload represents 8 bytes for handler id</returns>
        IExecutionResult CreateObject(IEnumerable<IDataContainer<Pkcs11Attribute>> attributes);

        /// <summary>
        /// Function associated with encrypt init
        /// </summary>
        /// <returns></returns>
        IExecutionResult EncryptInit(ulong keyIdentifier, IDataContainer<Pkcs11Mechanism> mechanism);
       
        /// <summary>
        /// Function associated with encrypt
        /// </summary>
        /// <returns></returns>
        IExecutionResult Encrypt(IDataContainer dataToEncrypt);

        /// <summary>
        /// Function associated with encrypt update
        /// </summary>
        /// <returns></returns>
        IExecutionResult EncryptUpdate(IDataContainer dataToEncrypt);

        /// <summary>
        /// Function associated with PKCS11 encrypt final
        /// </summary>
        /// <returns></returns>
        IExecutionResult EncryptFinal();
    

        #endregion

    }
}
