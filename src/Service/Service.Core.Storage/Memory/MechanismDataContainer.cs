using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Storage.Memory
{
    //todo: make abstract and implement concret containers
    public class MechanismDataContainer : IMechanismDataContainer
    {
        public IDataContainer<Pkcs11Mechanism> Data { get; }

        public MechanismDataContainer(IDataContainer<Pkcs11Mechanism> mechanismData)
        {
            this.Data = mechanismData;
        }

    }
}
