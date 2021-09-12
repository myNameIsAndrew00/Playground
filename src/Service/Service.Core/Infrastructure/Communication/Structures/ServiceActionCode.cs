using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Communication.Structures
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
        CreateObject = 0x04,
        EncryptInit = 0x05,
        Encrypt = 0x06,
        EncryptFinal = 0x07
        
    } 
}
