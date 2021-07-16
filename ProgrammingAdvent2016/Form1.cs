using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProgrammingAdvent2016
{
    public partial class Form1 : Form
    {
        int buttonsClicked = 0;
        const int buttonsImplemented = 3;

        public Form1()
        {
            InitializeComponent();
            Form1_Resize(null, null);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            tableLayoutPanelDays.Location = new Point((panel1.Width - tableLayoutPanelDays.Width) / 2 - 8, tableLayoutPanelDays.Location.Y);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ClickAllButtons();
            button1.Enabled = false;
        }

        public void ClickAllButtons()
        {
            if (buttonDay1.Enabled) ButtonDay1_Click(null, null);
            if (buttonDay2.Enabled) ButtonDay2_Click(null, null);
            if (buttonDay3.Enabled) ButtonDay3_Click(null, null);
        }

        private void DayButtonClicked()
        {
            if (++buttonsClicked == buttonsImplemented)
            {
                button1.Enabled = false;
            }
        }

        private void ButtonDay1_Click(object sender, EventArgs e)
        {
            string input;
            try
            {
                input = System.IO.File.ReadAllText(@"InputFiles\InputDay01Part1.txt").Trim();
            }
            catch
            {
                textBoxPart1Day1.Text = "ERROR: Unable to read input file.";
                return;
            }

            Day01 day = new Day01();
            PuzzleSolution solution = day.FindSolution(input);
            textBoxPart1Day1.Text = solution.PartOneSolution();
            textBoxPart2Day1.Text = solution.PartTwoSolution();
            buttonDay1.Enabled = false;
            DayButtonClicked();
        }

        private void ButtonDay2_Click(object sender, EventArgs e)
        {
            string input;
            try
            {
                input = System.IO.File.ReadAllText(@"InputFiles\InputDay02Part1.txt");
            }
            catch
            {
                textBoxPart1Day2.Text = "ERROR: Unable to read input file.";
                return;
            }

            Day02 day = new Day02();
            PuzzleSolution solution = day.FindSolution(input);
            textBoxPart1Day2.Text = solution.PartOneSolution();
            textBoxPart2Day2.Text = solution.PartTwoSolution();
            buttonDay2.Enabled = false;
            DayButtonClicked();
        }

        private void ButtonDay3_Click(object sender, EventArgs e)
        {
            string input;
            try
            {
                input = System.IO.File.ReadAllText(@"InputFiles\InputDay03Part1.txt");
            }
            catch
            {
                textBoxPart1Day3.Text = "ERROR: Unable to read input file.";
                return;
            }

            Day03 day = new Day03();
            PuzzleSolution solution = day.FindSolution(input);
            textBoxPart1Day3.Text = solution.PartOneSolution();
            textBoxPart2Day3.Text = solution.PartTwoSolution();
            buttonDay3.Enabled = false;
            DayButtonClicked();
        }
    }
}
