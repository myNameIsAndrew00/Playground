using Service.Core.Abstractions.Communication.Interfaces;
using Service.Core.Abstractions.Communication.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Reflection;
using System.Text;

namespace Service.Core.Communication.Abstractions
{
    /// <summary>
    /// A class which can handle received messages from pkcs11 clients
    /// </summary>
    /// <typeparam name="Executor">Executor which handle client requests. For now only one executor is allowed</typeparam>
    internal class Server<Executor> : IPkcs11Server
        where Executor : IServiceExecutor, new()
    { 
        private IServiceCommunicationResolver resolver;

        internal Server(IServiceCommunicationResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Start()
        { 
            resolver.OnCommunicationCreated += onCommunicationCreated;
            resolver.OnClientConnectionError += onClientConnectionError;
            resolver.OnRequestHandlingError += onRequestHandlingError;

            resolver.Listen();           
        }



        #region Private

        private IExecutionResult onCommunicationCreated(DispatchResult dispatchResult)
        {        
            Executor executor = new Executor();
            executor.SetDispatcherResult(dispatchResult);

            if (!dispatchResult.SessionCheckPass)
                return executor.GetBadSessionResult();

            MethodInfo method = typeof(Executor).GetMethod(
                name: dispatchResult.DispatchedAction.ToString(),
                bindingAttr: BindingFlags.Public | BindingFlags.Instance);

            if (method == null)
                throw new NotImplementedException($"Provided executor type { typeof(Executor) } doesn't implement {dispatchResult.DispatchedAction}");

            IExecutionResult executionResult = (IExecutionResult) method.Invoke(executor, null);

            return executionResult;
        }

        private void onClientConnectionError(Exception exception)
        {
            //todo: handle the exception
            Debug.WriteLine(exception);
        }

        private void onRequestHandlingError(Exception exception)
        {
            //todo: handle the exception
            Debug.WriteLine(exception);
        }
        #endregion
    }
}
