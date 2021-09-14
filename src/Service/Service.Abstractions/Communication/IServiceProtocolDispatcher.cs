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
    public interface IServiceProtocolDispatcher<DispatchResultType, SessionType> 
        where DispatchResultType : IDispatchResult<SessionType>
        where SessionType : ISession
        
    {

        ///Dispatch messages received from clients to the server.
        public DispatchResultType DispatchClientRequest(byte[] inputBytes);

        public byte[] BuildClientResponse(IExecutionResult executionResult);
    }


}
