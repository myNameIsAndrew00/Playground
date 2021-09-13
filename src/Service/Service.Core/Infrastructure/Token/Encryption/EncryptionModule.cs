using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.DefinedTypes;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.Infrastructure.Communication.Structures;
using Service.Core.Infrastructure.Storage.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Core.Infrastructure.Token.Encryption
{
    /// <summary>
    /// Default implementation for encryption handler interface
    /// </summary>
    internal class EncryptionModule : IEncryptionModule
    {
        private Pkcs11ContextObject context;

        private Dictionary<Pkcs11Mechanism, IMechanismCommand> storedMechanisms;

        /// <summary>
        /// Shorthand for EncryptionContext
        /// </summary>
        private EncryptionContext keyContext => context as EncryptionContext;

        public EncryptionModule(Pkcs11ContextObject context)
        {
            this.context = context;
            this.storedMechanisms = new Dictionary<Pkcs11Mechanism, IMechanismCommand>();
        }

        public Pkcs11ContextObject Context => context;

        public bool Encrypt(byte[] plainData, out ExecutionResultCode executionResultCode)
        {
            //todo: implement
            executionResultCode = ExecutionResultCode.GENERAL_ERROR;

            return false;
        }

        public bool EncryptFinalise(out ExecutionResultCode executionResultCode)
        {
            //todo: implement
            executionResultCode = ExecutionResultCode.GENERAL_ERROR;

            return false;
        }

        public EncryptionContext Initialise(DataContainer<Pkcs11Mechanism> mechanism, out ExecutionResultCode executionResultCode)
        {
            //check if attribute encrypt is set
            if (context[Pkcs11Attribute.ENCRYPT] == null)
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

        public bool Initialise(Pkcs11ContextObject keyHandler, DataContainer<Pkcs11Mechanism> mechanism, out ExecutionResultCode executionResultCode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Pkcs11Mechanism> GetMechanisms() => storedMechanisms.Keys;

        public IAllowMechanism SetMechanism(IMechanismCommand mechanismCommand)
        {
            storedMechanisms[mechanismCommand.MechanismType] = mechanismCommand;
            return this;
        }

        #region Private

        #endregion
    }
}
