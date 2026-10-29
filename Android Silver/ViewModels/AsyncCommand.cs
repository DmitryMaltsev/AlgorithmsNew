using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Android_Silver.ViewModels
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private bool _isRunning;
        public AsyncCommand(Func<Task> execute)=>_execute=execute;

        public bool CanExecute(object parameter) => !_isRunning;
        public async void Execute(object parameter)
        {
           if (_isRunning) return;
           _isRunning = true;
            RaiseCanExecuteChanged();
            try { await _execute(); }
            catch(Exception){}
            finally
            {
                _isRunning = false;
                RaiseCanExecuteChanged();
            }
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()=>
            CanExecuteChanged?.Invoke(this,EventArgs.Empty);
    }
}
