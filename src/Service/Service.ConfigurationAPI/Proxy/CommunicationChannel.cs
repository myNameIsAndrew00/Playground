using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;

namespace Service.ConfigurationAPI.Proxy
{
    /// <summary>
    /// Represents the channel used to communicate with ConfigurationAPIProxy
    /// </summary>
    public class CommunicationChannel
    {
        private string parentPipe;
        public CommunicationChannel(string parentPipe)
        {
            this.parentPipe = parentPipe;
        }

        public async Task<string> Send(string request)
        {
            using NamedPipeClientStream pipeClientStream = new NamedPipeClientStream(".", parentPipe);

            await pipeClientStream.ConnectAsync();

            StreamWriter writer = new StreamWriter(pipeClientStream);
            StreamReader reader = new StreamReader(pipeClientStream);

            writer.WriteLine(request);
            writer.Flush();

            string result = reader.ReadLine();

            pipeClientStream.WaitForPipeDrain();

            return result;
        }

    }
}
