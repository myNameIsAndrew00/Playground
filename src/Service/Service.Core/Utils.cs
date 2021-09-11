using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Core
{
    /// <summary>
    /// Utils methods and properties for .NET
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Returns true if this type inherit type given as parameter
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static bool Inherits(this Type type, Type inherit)
        {
            if (inherit.IsAssignableFrom(type))
                return true;
            if (type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == inherit))
                return true;
            return false;
        }
    }
}
