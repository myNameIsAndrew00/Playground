using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Runtime
{
    public static class CollectionsExtensions
    {


        #region IEnumerable extensions

        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to a boolean
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool ToBoolean(this IEnumerable<byte> bytes) => toValueType(bytes, sizeof(bool), BitConverter.ToBoolean);

        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to a char
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static char ToChar(this IEnumerable<byte> bytes) => toValueType(bytes, sizeof(char), BitConverter.ToChar);


        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to a short
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static short ToShort(this IEnumerable<byte> bytes) => toValueType(bytes, sizeof(short), BitConverter.ToInt16);


        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to an ushort
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ushort ToUShort(this IEnumerable<byte> bytes) => toValueType(bytes, sizeof(ushort), BitConverter.ToUInt16);


        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to an int
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int ToInt32(this IEnumerable<byte> bytes) => toValueType(bytes, sizeof(int), BitConverter.ToInt32);
       
        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to an uint
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static uint ToUInt32(this IEnumerable<byte> bytes) => toValueType(bytes, sizeof(uint), BitConverter.ToUInt32);

        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to a long
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static long ToLong(this IEnumerable<byte> bytes) => toValueType(bytes, sizeof(long), BitConverter.ToInt64);

        /// <summary>
        /// Extension method to convert an array of bytes formatted in big endian format to an ulong
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ulong ToULong(this IEnumerable<byte> bytes) => toValueType(bytes, sizeof(ulong), BitConverter.ToUInt64);

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


        private static TargeType toValueType<TargeType>(this IEnumerable<byte> bytes, int size, Func<byte[], int, TargeType> factoryFunc)
        {
            try
            {
                int bytesCount = bytes.Count();

                //if bytes count are not of size, padd with zeros
                if(bytesCount < size)
                {
                    bytes = Enumerable.Range(0, size - bytesCount).Select(i => (byte)0).Concat(bytes);
                }
                    
                byte[] parsingBytes = bytes.Take(size).ToArray();
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
