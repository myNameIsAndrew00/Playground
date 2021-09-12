using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Communication
{
    /// <summary>
    /// Expose methods which describe a PKCS 11 service behavior
    /// </summary> 
    public interface IPkcs11Server
    {
        /// <summary>
        /// Enable server instance to wait for listening client requests
        /// </summary>
        void Start();

        /// <summary>
        /// The instance which will initialise the communication and handle messages between server and client
        /// </summary>
        IServiceCommunicationResolver Resolver { get; }

        /// <summary>
        /// A method which will create the instance to execute the desired action from client
        /// </summary>
        /// <returns></returns>
        IServiceExecutor CreateExecutor();

    }
}
