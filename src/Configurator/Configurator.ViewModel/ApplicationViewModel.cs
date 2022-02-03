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
            CurrentPageShouldAnimateOut = true;
            await Task.Delay(800);

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
            if (Client != null) Client.Dispose();

            Client = new ConfigurationAPIClient(endpoint, "5000", true);

            // Check that client inserted is valid using the ping endpoint
            var pingResponse = await Client.Get<bool>(Endpoint.Ping);

            if(! (pingResponse?.Data ?? false))
            {
                Client.Dispose();
                Client = null;

                return false;
            }

            // If client is valid, return
            return true;
        }

    }
}
