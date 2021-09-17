using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Storage.Mechanism
{
    internal class AesMechanismOptions : MechanismOptions, IAesMechanismOptions
    {
        public AesMechanismOptions(IDataContainer<Pkcs11Mechanism> mechanismData) : base(mechanismData)
        {
        }
    }
}
