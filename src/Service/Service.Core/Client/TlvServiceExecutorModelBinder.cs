using Service.Core.Abstractions.Communication;
using Service.Core.Abstractions.Execution;
using Service.Core.Abstractions.Storage;
using Service.Core.Execution;
using Service.Core.Storage.Memory;
using Service.Runtime;
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
    /// Model binder will fill bind only value types and IDataContainer in the order passed to the method.
    /// A single collection of data containers can be parsed by this binder.
    /// </summary>
    public class TlvServiceExecutorModelBinder : IServiceExecutorModelBinder<DispatchResult, Session>
    {
        private TlvPayloadDataParser tlvPayloadDataParser = new TlvPayloadDataParser();

        public object[] GetMethodParameters(MethodInfo method, DispatchResult dispatcherResult, IDataContainerBuilder parser)
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
                
                //check if parameter is DataContainer 
                else if (parameter.ParameterType.Inherits(typeof(IDataContainer)))
                {
                    cursor += tlvPayloadDataParser.TryParseTlvStructure(
                           bytes: parsingBytes,
                           out TlvPayloadDataParser.TlvStructure tlvStructure);
                    if (tlvStructure != null)
                        parameterBuilt = parser.CreateDataContainer(
                            tlvStructure.dataType,
                            tlvStructure.bytes,
                            parameter.ParameterType.IsGenericType ? parameter.ParameterType.GenericTypeArguments[0] : null);
                }

                //check if parameter is a list of DataContainer
                else if (parameter.ParameterType.Inherits(typeof(IEnumerable)))
                {
                    Type innerType = parameter.ParameterType.GenericTypeArguments.FirstOrDefault();
                    if (innerType.Inherits(typeof(IDataContainer)))
                    {
                        cursor += tlvPayloadDataParser.TryParseTlvStructureCollection(
                            bytes: parsingBytes,
                            out List<TlvPayloadDataParser.TlvStructure> tlvStructureCollection);
                        if (tlvStructureCollection != null)
                            parameterBuilt = parser.CreateDataContainerCollection(
                                 tlvStructureCollection.Select(item => (item.dataType, item.bytes)),
                                 innerType.IsGenericType ? innerType.GenericTypeArguments[0] : null
                                );
                    }
                }

                if (parameterBuilt != null) result.Add(parameterBuilt);
            }

            return result.ToArray();
        }

        private int tryParseValueType(IEnumerable<byte> parsingBytes, Type parameterType, out object parameterBuilt)
        {
            var parsingInfoContainer = new Dictionary<Type,  (int size, Func<byte[], object> convertFunction)>
            {
                [typeof(bool)]   = (sizeof(bool),    (bytes) => bytes.ToBoolean() ),
                [typeof(char)]   = (sizeof(char),    (bytes) => bytes.ToChar()),
                [typeof(short)]  = (sizeof(short),   (bytes) => bytes.ToShort()),
                [typeof(ushort)] = (sizeof(ushort),  (bytes) => bytes.ToUShort()),
                [typeof(int)]    = (sizeof(int),     (bytes) => bytes.ToInt32()),
                [typeof(uint)]   = (sizeof(uint),    (bytes) => bytes.ToUInt32()),
                [typeof(long)]   = (sizeof(long),    (bytes) => bytes.ToLong()),
                [typeof(ulong)] = (sizeof(long),     (bytes) => bytes.ToULong()),
            };

            if (parsingInfoContainer.TryGetValue(parameterType, out (int size, Func<byte[], object> convertFunction) parsingInfo))
            {
                byte[] inputBytes = parsingBytes.Take(parsingInfo.size).ToArray();

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


    /// <summary>
    /// Use this class to create data containers from bytes
    /// </summary>
    internal class TlvPayloadDataParser
    {
        public record TlvStructure(ulong dataType, byte[] bytes);

        /// <summary>
        /// Optain a pkcs11 attribute from an array of bytes
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public TlvStructure ToTlvStructure(IEnumerable<byte> bytes)
        {
            TryParseTlvStructure(bytes, out TlvStructure output);

            return output;
        }

        /// <summary>
        /// Optain a set of pkcs11 attributes container from an array of bytes 
        /// </summary>
        /// <param name="bytes">Bytes used to provide data</param>
        /// <returns></returns>
        public List<TlvStructure> ToTlvStructureCollection(IEnumerable<byte> bytes)
        {
            TryParseTlvStructureCollection(bytes, out List<TlvStructure> output);

            return output;
        }

        public int TryParseTlvStructure(IEnumerable<byte> bytes, out TlvStructure output)
        {
            output = null;

            if (bytes == null) return 0;

            int cursor = 0;

            try
            { 
                output = parseContainer(bytes, ref cursor);

                return cursor;
            }
            catch
            {
                return cursor;
            }
        }

        public int TryParseTlvStructureCollection(IEnumerable<byte> bytes, out List<TlvStructure> output)
        {
            output = null;
            if (bytes == null) return 0;

            int cursor = 0;
            try
            {
                
                List<TlvStructure> result = new List<TlvStructure>();

                while (cursor < bytes.Count())
                    result.Add(parseContainer(bytes, ref cursor));

                output = result;
                return cursor;
            }
            catch
            {
                return cursor;
            }
        }



        #region Private

        private TlvStructure parseContainer(IEnumerable<byte> bytes, ref int cursor)
        { 
            // parse the type.
            ulong dataType = bytes.Skip(cursor).ToULong();
            cursor += sizeof(ulong);

            // parse the value.
            uint dataLength = bytes.Skip(cursor).ToUInt32();
            cursor += sizeof(uint);

            // parse the data.
            byte[] resultBytes = new byte[dataLength];
            byte[] _bytes = bytes.Skip(cursor).Take((int)dataLength).ToArray();
            Array.Copy(_bytes, resultBytes, _bytes.Length);

            cursor += (int)dataLength;

            return new TlvStructure(dataType, resultBytes);
        }

        #endregion
    }
}
