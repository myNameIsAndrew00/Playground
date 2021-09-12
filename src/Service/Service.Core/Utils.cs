using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Core
{
    /// <summary>
    /// Utils methods and properties for .NET
    /// </summary>
    internal static class Utils
    {
        private static uint nextId = 0x1;
        private static object nextIdLock = new object();

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

        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to an uint
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static uint ToUInt32(this IEnumerable<byte> bytes)
        {
            //parse the type
            byte[] parsingBytes = bytes.Take(sizeof(uint)).ToArray();
            if (BitConverter.IsLittleEndian) Array.Reverse(parsingBytes);
           
            return BitConverter.ToUInt32(parsingBytes, 0);
        }

        /// <summary>
        /// Use this method to safely generate unique ids
        /// </summary>
        /// <returns></returns>
        public static uint GetNextId()
        {
            lock (nextIdLock)
            {
                return nextId++;
            }
        }
    }
}
