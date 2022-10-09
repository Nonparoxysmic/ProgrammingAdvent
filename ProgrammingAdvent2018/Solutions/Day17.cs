// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day17 : Day
    {
        private readonly Regex horzClayLine = new Regex(@"^y=([0-9]{1,9}), x=([0-9]{1,9})\.\.([0-9]{1,9})$");
        private readonly Regex vertClayLine = new Regex(@"^x=([0-9]{1,9}), y=([0-9]{1,9})\.\.([0-9]{1,9})$");

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            input = input.Trim();
            if (input == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();
            foreach (string line in inputLines)
            {
                Match horizontalMatch = horzClayLine.Match(line);
                Match verticalMatch = vertClayLine.Match(line);
                if (!horizontalMatch.Success && !verticalMatch.Success)
                {
                    output.WriteError($"Invalid line in input \"{line}\".", sw);
                    return output;
                }
                Match match = horizontalMatch.Success ? horizontalMatch : verticalMatch;
                (int lowerX, int lowerY, int upperX, int upperY) = Clay(match);

            }

            sw.Stop();
            output.WriteAnswers(null, null, sw);
            return output;
        }

        private (int, int, int, int) Clay(Match match)
        {
            int fixedCoord = int.Parse(match.Groups[1].Value);
            int lowerRange = int.Parse(match.Groups[2].Value);
            int upperRange = int.Parse(match.Groups[3].Value);
            if (upperRange < lowerRange)
            {
                int temp = upperRange;
                upperRange = lowerRange;
                lowerRange = temp;
            }
            if (match.Groups[0].Value[0] == 'x')
            {
                return (fixedCoord, lowerRange, fixedCoord, upperRange);
            }
            else
            {
                return (lowerRange, fixedCoord, upperRange, fixedCoord);
            }
        }
    }
}
