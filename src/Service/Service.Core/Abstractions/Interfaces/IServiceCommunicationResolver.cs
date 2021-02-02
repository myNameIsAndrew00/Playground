using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Interfaces
{
    public interface IServiceCommunicationResolver
    {
        void Listen();

        event Func<byte[],byte[]> OnCommunicationCreated;
    }
}
