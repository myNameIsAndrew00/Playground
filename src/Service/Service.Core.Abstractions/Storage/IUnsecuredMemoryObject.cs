using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Storage
{
    public interface IUnsecuredMemoryObject : IDisposable
    {
        public IEnumerable<IDataContainer<Pkcs11Attribute>> Attributes { get; set; }
    }
}
