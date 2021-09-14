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
    public class MemoryObject : IMemoryObject
    {
        internal MemoryObject() { }

        internal MemoryObject(IEnumerable<IPkcs11AttributeDataContainer> attributes)
        {
            this.Attributes = attributes;
        }

        public virtual void SetId(uint id) => this.Id = id;

        public virtual uint Id { get; private set; }

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
    public abstract class ContextDecorator : MemoryObject
    {
        public MemoryObject ObjectHandler { get; }

        public ContextDecorator(IMemoryObject objectHandler)
        {
            this.ObjectHandler = objectHandler as MemoryObject;
            //todo: replace here
        }

        public override IEnumerable<IDataContainer<Pkcs11Attribute>> Attributes
        {
            get => ObjectHandler.Attributes;
            set => ObjectHandler.Attributes = value;
        }

        public override void SetId(uint id) => ObjectHandler.SetId(id);
        
        public override uint Id => ObjectHandler.Id;

        public override void Dispose()
        {
            ObjectHandler.Dispose();
        }

        public override IDataContainer<Pkcs11Attribute> this[Pkcs11Attribute type]
        {
            get
            {
                return ObjectHandler[type];
            }
        }

    }
}
