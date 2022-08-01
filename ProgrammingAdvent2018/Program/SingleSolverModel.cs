// ProgrammingAdvent2018 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

namespace ProgrammingAdvent2018.Program
{
    internal class SingleSolverModel
    {
        internal string DaySelected { get; set; }

        internal string InputText { get; set; } = "";

        internal bool SolveButtonEnabled { get; set; } = true;

        internal bool LoadButtonEnabled { get; set; } = true;

        internal bool SaveButtonEnabled { get; set; } = true;

        internal string Status { get; set; }

        internal string PartOneOutput { get; set; }

        internal string PartTwoOutput { get; set; }

        internal string TimeOutput { get; set; }
    }
}
