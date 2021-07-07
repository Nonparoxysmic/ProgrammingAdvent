using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Day01.SetSolutionText(textBoxPart1Day1, textBoxPart2Day1);
            buttonDay1.Enabled = false;
            DayButtonClicked();
        }

        private void ButtonDay2_Click(object sender, EventArgs e)
        {
            Day02.SetSolutionText(textBoxPart1Day2, textBoxPart2Day2);
            buttonDay2.Enabled = false;
            DayButtonClicked();
        }

        private void ButtonDay3_Click(object sender, EventArgs e)
        {
            Day03.SetSolutionText(textBoxPart1Day3, textBoxPart2Day3);
            buttonDay3.Enabled = false;
            DayButtonClicked();
        }
    }
}
