using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Model;

namespace GUI.ViewModel
{
    class Window6VM : Ultilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public string Window6
        {
            get { return _pageModel.Window6; }
            set { _pageModel.Window6 = value; OnPropertyChanged(); }
        }

        public Window6VM()
        {
            _pageModel = new PageModel();
        }
    }
}

public class RelayCommand<T> : ICommand
{
    private Action<T> _execute;
    private Func<T, bool> _canExecute;

    public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute((T)parameter);
    }

    public void Execute(object parameter)
    {
        _execute((T)parameter);
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}
