using Service.Core.Infrastructure.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Structures
{

    /// <summary>
    /// Result of dispatching method, contains data about call which has been made from client
    /// </summary>
    public class DispatchResult
    {
        internal DispatchResult(ServiceActionCode dispatchedAction, byte[] payload, bool requireSession)
        {
            this.DispatchedAction = dispatchedAction;
            this.Payload = payload;
            this.RequireSession = requireSession;
        }

        internal DispatchResult(ServiceActionCode dispatchedAction, byte[] payload, Session session, bool requireSession)
        : this(dispatchedAction, payload, requireSession)
        {
            this.Session = session;
        }

        internal DispatchResult(ServiceActionCode dispatchedAction, byte[] payload, Session session, bool requireSession, object optionalData)
       : this(dispatchedAction, payload, session, requireSession)
        {
            this.Session = session;
            this.OptionalData = optionalData;
        }



        /// <summary>
        /// Action which has been chosed after dispatching
        /// </summary>
        public ServiceActionCode DispatchedAction { get; }

        public bool RequireSession { get; }

        public bool SessionCheckPass => (Session == null && !RequireSession) || (Session != null);

        /// <summary>
        /// A boolean which will specify if the request if authorized
        /// </summary>
        public Session Session { get; }

        /// <summary>
        /// Data from request, without session or control byte
        /// </summary>
        public byte[] Payload { get; }

        /// <summary>
        /// Optional data set by dispatcher
        /// </summary>
        public object OptionalData { get; }
    }
 
}
