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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClickAllButtons();
        }

        public void ClickAllButtons()
        {
            ButtonDay1_Click(null, null);
            ButtonDay2_Click(null, null);
        }

        public void ButtonDay1_Click(object sender, EventArgs e)
        {
            Day01.SetSolutionText(textBoxPart1Day1, textBoxPart2Day1);
        }

        public void ButtonDay2_Click(object sender, EventArgs e)
        {
            Day02.SetSolutionText(textBoxPart1Day2, textBoxPart2Day2);
        }
    }
}
