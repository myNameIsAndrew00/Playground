using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Configurator.ViewModel.Pages.Dashboard
{
    public class DashboardItemViewModel : BaseViewModel
    {
        public string Icon { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ICommand NavigateCommand { get; set; }

        public DashboardItemViewModel(ApplicationPages page)
        {
            NavigateCommand = new CommandInitiator(async () => await Application.ChangePage(page));
        }
    }
}
