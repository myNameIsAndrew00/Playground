using Service.Core.Abstractions.Communication;
using Service.Core.Abstractions.Token;
using Service.Core.Client;
using Service.Core.Communication.Infrastructure;
using Service.Core.Infrastructure;
using Service.Core.Infrastructure.Communication;
using Service.Core.Infrastructure.Token;
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
        /// Creates the default server exposed by library
        /// </summary>
        /// <param name="address">IP Address used by the server</param>
        /// <param name="port">TCP Port where the server will listen to</param>
        /// <returns></returns>
        public static IPkcs11Server CreateDefaultServer(string address, int port) => CreateDefaultSocketServer(address, port);

        /// <summary>
        /// Creates a socket server with the executor implemented by the library
        /// </summary>
        /// <param name="address">IP Address used by the server</param>
        /// <param name="port">TCP Port where the server will listen to</param>
        /// <returns>A server instance</returns>
        public static IPkcs11Server CreateDefaultSocketServer(string address, int port)
        {
            IPkcs11Server result = CreateSocketServer<TlvServiceExecutor>(address, port);
             
            result.RegisterEncryptionModule( opt => new EncryptionModule(opt) );
            result.RegisterHashingModule( opt => new HashingModule() );
            result.RegisterSigningModule( opt => new SigningModule() );

            return result;
        }

 
        /// <summary>
        /// Creates a socket server with a custom executor with no modules registered in the service collection.
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
