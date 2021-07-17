using System;
using System.Diagnostics;

namespace ProgrammingAdvent2016
{
    public class DayTEMPLATE : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            // string[] inputLines = input.ToLines();
            stopwatch.Start();



            stopwatch.Reset();
            return solution;
        }
    }
}
