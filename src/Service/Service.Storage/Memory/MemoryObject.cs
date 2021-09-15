using Service.Core.Abstractions.Storage;
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
        internal MemoryObject() { }

        internal MemoryObject(IEnumerable<IDataContainer<Pkcs11Attribute>> attributes)
        {
            this.Attributes = attributes;
        }

        public virtual void SetId(ulong id) => this.Id = id;

        public virtual ulong Id { get; private set; }

        public virtual IEnumerable<IDataContainer<Pkcs11Attribute>> Attributes { get; set; }

        public virtual void Dispose()
        {
        }

        public virtual IDataContainer<Pkcs11Attribute> this[Pkcs11Attribute type]
        {
            get
            {
                return Attributes?.Where(attribute => attribute.Type == type).FirstOrDefault();
            }
        }
    }

    /// <summary>
    /// Represents a decorator class for Pkcs 11 memory objects
    /// </summary>
    public abstract class ContextDecorator : IMemoryObject
    {
        public IMemoryObject ObjectHandler { get; }

        public ContextDecorator(IMemoryObject objectHandler)
        {
            this.ObjectHandler = objectHandler;
            //todo: replace here
        }

        public IEnumerable<IDataContainer<Pkcs11Attribute>> Attributes
        {
            get => ObjectHandler.Attributes;
            set => ObjectHandler.Attributes = value;
        }

        public void SetId(ulong id) => ObjectHandler.SetId(id);
               
        public ulong Id => ObjectHandler.Id;
               
        public void Dispose()
        {
            ObjectHandler.Dispose();
        }

        public IDataContainer<Pkcs11Attribute> this[Pkcs11Attribute type]
        {
            get
            {
                return ObjectHandler[type];
            }
        }

    }
}
