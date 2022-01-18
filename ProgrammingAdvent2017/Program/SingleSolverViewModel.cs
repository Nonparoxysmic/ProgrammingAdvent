// ProgrammingAdvent2017 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProgrammingAdvent2017.Program
{
    internal class SingleSolverViewModel : INotifyPropertyChanged
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
            LoadButtonCommand = new SimpleRelayCommand(LoadButton_Click);
            SaveButtonCommand = new SimpleRelayCommand(SaveButton_Click);
        }

        public string DaySelected
        {
            get => _singleSolverModel.DaySelected;
            set
            {
                _singleSolverModel.DaySelected = value;
                OnPropertyChanged(nameof(DaySelected));
            }
        }

        public string InputText
        {
            get => _singleSolverModel.InputText;
            set
            {
                _singleSolverModel.InputText = value;
                OnPropertyChanged(nameof(InputText));
            }
        }

        public bool SolveButtonEnabled
        {
            get => _singleSolverModel.SolveButtonEnabled;
            set
            {
                _singleSolverModel.SolveButtonEnabled = value;
                OnPropertyChanged(nameof(SolveButtonEnabled));
            }
        }

        public bool LoadButtonEnabled
        {
            get => _singleSolverModel.LoadButtonEnabled;
            set
            {
                _singleSolverModel.LoadButtonEnabled = value;
                OnPropertyChanged(nameof(LoadButtonEnabled));
            }
        }

        public bool SaveButtonEnabled
        {
            get => _singleSolverModel.SaveButtonEnabled;
            set
            {
                _singleSolverModel.SaveButtonEnabled = value;
                OnPropertyChanged(nameof(SaveButtonEnabled));
            }
        }

        public string Status
        {
            get => _singleSolverModel.Status;
            set
            {
                _singleSolverModel.Status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public string PartOneOutput
        {
            get => _singleSolverModel.PartOneOutput;
            set
            {
                _singleSolverModel.PartOneOutput = value;
                OnPropertyChanged(nameof(PartOneOutput));
            }
        }

        public string PartTwoOutput
        {
            get => _singleSolverModel.PartTwoOutput;
            set
            {
                _singleSolverModel.PartTwoOutput = value;
                OnPropertyChanged(nameof(PartTwoOutput));
            }
        }

        public string TimeOutput
        {
            get => _singleSolverModel.TimeOutput;
            set
            {
                _singleSolverModel.TimeOutput = value;
                OnPropertyChanged(nameof(TimeOutput));
            }
        }

        public IList<string> DayOptions => _dayOptions;

        private readonly ObservableCollection<string> _dayOptions = InitializeDayOptions();

        private static ObservableCollection<string> InitializeDayOptions()
        {
            int[] dayNumbers = Reflection.GetDayNumbers();
            List<string> output = new List<string>();
            foreach (int i in dayNumbers)
            {
                output.Add("Day " + i);
            }
            return new ObservableCollection<string>(output);
        }

        public ICommand SolveButtonCommand { get; set; }

        private void SolveButton_Click()
        {
            SolveButtonEnabled = false;
            Status = "Working...";
            _ = Task.Run(() => CalculateAnswers());
        }

        private void CalculateAnswers()
        {
            int dayNumber = Day.ParseDayNumber(DaySelected);
            PuzzleAnswers answers = Day.GetDayObject(dayNumber).Solve(InputText);
            Status = DaySelected + " Solution";
            PartOneOutput = answers.PartOneAnswer;
            PartTwoOutput = answers.PartTwoAnswer;
            double seconds = ((answers.ElapsedMilliseconds / 10) + 1) / 100.0;
            TimeOutput = seconds.ToString("F2") + " seconds";
            SolveButtonEnabled = true;
        }

        public ICommand LoadButtonCommand { get; set; }

        private void LoadButton_Click()
        {
            int dayNumber = Day.ParseDayNumber(DaySelected);
            if (IO.TryReadInputFile(dayNumber, out string result))
            {
                InputText = result;
            }
            else
            {
                InputText = "";
                _ = MessageBox.Show(result, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand SaveButtonCommand { get; set; }

        private void SaveButton_Click()
        {
            int dayNumber = Day.ParseDayNumber(DaySelected);
            if (InputText != null && InputText.Trim().Length > 0)
            {
                string result = IO.WriteInputFile(dayNumber, InputText);
                _ = MessageBox.Show(result, "Result",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string error = "Nothing to save. Input text box is empty.";
                _ = MessageBox.Show(error, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
