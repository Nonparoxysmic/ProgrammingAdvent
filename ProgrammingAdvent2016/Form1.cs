﻿using System;
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

        public void ButtonDay1_Click(object sender, EventArgs e)
        {
            DayTEMPLATE.SetSolutionText(textBoxPart1Day1, textBoxPart2Day1);
        }

        public void ButtonDay2_Click(object sender, EventArgs e)
        {
            DayTEMPLATE.SetSolutionText(textBoxPart1Day2, textBoxPart2Day2);
        }
    }
}
