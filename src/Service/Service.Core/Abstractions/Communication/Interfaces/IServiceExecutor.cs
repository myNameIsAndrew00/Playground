using Service.Core.Abstractions.Communication.Structures;
using Service.Core.Abstractions.Token.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Communication.Interfaces
{
    /// <summary>
    /// Provide methods to execute service actions
    /// </summary>
    public interface IServiceExecutor 
    {
        /// <summary>
        /// Set the dispatch result for executor. A dispatch result can be consider as the main context of the executor
        /// </summary>
        /// <param name="dispatchResult">Value which will be set</param>
        void SetDispatcherResult(DispatchResult dispatchResult);

        /// <summary>
        /// Returns a execution result associated with a bad session
        /// </summary>
        /// <returns></returns>
        IExecutionResult GetBadSessionResult();

        IEncryptionHandler EncryptionHandler { get; }

        ISigningHandler SigningHandler { get; }

        IHashingHandler HashingHandler { get; }
    }
}
