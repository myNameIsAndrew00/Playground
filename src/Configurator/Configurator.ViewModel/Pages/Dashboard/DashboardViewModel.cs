using Configurator.ViewModel.Pages.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurator.ViewModel.Pages.Dashboard
{
    public class DashboardViewModel : BaseViewModel
    { 
        public List<DashboardItemViewModel> Sections { get; set; } = new List<DashboardItemViewModel>()
        {
            new DashboardItemViewModel(ApplicationPages.Sessions )
            {
                Title = SessionsViewModel.TITLE,
                Description = SessionsViewModel.DESCRIPTION,
                Icon = "session"
            },
            new DashboardItemViewModel(ApplicationPages.Dashboard)
            {
                Title = "Logs",
                Description = "Check logs ocurred on token",
                Icon = "logs"
            },
            new DashboardItemViewModel(ApplicationPages.Dashboard)
            {
                Title = "Certificates",
                Description = "Check certificates stored on token",
                Icon = "certificate"
            },
            new DashboardItemViewModel(ApplicationPages.Connect)
            {
                Title = "Disconnect",
                Description = "Disconnect from service",
                Icon = "disconnect"
            }
        };
    }
}
