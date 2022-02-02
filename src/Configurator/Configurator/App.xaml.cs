using Configurator.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Configurator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SetupWindow();

            MainWindow.Show();
        }

        private void SetupWindow()
        {
            Current.MainWindow = new MainWindow();
            Current.MainWindow.DataContext = new MainWindowViewModel(  
                closeCallback: () => MainWindow.Close() 
            );

            Current.MainWindow.MouseLeftButtonDown += delegate { MainWindow.DragMove(); };
        }
    }
}
