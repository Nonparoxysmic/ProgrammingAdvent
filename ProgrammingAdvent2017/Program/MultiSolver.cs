// ProgrammingAdvent2017 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProgrammingAdvent2017.Program
{
    internal static class MultiSolver
    {
        private static Dictionary<int, TextBox> partOneTextBoxes;
        private static Dictionary<int, TextBox> partTwoTextBoxes;
        private static Dictionary<int, Label> timeOutputLabels;
        private static readonly Stopwatch sw = new Stopwatch();

        internal static void Initialize(StackPanel multiSolverPanel)
        {
            partOneTextBoxes = new Dictionary<int, TextBox>();
            partTwoTextBoxes = new Dictionary<int, TextBox>();
            timeOutputLabels = new Dictionary<int, Label>();

            Thickness marginThickness = new Thickness(15, 5, 15, 5);
            Thickness borderThickness = new Thickness(1);

            int[] days = Reflection.GetDayNumbers();
            foreach (int day in days)
            {
                Label dayLabel = new Label
                {
                    Content = "Day " + day,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = marginThickness
                };
                dayLabel.SetValue(Grid.RowSpanProperty, 3);

                Label partOneLabel = new Label
                {
                    Content = "Part 1:",
                    Margin = marginThickness
                };
                partOneLabel.SetValue(Grid.ColumnProperty, 1);

                TextBox partOneTextBox = new TextBox
                {
                    Width = 300,
                    IsReadOnly = true,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = marginThickness
                };
                partOneTextBox.SetValue(Grid.ColumnProperty, 2);
                partOneTextBoxes.Add(day, partOneTextBox);

                Label partTwoLabel = new Label
                {
                    Content = "Part 2:",
                    Margin = marginThickness
                };
                partTwoLabel.SetValue(Grid.ColumnProperty, 1);
                partTwoLabel.SetValue(Grid.RowProperty, 1);

                TextBox partTwoTextBox = new TextBox
                {
                    Width = 300,
                    IsReadOnly = true,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = marginThickness
                };
                partTwoTextBox.SetValue(Grid.ColumnProperty, 2);
                partTwoTextBox.SetValue(Grid.RowProperty, 1);
                partTwoTextBoxes.Add(day, partTwoTextBox);

                Label timeLabel = new Label
                {
                    Content = "Time:",
                    Margin = marginThickness
                };
                timeLabel.SetValue(Grid.ColumnProperty, 1);
                timeLabel.SetValue(Grid.RowProperty, 2);

                Label timeOutputLabel = new Label
                {
                    Content = "-.-- seconds",
                    Margin = marginThickness
                };
                timeOutputLabel.SetValue(Grid.ColumnProperty, 2);
                timeOutputLabel.SetValue(Grid.RowProperty, 2);
                timeOutputLabels.Add(day, timeOutputLabel);

                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                ColumnDefinition col = new ColumnDefinition
                {
                    MinWidth = 350
                };
                grid.ColumnDefinitions.Add(col);
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());

                _ = grid.Children.Add(dayLabel);
                _ = grid.Children.Add(partOneLabel);
                _ = grid.Children.Add(partOneTextBox);
                _ = grid.Children.Add(partTwoLabel);
                _ = grid.Children.Add(partTwoTextBox);
                _ = grid.Children.Add(timeLabel);
                _ = grid.Children.Add(timeOutputLabel);

                Border border = new Border
                {
                    BorderBrush = Brushes.Gray,
                    BorderThickness = borderThickness,
                    Child = grid
                };
                borderThickness = new Thickness(1, 0, 1, 1);

                _ = multiSolverPanel.Children.Add(border);
            }
        }

        internal static async Task SolveAll()
        {
            int[] days = Reflection.GetDayNumbers();
            _ = Day.GetDayObject(days[0]);  // Initializes the Day objects
            List<Task<(PuzzleAnswers, int)>> tasks = new List<Task<(PuzzleAnswers, int)>>();
            foreach (int day in days)
            {
                tasks.Add(Task.Run(() => CalculateAnswers(day)));
            }
            while (tasks.Any())
            {
                Task<(PuzzleAnswers, int)> finishedTask = await Task.WhenAny(tasks);
                int dayNumber = finishedTask.Result.Item2;
                partOneTextBoxes[dayNumber].Text = finishedTask.Result.Item1.PartOneAnswer;
                partTwoTextBoxes[dayNumber].Text = finishedTask.Result.Item1.PartTwoAnswer;
                long elapsedMilliseconds = finishedTask.Result.Item1.ElapsedMilliseconds;
                double seconds = ((elapsedMilliseconds / 10) + 1) / 100.0;
                timeOutputLabels[dayNumber].Content = seconds.ToString("F2") + " seconds";
                _ = tasks.Remove(finishedTask);
            }
        }

        private static (PuzzleAnswers, int) CalculateAnswers(int dayNumber)
        {
            if (!IO.TryReadInputFile(dayNumber, out string input))
            {
                PuzzleAnswers noInput = new PuzzleAnswers();
                noInput.WriteError("Unable to load input file.", sw);
                return (noInput, dayNumber);
            }
            return (Day.GetDayObject(dayNumber).Solve(input), dayNumber);
        }
    }
}
