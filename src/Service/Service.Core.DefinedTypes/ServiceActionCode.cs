using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.DefinedTypes
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
        /// Use this code to trigger service encryption encrypt
        /// </summary>
        Encrypt = 0x06,
        /// <summary>
        /// Use this code to trigger service encryption finalisation
        /// </summary>
        EncryptFinal = 0x07,
        /// <summary>
        /// Use this code to trigger service encrypt update
        /// </summary>
        EncryptUpdate = 0x08,
        /// <summary>
        /// Use this code to trigger service decryption initialisation
        /// </summary>
        DecryptInit = 0x09,
        /// <summary>
        /// Use this code to trigger service decryption (decrypt and decrypt update)
        /// </summary>
        Decrypt = 0x0A,
        /// <summary>
        /// Use this code to trigger service decryption finalisation
        /// </summary>
        DecryptFinal = 0x0B,
        /// <summary>
        /// Use this code to trigger service decrypt update
        /// </summary>
        DecryptUpdate = 0x0C,
        /// <summary>
        /// Use this code to trigger service digest initialisation
        /// </summary>
        DigestInit = 0x0D,
        /// <summary>
        /// Use this code to trigger service digest
        /// </summary>
        Digest = 0x0E,
        /// <summary>
        /// Use this code to trigger service digest finalisation
        /// </summary>
        DigestFinal = 0x0F,
        /// <summary>
        /// Use this code to trigger service digest update
        /// </summary>
        DigestUpdate = 0x10
    }
}
