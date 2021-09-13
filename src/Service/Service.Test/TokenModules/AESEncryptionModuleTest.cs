using Service.Core.Abstractions.Token.DefinedTypes;
using Service.Core.Infrastructure.Communication.Structures;
using Service.Core.Infrastructure.Storage.Structures;
using Service.Core.Infrastructure.Token.Encryption;
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
        private byte[] data = Encoding.ASCII.GetBytes("Ana are mere");

        private byte[] key = new byte[] {
                               0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                               0x08, 0x09 ,0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

        private byte[] iv = new byte[] {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                0x08, 0x09 ,0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

        private byte[] keyType = BitConverter.GetBytes((uint)Pkcs11KeyType.AES).Reverse().ToArray();



        [Fact]
        public void TestInitialise()
        {
            // arrange
            Pkcs11ContextObject unitialisedContextObject = new Pkcs11ContextObject();

            Pkcs11ContextObject contextObject = new Pkcs11ContextObject(new[] {
                new DataContainer<Pkcs11Attribute>(){ Type = Pkcs11Attribute.ENCRYPT, Value = BitConverter.GetBytes(true) },
                new DataContainer<Pkcs11Attribute>(){ Type = Pkcs11Attribute.VALUE, Value = key },
                new DataContainer<Pkcs11Attribute>(){ Type = Pkcs11Attribute.KEY_TYPE, Value = keyType }
            });

            DataContainer<Pkcs11Mechanism> mechanism = new DataContainer<Pkcs11Mechanism>()
            {
                Type = Pkcs11Mechanism.AES_ECB,
                Value = iv
            };

            // act and assert
            //Test 1: invalid attributes for context
            EncryptionModule encryptionModule = new EncryptionModule(unitialisedContextObject);
            encryptionModule.Initialise(mechanism, out ExecutionResultCode code);
            Assert.Equal(ExecutionResultCode.KEY_FUNCTION_NOT_PERMITTED, code);

            //Test 2: invalid mechanism
            encryptionModule = new EncryptionModule(contextObject);
            encryptionModule.Initialise(mechanism, out code);
            Assert.Equal(ExecutionResultCode.MECHANISM_INVALID, code);

            //Test 3: 
            encryptionModule.SetMechanism(new AESECBEncryptMechanismCommand());
            encryptionModule.Initialise(mechanism, out code);
            Assert.Equal(ExecutionResultCode.OK, code);
        }

        [Fact]
        public void TestEncrypt()
        {
            Pkcs11ContextObject contextObject = new Pkcs11ContextObject(new[] {
                new DataContainer<Pkcs11Attribute>(){ Type = Pkcs11Attribute.ENCRYPT, Value = BitConverter.GetBytes(true) },
                new DataContainer<Pkcs11Attribute>(){ Type = Pkcs11Attribute.VALUE, Value = key },
                new DataContainer<Pkcs11Attribute>(){ Type = Pkcs11Attribute.KEY_TYPE, Value = keyType }
            });

            DataContainer<Pkcs11Mechanism> mechanism = new DataContainer<Pkcs11Mechanism>()
            {
                Type = Pkcs11Mechanism.AES_ECB,
                Value = iv
            };
            
            //Pretest - initialisation
            EncryptionModule encryptionModule = new EncryptionModule(contextObject);
            encryptionModule.SetMechanism(new AESECBEncryptMechanismCommand());
            EncryptionContext initialisedContext = encryptionModule.Initialise(mechanism, out ExecutionResultCode code);

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
