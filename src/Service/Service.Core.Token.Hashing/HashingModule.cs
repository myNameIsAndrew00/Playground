
using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Hashing;
using Service.Core.DefinedTypes;
using Service.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Token.Hashing
{
    /// <summary>
    /// Default implementation for hashing handler interface
    /// </summary>
    public class HashingModule : IHashingModule
    {
        // this dictionary contains the mechanisms allowed for this module.
        private Dictionary<Pkcs11Mechanism, IMechanismCommand> storedMechanisms;

        public IContext Context { get; }

        /// <summary>
        /// Shorthand for EncryptionContext
        /// </summary>
        private DigestContext digestContext => Context as DigestContext;

        public HashingModule(IContext context)
        {
            this.Context = context;

            storedMechanisms = new Dictionary<Pkcs11Mechanism, IMechanismCommand>();
        }

        public IDigestContext Initialise<MechanismContainer>(MechanismContainer mechanism, out ExecutionResultCode executionResultCode) where MechanismContainer : IMechanismOptions
        {
            //check if mechanism is allowed for this module
            if (this.storedMechanisms.TryGetValue(mechanism.Data.Type, out IMechanismCommand mechanismCommand))
            {
                DigestContext result = new DigestContext(mechanism.Data.Type);

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

        public byte[] Hash(byte[] hashingData, bool isPartOperation, out ExecutionResultCode executionResultCode)
        {
            if(digestContext is null)
            {
                executionResultCode = ExecutionResultCode.OPERATION_NOT_INITIALIZED;
                return null;
            }

            // If is part operation, append unprocessed data to the context internals and return.
            if (isPartOperation)
            {
                digestContext.UnprocessedData = digestContext.UnprocessedData is null ? hashingData : digestContext.UnprocessedData.Concat(hashingData);

                executionResultCode = ExecutionResultCode.OK;
                
                return null;
            }

            // Execute the request using the context mechanism.
            return storedMechanisms[digestContext.Mechanism].Execute(digestContext, hashingData, out executionResultCode);
        }

        public byte[] HashFinalise(out ExecutionResultCode executionResultCode)
        {
            if (digestContext is null)
            {
                executionResultCode = ExecutionResultCode.OPERATION_NOT_INITIALIZED;
                return null;
            }

            // Remove unprocessed data from context and execute the hash function.
            var hashingData = digestContext.UnprocessedData;

            return storedMechanisms[digestContext.Mechanism].Execute(digestContext, hashingData, out executionResultCode);
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
