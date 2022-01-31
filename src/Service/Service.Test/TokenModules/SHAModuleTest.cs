using Service.Core.DefinedTypes;
using Service.Core.Storage.Mechanism;
using Service.Core.Storage.Memory;
using Service.Core.Token.Hashing;
using Service.Core.Token.Hashing.SHA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Service.Test.TokenModules
{
    public class SHAModuleTest
    {
        private byte[] data = Encoding.ASCII.GetBytes("Ana are mere si pere");

        [Fact]
        public void TestInitialise()
        {
            MechanismOptions mechanism = new MechanismOptions(new DataContainer<Pkcs11Mechanism>()
            {
                Type = Pkcs11Mechanism.SHA_1
            });

            // Test 1: mechanism invalid test
            HashingModule hashingModule = new HashingModule(null);
            hashingModule.Initialise(mechanism, out ExecutionResultCode code);

            Assert.Equal(ExecutionResultCode.MECHANISM_INVALID, code);

            // Test 2: initialise test
            hashingModule.SetMechanism(new SHA1MechanismCommand());
            hashingModule.Initialise(mechanism, out code);

            Assert.Equal(ExecutionResultCode.OK, code);
        }


        [Fact]
        public void TestHash()
        {
            // Arrange: SHA1 will be used for testing
            MechanismOptions mechanism = new MechanismOptions(new DataContainer<Pkcs11Mechanism>()
            {
                Type = Pkcs11Mechanism.SHA_1
            });
            HashingModule hashingModule = new HashingModule(null);
            hashingModule.SetMechanism(new SHA1MechanismCommand());

            // Act and assert
            // Test 1: uninitialised context
            hashingModule.Hash(data, false, out ExecutionResultCode code);

            Assert.Equal(ExecutionResultCode.OPERATION_NOT_INITIALIZED, code);

            var context = hashingModule.Initialise(mechanism, out code);
            hashingModule = new HashingModule(context);
            hashingModule.SetMechanism(new SHA1MechanismCommand());

            byte[] digest = hashingModule.Hash(data, false, out code);

            Assert.Equal(ExecutionResultCode.OK, code);
        }
    }
}
