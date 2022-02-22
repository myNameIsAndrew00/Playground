using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Signing;
using Service.Core.DefinedTypes;
using Service.Runtime;
using System;
using System.Collections.Generic; 
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Token.Signing
{
    public class VerifyModule : IVerifyModule
    {
        // this dictionary contains the mechanisms allowed for this module.
        private Dictionary<Pkcs11Mechanism, IMechanismCommand> storedMechanisms;

        public IContext Context { get; }

        public IVerifyContext VerifyContext => Context as VerifyContext;


        public IVerifyContext Initialise<MechanismContainer>(IMemoryObject publicKey, MechanismContainer mechanism, out ExecutionResultCode executionResultCode) where MechanismContainer : IMechanismOptions
        {
            //check if mechanism is allowed for this module
            if (this.storedMechanisms.TryGetValue(mechanism.Data.Type, out IMechanismCommand mechanismCommand))
            {
                VerifyContext result = new VerifyContext(mechanism.Data.Type, publicKey);

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

        public bool Verify(byte[] verifyingData, byte[] signedData, bool isPartOperation, out ExecutionResultCode executionResultCode)
        {
            if (VerifyContext is null)
            {
                executionResultCode = ExecutionResultCode.OPERATION_NOT_INITIALIZED;
                return false;
            }

            VerifyContext.VerifyData = VerifyContext.VerifyData is null ? verifyingData : VerifyContext.VerifyData.Concat(verifyingData);

            // If is part operation, append verifying data to the context internals and return.
            if (isPartOperation)
            {
            
                executionResultCode = ExecutionResultCode.OK;

                return true;
            }

            // Execute the request using the context mechanism.
            return Convert.ToBoolean(storedMechanisms[((VerifyContext)VerifyContext).Mechanism].Execute(VerifyContext, signedData, out executionResultCode));
        }

        public bool VerifyFinalise(byte[] signatureData, out ExecutionResultCode executionResultCode)
        {
            if (VerifyContext is null)
            {
                executionResultCode = ExecutionResultCode.OPERATION_NOT_INITIALIZED;
                return false;
            }

            return Convert.ToBoolean(storedMechanisms[((SigningContext)VerifyContext).Mechanism].Execute(VerifyContext, signatureData, out executionResultCode));
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
