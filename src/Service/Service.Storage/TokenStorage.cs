using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Storage
{
    /// <summary>
    /// Default implementation for token storage
    /// </summary>
    public class TokenStorage : ITokenStorage
    {
        private PayloadDataParser dataParser = new PayloadDataParser();

        public bool CreateInMemoryObject(IEnumerable<IPkcs11AttributeDataContainer> attributes, out IMemoryObject createdObject, out ExecutionResultCode code)
        {
            return MemoryObjectsBuilder.Instance.Get(attributes, out createdObject, out code);
        }

        public int CreatePkcs11DataContainer(IEnumerable<byte> bytes, Type enumType, out object output) => dataParser.TryParsePkcs11DataContainer(bytes, enumType, out output);
        

        public int CreatePkcs11DataContainerCollection(IEnumerable<byte> bytes, Type enumType, out object output) => dataParser.TryParsePkcs11DataContainerCollection(bytes, enumType, out output);
    }
}
