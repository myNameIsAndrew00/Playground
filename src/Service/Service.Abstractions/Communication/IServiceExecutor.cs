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
    /// Provide methods to execute service actions.
    /// Other optional methods can be contained which have names of values contained by ServiceActionCode enum.
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
        /// Returns a empty session result
        /// </summary>
        /// <returns></returns>
        IExecutionResult GetEmptySessionResult(ExecutionResultCode code);

        /// <summary>
        /// Represents an object which may be used by server to parse parameters to ServiceExecutor invoked method
        /// </summary>
        IServiceExecutorModelBinder<DispatchResultType, SessionType> ModelBinder { get; }

    }
}
