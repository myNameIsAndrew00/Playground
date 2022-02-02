using Newtonsoft.Json;
using Service.Core.Abstractions.Communication;
using Service.Core.Abstractions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Configuration.CommandsHandlers
{
    internal abstract class ProxyMessageHandler
    {
        protected IConfigurablePkcs11Server Server;

        internal ProxyMessageHandler(IConfigurablePkcs11Server server)
        {
            this.Server = server;
        }

        internal abstract string Execute();

        protected string Serialize<ReturnType>(ReturnType data)
        {
            return JsonConvert.SerializeObject(data, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
        }
    }
}
