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
    public class AESEncryptionModuleTest
    {
        private byte[] data = Encoding.ASCII.GetBytes("Ana are mere si pere");

        private byte[] key = new byte[] {
                               0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                               0x08, 0x09 ,0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        private byte[] keyLength = BitConverter.GetBytes((uint)128).Reverse().ToArray();

        private byte[] iv = new byte[] {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                0x08, 0x09 ,0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

        private byte[] keyType = BitConverter.GetBytes((ulong)Pkcs11KeyType.AES).Reverse().ToArray();



        [Fact]
        public void TestInitialise()
        {
            // arrange
            MemoryObject unitialisedContextObject = new MemoryObject();

            MemoryObject contextObject = new MemoryObject(new[] {
                new Pkcs11AttributeContainer(){ Type = Pkcs11Attribute.ENCRYPT, Value = BitConverter.GetBytes(true) },
                new Pkcs11AttributeContainer(){ Type = Pkcs11Attribute.VALUE, Value = key },
                new Pkcs11AttributeContainer(){ Type = Pkcs11Attribute.KEY_TYPE, Value = keyType },
                new Pkcs11AttributeContainer(){ Type = Pkcs11Attribute.VALUE_LEN, Value = keyLength },
            });

            AesMechanismOptions mechanism = new AesMechanismOptions( new DataContainer<Pkcs11Mechanism>()
            {
                Type = Pkcs11Mechanism.AES_ECB,
                Value = iv
            });

            // act and assert
            //Test 1: invalid attributes for context
            EncryptionModule encryptionModule = new EncryptionModule(null);
            encryptionModule.Initialise(unitialisedContextObject, mechanism, out ExecutionResultCode code);
            Assert.Equal(ExecutionResultCode.KEY_FUNCTION_NOT_PERMITTED, code);

            //Test 2: invalid mechanism
            encryptionModule = new EncryptionModule(null);
            encryptionModule.Initialise(contextObject, mechanism, out code); 
            Assert.Equal(ExecutionResultCode.MECHANISM_INVALID, code);

            //Test 3: 
            encryptionModule = new EncryptionModule(null);
            encryptionModule.SetMechanism(new AESECBEncryptMechanismCommand());
            encryptionModule.Initialise(contextObject, mechanism, out code);

            Assert.Equal(ExecutionResultCode.OK, code);
        }

        [Fact]
        public void TestEncrypt()
        {
            IMemoryObject contextObject = new MemoryObject(new[] {
                new Pkcs11AttributeContainer(){ Type = Pkcs11Attribute.ENCRYPT, Value = BitConverter.GetBytes(true) },
                new Pkcs11AttributeContainer(){ Type = Pkcs11Attribute.VALUE, Value = key },
                new Pkcs11AttributeContainer(){ Type = Pkcs11Attribute.KEY_TYPE, Value = keyType },
                new Pkcs11AttributeContainer(){ Type = Pkcs11Attribute.VALUE_LEN, Value = keyLength }
            });

            AesMechanismOptions mechanism = new AesMechanismOptions(new DataContainer<Pkcs11Mechanism>()
            {
                Type = Pkcs11Mechanism.AES_ECB,
                Value = iv
            });

            //Pretest - initialisation
            EncryptionModule encryptionModule = new EncryptionModule(null);
            encryptionModule.SetMechanism(new AESECBEncryptMechanismCommand());
            IContext initialisedContext = encryptionModule.Initialise(contextObject, mechanism, out ExecutionResultCode code);

            Assert.Equal(ExecutionResultCode.OK, code);

            //Test 1: unitialised
            encryptionModule.Encrypt(data, false, out code);
            Assert.Equal(ExecutionResultCode.OPERATION_NOT_INITIALIZED, code);

            //Test 2: all at once
            encryptionModule = new EncryptionModule(initialisedContext);
            byte[] encryptedData = encryptionModule.Encrypt(data, false, out code);

            Assert.Equal(ExecutionResultCode.OK, code);
            Assert.NotNull(encryptedData);

            //Test 3: multi part
            encryptionModule = new EncryptionModule(initialisedContext);
            encryptedData = encryptionModule.Encrypt(data.Take(4).ToArray(), true, out code);
            Assert.Equal(ExecutionResultCode.OK, code);
            encryptedData = encryptionModule.Encrypt(data.Skip(4).ToArray(), true, out code);
            Assert.Equal(ExecutionResultCode.OK, code);
            encryptedData = encryptionModule.EncryptFinalise(out code);

            Assert.Equal(ExecutionResultCode.OK, code);
            Assert.NotNull(encryptedData);
        }
    }
}
