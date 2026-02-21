using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentClient.ViewModel.Commands
{
    public class RelaisCommandeParametre: ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelaisCommandeParametre(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object? parameter) => _execute(parameter);

        public event EventHandler? CanExecuteChanged;
    }
}
