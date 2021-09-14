using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Runtime
{
    /// <summary>
    /// Utils methods and properties for .NET
    /// </summary>
    public static class Utils
    {
        private static ulong nextId = 0x1;
        private static object nextIdLock = new object();
        
        /// <summary>
        /// Use this method to safely generate unique ids
        /// </summary>
        /// <returns></returns>
        public static ulong GetNextId()
        {
            lock (nextIdLock)
            {
                return nextId++;
            }
        }
        
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

        #region Bit operations

        public static byte[] GetBytes(this int integer) => getBytes<int>(integer, BitConverter.GetBytes);

        public static byte[] GetBytes(this uint integer) => getBytes<uint>(integer, BitConverter.GetBytes);
       
        public static byte[] GetBytes(this ulong value) => getBytes<ulong>(value, BitConverter.GetBytes);

        private static byte[] getBytes<Type>(Type value, Func<Type, byte[]> factoryMethod)
        {
            byte[] result = factoryMethod(value);

            if (BitConverter.IsLittleEndian) Array.Reverse(result);

            return result;
        }
        #endregion


    }
}
