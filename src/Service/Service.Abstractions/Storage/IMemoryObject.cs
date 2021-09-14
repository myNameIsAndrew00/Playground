using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Storage
{
    /// <summary>
    /// Represents an model of a pcks11 object. A pkcs11 object context represents an inmemory storage class which store a collection of attributes.
    /// It is used to handle internal operations for different modules.
    /// </summary>
    public interface IMemoryObject : IDisposable
    {
        void SetId(uint id);

        uint Id { get; }

        IEnumerable<IDataContainer<Pkcs11Attribute>> Attributes { get; set; }

        IDataContainer<Pkcs11Attribute> this[Pkcs11Attribute type] { get;  }

    }
}
