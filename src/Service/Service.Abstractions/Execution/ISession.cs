using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Execution
{
    public interface ISession : IDisposable
    {
        ulong Id { get; init; }

        bool Authenticate(Pkcs11UserType userType, string password);
    }
}
