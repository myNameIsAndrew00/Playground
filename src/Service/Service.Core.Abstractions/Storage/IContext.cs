using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Storage
{
    /// <summary>
    /// Provides methods and properties for an context object. An context object is used during a mechanism execution or inside a token module.
    /// </summary>
    public interface IContext : IMemoryObject
    {
        /// <summary>
        /// Represents the object which store the actualy sensitive data
        /// </summary>
        public IMemoryObject MemoryObject { get; }


        IEnumerable<IDataContainer<Pkcs11Attribute>> IMemoryObject.Attributes
        {
            get => MemoryObject.Attributes;
            set => MemoryObject.Attributes = value;
        }


        void IMemoryObject.SetId(ulong id) => MemoryObject.SetId(id);
               
        ulong IMemoryObject.Id => MemoryObject.Id;
               
        void IDisposable.Dispose() => MemoryObject.Dispose();        
               
        IDataContainer<Pkcs11Attribute> IMemoryObject.this[Pkcs11Attribute type]
        {
            get
            {
                return MemoryObject[type];
            }
        }
    }

}
