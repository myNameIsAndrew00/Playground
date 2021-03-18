using Service.Core.Abstractions.Communication.Interfaces;
using Service.Core.Abstractions.Communication.Structures;
using Service.Core.Infrastructure.Communication.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Core.Communication.Infrastructure
{  
    /// <summary>
    /// Represents the main protocol dispatcher used by this service.
    /// Client messages format used by this dispatcher accepts first byte for called method code, second byte for session id 
    /// and following bytes payload. 
    /// Response messages accept first byte for result code and following bytes payload
    /// </summary>
    internal class AlphaProtocolDispatcher : IServiceProtocolDispatcher
    {
        private byte nextSessionId = 0x01;

        Dictionary<byte, Session> sessions = new Dictionary<byte, Session>();

        public DispatchResult DispatchClientRequest(byte[] inputBytes)
        
        {
            ServiceActionCode requestedAction = (ServiceActionCode)inputBytes[0];
           
            int payloadOffset = 1;    

            handleCommandByte(requestedAction,
                out bool requireSession,
                out Session session
                );

            if (requireSession)
            {
                byte sessionId = inputBytes[payloadOffset++];

                sessions.TryGetValue(sessionId, out session);       

                if (requestedAction == ServiceActionCode.EndSession)
                    removeSession(sessionId);
            }

            return new DispatchResult(
                dispatchedAction: requestedAction, 
                payload: inputBytes.Skip(payloadOffset).ToArray(),
                session: session,
                requireSession: requireSession);
        }

        public byte[] BuildClientResponse(IExecutionResult executionResult)
        {
            byte[] executionResultBytes = executionResult.GetBytes();
            byte[] resultBytes = new byte[sizeof(int) + executionResultBytes.Length];
             
            BitConverter.GetBytes((int)executionResult.ResultCode).CopyTo(resultBytes, 0);
            executionResultBytes.CopyTo(resultBytes, sizeof(int));

            return resultBytes;
        }

        #region Private

        private Session beginSession()
        {            
            Session session = new Session(nextSessionId);
            sessions.Add(nextSessionId++, session);

            return session;
        }

        private void removeSession(byte sessionId)
        {
            sessions.Remove(sessionId);
        }

        private void handleCommandByte(ServiceActionCode requestedAction, out bool requireSession, out Session createdSession)
        {
            createdSession = null;
            requireSession = false;

            switch (requestedAction)
            {          
                case ServiceActionCode.BeginSession:
                    createdSession = beginSession();
                    requireSession = false;
                    break;
                case ServiceActionCode.EndSession:
                    requireSession = true;
                    break;
                case ServiceActionCode.Authenticate:
                    requireSession = true;
                    break;
                default:
                    requireSession = false;
                    break;
            }


        }


        #endregion
    }
}
