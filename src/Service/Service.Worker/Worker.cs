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
    public class Worker : BackgroundService, IDisposable
    {
        private readonly IPkcs11Server server;

        public Worker(IPkcs11Server server)
        {
            this.server = server;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            server.Start();

            return Task.Run(() =>
           {
               while (!stoppingToken.IsCancellationRequested)
               {
               }
           });
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            server.Stop();
        }

        public override void Dispose()
        {
            base.Dispose();

            server.Dispose();
        }
    }
}
