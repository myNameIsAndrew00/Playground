using Service.Core.Abstractions.Execution;
using Service.Core.Abstractions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Communication
{
    /// <summary>
    /// Resolve the communication between clients and service
    /// </summary>
    public interface IServiceCommunicationResolver<DispatchResultType,SessionType> : IAllowLogging, IDisposable
        where DispatchResultType : IDispatchResult<SessionType>
        where SessionType : ISession
    {
        /// <summary>
        /// Start resolver listening for connections
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Listen(CancellationToken cancellationToken);

        /// <summary>
        /// An event which will trigger when a communication with a client is estanblished
        /// </summary>
        event Func<DispatchResultType, IExecutionResult> OnCommunicationCreated;

        /// <summary>
        /// Occurs when an connection with a client fail
        /// </summary>
        event Action<Exception> OnClientConnectionError;

        /// <summary>
        /// Occurs when service fail to process the request
        /// </summary>
        event Action<Exception> OnRequestHandlingError;

        IServiceProtocolDispatcher<DispatchResultType, SessionType> Dispatcher { get; }
    }
}
