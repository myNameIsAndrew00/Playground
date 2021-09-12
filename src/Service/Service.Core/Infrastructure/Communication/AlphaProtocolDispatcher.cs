using Service.Core.Abstractions.Communication;
using Service.Core.Infrastructure.Communication.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Core.Infrastructure.Communication
{  
    /// <summary>
    /// Represents the main protocol dispatcher used by this service.
    /// Client messages format used by this dispatcher accepts first byte for called method code, second byte for session id (if required)
    /// and following bytes payload. 
    /// Response messages accept first byte for result code and following bytes payload
    /// </summary>
    internal class AlphaProtocolDispatcher : IServiceProtocolDispatcher
    {

        Dictionary<long, Session> sessions = new Dictionary<long, Session>();

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
            byte[] resultBytes = new byte[sizeof(uint) + executionResultBytes.Length];
             
            BitConverter.GetBytes((uint)executionResult.ResultCode).CopyTo(resultBytes, 0);
            executionResultBytes.CopyTo(resultBytes, sizeof(uint));

            return resultBytes;
        }

        #region Private

        private Session beginSession()
        {
            uint nextSessionId = Utils.GetNextId();

            Session session = new Session(nextSessionId);
            sessions.Add(nextSessionId, session);

            return session;
        }

        private void removeSession(byte sessionId)
        {
            if (sessions.TryGetValue(sessionId, out Session session))
                session.Dispose();

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
                    break;
                case ServiceActionCode.EndSession:
                case ServiceActionCode.CreateObject:
                case ServiceActionCode.Authenticate:
                case ServiceActionCode.EncryptInit:
                case ServiceActionCode.Encrypt:
                case ServiceActionCode.EncryptFinal:
                    requireSession = true;
                    break;
                default:
                    break;
            }


        }


        #endregion
    }
}
