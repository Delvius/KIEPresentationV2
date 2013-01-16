using System;
using System.Windows.Input;

namespace KIEPresentation.Service
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Func<bool> _canExecute = null;
        Action _executeAction = null;

        public RelayCommand(Action executeAction, Func<bool> canExecute = null)
        {
            _executeAction = executeAction;
            _canExecute = canExecute;
        }

        public void UpdateCanExecute()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                return _canExecute();

            return true;
        }

        public void Execute(object parameter)
        {
            if (_executeAction != null)
                _executeAction();
        }



    }
}
