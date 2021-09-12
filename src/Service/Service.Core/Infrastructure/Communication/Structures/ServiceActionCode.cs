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
        /// <summary>
        /// Use this code to trigger the service authenticate method
        /// </summary>
        Authenticate = 0x03,
        /// <summary>
        /// Use this code to trigger service method used to create an internal object handler
        /// </summary>
        CreateObject = 0x04,
        /// <summary>
        /// Use this code to trigger service encryption initialisation
        /// </summary>
        EncryptInit = 0x05,
        /// <summary>
        /// Use this code to trigger service encryption
        /// </summary>
        Encrypt = 0x06,
        /// <summary>
        /// Use this code to trigger service encryption finalisation
        /// </summary>
        EncryptFinal = 0x07,
        /// <summary>
        /// Use this code to trigger service encrypt update
        /// </summary>
        EncryptUpdate = 0x08
        
    } 
}
