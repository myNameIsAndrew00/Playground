using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Storage.Mechanism
{
    //todo: make abstract and implement concret containers
    internal class MechanismOptions : IMechanismOptions
    {
        public IDataContainer<Pkcs11Mechanism> Data { get; }

        public MechanismOptions(IDataContainer<Pkcs11Mechanism> mechanismData)
        {
            this.Data = mechanismData;
        }

    }
}
