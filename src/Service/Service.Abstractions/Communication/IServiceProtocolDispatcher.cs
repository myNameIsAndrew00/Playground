using Service.Core.Abstractions.Execution;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Communication
{
    /// <summary>
    /// Implements methods to handle client/server protocol.
    /// It is responsible for authentication and session management
    /// </summary>
    public interface IServiceProtocolDispatcher<DispatchResultType, SessionType> : IAllowCloseSession
        where DispatchResultType : IDispatchResult<SessionType>
        where SessionType : ISession
        
    {

        ///Dispatch messages received from clients to the server. This method can handle session close operation.
        public DispatchResultType DispatchClientRequest(byte[] inputBytes);

        /// <summary>
        /// Build the bytes which will be returned to client
        /// </summary>
        /// <param name="executionResult">Execution result of this method</param>
        /// <returns></returns>
        public byte[] BuildClientResponse(IExecutionResult executionResult);
    }


}
