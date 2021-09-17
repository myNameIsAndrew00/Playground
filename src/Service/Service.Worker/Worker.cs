using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.Core.Abstractions.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IPkcs11Server server;

        public Worker(IPkcs11Server server)
        {
            this.server = server;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            server.Start();

            while (!stoppingToken.IsCancellationRequested)
            { 
            }
        }
    }
}
