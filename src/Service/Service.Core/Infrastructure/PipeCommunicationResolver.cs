using Service.Core.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace Service.Core.Infrastructure
{
    internal class PipeCommunicationResolver : IServiceCommunicationResolver
    {
        public event Func<byte[], byte[]> OnCommunicationCreated;
        
        private NamedPipeServerStream pipe;
        private string pipeName;

        public PipeCommunicationResolver(string pipeName)
        {
            this.pipeName = pipeName;
        }

        public void Listen()
        {
            while (true) {
                pipe = new NamedPipeServerStream(pipeName, PipeDirection.InOut);

                pipe.BeginWaitForConnection(OnConnected, null);
            }
        }


        #region Private

        private void OnConnected(IAsyncResult result)
        {
            pipe.EndWaitForConnection(result);

            using (var memoryStream = new MemoryStream())
            {
                pipe.CopyTo(memoryStream);
                
            }
        }
 

        #endregion
    }
}
