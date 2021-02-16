using Service.Core.Abstractions.Structures;
using System;
using System.Collections.Generic;
using System.Text;
using static Service.Core.Abstractions.Interfaces.IServiceDispatcher;

namespace Service.Core.Abstractions.Interfaces
{
    /// <summary>
    /// Implements methods to handle messages received from clients and dispatch them to the server.
    /// It is responsible for authentication and session management
    /// </summary>
    public interface IServiceDispatcher
    {
        public DispatchResult Dispatch(byte[] inputBytes);
    }


}
