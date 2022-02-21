using Service.Core.Abstractions.Communication;
using Service.Core.Abstractions.Execution;
using Service.Core.Abstractions.Logging;
using Service.Core.DefinedTypes;
using Service.Runtime;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Service.Core.Abstractions.Logging.IAllowLogging;

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

        private ConcurrentBag<LogData> logs;
        private Dictionary<ulong, SessionType> sessions = new Dictionary<ulong, SessionType>();

        public LogSection LogSection => LogSection.COMMUNICATION_DISPATCHER;

        public IReadOnlyCollection<LogData> Logs => logs;

        public void ClearLogs()
        {
            logs.Clear();
        }

        public AlphaProtocolDispatcher()
        {
            logs = new ConcurrentBag<LogData>();
        }

        public IEnumerable<IReadOnlySession> GetSessions() => sessions.Values.Select(session => session as IReadOnlySession).ToList();

        public DispatchResultType DispatchClientRequest(byte[] inputBytes)
        {
            bool sessionClosed = false;
            ServiceActionCode requestedAction = (ServiceActionCode)inputBytes[0];

            //log the requested action
            logs.Add(new LogData($"Dispatched following action: {requestedAction}", null, LogLevel.Info));

            int payloadOffset = 1;

            handleCommandByte(requestedAction,
                out bool requireSession,
                out SessionType session
                );

            if (requireSession)
            {
                ulong sessionId = inputBytes.Skip(payloadOffset).ToULong();
                payloadOffset += sizeof(ulong);

                sessions.TryGetValue(sessionId, out session);

                if (requestedAction == ServiceActionCode.EndSession)
                    sessionClosed = session.Close(this);
            }

            return new DispatchResultType()
            {
                DispatchedAction = requestedAction,
                Payload = inputBytes.Skip(payloadOffset).ToArray(),
                Session = session,
                RequireSession = requireSession,
                ClosedSession = sessionClosed
            };
        }

        public byte[] BuildClientResponse(IExecutionResult executionResult)
        {
            byte[] executionResultBytes = executionResult.GetBytes();
            byte[] resultBytes = new byte[sizeof(ulong) + executionResultBytes.Length];

            ((ulong)executionResult.ResultCode).GetBytes().CopyTo(resultBytes, 0);
            executionResultBytes.CopyTo(resultBytes, sizeof(ulong));

            return resultBytes;
        }

        public bool CloseSession(ISession session)
        {
            if (sessions.TryGetValue(session.Id, out SessionType dispatcherSession))
            {
                dispatcherSession.Dispose();
                return true;
            }

            return false;
        }


        #region Private

        private SessionType beginSession()
        {
            ulong nextSessionId = Utils.GetNextId();

            SessionType session = new SessionType() { Id = nextSessionId, TimeStamp = DateTime.Now };
            sessions.Add(nextSessionId, session);

            return session;
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
                default:
                    requireSession = true;
                    break;
            }


        }




        #endregion
    }
}
