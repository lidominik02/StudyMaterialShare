﻿using System;
using System.Windows.Input;

namespace StudyMaterialShare.Desktop.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object?> _execute; // a tevékenységet végrehajtó lambda-kifejezés
        private readonly Func<object?, Boolean> _canExecute; // a tevékenység feltételét ellenőző lambda-kifejezés

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateCommand(Action<object?> execute) : this(null!, execute) { }

        public DelegateCommand(Func<object?, Boolean> canExecute, Action<object?> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        public Boolean CanExecute(object? parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("Command execution is disabled.");
            }
            _execute(parameter);
        }
    }
}
