﻿using Service.Core.Abstractions.Token.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure.Token
{
    internal static class Extensions
    {
        /// <summary>
        /// Optain a set of pkcs11 attributes container from an array of bytes 
        /// </summary>
        /// <param name="bytes">Bytes used to provide data</param>
        /// <returns></returns>
        public static IEnumerable<Pkcs11DataContainer<Type>> ToPkcs11DataContainer<Type>(this byte[] bytes)
            where Type : Enum
        {
            if (bytes == null) return null;

            try
            {
                List<Pkcs11DataContainer<Type>> result = new List<Pkcs11DataContainer<Type>>();
                int cursor = 0;

                while (cursor < bytes.Length)
                {
                    Pkcs11DataContainer<Type> container = new Pkcs11DataContainer<Type>();
                    
                    container.Type = (Type)Enum.ToObject(typeof(Type), BitConverter.ToInt64(bytes, cursor));
                    cursor += sizeof(long);

                    long dataLength = BitConverter.ToInt64(bytes, cursor);
                    cursor += sizeof(long);

                    container.Value = new byte[dataLength];
                    bytes.CopyTo(container.Value, cursor);
                    cursor += (int)dataLength;
                }

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}