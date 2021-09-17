using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token
{
    /// <summary>
    /// Implements an method which execute a mechanism
    /// </summary>
    public interface IMechanismCommand 
    {
        Pkcs11Mechanism MechanismType { get; }

        /// <summary>
        /// Use this method to initialise an context initialisation data
        /// </summary>
        /// <param name="contextObject"></param>
        /// <param name="initialisationBytes"></param>
        void InitialiseContext(IMemoryObject contextObject, IMechanismDataContainer initialisationData, out ExecutionResultCode resultCode);

        /// <summary>
        /// Execute the mechanism implemented by this object
        /// </summary>
        /// <param name="data">Data used as input</param>
        /// <returns>The result of the operation</returns>
        byte[] Execute(IMemoryObject contextObject, byte[] data, out ExecutionResultCode resultCode);
    }
}
