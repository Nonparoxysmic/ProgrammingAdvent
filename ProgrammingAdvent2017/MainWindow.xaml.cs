using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            Reflection.InitializeDictionary();
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
            _ = MessageBox.Show("WIP: Feature to be added.", "Multi Solve");
        }

        private void ReturnToMenuFromSingle_Click(object sender, RoutedEventArgs e)
        {
            MainMenuStackPanel.Visibility = Visibility.Visible;
            SingleSolveGrid.Visibility = Visibility.Hidden;
        }
    }
}
