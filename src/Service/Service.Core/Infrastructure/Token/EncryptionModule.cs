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
        EncryptionObjectHandler context;

        public EncryptionModule(EncryptionObjectHandler context)
        {
            this.context = context;
        }
        public EncryptionObjectHandler Context => context;

        public bool Encrypt(byte[] plainData, out byte[] encryptedData, out ExecutionResultCode executionResultCode)
        {
            //todo: implement
            encryptedData = null;
            executionResultCode = ExecutionResultCode.GENERAL_ERROR; 

            return false;
        }

        public bool EncryptFinalise(out byte[] encryptedData, out ExecutionResultCode executionResultCode)
        {
            //todo: implement
            encryptedData = null;
            executionResultCode = ExecutionResultCode.GENERAL_ERROR;

            return false;
        }

        public bool Initialise(Pkcs11DataContainer<Pkcs11Mechanism> mechanism, out ExecutionResultCode executionResultCode)
        {
            //todo: implement
            executionResultCode = ExecutionResultCode.GENERAL_ERROR;

            return false;
        }

        public bool Initialise(Pkcs11ObjectHandler keyHandler, Pkcs11DataContainer<Pkcs11Mechanism> mechanism, out ExecutionResultCode executionResultCode)
        {
            throw new NotImplementedException();
        }
    }
}
