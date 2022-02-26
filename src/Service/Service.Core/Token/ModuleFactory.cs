using Service.Core.Abstractions.Storage;
using Service.Core.Abstractions.Token;
using Service.Core.Abstractions.Token.Encryption;
using Service.Core.Abstractions.Token.Hashing;
using Service.Core.Abstractions.Token.Signing;
using Service.Core.Storage.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Token
{
    public class ModuleFactory : IModuleFactory
    {
        //todo: add scope to created objects?

        private readonly Dictionary<Type, Func<object, ITokenModule>> factoryStorage = new Dictionary<Type, Func<object, ITokenModule>>();
        private readonly Dictionary<Type, Type> moduleStorage = new Dictionary<Type, Type>();


        public void RegisterModule(Type type, Type implementation, Func<object, ITokenModule> factoryMethod = null)
        {
            if (type is null)
                throw new ArgumentNullException($"Passed argument {type} is null");
            if (implementation is null)
                throw new ArgumentNullException($"Passed argument {implementation} is null");

            if (moduleStorage.ContainsKey(type))
                throw new ArgumentException($"Can't register {type} to the service collection. An module with the same type already exists");

            moduleStorage.Add(type, implementation);

            if (factoryMethod != null)
                factoryStorage.Add(implementation, factoryMethod);
        }

        public IEncryptionModule GetEncryptionModule(IMemoryObject objectHandler) => getModule<IEncryptionModule, IMemoryObject>(objectHandler);

        public IDecryptionModule GetDecryptionModule(IMemoryObject objectHandler) => getModule<IDecryptionModule, IMemoryObject>(objectHandler);

        public IHashingModule GetHashingModule(IMemoryObject objectHandler) => getModule<IHashingModule, IMemoryObject>(objectHandler);

        public ISigningModule GetSigningModule(IMemoryObject objectHandler) => getModule<ISigningModule, IMemoryObject>(objectHandler);
        
        public IVerifyModule GetVerifyModule(IMemoryObject objectHandler) => getModule<IVerifyModule, IMemoryObject>(objectHandler);

        public ModuleType GetModule<ModuleType>() where ModuleType : class, ITokenModule => getModule<ModuleType, object>(null);


        private ModuleType getModule<ModuleType, ModuleBuilderParameterType>(ModuleBuilderParameterType builderParameter)
            where ModuleType : class, ITokenModule
        {
            if (moduleStorage.TryGetValue(typeof(ModuleType), out Type implementation))
            {
                if (factoryStorage.TryGetValue(implementation, out var factoryFunction))
                {
                    if (factoryFunction != null)
                        return factoryFunction(builderParameter) as ModuleType;
                }
                else return Activator.CreateInstance(implementation) as ModuleType;
            }
            return null;
        }

       
    }
}
