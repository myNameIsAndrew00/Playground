using Service.Core.Abstractions.Communication;
using Service.Core.DefinedTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Communication.Infrastructure
{
    public class BytesResult : IExecutionResult
    {
        private byte[] bytes;

        private ExecutionResultCode resultCode;

        public BytesResult(ExecutionResultCode executionResultCode)
        {
            this.resultCode = executionResultCode;
        }

        public BytesResult(byte[] bytes, ExecutionResultCode executionResultCode) : this(executionResultCode)
        {
            this.bytes = bytes;
        }

        public ExecutionResultCode ResultCode => resultCode;

        public byte[] GetBytes() => bytes ?? new byte[0];
       

    }
}
