using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Storage.Memory
{
    internal class UnsecuredMemoryObject : IUnsecuredMemoryObject
    {
        public UnsecuredMemoryObject(IEnumerable<IDataContainer<Pkcs11Attribute>> attributes)
        {
            this.Attributes = attributes;
        }

        public IEnumerable<IDataContainer<Pkcs11Attribute>> Attributes { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
