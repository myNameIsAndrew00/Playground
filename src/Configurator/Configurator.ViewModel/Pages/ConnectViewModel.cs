using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Configurator.ViewModel.Pages
{
    public class ConnectViewModel : BaseViewModel
    {
        public ICommand ConnectCommand { get; set; }
    
        public bool IsConnecting { get; set; }

        public string SelectedConnectionEndpoint { get; set; }

        public string ConnectionErrorMessage { get; set; }

        public ConnectViewModel()
        {
            IsConnecting = false;

            ConnectCommand = new CommandInitiator(async () => await ConnectAsync());

            ConnectionErrorMessage = string.Empty;
        }


        #region Private

        private async Task ConnectAsync() {
            ConnectionErrorMessage = string.Empty;
            IsConnecting = true;

            // Check that endpoint is not null or empty.
            if (string.IsNullOrEmpty(SelectedConnectionEndpoint))
            {
                IsConnecting = false;
                ConnectionErrorMessage = "Please enter the server address.";    
                return;
            }

            string connectionEndpoint = SelectedConnectionEndpoint;

            // Check that endpoint contains "http" string
            if (!connectionEndpoint.StartsWith("http://"))
                connectionEndpoint = string.Concat("http://", connectionEndpoint);

            // Try to set the client.
            if (await MainWindowViewModel.Application.InitialiseClient(connectionEndpoint))
            {
                await MainWindowViewModel.Application.ChangePage(ApplicationPages.Dashboard);
                return;
            }

            ConnectionErrorMessage = "Failed to connect to the server. Invalid address or service is unavailable.";
            IsConnecting = false;

            return;
        }


        #endregion
    }
}
