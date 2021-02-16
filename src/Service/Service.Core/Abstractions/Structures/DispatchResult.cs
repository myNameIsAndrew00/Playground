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
        public DispatchResult(ServiceAction dispatchedAction, byte[] payload, bool authorized)
        {
            this.DispatchedAction = dispatchedAction;
            this.Authorized = authorized;
            this.Payload = payload;
        }

        public DispatchResult(ServiceAction dispatchedAction, byte[] payload, bool authorized, int sesionId)
        : this(dispatchedAction, payload, authorized)
        {
            this.SessionId = sesionId;
        }

        public DispatchResult(ServiceAction dispatchedAction, byte[] payload, bool authorized, int sesionId, object optionalData)
       : this(dispatchedAction, payload, authorized)
        {
            this.SessionId = sesionId;
        }



        /// <summary>
        /// Action which has been chosed after dispatching
        /// </summary>
        public ServiceAction DispatchedAction { get; }

        /// <summary>
        /// A boolean which will specify if the request if authorized
        /// </summary>
        public bool Authorized { get; }

        /// <summary>
        /// Id of the session for which has been made the request
        /// </summary>
        public int? SessionId { get; }

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
