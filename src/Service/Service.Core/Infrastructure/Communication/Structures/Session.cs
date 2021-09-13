using Service.Core.Abstractions.Token.DefinedTypes;
using Service.Core.Infrastructure.Storage.Structures;
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
        private Dictionary<uint, Pkcs11ContextObject> sessionObjects = new Dictionary<uint, Pkcs11ContextObject>();

        public uint Id { get; }
    
        public bool Authenticated { get; private set; }

        /// <summary>
        /// Represents the session key object registered for encryption.
        /// When a encryption session object is added or updated via register, last value added will be provided by this reference.
        /// </summary>
        public EncryptionContext RegisteredEncryptionContext { get; private set; }

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


        public Pkcs11ContextObject GetSessionObject(uint id)
        {
            this.sessionObjects.TryGetValue(id, out Pkcs11ContextObject @object);

            return @object;
        }

        /// <summary>
        /// Add an object to the current session in memory storage
        /// </summary>
        /// <param name="pkcs11Object">Object which will be added to the current session</param>
        /// <returns></returns>
        public uint AddSesionObject(Pkcs11ContextObject pkcs11Object)
        {
            //todo: check type of pkcs11object. 

            uint nextId = Utils.GetNextId();

            this.sessionObjects.Add(nextId, pkcs11Object);

            pkcs11Object.SetId(nextId);

            return nextId;
        }

        /// <summary>
        /// Update an object contained by the current session.
        /// </summary>
        /// <param name="id">Id of the object which will be update</param>
        /// <param name="pkcs11Object">Object which will update the current session object</param>
        /// <returns></returns>
        public Pkcs11ContextObject UpdateSessionObject(uint id, Pkcs11ContextObject pkcs11Object)
        {
            if (this.sessionObjects.ContainsKey(id))
            {
                this.sessionObjects[id] = pkcs11Object;

                return pkcs11Object;
            }
            return null;
        }

        /// <summary>
        /// Update an object contained by the current session and register to the session registered handlers.
        /// </summary>
        /// <param name="id">Id of the object which will be update</param>
        /// <param name="pkcs11Object">Object which will update the current session object</param>
        /// <returns></returns>
        public Pkcs11ContextObject UpdateAndRegisterSesionObject(uint id, Pkcs11ContextObject pkcs11Object)
        {
            Pkcs11ContextObject updateResult = this.UpdateSessionObject(id, pkcs11Object);

            if (updateResult != null)
            {
                updateRegisteredObjects(pkcs11Object);
                return updateResult;
            }

            return updateResult;
        }

        public void ResetRegisteredEncryptionObject() => RegisteredEncryptionContext = null;

   

        public void Dispose()
        {
            foreach (var @object in sessionObjects)
                @object.Value.Dispose();

            //todo: add disposing code here
            return;
        }

        public override bool Equals(object obj)
        {
            return (obj as Session)?.Id.Equals(Id) ?? false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #region Private

        private void updateRegisteredObjects(Pkcs11ContextObject objectHandler)
        {
            if (objectHandler is EncryptionContext) RegisteredEncryptionContext = objectHandler as EncryptionContext;
        }

        #endregion
    }
}
