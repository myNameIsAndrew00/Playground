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
        private Guid? sessionsPollingGuid;

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

            sessionsPollingGuid = Application.Client.StartLongPoll<List<SessionDTO>>(
                endpoint: Endpoint.Sessions,
                method: HttpMethod.Get,
                callback: (sessionsResponse) => ReloadSessions(sessionsResponse.Data)
                );
        }

        public override async Task Initialise()
        {
            await SynchroniseSessions();
        }

        public override void Dispose()
        {
            if (sessionsPollingGuid is not null)
                Application.Client.CancelLongPolling(sessionsPollingGuid.Value);

            base.Dispose();
        }

        #region Private

        private async Task SynchroniseSessions()
        {
            var sessionsResponse = await Application.Instance.Client.Get<List<SessionDTO>>(Endpoint.Sessions);

            ReloadSessions(sessionsResponse.Data);

            await base.Initialise();
        }

        private void ReloadSessions(List<SessionDTO> sessionDTOs)
        {
            lock (Sessions)
            {
                UIContext.Send(_ =>
               {
                   Sessions.Clear();

                   if (sessionDTOs is not null)
                       foreach (var sessionResponse in sessionDTOs)
                       {
                           Sessions.Add(new SessionItemViewModel(sessionResponse));
                       }
               }, null);
            }
        }

        #endregion
    }
}
