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
    /// The resolver will listen on port which was provided on construction and will use first 4 bytes in the receiving packet to get the packet size
    /// </summary>
    internal class SocketCommunicationResolver : IServiceCommunicationResolver
    {
        ///Handles maximum number of bytes which can be read from a client at once before keep them in memory
        private const int RECEIVING_BUFFER_SIZE = 4096;
        
        ///Handles the size of a packet header. For now, header are 4 bytes which specifies the size of the packet
        private const int PACKET_HEADER_SIZE = 4;
        
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
            int packetSize = readPacketSize(clientStream);
            int handledBytesCount = 0;
            int readBytesCount = 0;

            using (MemoryStream memoryStream = new MemoryStream())
            {
               
                while (readBytesCount < packetSize && 
                    (handledBytesCount = clientStream.Read(receivingBuffer, 0, receivingBuffer.Length)) != 0)
                {
                    memoryStream.Write(receivingBuffer, 0, handledBytesCount);
                    readBytesCount += handledBytesCount;
                }

                DispatchResult dispatchResult = Dispatcher.Dispatch(memoryStream.ToArray());

                var returnBytes = OnCommunicationCreated?.Invoke(dispatchResult);

                clientStream.Write(returnBytes, 0, returnBytes.Length);                
            }

            client.Close();
            client.Dispose();
        }

        private int readPacketSize(NetworkStream clientStream)
        {
            byte[] entirePacketSize = new byte[PACKET_HEADER_SIZE];

            clientStream.Read(entirePacketSize, 0, PACKET_HEADER_SIZE);

            if (BitConverter.IsLittleEndian) Array.Reverse(entirePacketSize);

            return BitConverter.ToInt32(entirePacketSize, 0);
        }



        #endregion
    }
}
