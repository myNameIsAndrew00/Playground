using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using Service.Core.Storage.Mechanism;
using Service.Core.Storage.Memory;
using Service.Core.Token.Encryption;
using Service.Core.Token.Encryption.AES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Service.Test.TokenModules
{
    public class AESDencryptionModuleTest
    {
        private byte[] expectedDecryptedData = Encoding.ASCII.GetBytes("Ana are mere si pere");
        private byte[] encryptedData = new byte[] { 125, 26, 72, 92, 246, 87, 81, 204, 87, 55, 211, 125, 45, 73, 237, 74, 230, 28, 97, 108, 129, 160, 219, 235, 227, 76, 25, 241, 209, 32, 96, 3 };

        private byte[] key = new byte[] {
                               0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                               0x08, 0x09 ,0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        private byte[] keyLength = BitConverter.GetBytes((uint)128).Reverse().ToArray();

        private byte[] iv = new byte[] {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                0x08, 0x09 ,0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

        private byte[] keyType = BitConverter.GetBytes((ulong)Pkcs11KeyType.AES).Reverse().ToArray();

        private MemoryObject createNewContext() => new MemoryObject(new[] {
                new Pkcs11AttributeContainer(){ Type = Pkcs11Attribute.DECRYPT, Value = BitConverter.GetBytes(true) },
                new Pkcs11AttributeContainer(){ Type = Pkcs11Attribute.VALUE, Value = this.key },
                new Pkcs11AttributeContainer(){ Type = Pkcs11Attribute.KEY_TYPE, Value = keyType },
                new Pkcs11AttributeContainer(){ Type = Pkcs11Attribute.VALUE_LEN, Value = keyLength },
            });

        [Fact]
        public void TestInitialise()
        {
            // arrange
            MemoryObject unitialisedContextObject = new MemoryObject();

            MemoryObject contextObject = createNewContext();

            AesMechanismOptions mechanism = new AesMechanismOptions(new DataContainer<Pkcs11Mechanism>()
            {
                Type = Pkcs11Mechanism.AES_ECB,
                Value = iv
            });

            // act and assert
            //Test 1: invalid attributes for context
            DecryptionModule decryptionModule = new DecryptionModule(null);
            decryptionModule.Initialise(unitialisedContextObject, mechanism, out ExecutionResultCode code);
            Assert.Equal(ExecutionResultCode.KEY_FUNCTION_NOT_PERMITTED, code);

            //Test 2: invalid mechanism
            decryptionModule = new DecryptionModule(null);
            decryptionModule.Initialise(contextObject, mechanism, out code);
            Assert.Equal(ExecutionResultCode.MECHANISM_INVALID, code);

            //Test 3: 
            decryptionModule = new DecryptionModule(null);
            decryptionModule.SetMechanism(new AESECBDecryptMechanismCommand());
            decryptionModule.Initialise(contextObject, mechanism, out code);

            Assert.Equal(ExecutionResultCode.OK, code);
        }


        [Fact]
        public void TestDecrypt()
        { 

            AesMechanismOptions mechanism = new AesMechanismOptions(new DataContainer<Pkcs11Mechanism>()
            {
                Type = Pkcs11Mechanism.AES_ECB,
                Value = iv
            });

            //Pretest - initialisation
            DecryptionModule decryptionModule = new DecryptionModule(null);
            decryptionModule.SetMechanism(new AESECBDecryptMechanismCommand());
            IContext initialisedContext = decryptionModule.Initialise(createNewContext(), mechanism, out ExecutionResultCode code);

            Assert.Equal(ExecutionResultCode.OK, code);

            //Test 1: unitialised
            decryptionModule.Decrypt(this.encryptedData, false, out code);
            Assert.Equal(ExecutionResultCode.OPERATION_NOT_INITIALIZED, code);

            //Test 2: all at once
            decryptionModule = new DecryptionModule(initialisedContext);
            byte[] dencryptedData = decryptionModule.Decrypt(this.encryptedData, false, out code);

            Assert.Equal(ExecutionResultCode.OK, code);
            Assert.NotNull(dencryptedData);
            Assert.Equal(dencryptedData, expectedDecryptedData);

            //Test 3: multi part with wrong data size
            decryptionModule = new DecryptionModule(initialisedContext);
            dencryptedData = decryptionModule.Decrypt(encryptedData.Take(4).ToArray(), true, out code);
            Assert.Equal(ExecutionResultCode.ENCRYPTED_DATA_LEN_RANGE, code);
        }
    }
}
