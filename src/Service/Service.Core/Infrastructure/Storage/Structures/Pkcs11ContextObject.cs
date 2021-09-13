using Service.Core.Abstractions.Token.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Core.Infrastructure.Storage.Structures
{
    /// <summary>
    /// Represents an model of a pcks11 object. A pkcs11 object context represents an inmemory storage class which store a collection of attributes.
    /// It is used to handle internal operations for different modules.
    /// </summary>
    public class Pkcs11ContextObject : IDisposable
    {
        internal Pkcs11ContextObject() { }

        internal Pkcs11ContextObject(IEnumerable<DataContainer<Pkcs11Attribute>> attributes)
        {
            this.Attributes = attributes;
        }

        public virtual void SetId(uint id) => this.Id = id;

        public virtual uint Id { get; private set; }

        public virtual IEnumerable<DataContainer<Pkcs11Attribute>> Attributes { get; set; }

        public virtual void Dispose()
        {
        }

        public virtual DataContainer<Pkcs11Attribute> this[Pkcs11Attribute type]
        {
            get
            {
                return Attributes.Where(attribute => attribute.Type == type).FirstOrDefault();
            }
        }
    }

    /// <summary>
    /// Represents a decorator class for Pkcs11ObjectHandler
    /// </summary>
    public abstract class Pkcs11ContextObjectDecorator : Pkcs11ContextObject
    {
        public Pkcs11ContextObject ObjectHandler { get; }

        public Pkcs11ContextObjectDecorator(Pkcs11ContextObject objectHandler)
        {
            this.ObjectHandler = objectHandler;
        }

        public override IEnumerable<DataContainer<Pkcs11Attribute>> Attributes
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

        public override DataContainer<Pkcs11Attribute> this[Pkcs11Attribute type]
        {
            get
            {
                return ObjectHandler.Attributes.Where(attribute => attribute.Type == type).FirstOrDefault();
            }
        }

    }
}
