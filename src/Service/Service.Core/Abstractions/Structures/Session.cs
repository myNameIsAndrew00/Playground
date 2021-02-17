using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Structures
{
    /// <summary>
    /// Encapsulates data available for a session
    /// </summary>
    public class Session
    {        

        public byte Id { get; }
    
        public bool Authenticated { get; private set; }

        public Session(byte id)
        {
            this.Id = id;
            this.Authenticated = false;
        }

        public bool Authenticate(string username, string password)
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
            return Id;
        }
    }
}
