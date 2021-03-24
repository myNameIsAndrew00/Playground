using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Communication.Structures
{ 
    public enum ServiceActionCode : byte
    {
        /// <summary>
        /// Use this code to trigger the service to start a session
        /// </summary>
        BeginSession = 0x01,
        /// <summary>
        /// Use this code to trigger the service to end a session
        /// </summary>
        EndSession = 0x02,
        Authenticate = 0x03,
        CreateObject = 0x04
        
    } 
}
