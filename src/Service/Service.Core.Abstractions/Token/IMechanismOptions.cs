using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Token
{
    /// <summary>
    /// Implements properties and method to provide options of a specific mechanism
    /// </summary>
    public interface IMechanismOptions
    {
        public IDataContainer<Pkcs11Mechanism> Data { get; }
    }
}
