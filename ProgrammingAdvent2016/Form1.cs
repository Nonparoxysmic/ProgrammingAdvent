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

        private void ButtonDay1_Click(object sender, EventArgs e)
        {
            textBoxPart1Day1.Text = "[Day 1 Part 1 solution here]";
            textBoxPart2Day1.Text = "[Day 1 Part 2 solution here]";
        }

        private void ButtonDay2_Click(object sender, EventArgs e)
        {
            textBoxPart1Day2.Text = "[Day 2 Part 1 solution here]";
            textBoxPart2Day2.Text = "[Day 2 Part 2 solution here]";
        }
    }
}
