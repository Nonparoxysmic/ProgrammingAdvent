// ProgrammingAdvent2018 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Windows.Input;

namespace ProgrammingAdvent2018.Program
{
    internal class SimpleRelayCommand : ICommand
    {
        private readonly Action _executeMethod;

        internal SimpleRelayCommand(Action executeMethod)
        {
            _executeMethod = executeMethod;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _executeMethod();
        }
    }
}
