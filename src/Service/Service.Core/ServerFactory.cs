﻿using Service.Core.Abstractions.Communication;
using Service.Core.Client;
using Service.Core.Communication.Infrastructure;
using Service.Core.Execution;
using Service.Core.Infrastructure.Communication;
using Service.Core.Logging;
using Service.Core.Storage;
using Service.Core.Token.Encryption;
using Service.Core.Token.Encryption.AES;
using Service.Core.Token.Hashing;
using Service.Core.Token.Hashing.SHA;
using Service.Core.Token.Signing;
using Service.Core.Token.Signing.RSA;

namespace Service.Core
{
    /// <summary>
    /// Use this class as to create server instances
    /// </summary>
    public static class ServerFactory
    {
        /// <summary>
        /// Creates the default server exposed by library. This server allows configurations.
        /// </summary>
        /// <param name="address">IP Address used by the server</param>
        /// <param name="port">TCP Port where the server will listen to</param>
        /// <returns></returns>
        public static IPkcs11Server CreateDefaultServer(string address, int port) => CreateDefaultSocketServer(address, port);

        /// <summary>
        /// Creates a socket server with the executor, modules and storage implemented by the library
        /// </summary>
        /// <param name="address">IP Address used by the server</param>
        /// <param name="port">TCP Port where the server will listen to</param>
        /// <returns>A server instance</returns>
        public static IPkcs11Server CreateDefaultSocketServer(string address, int port)
        {
            IPkcs11Server result = CreateSocketServer<TlvServiceExecutor>(address, port)
                .SetStorage(new TokenStorage())
                .SetLogger(new SqliteLogger())
                .RegisterEncryptionModule(options => {
                    return new EncryptionModule(options)
                        .SetMechanism(new AESECBEncryptMechanismCommand())
                        .SetMechanism(new AESCBCEncryptMechanismCommand())
                        .SetMechanism(new AESCFBEncryptMechanismCommand())
                        .SetMechanism(new AESOFBEncryptMechanismCommand()) 
                        as EncryptionModule;
                })
                .RegisterDecryptionModule(options => {
                    return new DecryptionModule(options)
                        .SetMechanism(new AESECBDecryptMechanismCommand())
                        .SetMechanism(new AESCBCDecryptMechanismCommand())
                        .SetMechanism(new AESCFBDecryptMechanismCommand())
                        .SetMechanism(new AESOFBDecryptMechanismCommand())
                        as DecryptionModule;
                })
                .RegisterHashingModule(options => {
                     return new HashingModule(options)
                        .SetMechanism(new SHA1MechanismCommand())
                        .SetMechanism(new SHA256MechanismCommand())
                        .SetMechanism(new SHA384MechanismCommand())
                        .SetMechanism(new SHA512MechanismCommand())
                        as HashingModule;
                 })
                .RegisterSigningModule(options => {
                    return new SigningModule(options)
                        .SetMechanism(new RSASigningMechanismCommand())
                        as SigningModule;
                 })
                .RegisterVerifyingModule(options => {
                    return new VerifyModule(options)
                        .SetMechanism(new RSAVerifyingMechanismCommand())
                        as VerifyModule;
                });
                

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
            where Executor : IServiceExecutor<DispatchResult, Session>, new()
        {
            return new Server<Executor, DispatchResult, Session>(
                new SocketCommunicationResolver<DispatchResult, Session>(
                    address: address,
                    port: port,
                    dispatcher: new AlphaProtocolDispatcher<DispatchResult, Session>()
                )
             );
        }
    }
}
