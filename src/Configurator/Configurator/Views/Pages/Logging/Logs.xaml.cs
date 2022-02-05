using Configurator.ViewModel.Pages;
using Configurator.ViewModel.Pages.Dashboard;
using Configurator.ViewModel.Pages.Logging;
using Configurator.ViewModel.Pages.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Configurator.Views.Pages.Logging
{
    /// <summary>
    /// Interaction logic for Connect.xaml
    /// </summary>
    public partial class Logs : BasePage<LogsViewModel>
    {
        public Logs()
        {
            InitializeComponent();
        }
    }
}
