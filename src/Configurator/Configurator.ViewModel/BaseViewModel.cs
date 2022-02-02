using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Configurator.ViewModel
{
    /// <summary>
    /// A base class for view models.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };
    }

    public class CommandInitiator : ICommand
    {
        private Action mAction;
        public event EventHandler CanExecuteChanged = (s, e) => { };

        public CommandInitiator(Action Action) => mAction = Action;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => mAction();
    }

    class ParameterizedCommandInitiator : ICommand
    {
        private Action<object> mAction;
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public ParameterizedCommandInitiator(Action<object> action) => mAction = action;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => mAction(parameter);
    }
}
