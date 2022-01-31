using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core.Token.Hashing.SHA
{
    public abstract class SHAMechanismCommand : IMechanismCommand
    {
        public SHAMechanismCommand()
        {
            hashAlgorithm = HashAlgorithm.Create(HashAlgorithmName.Name);
        }

        public abstract Pkcs11Mechanism MechanismType { get; }

        protected abstract HashAlgorithmName HashAlgorithmName { get; }

        private HashAlgorithm hashAlgorithm;

        public virtual byte[] Execute(IContext contextObject, byte[] data, out ExecutionResultCode resultCode)
        {            
            resultCode = ExecutionResultCode.OK;

            return data == null ? null : hashAlgorithm.ComputeHash(data);
        }

        public virtual void InitialiseContext(IContext contextObject, IMechanismOptions options, out ExecutionResultCode resultCode)
        {
            // Initialisation should not make any changes in context, for a sha digest command object.
            resultCode = ExecutionResultCode.OK;
        }
    }
}
