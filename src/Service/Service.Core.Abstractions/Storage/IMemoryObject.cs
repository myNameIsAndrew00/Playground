using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Storage
{
    /// <summary>
    /// Represents an model of a in memory object.
    /// Implements a secure store environment for a collection of attributes.
    /// It is used to handle store sensitive data (attributes) for certain operations for different modules.
    /// </summary>
    public interface IMemoryObject : IDisposable
    {
        void SetId(ulong id);

        ulong Id { get; }

        IEnumerable<IDataContainer<Pkcs11Attribute>> Attributes { get; set; }

        IDataContainer<Pkcs11Attribute> this[Pkcs11Attribute type] { get;  }

    }
}
