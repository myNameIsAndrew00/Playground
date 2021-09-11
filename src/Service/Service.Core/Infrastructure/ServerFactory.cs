using Service.Core.Abstractions.Communication.Interfaces;
using Service.Core.Abstractions.Token.Interfaces;
using Service.Core.Client;
using Service.Core.Communication.Abstractions;
using Service.Core.Communication.Infrastructure;
using Service.Core.Infrastructure;
using Service.Core.Infrastructure.Communication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure
{
    /// <summary>
    /// Use this class as to create server instances
    /// </summary>
    public static class ServerFactory
    {
        /// <summary>
        /// Creates a socket server with the executor implemented by the library
        /// </summary>
        /// <param name="address">IP Address used by the server</param>
        /// <param name="port">TCP Port where the server will listen to</param>
        /// <returns>A server instance</returns>
        public static IPkcs11Server CreateDefaultSocketServer(string address, int port)
        {
            return new Server<TlvServiceExecutor>(
                   new SocketCommunicationResolver(
                       address: address,
                       port: port,
                       dispatcher: new AlphaProtocolDispatcher()
                   )
               );
        }

 
        /// <summary>
        /// Creates a socket server with a custom executor       
        /// </summary>
        /// <typeparam name="Executor">Type of executor which is used for client requests execution</typeparam>
        /// <param name="address">IP Address used by the server</param>
        /// <param name="port">TCP Port where the server will listen to</param>
        /// <returns>A server instance</returns>
        public static IPkcs11Server CreateSocketServer<Executor>(string address, int port)
            where Executor : IServiceExecutor, new()
        {
            return new Server<Executor>(
                new SocketCommunicationResolver(
                    address: address,
                    port: port,
                    dispatcher: new AlphaProtocolDispatcher()
                )
                );
        }
    }
}
