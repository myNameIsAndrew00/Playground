using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Storage
{
    /// <summary>
    /// Implements methods which can be used to generate mechanism containers from input data
    /// </summary>
    public interface IMechanismDataContainerBuilder
    {
        IMechanismDataContainer GetDefault(IDataContainer<Pkcs11Mechanism> dataContainer);
    }
}
