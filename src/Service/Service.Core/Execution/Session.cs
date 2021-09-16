﻿using Service.Core.Abstractions.Execution;
using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using Service.Core.Storage.Memory;
using Service.Core.Token.Encryption;
using Service.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Execution
{
    /// <summary>
    /// Encapsulates data available for a session
    /// </summary>
    public class Session : ISession
    {
        public Session() { }

        private Dictionary<ulong, IMemoryObject> sessionObjects = new Dictionary<ulong, IMemoryObject>();

        public ulong Id { get; init; }
    
        public bool Authenticated { get; private set; }

        /// <summary>
        /// Represents the session key object registered for encryption.
        /// When a encryption session object is added or updated via register, last value added will be provided by this reference.
        /// </summary>
        public EncryptionContext RegisteredEncryptionContext { get; private set; }

        public Session(ulong id)
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


        public IMemoryObject GetSessionObject(ulong id)
        {
            this.sessionObjects.TryGetValue(id, out IMemoryObject @object);

            return @object;
        }

        /// <summary>
        /// Add an object to the current session in memory storage
        /// </summary>
        /// <param name="pkcs11Object">Object which will be added to the current session</param>
        /// <returns></returns>
        public ulong AddSesionObject(IMemoryObject pkcs11Object)
        {
            //todo: check type of pkcs11object. 

            ulong nextId = Utils.GetNextId();

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
        public IMemoryObject UpdateSessionObject(ulong id, IMemoryObject pkcs11Object)
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
        public IMemoryObject UpdateAndRegisterSesionObject(ulong id, IMemoryObject pkcs11Object)
        {
            IMemoryObject updateResult = this.UpdateSessionObject(id, pkcs11Object);

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

        private void updateRegisteredObjects(IMemoryObject objectHandler)
        {
            if (objectHandler is EncryptionContext) RegisteredEncryptionContext = objectHandler as EncryptionContext;
        }

        #endregion
    }
}