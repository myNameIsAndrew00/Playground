using Service.Core.Abstractions.Execution;
using Service.Core.Abstractions.Storage;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Service.Core.Abstractions.Communication
{
    /// <summary>
    /// Represents a class which can be used to provide parameters to a executor method
    /// </summary>
    public interface IServiceExecutorModelBinder<DispatchResultType, SessionType>
        where DispatchResultType : IDispatchResult<SessionType>
        where SessionType : ISession
    {
        /// <summary>
        /// Use this method to build the parameters required for executor, using the dispatching result.
        /// </summary>
        /// <param name="method">Method dispatched</param>
        /// <param name="dispatcherResult">Result of the dispatch process execution</param>
        /// <returns></returns>
        object[] GetMethodParameters(MethodInfo method, DispatchResultType dispatcherResult, IPayloadDataParser dataContainerParser);
    }
}
