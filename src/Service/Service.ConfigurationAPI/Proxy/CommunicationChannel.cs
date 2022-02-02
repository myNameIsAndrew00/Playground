using Newtonsoft.Json;
using Service.Core.Configuration.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json.Serialization;
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

        public async Task<ReturnType> Send<ReturnType>(ProxyMessage<ReturnType> message)
        {
            // open a pipe with proxy
            using NamedPipeClientStream pipeClientStream = new NamedPipeClientStream(".", parentPipe);

            await pipeClientStream.ConnectAsync();

            // create reader and writer
            StreamWriter writer = new StreamWriter(pipeClientStream);
            StreamReader reader = new StreamReader(pipeClientStream);

            // send the command message
            writer.WriteLine(message.Name);
            writer.Flush();

            // receive the encoded json object 
            string encodedObject = reader.ReadLine();

            pipeClientStream.WaitForPipeDrain();
            pipeClientStream.Close();

            return JsonConvert.DeserializeObject<ReturnType>(encodedObject, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
        }

    }
}
