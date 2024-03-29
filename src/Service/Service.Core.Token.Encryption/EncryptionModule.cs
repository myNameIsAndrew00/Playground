﻿using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.DefinedTypes;
using Service.Core.Storage;
using Service.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Token.Encryption
{
    /// <summary>
    /// Default implementation for encryption handler interface
    /// </summary>
    public class EncryptionModule : IEncryptionModule 
    {
        private IContext context;

        private Dictionary<Pkcs11Mechanism, IMechanismCommand> storedMechanisms;

        /// <summary>
        /// Shorthand for EncryptionContext
        /// </summary>
        private EncryptionContext keyContext => context as EncryptionContext;

        public EncryptionModule(IContext context)
        {
            this.context = context;
            this.storedMechanisms = new Dictionary<Pkcs11Mechanism, IMechanismCommand>();
        }

        public IContext Context => context;

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
            return this.storedMechanisms[keyContext.Mechanism].Execute(keyContext, plainData, out executionResultCode);
        }

        public byte[] EncryptFinalise(out ExecutionResultCode executionResultCode)
        {
            if (keyContext is null)
            {
                executionResultCode = ExecutionResultCode.OPERATION_NOT_INITIALIZED;
                return null;
            }

            keyContext.AddPadding = true;

            byte[] plainData = keyContext.UnprocessedData ?? new byte[0];

            return this.storedMechanisms[keyContext.Mechanism].Execute(keyContext, plainData, out executionResultCode);
        }

        public IKeyContext Initialise<MechanismContainer>(IMemoryObject contextObject, MechanismContainer mechanism, out ExecutionResultCode executionResultCode)
            where MechanismContainer : IMechanismOptions
        {
            //check if attribute encrypt is set
            if (!contextObject.IsEncrypt())
            {
                executionResultCode = ExecutionResultCode.KEY_FUNCTION_NOT_PERMITTED;
                return null;
            }

            //check if mechanism is allowed for this module
            if (this.storedMechanisms.TryGetValue(mechanism.Data.Type, out IMechanismCommand mechanismCommand))
            { 
                EncryptionContext result = new EncryptionContext(mechanism.Data.Type, contextObject);

                //initialise the context using the given command
                mechanismCommand.InitialiseContext(
                    contextObject: result,
                    options: mechanism,
                    out executionResultCode
                    );

                //check if initialisation was with success and return the context built
                if (executionResultCode == ExecutionResultCode.OK) 
                    return result;
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
