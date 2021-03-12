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
        public IExecutionResult GetBadSessionResult()
        {
            return new BytesResult(ExecutionResultCode.BadArguments);
        }

        public virtual IExecutionResult BeginSession()
        { 
            return new BytesResult(new byte[] { dispatchResult.Session.Id }, ExecutionResultCode.Ok);
        }

        public virtual IExecutionResult EndSession()
        {
            return new BytesResult(ExecutionResultCode.Ok);
        }

        public virtual IExecutionResult Authenticate()
        {
            //todo: authenticate
            this.dispatchResult.Session.Authenticate(null, null);

            return new BytesResult(ExecutionResultCode.Ok);
        }

    }
}
