using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.DefinedTypes;
using Service.Core.Storage;
using Service.Core.Storage.Memory;
using Service.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Token.Encryption
{
    /// <summary>
    /// Default implementation for encryption handler interface
    /// </summary>
    internal class EncryptionModule : IEncryptionModule 
    {
        private IMemoryObject context;

        private Dictionary<Pkcs11Mechanism, IMechanismCommand> storedMechanisms;

        /// <summary>
        /// Shorthand for EncryptionContext
        /// </summary>
        private EncryptionContext keyContext => context as EncryptionContext;

        public EncryptionModule(IMemoryObject context)
        {
            this.context = context;
            this.storedMechanisms = new Dictionary<Pkcs11Mechanism, IMechanismCommand>();
        }

        public IMemoryObject Context => context;

        public byte[] Encrypt(byte[] plainData, bool isPartOperation, out ExecutionResultCode executionResultCode)
        {
            if(keyContext is null)
            {
                executionResultCode = ExecutionResultCode.OPERATION_NOT_INITIALIZED;
                return null;
            }

            //if a part operation is made, padding is not required to be added
            keyContext.AddPadding = !isPartOperation;
             
            //if a part operation is made and unprocessed data from previous execution exists, append that data to the current processing data
            if(isPartOperation && keyContext.UnprocessedData is not null && keyContext.UnprocessedData.Length > 0)
            {
                plainData = keyContext.UnprocessedData.Concat(plainData);
                keyContext.UnprocessedData = null;
            }

            //execute the command
            return keyContext.MechanismCommand.Execute(keyContext, plainData, out executionResultCode);
        }

        public byte[] EncryptFinalise(out ExecutionResultCode executionResultCode)
        {
            if (keyContext is null)
            {
                executionResultCode = ExecutionResultCode.OPERATION_NOT_INITIALIZED;
                return null;
            }

            keyContext.AddPadding = true;

            byte[] plainData = keyContext.UnprocessedData;
            keyContext.UnprocessedData = null;

            if (plainData is null) plainData = new byte[0];

            return keyContext.MechanismCommand.Execute(keyContext, plainData, out executionResultCode);
        }

        public IMemoryObject Initialise<MechanismContainer>(MechanismContainer mechanism, out ExecutionResultCode executionResultCode)
            where MechanismContainer : IDataContainer<Pkcs11Mechanism>
        {
            //check if attribute encrypt is set
            if (!context.IsEncrypt())
            {
                executionResultCode = ExecutionResultCode.KEY_FUNCTION_NOT_PERMITTED;
                return null;
            }

            //check if mechanism is allowed for this module
            if (this.storedMechanisms.ContainsKey(mechanism.Type))
            { 
                EncryptionContext result = new EncryptionContext(this.storedMechanisms[mechanism.Type], this.context);

                //initialise the context using the given command
                result.MechanismCommand.InitialiseContext(
                    contextObject: result,
                    initialisationBytes: mechanism.Value,
                    out executionResultCode
                    );

                //check if initialisation was with success and return the context built
                if (executionResultCode == ExecutionResultCode.OK) return result;
            }
            else executionResultCode = ExecutionResultCode.MECHANISM_INVALID;

            return null;
        }

        public IEnumerable<Pkcs11Mechanism> GetMechanisms() => storedMechanisms.Keys;

        public IAllowMechanism SetMechanism(IMechanismCommand mechanismCommand)
        {
            //todo: maybe not all mechanism should be allowed?
            storedMechanisms[mechanismCommand.MechanismType] = mechanismCommand;
            return this;
        }

       
    }
}
