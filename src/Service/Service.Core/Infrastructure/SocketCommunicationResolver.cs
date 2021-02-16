using Service.Core.Abstractions.Interfaces;
using Service.Core.Abstractions.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Infrastructure
{
    /// <summary>
    /// Resolve the communication between client and service using sockets.
    /// The resolver will listen on port which was provided on construction
    /// </summary>
    internal class SocketCommunicationResolver : IServiceCommunicationResolver
    {
        private const int RECEIVING_BUFFER_SIZE = 4096;

        private TcpListener server;
        private byte[] receivingBuffer = new byte[RECEIVING_BUFFER_SIZE];

        public IServiceDispatcher Dispatcher { get; }

        public event Func<DispatchResult, byte[]> OnCommunicationCreated;        
        
        public SocketCommunicationResolver(string address, int port, IServiceDispatcher dispatcher)
        {
            this.Dispatcher = dispatcher;

            IPAddress addressObject = IPAddress.Parse(address);

            server = new TcpListener(addressObject, port);
        }

        public void Listen()
        {
            server.Start();

            while (true) {
                TcpClient client = server.AcceptTcpClient();

                Task.Run(() => handleClient(client));
            }
        }


        #region Private

        private void handleClient(TcpClient client)
        {
            NetworkStream clientStream = client.GetStream();
            int handledBytesCount;

            using (MemoryStream writingStream = new MemoryStream())
            { 
                while ((handledBytesCount = clientStream.Read(receivingBuffer, 0, receivingBuffer.Length)) != 0)
                {
                    writingStream.Write(receivingBuffer, 0, handledBytesCount);
                }

                DispatchResult dispatchResult = Dispatcher.Dispatch(writingStream.ToArray());

                var returnBytes = OnCommunicationCreated?.Invoke(dispatchResult);

                clientStream.Write(returnBytes, 0, returnBytes.Length);                
            }

            client.Close();
            client.Dispose();
        }
 

        #endregion
    }
}
