// ProgrammingAdvent2018 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProgrammingAdvent2018.Program
{
    internal static class MultiSolver
    {
        private static Dictionary<int, TextBox> partOneTextBoxes;
        private static Dictionary<int, TextBox> partTwoTextBoxes;
        private static Dictionary<int, Label> timeOutputLabels;
        private static Dictionary<int, Image> images;
        private static readonly Stopwatch sw = new Stopwatch();

        internal static void Initialize(StackPanel multiSolverPanel)
        {
            partOneTextBoxes = new Dictionary<int, TextBox>();
            partTwoTextBoxes = new Dictionary<int, TextBox>();
            timeOutputLabels = new Dictionary<int, Label>();
            images = new Dictionary<int, Image>();

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
                if (day == 10)
                {
                    Image image = new Image
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = marginThickness,
                        SnapsToDevicePixels = true
                    };
                    image.SetValue(Grid.ColumnProperty, 2);
                    RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
                    images.Add(day, image);
                    _ = grid.Children.Add(image);
                }
                else
                {
                    _ = grid.Children.Add(partOneTextBox);
                }
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

        internal static async Task<long> SolveAll()
        {
            int[] days = Reflection.GetDayNumbers();

            // Reset the displayed output.
            foreach (int dayNumber in days)
            {
                partOneTextBoxes[dayNumber].Text = "";
                partTwoTextBoxes[dayNumber].Text = "";
                timeOutputLabels[dayNumber].Content = "-.-- seconds";
            }
            images[10].Source = null;

            // Make sure the Day objects are initialized before parallel computation.
            _ = Day.GetDayObject(days[0]);

            // Start calculating all of the answers.
            List<Task<(PuzzleAnswers, int)>> tasks = new List<Task<(PuzzleAnswers, int)>>();
            foreach (int day in days)
            {
                tasks.Add(Task.Run(() => CalculateAnswers(day)));
            }

            // As tasks are completed, output results.
            long longestTime = 0;
            while (tasks.Any())
            {
                Task<(PuzzleAnswers, int)> finishedTask = await Task.WhenAny(tasks);
                int dayNumber = finishedTask.Result.Item2;
                partOneTextBoxes[dayNumber].Text = finishedTask.Result.Item1.PartOneAnswer;
                if (dayNumber == 10)
                {
                    SimpleBitmap bitmap = finishedTask.Result.Item1.PartOneBitmap;
                    images[dayNumber].Source = bitmap.ToBitmapSource();
                    images[dayNumber].Height = bitmap.Height;
                }
                partTwoTextBoxes[dayNumber].Text = finishedTask.Result.Item1.PartTwoAnswer;
                long elapsedMilliseconds = finishedTask.Result.Item1.ElapsedMilliseconds;
                longestTime = Math.Max(longestTime, elapsedMilliseconds);
                timeOutputLabels[dayNumber].Content = PuzzleAnswers
                    .MillisecondsToDisplayTime(elapsedMilliseconds);
                _ = tasks.Remove(finishedTask);
            }

            return longestTime;
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
