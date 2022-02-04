using Service.ConfigurationAPI.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Configurator.ViewModel
{
    /// <summary>
    /// Main instance of the application
    /// </summary>
    public class Application : BaseViewModel
    {
        public static Application Instance { get; } = new Application();


        /// <summary>
        /// Represents current number of requests made to server.
        /// </summary>
        private int? currentServerRequests = 0;
        private object serverRequestLock = new object();


        private Application()
        {
            /// <summary>
            /// Cofigure client to ignore ssl certificate. Todo: remove this in future releases.
            /// </summary>
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public bool CurrentPageShouldAnimateOut { get; private set; } = false;

        public bool ServerRequestSent { get; private set; } = false;   


        public ApplicationPages CurrentPage { get; set; } = ApplicationPages.Connect;
 
        /// <summary>
        /// Represents the context (view model) of current page.
        /// </summary>
        public BaseViewModel CurrentPageContext { get; set; }

        public async Task ChangePage(ApplicationPages NewPage)
        {
            // If animation is triggered, return.
            if (CurrentPageShouldAnimateOut) return;

            // If page is already set, stop the action.
            if (NewPage == CurrentPage) return;

            CurrentPageContext = null;
            CurrentPageShouldAnimateOut = true;
            
            await Task.Delay(300);

            // If new page is connect, disconnect the client from service.
            if (NewPage == ApplicationPages.Connect) DisconnectClient();

            CurrentPage = NewPage;
            CurrentPageShouldAnimateOut = false;

            // Wait until the context is loaded.
            while (CurrentPageContext == null) ;

            await CurrentPageContext.Initialise();
        }

        public ConfigurationAPIClient Client { get; private set; }

       

        /// <summary>
        /// Initialise the client used to communicate with the rest api.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<bool> InitialiseClient(string endpoint)
        {
            DisconnectClient();

            try
            {
                Client = new ConfigurationAPIClient(endpoint, "5000", true);

                // Check that client inserted is valid using the ping endpoint
                var pingResponse = await Client.Get<bool>(Endpoint.Ping);

                if (!(pingResponse?.Data ?? false))
                {
                    DisconnectClient();
                    return false;
                }

                Client.OnRequestStart += OnRequestStart;
                Client.OnRequestEnd += OnRequestEnd;

                // If client is valid, return true
                return true;
            }
            catch
            {
                // Error ocurred, return false
                return false;
            }
        }

        /// <summary>
        /// Disconnect client from service.
        /// </summary>
        public void DisconnectClient()
        {
            if (Client is not null) Client.Dispose();

            Client = null;
        }


        private void OnRequestStart()
        {
            lock (serverRequestLock)
            {
                currentServerRequests++;
                ServerRequestSent = true;
            }

        }

        private void OnRequestEnd()
        {
            lock (serverRequestLock)
            {
                currentServerRequests--;
                if (currentServerRequests <= 0)
                    ServerRequestSent = false;
            }
        }

    }
}
