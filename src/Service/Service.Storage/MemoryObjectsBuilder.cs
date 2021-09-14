using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using Service.Core.Storage.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Storage
{
    /// <summary>
    /// Representa a builder class to create Pkcs11Objects
    /// </summary>
    public class MemoryObjectsBuilder
    {
        private static MemoryObjectsBuilder instance = null;
        public static MemoryObjectsBuilder Instance
        {
            get
            {
                if (instance == null) instance = new MemoryObjectsBuilder();

                return instance;
            }
        }

        private MemoryObjectsBuilder() { }

        /// <summary>
        /// Use this method to create a pkcs11 object
        /// </summary>
        /// <param name="attributes">Attributes used for object creation</param>
        /// <param name="createdObject">Object created</param>
        /// <param name="code">Result code. Ok code is returned if object was created with success</param>
        /// <returns>A boolean which is true if object was created with success, false otherwise</returns>
        public bool Get(IEnumerable<IPkcs11AttributeDataContainer> attributes, out IMemoryObject createdObject, out ExecutionResultCode code)
        {
            //todo: check unhandled attributes and better handling for codes
            //todo: check value of the attributes to be valid
            //todo: in case of some attributes, a specific object can be deduced
            createdObject = new MemoryObject(attributes);
            code = ExecutionResultCode.OK;

            return true;
        }
    }
}
