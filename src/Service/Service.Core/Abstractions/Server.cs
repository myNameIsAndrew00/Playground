using Service.Core.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;

namespace Service.Core.Abstractions
{
    /// <summary>
    /// A class which handles received connections
    /// </summary>
    public class Server<Resolver> 
        where Resolver : IServiceCommunicationResolver
    {
        private const string PIPE_NAME = "PKCS11PIPE";


        private Resolver resolver;

        public void Start()
        {
            resolver.OnCommunicationCreated += OnCommunicationCreated;

            resolver.Listen();           
        }

        #region Private

        private byte[] OnCommunicationCreated(byte[] clientInput)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
