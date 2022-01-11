// ProgrammingAdvent2017 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Windows.Input;

namespace ProgrammingAdvent2017.Program
{
    public class SimpleRelayCommand : ICommand
    {
        readonly Action _executeMethod;

        public SimpleRelayCommand(Action executeMethod)
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
