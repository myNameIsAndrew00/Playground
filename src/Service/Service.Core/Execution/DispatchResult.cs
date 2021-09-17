using Service.Core.Abstractions.Execution;
using Service.Core.DefinedTypes;
using Service.Core.Infrastructure.Communication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Execution
{
    public class DispatchResult : IDispatchResult<Session>
    {
        public DispatchResult() { }

        public DispatchResult(ServiceActionCode dispatchedAction, byte[] payload, bool requireSession)
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


 
        public ServiceActionCode DispatchedAction { get; init; }

        public bool RequireSession { get; init; }

  
        public Session Session { get; init; }

        /// <summary>
        /// Data from request, without session or header bytes
        /// </summary>
        public byte[] Payload { get; init; }

        /// <summary>
        /// Optional data set by dispatcher
        /// </summary>
        public object OptionalData { get; init; }

        public bool ClosedSession { get; init; }
    }
 
}
