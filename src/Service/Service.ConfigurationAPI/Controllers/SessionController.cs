﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.ConfigurationAPI.Models;
using Service.ConfigurationAPI.Proxy;
using Service.ConfigurationAPI.Response;
using Service.Core.Configuration.Commands;
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
        private IMapper mapper;

        public SessionController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromServices] CommunicationChannel communicationChannel)
        {
            var sessions = await communicationChannel.Send(ProxyMessage.SessionsRequest);

            return this.StandardResponse(mapper.Map<List<SessionDTO>>(sessions));
        }
    }
}
