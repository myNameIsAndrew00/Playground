using Service.Core.Abstractions.Execution;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Communication
{
    /// <summary>
    /// Resolve the communication between clients and service
    /// </summary>
    public interface IServiceCommunicationResolver<DispatchResultType,SessionType>
        where DispatchResultType : IDispatchResult<SessionType>
        where SessionType : ISession
    {
        void Listen();

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
