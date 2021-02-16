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

        event Func<DispatchResult, byte[]> OnCommunicationCreated;

        IServiceDispatcher Dispatcher { get; }
    }
}
