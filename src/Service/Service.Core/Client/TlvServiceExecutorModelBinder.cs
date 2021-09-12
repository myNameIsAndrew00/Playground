using Service.Core.Abstractions.Communication;
using Service.Core.Infrastructure.Communication.Structures;
using Service.Core.Infrastructure.Storage.Structures;
using Service.Core.Infrastructure.Token.Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Service.Core.Client
{
    /// <summary>
    /// Use this class to bind parameters to a service executor method.
    /// Model binder will fill bind only parameter of types int, long and Pkcs11DataContainer in the order passed to the method.
    /// </summary>
    public class TlvServiceExecutorModelBinder : IServiceExecutorModelBinder
    {
        public object[] GetMethodParameters(MethodInfo method, DispatchResult dispatcherResult)
        {
            List<object> result = new List<object>();
             
            int cursor = 0;
             

            foreach (ParameterInfo parameter in method.GetParameters())
            {
                if (cursor >= dispatcherResult.Payload.Length) break;

                object parameterBuilt = null;
                IEnumerable<byte> parsingBytes = dispatcherResult.Payload.Skip(cursor);

                //check if parameter is value type
                if (parameter.ParameterType.IsValueType)
                {
                    cursor += tryParseValueType(parsingBytes, parameter.ParameterType, out parameterBuilt);
                }
                //check if parameter is Pkcs11DataContainer 
                else if (parameter.ParameterType.Inherits(typeof(DataContainer)))
                    cursor += parsingBytes.TryParsePkcs11DataContainer(
                           enumType: parameter.ParameterType.IsGenericType ? parameter.ParameterType.GenericTypeArguments[0] : null,
                           out parameterBuilt);
                //check if parameter is a list of Pkcs11DataContainer
                else if (parameter.ParameterType.Inherits(typeof(IEnumerable)))
                {
                    Type innerType = parameter.ParameterType.GenericTypeArguments.FirstOrDefault();
                    if (innerType.Inherits(typeof(DataContainer)))
                        cursor += parsingBytes.TryParsePkcs11DataContainerCollection(
                            enumType: innerType.IsGenericType ? innerType.GenericTypeArguments[0] : null,
                            out parameterBuilt);
                }

                if (parameterBuilt != null) result.Add(parameterBuilt);
            }

            return result.ToArray();
        }

        private int tryParseValueType(IEnumerable<byte> parsingBytes, Type parameterType, out object parameterBuilt)
        {
            var parsingInfoContainer = new Dictionary<Type,  (int size, Func<byte[], object> convertFunction)>
            {
                [typeof(char)]   = (sizeof(char),    (bytes) => BitConverter.ToChar(bytes, 0)),
                [typeof(short)]  = (sizeof(short),   (bytes) => BitConverter.ToInt16(bytes, 0)),
                [typeof(ushort)] = (sizeof(ushort),  (bytes) => BitConverter.ToUInt16(bytes, 0)),
                [typeof(int)]    = (sizeof(int),     (bytes) => BitConverter.ToInt32(bytes, 0)),
                [typeof(uint)]   = (sizeof(uint),    (bytes) => BitConverter.ToUInt32(bytes, 0)),
                [typeof(long)]   = (sizeof(long),    (bytes) => BitConverter.ToInt64(bytes, 0))
            };

            if (parsingInfoContainer.TryGetValue(parameterType, out (int size, Func<byte[], object> convertFunction) parsingInfo))
            {
                byte[] inputBytes = parsingBytes.Take(parsingInfo.size).ToArray();

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(inputBytes);

                //available value types which model binder may bind are int and long
                parameterBuilt = parsingInfo.convertFunction(inputBytes);
                 
                return parsingInfo.size;
            }
            else
            {
                parameterBuilt = 0;
                return 0;
            }
           
           

        }
    }
}
