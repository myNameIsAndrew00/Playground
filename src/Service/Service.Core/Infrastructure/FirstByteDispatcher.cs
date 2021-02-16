using Service.Core.Abstractions.Interfaces;
using Service.Core.Abstractions.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Core.Infrastructure
{  
    /// <summary>
    /// Represents the main dispatcher used by this service.
    /// Format used by this dispatcher accepts first byte for called method code, second byte for session id 
    /// and following bytes payload. 
    /// </summary>
    internal class FirstByteDispatcher : IServiceDispatcher
    {
        private byte nextSessionId = 0x01;

        HashSet<byte> sessionIds = new HashSet<byte>();

        public DispatchResult Dispatch(byte[] inputBytes)
        {
            ServiceAction requestedAction = (ServiceAction)inputBytes[0];
            bool authorized = true; 
            int payloadOffset = 1;    

            handleCommandByte(requestedAction,
                out bool requireAuthentication,
                out byte sessionId
                );

            if (requireAuthentication)
            {
                sessionId = inputBytes[payloadOffset++];
                 
                if (!sessionIds.Contains(sessionId))
                    authorized = false;

                if (requestedAction == ServiceAction.EndSession)
                    removeSession(sessionId);
            }

            return new DispatchResult(
                dispatchedAction: requestedAction, 
                payload: inputBytes.Skip(payloadOffset).ToArray(),
                authorized: authorized, 
                sesionId: (int)sessionId);
        }

        #region Private

        private byte beginSession()
        {
            byte sessionId = nextSessionId++;
            sessionIds.Add(sessionId);

            return sessionId;
        }

        private void removeSession(byte sessionId)
        {
            sessionIds.Remove(sessionId);
        }

        private void handleCommandByte(ServiceAction requestedAction, out bool requireAuthentication, out byte sessionId)
        {
            sessionId = 0x00;
            requireAuthentication = false;

            switch (requestedAction)
            {
                case ServiceAction.EndSession:
                    requireAuthentication = true;
                    break;
                case ServiceAction.BeginSession:
                    sessionId = beginSession();
                    requireAuthentication = false;
                    break;
                default:
                    requireAuthentication = false;
                    break;
            }


        }

        #endregion
    }
}
