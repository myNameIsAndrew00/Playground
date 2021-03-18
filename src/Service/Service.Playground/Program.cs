using System;
using System.Net;
using Service.Core.Abstractions;
using Service.Core.Abstractions.Communication.Interfaces;

namespace Service.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            IPkcs11Server server = ServerFactory.CreateDefaultSocketServer("127.0.0.1", 5123);

            Console.WriteLine("Waiting for clients...");

            server.Start();

        }
    }
}
