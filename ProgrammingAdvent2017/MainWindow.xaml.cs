using System.Windows;
using System.Windows.Input;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Program.MultiSolver.Initialize(MultiSolveResultPanel);
        }

        private void ExitProgramButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow
            {
                Owner = this
            };
            _ = aboutWindow.ShowDialog();
        }

        private void MenuSingleSolveButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuStackPanel.Visibility = Visibility.Hidden;
            SingleSolveGrid.Visibility = Visibility.Visible;
        }

        private void MenuMultiSolveButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuStackPanel.Visibility = Visibility.Hidden;
            MultiSolveGrid.Visibility = Visibility.Visible;
        }

        private void ReturnToMenuFromSolver_Click(object sender, RoutedEventArgs e)
        {
            MainMenuStackPanel.Visibility = Visibility.Visible;
            SingleSolveGrid.Visibility = Visibility.Hidden;
            MultiSolveGrid.Visibility = Visibility.Hidden;
        }

        private async void SolveAllButton_Click(object sender, RoutedEventArgs e)
        {
            SolveAllButton.IsEnabled = false;
            SolveAllButton.Content = "Solving...";
            OverallTimeLabel.Content = "";
            long milliseconds = await MultiSolver.SolveAll();
            OverallTimeLabel.Content = PuzzleAnswers.MillisecondsToDisplayTime(milliseconds);
            SolveAllButton.Content = "Solve All";
            SolveAllButton.IsEnabled = true;
        }
    }
}
