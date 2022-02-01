using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Core;
using Service.Core.Abstractions.Communication;
using Service.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService(options =>
                {
                    options.ServiceName = ".NET Pkcs11 Token Service";                   
                })                 
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddScoped<IPkcs11Server>(serviceProvider =>
                    {
                        IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        return ServerFactory.CreateDefaultServer("127.0.0.1", int.Parse(configuration["Pkcs11Service:Port"]))
                                            .SetConfiguratorAPI(new ConfigurationAPI(configuration["Pkcs11ConfigurationService:Path"]));
                    });
                });
    }
}
