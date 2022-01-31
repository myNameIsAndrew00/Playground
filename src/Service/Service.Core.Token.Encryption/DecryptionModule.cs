using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.DefinedTypes;
using Service.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using Service.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Token.Encryption
{
    public class DecryptionModule : IDecryptionModule
    {
        private IContext context;

        private Dictionary<Pkcs11Mechanism, IMechanismCommand> storedMechanisms;

        public DecryptionModule(IContext context)
        {
            this.context = context;
            this.storedMechanisms = new Dictionary<Pkcs11Mechanism, IMechanismCommand>();
        }

        public IContext Context => context;

        /// <summary>
        /// Shorthand for DecryptionContext
        /// </summary>
        private DecryptionContext keyContext => context as DecryptionContext;

        public IKeyContext Initialise<MechanismOptionsType>(IMemoryObject contextObject, MechanismOptionsType mechanism, out ExecutionResultCode executionResultCode) where MechanismOptionsType : IMechanismOptions
        { 
            //check if attribute decrypt is set
            if (!contextObject.IsDecrypt())
            {
                executionResultCode = ExecutionResultCode.KEY_FUNCTION_NOT_PERMITTED;
                return null;
            }

            //check if mechanism is allowed for this module
            if (this.storedMechanisms.TryGetValue(mechanism.Data.Type, out IMechanismCommand mechanismCommand))
            {
                DecryptionContext result = new DecryptionContext(mechanism.Data.Type, contextObject);

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

        public byte[] Decrypt(byte[] encryptedData, bool isPartOperation, out ExecutionResultCode executionResultCode)
        {
            if (keyContext is null)
            {
                executionResultCode = ExecutionResultCode.OPERATION_NOT_INITIALIZED;
                return null;
            }

            // if a part operation is made and unprocessed data from previous execution exists, append that data to the current processing data.
            if (isPartOperation && keyContext.UnprocessedData is not null && keyContext.UnprocessedData.Length > 0)
            {
                encryptedData = keyContext.UnprocessedData.Concat(encryptedData);
                keyContext.UnprocessedData = null;
            }

            // execute the command.
            return storedMechanisms[keyContext.Mechanism].Execute(keyContext, encryptedData, out executionResultCode);
        }

        public byte[] DecryptFinalise(out ExecutionResultCode executionResultCode)
        {
            if (keyContext is null)
            {
                executionResultCode = ExecutionResultCode.OPERATION_NOT_INITIALIZED;
                return null;
            }

            byte[] plainData = keyContext.UnprocessedData;
            
            if(plainData == null)
            {
                executionResultCode = ExecutionResultCode.OK;
                return null;
            }

            // execute the command.
            return storedMechanisms[keyContext.Mechanism].Execute(keyContext, plainData, out executionResultCode);
        }

        public IEnumerable<Pkcs11Mechanism> GetMechanisms() => storedMechanisms.Keys;

        public IAllowMechanism SetMechanism(IMechanismCommand mechanismCommand)
        {
            //todo: maybe not all mechanism should be allowed? R: definetly, here must be filters to prevent bad use of mechanisms.
            storedMechanisms[mechanismCommand.MechanismType] = mechanismCommand;
            return this;
        }
    }
}
