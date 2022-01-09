// ProgrammingAdvent2017 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProgrammingAdvent2017.Program
{
    class SingleSolverViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public IList<string> DayOptions { get { return _dayOptions; } }

        readonly private ObservableCollection<string> _dayOptions = InitializeDayOptions();

        public string DaySelected
        {
            get { return _daySelected; }
            set { _daySelected = value; OnPropertyChanged("DaySelected"); }
        }

        private string _daySelected;

        private static ObservableCollection<string> InitializeDayOptions()
        {
            int[] dayNumbers = Reflection.GetDayNumbers();
            var output = new List<string>();
            foreach (int i in dayNumbers)
            {
                output.Add("Day " + i);
            }
            return new ObservableCollection<string>(output);
        }
    }
}
