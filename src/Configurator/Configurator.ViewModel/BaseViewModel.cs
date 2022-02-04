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
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public static Application Application { get; set; } = Application.Instance;


        public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };

        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public ICommand GoBackCommand { get; set; }

        public virtual ApplicationPages PreviousPage { get; set; } = ApplicationPages.Connect;

        public BaseViewModel()
        {
            GoBackCommand = new CommandInitiator(async () => await Application.Instance.ChangePage(PreviousPage));
        }

        public virtual Task Initialise()
        {
            return Task.FromResult(0);
        }
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
