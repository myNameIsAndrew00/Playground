﻿using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Core.Storage.Memory
{
    /// <summary>
    /// Represents an model of a pcks11 object. A pkcs11 object context represents an inmemory storage class which store a collection of attributes.
    /// It is used to handle internal operations for different modules.
    /// </summary>
    internal class MemoryObject : IMemoryObject
    {
        //todo: this should be encrypted or stored in a secure manner
        protected IEnumerable<IDataContainer<Pkcs11Attribute>> attributes;

        internal MemoryObject() { }

        internal MemoryObject(IEnumerable<IDataContainer<Pkcs11Attribute>> attributes)
        {
            this.attributes = attributes;
        }

        public virtual void SetId(ulong id) => this.Id = id;

        public virtual ulong Id { get; private set; }
        
        public virtual void Dispose()
        {
        }

        //todo: implement unsecure method properly.
        public virtual IUnsecuredMemoryObject Unsecure() => new UnsecuredMemoryObject(attributes);

        public virtual IDataContainer<Pkcs11Attribute> this[Pkcs11Attribute type]
        {
            get
            {
                return attributes?.Where(attribute => attribute.Type == type).FirstOrDefault();
            }
        }
    }
     
}
