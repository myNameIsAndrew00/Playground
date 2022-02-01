using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.ConfigurationAPI.Proxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Service.ConfigurationAPI.Controllers
{
    [ApiController]
    [Route("session")]
    public class SessionController : ControllerBase
    {

        [HttpGet]
        public async Task<JsonResult> Get([FromServices] CommunicationChannel communicationChannel)
        {           
            string resposne =  await communicationChannel.Send(Guid.NewGuid().ToString());

            return new JsonResult(new { data = resposne });
        }
    }
}
