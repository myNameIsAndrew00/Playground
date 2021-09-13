using Service.Core.Abstractions.Token.DefinedTypes;
using Service.Core.Infrastructure.Storage.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Storage
{
    /// <summary>
    /// Contains extension methods for Pkcs11ObjectContexts
    /// </summary>
    public static class Pkcs11ContextObjectsExtensions
    {
        public static bool IsEncrypt(this Pkcs11ContextObject contextObject) => contextObject[Pkcs11Attribute.ENCRYPT]?.Value.ToBoolean() ?? false;
    }
}
