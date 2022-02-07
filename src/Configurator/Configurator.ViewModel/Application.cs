using Service.ConfigurationAPI.Client;
using Service.ConfigurationAPI.Models.Response;
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
        /// Frequency of connectivity check, in secoonds.
        /// </summary>
        private const int CONNECTIVITY_CHECK_FREQUENCY = 3;

        /// <summary>
        /// Represents current number of requests made to server.
        /// </summary>
        private int? currentServerRequests = 0;
        private object serverRequestLock = new object();
        private object changePageLock = new object();

        private Application()
        {
            /// <summary>
            /// Cofigure client to ignore ssl certificate. Todo: remove this in future releases.
            /// </summary>
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public bool CurrentPageShouldAnimateOut { get; private set; } = false;

        public bool ServerRequestSent { get; private set; } = false;

        public bool ServerDisconnected { get; private set; } = true;



        public ApplicationPages CurrentPage { get; set; } = ApplicationPages.Connect;

        /// <summary>
        /// Represents the context (view model) of current page.
        /// </summary>
        public BaseViewModel CurrentPageContext { get; set; }

        public async Task ChangePage(ApplicationPages NewPage)
        {
            lock (changePageLock)
            {
                // If animation is triggered, return.
                if (CurrentPageShouldAnimateOut) return;

                // If page is already set or page chaning ocurred, stop the action.
                if (NewPage == CurrentPage || CurrentPageContext is null) return;

                // Dispose the current context.
                CurrentPageContext.Dispose();
                CurrentPageContext = null;

                CurrentPageShouldAnimateOut = true;
            }

            await Task.Delay(300);

            // If new page is connect, disconnect the client from service.
            if (NewPage == ApplicationPages.Connect) DisconnectClient();

            CurrentPageShouldAnimateOut = false;
            CurrentPage = NewPage;

            // Wait until the context is loaded by UI.
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

                ServerDisconnected = false;

                // Begin a long polling operation which makes requests on ping endpoint which will check the connection with server.
                Client.StartLongPoll<bool>(
                    endpoint: Endpoint.Ping,
                    method: HttpMethod.Get,
                    callback: CheckConnection,
                    frequency: CONNECTIVITY_CHECK_FREQUENCY);

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
            /* Mannualy set disconnected flag to true, to prevent disconnecting page to trigger */
            ServerDisconnected = true;

            if (Client is not null) Client.Dispose();

            Client = null;
        }

        #region Private

        /// <summary>
        /// Check the connection with server.
        /// </summary>
        /// <param name="pingResponse"></param>
        private async void CheckConnection(StandardResponse<bool> pingResponse)
        {
            /* If ping response is null or false, redirect user to disconnect page. */
            if (pingResponse is null || pingResponse.Data == false)
            {
                if (ServerDisconnected == true) return;

                ServerDisconnected = true;

                await ChangePage(ApplicationPages.Reconnect);

                return;
            }

            /* If server was disconnected and ping is responding, return user to dashboard. */
            if (ServerDisconnected)
            {
                ServerDisconnected = false;

                await ChangePage(ApplicationPages.Dashboard);

                return;
            }

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

        #endregion
    }
}
