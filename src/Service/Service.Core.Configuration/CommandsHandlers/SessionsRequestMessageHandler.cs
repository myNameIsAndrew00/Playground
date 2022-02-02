using Service.Core.Abstractions.Communication;
using Service.Core.Abstractions.Configuration;
using Service.Core.Abstractions.Execution;
using Service.Core.Configuration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Configuration.CommandsHandlers
{
    internal class SessionsRequestMessageHandler : ProxyMessageHandler
    {
        internal SessionsRequestMessageHandler(IConfigurablePkcs11Server server) : base(server)
        {
        }

        internal override  string Execute() => Serialize(Server.GetSessions().Select(session => new SessionDAO(session)));
       
    }
}
