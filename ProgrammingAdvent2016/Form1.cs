// Advent of Code 2016
// https://adventofcode.com/2016
// ProgrammingAdvent2016 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgrammingAdvent2016
{
    public partial class Form1 : Form
    {
        #region Array Declarations
        readonly string[] inputFilePaths = new string[] { "",
            @"InputFiles\InputDay1.txt",
            @"InputFiles\InputDay2.txt",
            @"InputFiles\InputDay3.txt",
            @"InputFiles\InputDay4.txt",
            @"InputFiles\InputDay5.txt",
            @"InputFiles\InputDay6.txt",
            @"InputFiles\InputDay7.txt",
            @"InputFiles\InputDay8.txt" };
        readonly Day[] dayClasses = new Day[] { null,
            new Day01(),
            new Day02(),
            new Day03(),
            new Day04(),
            new Day05(),
            new Day06(),
            new Day07(),
            new Day08() };
        readonly TextBox[] partOneTextBoxes;
        readonly TextBox[] partTwoTextBoxes;
        readonly Button[] dayButtons;
        readonly Label[] timeLabels;
        #endregion

        int buttonsClicked = 0;

        public Form1()
        {
            InitializeComponent();
            Form1_Resize(null, null);

            Bitmap blankScreenDay8 = new Bitmap(104, 16);
            using (Graphics g = Graphics.FromImage(blankScreenDay8))
            {
                g.FillRectangle(new SolidBrush(Color.Lime), 0, 0, 104, 16);
            }
            pictureBoxDay8Part2.Image = blankScreenDay8;

            #region Array Initialization
            partOneTextBoxes = new TextBox[] { null,
                textBoxPart1Day1,
                textBoxPart1Day2,
                textBoxPart1Day3,
                textBoxPart1Day4,
                textBoxPart1Day5,
                textBoxPart1Day6,
                textBoxPart1Day7,
                textBoxPart1Day8 };
            partTwoTextBoxes = new TextBox[] { null,
                textBoxPart2Day1,
                textBoxPart2Day2,
                textBoxPart2Day3,
                textBoxPart2Day4,
                textBoxPart2Day5,
                textBoxPart2Day6,
                textBoxPart2Day7,
                null };
            dayButtons = new Button[] { null,
                buttonDay1,
                buttonDay2,
                buttonDay3,
                buttonDay4,
                buttonDay5,
                buttonDay6,
                buttonDay7,
                buttonDay8 };
            timeLabels = new Label[] { null,
                labelTimeDay1,
                labelTimeDay2,
                labelTimeDay3,
                labelTimeDay4,
                labelTimeDay5,
                labelTimeDay6,
                labelTimeDay7,
                labelTimeDay8 };
            #endregion
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            tableLayoutPanelDays.Location = new Point((panel1.Width - tableLayoutPanelDays.Width) / 2 - 8, tableLayoutPanelDays.Location.Y);
        }

        private void ButtonSolveAll_Click(object sender, EventArgs e)
        {
            buttonSolveAll.Enabled = false;
            ClickAllButtons();
        }

        public void ClickAllButtons()
        {
            if (buttonDay1.Enabled) ButtonDay1_Click(null, null);
            if (buttonDay2.Enabled) ButtonDay2_Click(null, null);
            if (buttonDay3.Enabled) ButtonDay3_Click(null, null);
            if (buttonDay4.Enabled) ButtonDay4_Click(null, null);
            if (buttonDay5.Enabled) ButtonDay5_Click(null, null);
            if (buttonDay6.Enabled) ButtonDay6_Click(null, null);
            if (buttonDay7.Enabled) ButtonDay7_Click(null, null);
            if (buttonDay8.Enabled) ButtonDay8_Click(null, null);
        }

        private async void DayButtonClicked_Async(int day)
        {
            if (day < 1 || day >= inputFilePaths.Length) return;
            if (Program.ReadInputFile(inputFilePaths[day], out string input))
            {
                dayButtons[day].Text = "";
                dayButtons[day].Enabled = false;
                timeLabels[day].Text = "Time: TBD";
                PuzzleSolution solution = await Task.Run(() => dayClasses[day].FindSolution(input));
                dayButtons[day].Text = "Solved";
                partOneTextBoxes[day].Text = solution.PartOneSolution();
                if (day == 8)
                {
                    pictureBoxDay8Part2.Image = solution.ImageSolution();
                }
                else partTwoTextBoxes[day].Text = solution.PartTwoSolution();
                double solutionTime = Math.Ceiling(Math.Max(solution.TotalMilliseconds(), 1) / 10.0) / 100.0;
                if (solutionTime < 10)
                {
                    timeLabels[day].Text = "Time: " + solutionTime.ToString("F2") + " s";
                }
                else timeLabels[day].Text = "Time: " + solutionTime.ToString("F1") + " s";
                if (++buttonsClicked == dayClasses.Length - 1)
                {
                    buttonSolveAll.Enabled = false;
                }
            }
            else
            {
                partOneTextBoxes[day].Text = "Cannot read \\" + inputFilePaths[day];
            }
        }

        private void ButtonDay1_Click(object sender, EventArgs e)
        {
            DayButtonClicked_Async(1);
        }

        private void ButtonDay2_Click(object sender, EventArgs e)
        {
            DayButtonClicked_Async(2);
        }

        private void ButtonDay3_Click(object sender, EventArgs e)
        {
            DayButtonClicked_Async(3);
        }

        private void ButtonDay4_Click(object sender, EventArgs e)
        {
            DayButtonClicked_Async(4);
        }

        private void ButtonDay5_Click(object sender, EventArgs e)
        {
            DayButtonClicked_Async(5);
        }

        private void ButtonDay6_Click(object sender, EventArgs e)
        {
            DayButtonClicked_Async(6);
        }

        private void ButtonDay7_Click(object sender, EventArgs e)
        {
            DayButtonClicked_Async(7);
        }

        private void ButtonDay8_Click(object sender, EventArgs e)
        {
            DayButtonClicked_Async(8);
        }
    }
}
