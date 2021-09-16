using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Abstractions.Storage
{
    /// <summary>
    /// Implements methods which can be used to generate data containers from input bytes provided in service payload
    /// </summary>
    public interface IPayloadDataParser
    {
        int CreatePkcs11DataContainer(IEnumerable<byte> bytes, Type enumType, out object output);

        int CreatePkcs11DataContainerCollection(IEnumerable<byte> bytes, Type enumType, out object output);
    }
}
