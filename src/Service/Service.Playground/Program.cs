using System;
using System.Net;
using Service.Core.Abstractions;
using Service.Core.Abstractions.Interfaces;

namespace Service.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            IServer server = ServerFactory.CreateDefaultSocketServer(IPAddress.Any.ToString(), 5123);

            server.Start();

            Console.WriteLine("Hello World!");
        }
    }
}
