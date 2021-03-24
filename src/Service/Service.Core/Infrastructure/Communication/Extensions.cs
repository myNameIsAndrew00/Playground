using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Communication
{
    internal static class Extensions
    {
        private static long nextId = 0x1;
        private static object nextIdLock = new object();

        /// <summary>
        /// Use this method to safely generate unique ids
        /// </summary>
        /// <returns></returns>
        public static long GetNextId()
        {
            lock (nextIdLock)
            {
                return nextId++;
            }
        }
    }
}
