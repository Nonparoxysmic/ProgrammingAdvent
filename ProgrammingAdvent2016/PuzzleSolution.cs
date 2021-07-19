// Advent of Code 2016
// https://adventofcode.com/2016
// ProgrammingAdvent2016 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;

namespace ProgrammingAdvent2016
{
    public class PuzzleSolution
    {
        string partOneSolution;
        string partTwoSolution;
        long partOneMilliseconds;
        long totalMilliseconds;

        public PuzzleSolution()
        {
            partOneSolution = "Part One Solution Not Yet Implemented";
            partTwoSolution = "Part Two Solution Not Yet Implemented";
        }

        public string PartOneSolution()
        {
            return partOneSolution;
        }

        public string PartTwoSolution()
        {
            return partTwoSolution;
        }

        public long PartOneMilliseconds()
        {
            return partOneMilliseconds;
        }

        public long TotalMilliseconds()
        {
            return totalMilliseconds;
        }

        public void WriteSolution(int part, string solution, long milliseconds)
        {
            if (part == 1)
            {
                partOneSolution = solution;
                partOneMilliseconds = milliseconds;
            }
            else if (part == 2)
            {
                partTwoSolution = solution;
                totalMilliseconds = milliseconds;
            }
        }
    }
}
