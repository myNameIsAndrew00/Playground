using Service.Core.Abstractions.Communication.Interfaces;
using Service.Core.Abstractions.Communication.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Communication.Infrastructure
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

        ///Represents the value which will trigger server to close connection
        private const int CONNECTION_CLOSE_TRIGGER = -1;

        private const byte ERROR_BYTE = 0x01;

        private TcpListener server;
        private byte[] receivingBuffer = new byte[RECEIVING_BUFFER_SIZE];

        public IServiceProtocolDispatcher Dispatcher { get; }

        public event Func<DispatchResult, IExecutionResult> OnCommunicationCreated;

        public event Action<Exception> OnClientConnectionError;

        public event Action<Exception> OnRequestHandlingError;

        public SocketCommunicationResolver(string address, int port, IServiceProtocolDispatcher dispatcher)
        {
            this.Dispatcher = dispatcher;

            IPAddress addressObject = IPAddress.Parse(address);

            server = new TcpListener(addressObject, port);
        }

        public void Listen()
        {
            server.Start();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();

                Task.Run(() =>
                {
                    try
                    {
                        handleClient(client);
                    }
                    catch(Exception exception)
                    {
                        OnClientConnectionError?.Invoke(exception);
                    }
                });
            }
        }


        #region Private

        private void handleClient(TcpClient client)
        {
            NetworkStream clientStream = client.GetStream();
            
            while (true)
            {
                int handledBytesCount = 0;
                int readBytesCount = 0;
                int packetSize = readPacketSize(clientStream);

                if (packetSize == CONNECTION_CLOSE_TRIGGER) break;

                using (MemoryStream memoryStream = new MemoryStream())
                {

                    while (readBytesCount < packetSize &&
                        (handledBytesCount = clientStream.Read(receivingBuffer, 0, receivingBuffer.Length)) != 0)
                    {
                        memoryStream.Write(receivingBuffer, 0, handledBytesCount);
                        readBytesCount += handledBytesCount;
                    }

                    try
                    {
                        DispatchResult dispatchResult = Dispatcher.DispatchClientRequest(memoryStream.ToArray());

                        IExecutionResult executionResult = OnCommunicationCreated?.Invoke(dispatchResult);
                        byte[] executionResultBytes = Dispatcher.BuildClientResponse(executionResult);

                        byte[] returnBytes = buildReturnBytes(executionResultBytes);

                        clientStream.Write(returnBytes, 0, returnBytes.Length);
                    }
                    catch (Exception exception)
                    {
                        OnRequestHandlingError?.Invoke(exception);

                        //replace this with error byte handling
                        clientStream.Write(BitConverter.GetBytes(ERROR_BYTE), 0, 1);
                    }
                }
            }

            client.Close();
            client.Dispose();
        }

        /// <summary>
        /// Read the size of the packet from the first 4 PACHET_HEADER_SIZE bytes
        /// </summary>
        /// <param name="clientStream">Stream which provided by client</param>
        /// <returns></returns>
        private int readPacketSize(NetworkStream clientStream)
        {
            byte[] entirePacketSize = new byte[PACKET_HEADER_SIZE];

            clientStream.Read(entirePacketSize, 0, PACKET_HEADER_SIZE);

            if (BitConverter.IsLittleEndian) Array.Reverse(entirePacketSize);

            return BitConverter.ToInt32(entirePacketSize, 0);
        } 

        private byte[] buildReturnBytes(byte[] executionResultBytes)
        { 
            int packetSize = executionResultBytes.Length;

            byte[] resultBytes = new byte[sizeof(int) + packetSize];
            byte[] headerBytes = BitConverter.GetBytes(packetSize);

            if (BitConverter.IsLittleEndian) Array.Reverse(headerBytes);

            headerBytes.CopyTo(resultBytes, 0); 
            executionResultBytes.CopyTo(resultBytes, sizeof(int));

            return resultBytes;
        }


        #endregion
    }
}
