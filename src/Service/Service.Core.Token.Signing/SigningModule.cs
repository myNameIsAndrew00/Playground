using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Signing;
using Service.Core.DefinedTypes;
using Service.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Token.Signing
{
    /// <summary>
    /// Default implementation for signing handler interface
    /// </summary>
    public class SigningModule : ISigningModule
    { 
        // this dictionary contains the mechanisms allowed for this module.
        private Dictionary<Pkcs11Mechanism, IMechanismCommand> storedMechanisms;        

        public IContext Context { get; }

        public ISigningContext SignContext => Context as SigningContext;

        public SigningModule(IContext context)
        {
            this.Context = context;
        }

        public ISigningContext Initialise<MechanismContainer>(IMemoryObject privateKey, MechanismContainer mechanism, out ExecutionResultCode executionResultCode) where MechanismContainer : IMechanismOptions
        { 
            //check if mechanism is allowed for this module
            if (this.storedMechanisms.TryGetValue(mechanism.Data.Type, out IMechanismCommand mechanismCommand))
            {
                SigningContext result = new SigningContext(mechanism.Data.Type, privateKey);

                //initialise the context using the given command
                mechanismCommand.InitialiseContext(
                    contextObject: result,
                    options: mechanism,
                    out executionResultCode
                    );

                //check if initialisation was with success and return the context built
                if (executionResultCode == ExecutionResultCode.OK) return result;
            }
            else executionResultCode = ExecutionResultCode.MECHANISM_INVALID;

            return null;
        }

        public byte[] Sign(byte[] signingData, bool isPartOperation, out ExecutionResultCode executionResultCode)
        {
            if (SignContext is null)
            {
                executionResultCode = ExecutionResultCode.OPERATION_NOT_INITIALIZED;
                return null;
            }

            // If is part operation, append unprocessed data to the context internals and return.
            if (isPartOperation)
            {
                SignContext.UnprocessedData = SignContext.UnprocessedData is null ? signingData : SignContext.UnprocessedData.Concat(signingData);

                executionResultCode = ExecutionResultCode.OK;

                return null;
            }

            // Execute the request using the context mechanism.
            return storedMechanisms[((SigningContext)SignContext).Mechanism].Execute(SignContext, signingData, out executionResultCode);
        }

        public byte[] SignFinalise(out ExecutionResultCode executionResultCode)
        {
            if (SignContext is null)
            {
                executionResultCode = ExecutionResultCode.OPERATION_NOT_INITIALIZED;
                return null;
            }

            // Remove unprocessed data from context and execute the sign function.
            var hashingData = SignContext.UnprocessedData;

            return storedMechanisms[((SigningContext)SignContext).Mechanism].Execute(SignContext, hashingData, out executionResultCode);
        }

        public IEnumerable<Pkcs11Mechanism> GetMechanisms() => storedMechanisms.Keys;

        public IAllowMechanism SetMechanism(IMechanismCommand mechanismCommand)
        {
            //todo: maybe not all mechanism should be allowed? R: definetly - filtering and other checks should be done here.
            storedMechanisms[mechanismCommand.MechanismType] = mechanismCommand;

            return this;
        }
    }
}
