﻿using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Execution
{ 

    /// <summary>
    /// Result of dispatching method, contains data about call which has been made from client
    /// </summary>
    public interface IDispatchResult<SessionType>
        where SessionType : ISession
    {
        /// <summary>
        /// Represents the session
        /// </summary>
        SessionType Session { get; init; }
        
        /// <summary>
        /// An object representing the current session after dispatching
        /// </summary>
        byte[] Payload { get; init; }

        /// <summary>
        /// Action which has been chosed after dispatching
        /// </summary>
        ServiceActionCode DispatchedAction { get; init; }

        /// <summary>
        /// A bollean which indicate if the dispatched action requires action.
        /// </summary>
        bool RequireSession { get; init; }

        /// <summary>
        /// A boolean which specify if the session check passed
        /// </summary>
        bool SessionCheckPassed => (Session == null && !RequireSession) || (Session != null);

        /// <summary>
        /// A boolean which indicate if the dispatcher closed with success the session.
        /// </summary>
        bool ClosedSession { get; init; }
    }
     
}
