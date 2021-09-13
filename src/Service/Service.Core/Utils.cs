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

        public static byte[] GetBytes(this uint integer)
        {
            byte[] result = BitConverter.GetBytes(integer);
            
            if (BitConverter.IsLittleEndian) Array.Reverse(result);
            
            return result;
        }


        #endregion


        #region IEnumerable extensions

        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to a boolean
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool ToBoolean(this IEnumerable<byte> bytes) => toValueType(bytes, BitConverter.ToBoolean);

        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to a char
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static char ToChar(this IEnumerable<byte> bytes) => toValueType(bytes, BitConverter.ToChar);


        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to a short
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static short ToShort(this IEnumerable<byte> bytes) => toValueType(bytes, BitConverter.ToInt16);


        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to an ushort
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ushort ToUShort(this IEnumerable<byte> bytes) => toValueType(bytes, BitConverter.ToUInt16);


        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to an int
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int ToInt32(this IEnumerable<byte> bytes) => toValueType(bytes, BitConverter.ToInt32);
        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to an uint
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static uint ToUInt32(this IEnumerable<byte> bytes) => toValueType(bytes, BitConverter.ToUInt32);

        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to a long
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static long ToLong(this IEnumerable<byte> bytes) => toValueType(bytes, BitConverter.ToInt64);


        /// <summary>
        /// Use this extension method to concatenate two arrays.
        /// </summary>
        /// <typeparam name="ArrayType"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static ArrayType[] Concat<ArrayType>(this ArrayType[] first, ArrayType[] second)
        {
            if (first is null)
                return second;
            if (second is null)
                return first;
            
            var result = new ArrayType[first.Length + second.Length];
            first.CopyTo(result, 0);
            second.CopyTo(result, first.Length);

            return result;
        }


        private static TargeType toValueType<TargeType>(this IEnumerable<byte> bytes, Func<byte[], int, TargeType> factoryFunc)
        {
            try
            {
                byte[] parsingBytes = bytes.Take(sizeof(uint)).ToArray();
                if (BitConverter.IsLittleEndian) Array.Reverse(parsingBytes);

                return factoryFunc(parsingBytes, 0);
            }
            catch
            {
                return default;
            }
        }

        #endregion

    }
}
