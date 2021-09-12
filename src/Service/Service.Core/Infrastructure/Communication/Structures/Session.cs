using Service.Core.Infrastructure.Storage.Structures;
using Service.Core.Infrastructure.Token.Structures;
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
        private Dictionary<uint, Pkcs11ObjectHandler> sessionObjects = new Dictionary<uint, Pkcs11ObjectHandler>();

        public uint Id { get; }
    
        public bool Authenticated { get; private set; }

        public Session(uint id)
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

        public long AddSesionObject(Pkcs11ObjectHandler pkcs11Object)
        {
            //todo: check type of pkcs11object. 

            uint nextId = Utils.GetNextId();

            this.sessionObjects.Add(nextId, pkcs11Object);

            return nextId;
        }

        public Pkcs11ObjectHandler GetSessionObject(uint id)
        {
            this.sessionObjects.TryGetValue(id, out Pkcs11ObjectHandler @object);
            
            return @object;
        }

        public void Dispose()
        {
            //todo: add disposing code here
            return;
        }
    }
}
