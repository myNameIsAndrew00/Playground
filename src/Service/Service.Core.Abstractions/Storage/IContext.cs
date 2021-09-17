using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Storage
{
    /// <summary>
    /// Provides methods and properties for an context object
    /// </summary>
    public interface IContext : IMemoryObject
    {
        public IMemoryObject ObjectHandler { get; }


        IEnumerable<IDataContainer<Pkcs11Attribute>> IMemoryObject.Attributes
        {
            get => ObjectHandler.Attributes;
            set => ObjectHandler.Attributes = value;
        }


        void IMemoryObject.SetId(ulong id) =>ObjectHandler.SetId(id);
               
        ulong IMemoryObject.Id => ObjectHandler.Id;
               
        void IDisposable.Dispose() => ObjectHandler.Dispose();        
               
        IDataContainer<Pkcs11Attribute> IMemoryObject.this[Pkcs11Attribute type]
        {
            get
            {
                return ObjectHandler[type];
            }
        }
    }

}
