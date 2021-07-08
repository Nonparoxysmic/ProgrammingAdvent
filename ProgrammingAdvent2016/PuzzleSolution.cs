using System;

namespace ProgrammingAdvent2016
{
    public class PuzzleSolution
    {
        string partOneSolution;
        string partTwoSolution;
        long partOneMilliseconds;
        long partTwoMilliseconds;

        public PuzzleSolution()
        {
            partOneSolution = "Part One Solution Not Yet Implemented";
            partTwoSolution = "Part Two Solution Not Yet Implemented";
        }

        public long ElapsedMilliseconds(int part)
        {
            if (part == 1)
                return partOneMilliseconds;
            else if (part == 2)
                return partTwoMilliseconds;
            throw new ArgumentOutOfRangeException();
        }

        public string Solution(int part)
        {
            if (part == 1)
                return partOneSolution;
            else if (part == 2)
                return partTwoSolution;
            throw new ArgumentOutOfRangeException();
        }

        public void SetSolution(int part, string solution, long milliseconds)
        {
            if (part == 1)
            {
                partOneSolution = solution;
                partOneMilliseconds = milliseconds;
            }
            else if (part == 2)
            {
                partTwoSolution = solution;
                partTwoMilliseconds = milliseconds;
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}
