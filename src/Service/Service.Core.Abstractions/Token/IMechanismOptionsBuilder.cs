using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Token
{
    /// <summary>
    /// Implements methods which can be used to generate mechanism containers from input data
    /// </summary>
    public interface IMechanismOptionsBuilder
    {
        IMechanismOptions GetDefault(IDataContainer<Pkcs11Mechanism> dataContainer);

        IAesMechanismOptions GetAes(IDataContainer<Pkcs11Mechanism> dataContainer);
    }
}
