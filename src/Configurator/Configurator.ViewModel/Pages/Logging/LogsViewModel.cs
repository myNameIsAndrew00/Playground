using Service.ConfigurationAPI.Client;
using Service.ConfigurationAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Configurator.ViewModel.Pages.Logging
{
    public class LogsViewModel : BaseViewModel
    {
        private Guid? logsPollingGuid;

        public const string DESCRIPTION = "Visualise events which ocurred on token.";

        public const string TITLE = "Logs";

        public override string Title { get; set; } = TITLE;

        public override string Description { get; set; } = DESCRIPTION;

        public override ApplicationPages PreviousPage { get; set; } = ApplicationPages.Dashboard;

        public ObservableCollection<LogsItemViewModel> LogsData { get; set; } = new ObservableCollection<LogsItemViewModel>();

        /// <summary>
        /// This command handles the sessions list refresh.
        /// </summary>
        public ICommand RefreshLogsCommand { get; set; }


        public LogsViewModel()
        {
            RefreshLogsCommand = new CommandInitiator(async () => await SynchroniseLogs());


            logsPollingGuid = Application.Client.StartLongPoll<List<LogDTO>>(
                endpoint: Endpoint.Logs,
                method: HttpMethod.Get,
                callback: (logsResponse) => ReloadLogs(logsResponse.Data)
                );
        }

        public async override Task Initialise()
        {
            await SynchroniseLogs();

            await base.Initialise();
        }

        public override void Dispose()
        {
            if (logsPollingGuid is not null)
                Application.Client.CancelLongPolling(logsPollingGuid.Value);

            base.Dispose();
        }

        #region Private 

        private async Task SynchroniseLogs()
        {
            var sessionsResponse = await Application.Instance.Client.Get<List<LogDTO>>(Endpoint.Logs);

            ReloadLogs(sessionsResponse.Data);

        }

        private void ReloadLogs(List<LogDTO> logDtos)
        {
            lock (LogsData)
            {
                UIContext.Send(_ =>
                {
                    LogsData.Clear();

                    if (logDtos is not null)
                        foreach (var logDto in logDtos)
                        {
                            LogsData.Add(new LogsItemViewModel(logDto));
                        }
                }, null);
            }
        }


        #endregion

    }
}
