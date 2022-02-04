using Service.ConfigurationAPI.Client;
using Service.ConfigurationAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Configurator.ViewModel.Pages.Session
{
    public class SessionsViewModel : BaseViewModel
    {
        public const string DESCRIPTION = "View the sessions started on the token";

        public const string TITLE = "Sessions";

        public override ApplicationPages PreviousPage { get; set; } = ApplicationPages.Dashboard;

        public override string Description { get; set; } = DESCRIPTION;

        public override string Title { get; set; } = TITLE;

        /// <summary>
        /// This command handles the sessions list refresh.
        /// </summary>
        public ICommand RefreshSessionsCommand { get; set; }

        public ObservableCollection<SessionItemViewModel> Sessions { get; set; }

        public SessionsViewModel()
        {
            Sessions = new ObservableCollection<SessionItemViewModel>();

            RefreshSessionsCommand = new CommandInitiator(async () => await SynchroniseSessions());
        }

        public override async Task Initialise()
        {
            await SynchroniseSessions();
        }

        #region Private

        private async Task SynchroniseSessions()
        {
            var sessionsResponse = await Application.Instance.Client.Get<List<SessionDTO>>(Endpoint.Sessions);

            Sessions.Clear();

            if (sessionsResponse is not null)
                foreach (var sessionResponse in sessionsResponse.Data)
                {
                    Sessions.Add(new SessionItemViewModel(sessionResponse));
                } 

            await base.Initialise();
        }

        #endregion
    }
}
