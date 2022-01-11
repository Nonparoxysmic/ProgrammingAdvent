// ProgrammingAdvent2017 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ProgrammingAdvent2017.Program
{
    public class SingleSolverViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private readonly SingleSolverModel _singleSolverModel;

        public SingleSolverViewModel()
        {
            _singleSolverModel = new SingleSolverModel();
            SolveButtonCommand = new SimpleRelayCommand(SolveButton_Click);
            SolveButtonEnabled = true;
        }

        public string DaySelected
        {
            get { return _singleSolverModel.DaySelected; }
            set { _singleSolverModel.DaySelected = value; OnPropertyChanged(nameof(DaySelected)); }
        }

        public string InputText
        {
            get { return _singleSolverModel.InputText; }
            set { _singleSolverModel.InputText = value; OnPropertyChanged(nameof(InputText)); }
        }

        public string PartOneOutput
        {
            get { return _singleSolverModel.PartOneOutput; }
            set { _singleSolverModel.PartOneOutput = value; OnPropertyChanged(nameof(PartOneOutput)); }
        }

        public string PartTwoOutput
        {
            get { return _singleSolverModel.PartTwoOutput; }
            set { _singleSolverModel.PartTwoOutput = value; OnPropertyChanged(nameof(PartTwoOutput)); }
        }

        public string TimeOutput
        {
            get { return _singleSolverModel.TimeOutput; }
            set { _singleSolverModel.TimeOutput = value; OnPropertyChanged(nameof(TimeOutput)); }
        }

        public IList<string> DayOptions { get { return _dayOptions; } }

        private readonly ObservableCollection<string> _dayOptions = InitializeDayOptions();

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

        public bool SolveButtonEnabled { get; set; }

        public ICommand SolveButtonCommand { get; set; }

        private void SolveButton_Click()
        {

        }
    }
}
