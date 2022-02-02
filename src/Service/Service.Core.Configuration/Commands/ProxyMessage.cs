using Service.Core.Abstractions.Communication;
using Service.Core.Abstractions.Configuration;
using Service.Core.Abstractions.Execution;
using Service.Core.Configuration.CommandsHandlers;
using Service.Core.Configuration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Configuration.Commands
{
    public class ProxyMessage
    {
        public static Dictionary<string, ProxyMessage> Messages;

        #region Defined messages
        
        /// <summary>
        /// Represents a type of message which can be sent to proxy for receiving sessions.
        /// </summary>
        public static ProxyMessage<List<SessionDAO>> SessionsRequest = new ProxyMessage<List<SessionDAO>>(
            name: "__SESSION_REQUEST", 
            handlersFactory: pkcs11Server => new SessionsRequestMessageHandler(pkcs11Server) 
         );


        #endregion

        static ProxyMessage()
        {
            Messages = new Dictionary<string, ProxyMessage>()
            {
                [SessionsRequest.Name] = SessionsRequest
            };
        }

        internal ProxyMessage(string name, Func<IConfigurablePkcs11Server, ProxyMessageHandler> handlersFactory)
        {
            this.Name = name;

            this.HandlerFactory = handlersFactory;
        }

        public string Name { get; }

        internal Func<IConfigurablePkcs11Server, ProxyMessageHandler> HandlerFactory;

    }
    /// <summary>
    /// Represents a message which can be handled by proxy and communicated between processes
    /// </summary>
    public class ProxyMessage<ResultType> : ProxyMessage
    { 
        internal ProxyMessage(string name, Func<IConfigurablePkcs11Server, ProxyMessageHandler> handlersFactory) : base(name, handlersFactory)
        {
        }


    }
}
