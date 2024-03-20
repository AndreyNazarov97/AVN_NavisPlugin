using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Helpers;

namespace AVN_NavisPlugin.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<Exception> _onError;
        private readonly WeakAction _execute;

        private readonly WeakFunc<bool> _canExecute;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="execute">Метод выполнения команды</param>
        /// <param name="keepTargetAlive">If true, the target of the Action will
        /// be kept as a hard reference, which might cause a memory leak. You should only set this
        /// parameter to true if the action is causing a closure. See
        /// http://galasoft.ch/s/mvvmweakaction.</param>
        public RelayCommand(Action execute, bool keepTargetAlive = false)
            : this(execute, null, null, keepTargetAlive)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="execute">Метод выполнения команды</param>
        /// <param name="canExecute">Метод проверки возможности выполнения команды</param>
        /// <param name="keepTargetAlive">If true, the target of the Action will
        /// be kept as a hard reference, which might cause a memory leak. You should only set this
        /// parameter to true if the action is causing a closure. See
        /// http://galasoft.ch/s/mvvmweakaction.</param>
        public RelayCommand(Action execute, Func<bool> canExecute, bool keepTargetAlive = false)
            : this(execute, canExecute, null, keepTargetAlive)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="execute">Метод выполнения команды</param>
        /// <param name="canExecute">Метод проверки возможности выполнения команды</param>
        /// <param name="onError">Обработчик ошибки</param>
        /// <param name="keepTargetAlive">If true, the target of the Action will
        /// be kept as a hard reference, which might cause a memory leak. You should only set this
        /// parameter to true if the action is causing a closure. See
        /// http://galasoft.ch/s/mvvmweakaction.</param>
        public RelayCommand(
            Action execute,
            Func<bool> canExecute,
            Action<Exception> onError,
            bool keepTargetAlive = false)
        {
            _onError = onError;
            _execute = new WeakAction(execute, keepTargetAlive);

            if (canExecute != null)
                _canExecute = new WeakFunc<bool>(canExecute, keepTargetAlive);
        }

        /// <summary>
        /// Событие на проверку возможности выполнения команды
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Возможность выполнения команды
        /// </summary>
        /// <returns>true - возможно, иначе - false</returns>
        public bool CanExecute()
        {
            return _canExecute == null ||
                   ((_canExecute.IsStatic || _canExecute.IsAlive) && _canExecute.Execute());
        }

        /// <summary>
        /// Выполнение команды
        /// </summary>
        public virtual void Execute()
        {
            if (!CanExecute() || (!_execute.IsStatic && !_execute.IsAlive))
                return;

            try
            {
                _execute.Execute();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <inheritdoc/>
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        /// <inheritdoc/>
        void ICommand.Execute(object parameter)
        {
            Execute();
        }
    }
}
