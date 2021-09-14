using Service.Core.Abstractions.Communication;
using Service.Core.Abstractions.Execution;
using Service.Core.DefinedTypes;
using Service.Runtime;
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
    public class AlphaProtocolDispatcher<DispatchResultType, SessionType> : IServiceProtocolDispatcher<DispatchResultType, SessionType>
        where DispatchResultType : IDispatchResult<SessionType>, new()
        where SessionType : ISession, new()
    {

        Dictionary<ulong, SessionType> sessions = new Dictionary<ulong, SessionType>();

        public DispatchResultType DispatchClientRequest(byte[] inputBytes)

        {
            ServiceActionCode requestedAction = (ServiceActionCode)inputBytes[0];

            int payloadOffset = 1;

            handleCommandByte(requestedAction,
                out bool requireSession,
                out SessionType session
                );

            if (requireSession)
            {
                ulong sessionId = inputBytes.ToULong();
                payloadOffset += sizeof(ulong);

                sessions.TryGetValue(sessionId, out session);

                if (requestedAction == ServiceActionCode.EndSession)
                    removeSession(sessionId);
            }

            return new DispatchResultType()
            {
                DispatchedAction = requestedAction,
                Payload = inputBytes.Skip(payloadOffset).ToArray(),
                Session = session,
                RequireSession = requireSession
            };
        }

        public byte[] BuildClientResponse(IExecutionResult executionResult)
        {
            byte[] executionResultBytes = executionResult.GetBytes();
            byte[] resultBytes = new byte[sizeof(uint) + executionResultBytes.Length];

            ((ulong)executionResult.ResultCode).GetBytes().CopyTo(resultBytes, 0);
            executionResultBytes.CopyTo(resultBytes, sizeof(uint));

            return resultBytes;
        }

        #region Private

        private SessionType beginSession()
        {
            ulong nextSessionId = Utils.GetNextId();

            SessionType session = new SessionType() { Id = nextSessionId };
            sessions.Add(nextSessionId, session);

            return session;
        }

        private void removeSession(ulong sessionId)
        {
            if (sessions.TryGetValue(sessionId, out SessionType session))
                session.Dispose();

            sessions.Remove(sessionId);
        }

        private void handleCommandByte(ServiceActionCode requestedAction, out bool requireSession, out SessionType createdSession)
        {
            createdSession = default;
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
