using Service.Core.Abstractions.Communication.Structures;
using Service.Core.Abstractions.Token.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Communication.Interfaces
{
    /// <summary>
    /// Provide methods to execute service actions.
    /// Other optional methods can be contained which have names of values contained by ServiceActionCode enum.
    /// If a optional method is called, but it is not implemented by executor, service will return not implemented error code.
    /// </summary>
    public interface IServiceExecutor 
    {
        /// <summary>
        /// Set the dispatch result for executor. A dispatch result can be consider as the main context of the executor
        /// </summary>
        /// <param name="dispatchResult">Value which will be set</param>
        void SetDispatcherResult(DispatchResult dispatchResult);

        /// <summary>
        /// Returns a empty session result
        /// </summary>
        /// <returns></returns>
        IExecutionResult GetEmptySessionResult(ExecutionResultCode code);

        IEncryptionHandler EncryptionHandler { get; }

        ISigningHandler SigningHandler { get; }

        IHashingHandler HashingHandler { get; }
    }
}
