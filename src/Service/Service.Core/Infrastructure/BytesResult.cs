using Service.Core.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure
{
    internal class BytesResult : IExecutionResult
    {
        private byte[] bytes;

        public BytesResult(byte[] bytes)
        {
            this.bytes = bytes;
        }

        public byte[] GetBytes() => bytes;
       
    }
}
