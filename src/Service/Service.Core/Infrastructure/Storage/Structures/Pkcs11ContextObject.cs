using Service.Core.Infrastructure.Token.Structures;
using System;
using System.Collections.Generic;
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

        public virtual uint Id { get; }

        public virtual IEnumerable<DataContainer<Pkcs11Attribute>> Attributes { get; set; }

        public virtual void Dispose()
        {
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

        public override uint Id => ObjectHandler.Id;

        public override void Dispose()
        {
            ObjectHandler.Dispose();
        }
    }
}
