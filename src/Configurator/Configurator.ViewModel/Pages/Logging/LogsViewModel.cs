using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurator.ViewModel.Pages.Logging
{
    public class LogsViewModel : BaseViewModel
    {
        public const string DESCRIPTION = "Visualise events which ocurred on token.";

        public const string TITLE = "Logs";

        public override string Title { get; set; } = TITLE;

        public override string Description { get; set; } = DESCRIPTION;

        public override ApplicationPages PreviousPage { get; set; } = ApplicationPages.Dashboard;

        public List<LogsItemViewModel> LogsData { get; set; } = new List<LogsItemViewModel>();
    }
}
