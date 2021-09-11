using Service.Core.Abstractions.Communication.Structures;
using Service.Core.Abstractions.Storage.Structures;
using Service.Core.Abstractions.Token.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Storage
{
    public class Pkcs11ObjectsBuilder
    {
        private static Pkcs11ObjectsBuilder instance = null;
        public static Pkcs11ObjectsBuilder Instance
        {
            get
            {
                if (instance == null) instance = new Pkcs11ObjectsBuilder();

                return instance;
            }
        }

        private Pkcs11ObjectsBuilder() { }

        /// <summary>
        /// Use this method to create a pkcs11 object
        /// </summary>
        /// <param name="attributes">Attributes used for object creation</param>
        /// <param name="createdObject">Object created</param>
        /// <param name="code">Result code. Ok code is returned if object was created with success</param>
        /// <returns>A boolean which is true if object was created with success, false otherwise</returns>
        public bool Get(IEnumerable<Pkcs11DataContainer<Pkcs11Attribute>> attributes, out Pkcs11Object createdObject, out ExecutionResultCode code)
        {
            //todo: check unhandled attributes and better handling for codes
            createdObject = new Pkcs11Object(attributes);
            code = ExecutionResultCode.OK;

            return true;
        }
    }
}
