using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProgrammingAdvent2016
{
    public partial class Form1 : Form
    {
        #region Array Declarations
        readonly string[] inputFilePaths = new string[] { "",
            @"InputFiles\InputDay1.txt",
            @"InputFiles\InputDay2.txt",
            @"InputFiles\InputDay3.txt" };
        readonly Day[] dayClasses = new Day[] { null,
            new Day01(),
            new Day02(),
            new Day03() };
        readonly TextBox[] partOneTextBoxes;
        readonly TextBox[] partTwoTextBoxes;
        readonly Button[] dayButtons;
        #endregion

        int buttonsClicked = 0;

        public Form1()
        {
            InitializeComponent();
            Form1_Resize(null, null);

            #region Array Initialization
            partOneTextBoxes = new TextBox[] { null,
                textBoxPart1Day1,
                textBoxPart1Day2,
                textBoxPart1Day3 };
            partTwoTextBoxes = new TextBox[] { null,
                textBoxPart2Day1,
                textBoxPart2Day2,
                textBoxPart2Day3 };
            dayButtons = new Button[] { null,
                buttonDay1,
                buttonDay2,
                buttonDay3 };
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
        }

        private void DayButtonClicked(int day)
        {
            if (day < 1 || day >= inputFilePaths.Length) return;
            if (Program.ReadInputFile(inputFilePaths[day], out string input))
            {
                PuzzleSolution solution = dayClasses[day].FindSolution(input);
                partOneTextBoxes[day].Text = solution.PartOneSolution();
                partTwoTextBoxes[day].Text = solution.PartTwoSolution();
                dayButtons[day].Enabled = false;
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
            DayButtonClicked(1);
        }

        private void ButtonDay2_Click(object sender, EventArgs e)
        {
            DayButtonClicked(2);
        }

        private void ButtonDay3_Click(object sender, EventArgs e)
        {
            DayButtonClicked(3);
        }
    }
}
