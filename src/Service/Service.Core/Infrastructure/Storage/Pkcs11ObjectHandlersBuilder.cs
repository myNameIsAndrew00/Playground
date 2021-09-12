using Service.Core.Infrastructure.Communication.Structures;
using Service.Core.Infrastructure.Storage.Structures;
using Service.Core.Infrastructure.Token.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Storage
{
    /// <summary>
    /// Representa a builder class to create Pkcs11Objects
    /// </summary>
    public class Pkcs11ObjectHandlersBuilder
    {
        private static Pkcs11ObjectHandlersBuilder instance = null;
        public static Pkcs11ObjectHandlersBuilder Instance
        {
            get
            {
                if (instance == null) instance = new Pkcs11ObjectHandlersBuilder();

                return instance;
            }
        }

        private Pkcs11ObjectHandlersBuilder() { }

        /// <summary>
        /// Use this method to create a pkcs11 object
        /// </summary>
        /// <param name="attributes">Attributes used for object creation</param>
        /// <param name="createdObject">Object created</param>
        /// <param name="code">Result code. Ok code is returned if object was created with success</param>
        /// <returns>A boolean which is true if object was created with success, false otherwise</returns>
        internal bool Get(IEnumerable<DataContainer<Pkcs11Attribute>> attributes, out Pkcs11ContextObject createdObject, out ExecutionResultCode code)
        {
            //todo: check unhandled attributes and better handling for codes
            //todo: in case of some attributes, a specific object can be deduced
            createdObject = new Pkcs11ContextObject(attributes);
            code = ExecutionResultCode.OK;

            return true;
        }
    }
}
