using Service.Core.Abstractions.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Interfaces
{
    /// <summary>
    /// Resolve the communication between clients and service
    /// </summary>
    public interface IServiceCommunicationResolver
    {
        void Listen();

        /// <summary>
        /// An event which will trigger when a communication with a client is estanblished
        /// </summary>
        event Func<DispatchResult, byte[]> OnCommunicationCreated;

        /// <summary>
        /// Occurs when an connection creation fail
        /// </summary>
        event Action<Exception> OnClientConnectionError;

        IServiceDispatcher Dispatcher { get; }
    }
}
