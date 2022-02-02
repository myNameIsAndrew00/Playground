using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Configurator.ViewModel
{
    public class MainWindowViewModel
    {
        public static ApplicationViewModel ApplicationViewModel { get; set; } = new ApplicationViewModel();

        public ICommand CloseCommand { get; set; }
    
        public MainWindowViewModel(Action closeCallback)
        {
            CloseCommand = new CommandInitiator(closeCallback);
        }


    }
}
