using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Execution
{
    public interface IReadOnlySession: IDisposable
    {
        ulong Id { get; init; }

        bool Authenticate(Pkcs11UserType userType, string password);

        bool Closed { get; }
    }

    /// <summary>
    /// Provides properties and methods to Handle a token session.
    /// </summary>
    public interface ISession : IReadOnlySession
    {
     
        /// <summary>
        /// Close the current session using a session handler.
        /// </summary>
        /// <param name="sessionHandler"></param>
        /// <returns>A boolean which indicate if the close operation succeed</returns>
        bool Close(IAllowCloseSession sessionHandler);
    }
}
