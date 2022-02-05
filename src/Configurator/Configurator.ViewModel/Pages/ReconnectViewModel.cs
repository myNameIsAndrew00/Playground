using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Configurator.ViewModel.Pages
{
    public class ReconnectViewModel : BaseViewModel
    {
        public ICommand DisconnectCommand { get; set; }

        public ReconnectViewModel()
        {
            DisconnectCommand = new CommandInitiator(async () => await Application.ChangePage(ApplicationPages.Connect));
        }
    }
}
