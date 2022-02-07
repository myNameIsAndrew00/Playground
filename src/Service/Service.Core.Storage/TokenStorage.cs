using Service.Core.Abstractions.Logging;
using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.DefinedTypes;
using Service.Core.Storage.Mechanism;
using Service.Core.Storage.Memory;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Service.Core.Abstractions.Logging.IAllowLogging;

namespace Service.Core.Storage
{
    /// <summary>
    /// Default implementation for token storage
    /// </summary>
    public class TokenStorage : ITokenStorage
    {
        private ConcurrentBag<LogData> logs;

        public LogSection LogSection => LogSection.STORAGE;

        public IReadOnlyCollection<IAllowLogging.LogData> Logs => logs;

        public TokenStorage()
        {
            logs = new ConcurrentBag<LogData>();
        }

        public void ClearLogs() => logs.Clear();
       
        public IDataContainer CreateDataContainer(ulong type, byte[] bytes, Type enumType)
        {
            Type containerType = enumType == null ? typeof(DataContainer) : typeof(DataContainer<>).MakeGenericType(enumType);

            IDataContainer container = (IDataContainer)Activator.CreateInstance(containerType);

            container.Type = type;
            container.Value = bytes;

            return container;
        }

        public IList CreateDataContainerCollection(IEnumerable<(ulong type, byte[] bytes)> values, Type enumType)
        {
            Type containerType = enumType == null ? typeof(DataContainer) : typeof(DataContainer<>).MakeGenericType(enumType);

            Type resultType = typeof(List<>).MakeGenericType(containerType);

            var result = (IList)Activator.CreateInstance(resultType);

            foreach (var value in values)
            {
                IDataContainer container = (IDataContainer)Activator.CreateInstance(containerType);

                container.Type = value.type;
                container.Value = value.bytes;

                result.Add(container);
            }

            return result;
        }

        public bool CreateInMemoryObject(IEnumerable<IDataContainer<Pkcs11Attribute>> attributes, out IMemoryObject createdObject, out ExecutionResultCode code)
        {
            return MemoryObjectsBuilder.Instance.Get(attributes, out createdObject, out code);
        }

        public bool CreateTokenObject(IEnumerable<IDataContainer<Pkcs11Attribute>> attributes, out ITokenObject tokenObject, out ExecutionResultCode code)
        {
            //todo: implement
            throw new NotImplementedException();
        }

        public IAesMechanismOptions GetAes(IDataContainer<Pkcs11Mechanism> dataContainer)
        {
            return new AesMechanismOptions(dataContainer);
        }

        public IMechanismOptions GetDefault(IDataContainer<Pkcs11Mechanism> dataContainer)
        {
            return new MechanismOptions(dataContainer);
        }

    }
}
