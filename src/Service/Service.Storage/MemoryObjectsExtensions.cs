using Service.Core.Abstractions.Storage;
using Service.Core.DefinedTypes;
using Service.Core.Storage.Memory;
using Service.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Storage
{
    /// <summary>
    /// Contains extension methods for Pkcs11ObjectContexts
    /// </summary>
    public static class MemoryObjectsExtensions
    {
        public static bool IsEncrypt(this IMemoryObject contextObject) => contextObject[Pkcs11Attribute.ENCRYPT]?.Value.ToBoolean() ?? false;
    }
}
