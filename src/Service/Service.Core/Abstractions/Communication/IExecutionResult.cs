using Service.Core.Infrastructure.Communication.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Communication
{
    /// <summary>
    /// An object which encapsulates client request processing result
    /// </summary>
    public interface IExecutionResult
    {
        /// <summary>
        /// Return bytes which must be returned to client
        /// </summary>
        /// <returns></returns>
        byte[] GetBytes();

        ExecutionResultCode ResultCode { get; }
    }
}
