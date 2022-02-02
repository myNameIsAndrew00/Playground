using System;
using System.Net;
using Service.Core;
using Service.Core.Abstractions;
using Service.Core.Abstractions.Communication;
using Service.Core.Configuration;
using Service.Core.Infrastructure;

namespace Service.Playground
{
    class Program
    {

        static void Main(string[] args)
        {

            IPkcs11Server server = ServerFactory.CreateDefaultServer("127.0.0.1", 5123)
                                                .SetConfigurationAPI(configurableServer => new ConfigurationAPIProxy(configurableServer, @"..\..\..\..\Service.ConfigurationAPI\bin\Debug\net5.0\Service.ConfigurationAPI.exe"));
 
            
            Console.WriteLine("Waiting for clients...");
             
            server.Start();

            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();
                Console.WriteLine(keyinfo.Key + " was pressed");
            }
            while (keyinfo.Key != ConsoleKey.X);

            server.Stop();

            server.Dispose();
        }
    }
}
