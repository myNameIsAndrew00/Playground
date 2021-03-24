using Service.Core.Abstractions.Storage.Structures;
using Service.Core.Abstractions.Token.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Communication.Structures
{
    /// <summary>
    /// Encapsulates data available for a session
    /// </summary>
    public class Session : IDisposable
    {
        private Dictionary<long, Pkcs11Object> sessionObjects = new Dictionary<long, Pkcs11Object>();

        public long Id { get; }
    
        public bool Authenticated { get; private set; }

        public Session(long id)
        {
            this.Id = id;
            this.Authenticated = false;
        }

        public bool Authenticate(Pkcs11UserType userType, string password)
        {
            //todo: handle authentication
            Authenticated = false;
            return false;
        }

        public override bool Equals(object obj)
        {
            return (obj as Session)?.Id.Equals(Id) ?? false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public long AddSesionObject(Pkcs11Object pkcs11Object)
        {
            long nextId = Extensions.GetNextId();

            this.sessionObjects.Add(nextId, pkcs11Object);

            return nextId;
        }

        public void Dispose()
        {
            //todo: add disposing code here
            return;
        }
    }
}
