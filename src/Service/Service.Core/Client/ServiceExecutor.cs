using Service.Core.Abstractions.Interfaces;
using Service.Core.Abstractions.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Infrastructure
{
    /// <summary>
    /// Main executor used by service.
    /// Methods for commands execution are implemented on this class
    /// </summary>
    public class ServiceExecutor : IServiceExecutor
    {
        private DispatchResult dispatchResult;

        public void SetDispatcherResult(DispatchResult dispatchResult)
        {
            this.dispatchResult = dispatchResult;
        }

        public virtual IExecutionResult BeginSession()
        { 
            return new BytesResult(BitConverter.GetBytes(dispatchResult.SessionId ?? 0));
        }

        public virtual IExecutionResult EndSession()
        {
            return new BytesResult(BitConverter.GetBytes(0));
        }
    }
}
