using Service.Core.Abstractions.Token;
using Service.Core.Infrastructure.Communication.Structures;
using Service.Core.Infrastructure.Storage.Structures;
using Service.Core.Infrastructure.Token.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Token
{
    /// <summary>
    /// Default implementation for encryption handler interface
    /// </summary>
    internal class EncryptionModule : IEncryptionModule
    {
        private Pkcs11ContextObject context;

        private EncryptionContext keyContext => context as EncryptionContext;

        public EncryptionModule(Pkcs11ContextObject context)
        {
            this.context = context;
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
            //todo: implement
            executionResultCode = ExecutionResultCode.GENERAL_ERROR;


            return null;
        }

        public bool Initialise(Pkcs11ContextObject keyHandler, DataContainer<Pkcs11Mechanism> mechanism, out ExecutionResultCode executionResultCode)
        {
            throw new NotImplementedException();
        }
    }
}
