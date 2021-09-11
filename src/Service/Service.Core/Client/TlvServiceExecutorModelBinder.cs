using Service.Core.Abstractions.Communication.Interfaces;
using Service.Core.Abstractions.Communication.Structures;
using Service.Core.Abstractions.Token.Structures;
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
    internal class TlvServiceExecutorModelBinder : IServiceExecutorModelBinder
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
                else if (parameter.ParameterType.Inherits(typeof(Pkcs11DataContainer)))
                    cursor += parsingBytes.TryParsePkcs11DataContainer(
                           enumType: parameter.ParameterType.IsGenericType ? parameter.ParameterType.GenericTypeArguments[0] : null,
                           out parameterBuilt);
                //check if parameter is a list of Pkcs11DataContainer
                else if (parameter.ParameterType.Inherits(typeof(IEnumerable)))
                {
                    Type innerType = parameter.ParameterType.GenericTypeArguments.FirstOrDefault();
                    if (innerType.Inherits(typeof(Pkcs11DataContainer)))
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
            int result = 0;

            //available value types which model binder may bind are int and long
            var @switch = new Dictionary<Type, Func<int>>()
            {
                [typeof(int)] = () =>
                {
                    result = BitConverter.ToInt32(parsingBytes.Take(sizeof(int)).ToArray(), 0);
                    return sizeof(int);
                },
                [typeof(long)] = () =>
                {
                    result = BitConverter.ToInt32(parsingBytes.Take(sizeof(long)).ToArray(), 0);
                    return sizeof(long);
                }
            };

            if (@switch.TryGetValue(parameterType, out Func<int> builtFunction))
            {
                parameterBuilt = result;
                return builtFunction();
            }
            else
            {
                parameterBuilt = 0;
                return 0;
            }
        }
    }
}
