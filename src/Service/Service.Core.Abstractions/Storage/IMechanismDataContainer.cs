using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Storage
{
    /// <summary>
    /// Stores data about a mechanism
    /// </summary>
    public interface IMechanismDataContainer
    {
        public IDataContainer<Pkcs11Mechanism> Data { get; }
    }
}
