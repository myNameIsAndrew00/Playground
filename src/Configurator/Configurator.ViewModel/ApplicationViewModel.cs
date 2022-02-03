using Service.ConfigurationAPI.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Configurator.ViewModel
{
    public class ApplicationViewModel : BaseViewModel
    {  
        public ApplicationViewModel()
        {
            /// <summary>
            /// Cofigure client to ignore ssl certificate. Todo: remove this in future releases.
            /// </summary>
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public bool CurrentPageShouldAnimateOut { get; set; } = false;

        public ApplicationPages CurrentPage { get; set; } = ApplicationPages.Connect;
 
        public async Task ChangePage(ApplicationPages NewPage)
        {
            // If animation is triggered, return.
            if (CurrentPageShouldAnimateOut) return;

            // If page is already set, stop the action.
            if (NewPage == CurrentPage) return;

            CurrentPageShouldAnimateOut = true;
            await Task.Delay(800);

            // If new page is connect, disconnect the client from service.
            if (NewPage == ApplicationPages.Connect) DisconnectClient();

            CurrentPage = NewPage;
            CurrentPageShouldAnimateOut = false;
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

    }
}
