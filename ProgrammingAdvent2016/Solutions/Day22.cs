// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ProgrammingAdvent2016
{
    public class Day22 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            string[] inputLines = input.ToLines();
            if (inputLines.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            if (inputLines.Length < 3
                || inputLines[0] != "root@ebhq-gridcenter# df -h"
                || inputLines[1] != "Filesystem              Size  Used  Avail  Use%")
            {
                solution.WriteSolution(1, "ERROR: Invalid input.", 0);
                return solution;
            }
            var nodeNames = new List<string>();
            var availables = new List<int>();
            var useds = new List<int>();
            var validLinePattern = new Regex(@"^/dev/grid/node-x\d+-y\d+ +\d+T +\d+T +\d+T +\d+%$");
            var nodeNamePattern = new Regex(@"node-x\d+-y\d+(?= )");
            var numberPattern = new Regex(@"\d+(?=(\D|$))");
            for (int i = 2; i < inputLines.Length; i++)
            {
                string line = inputLines[i];
                if (!validLinePattern.IsMatch(line))
                {
                    solution.WriteSolution(1, "ERROR: Invalid line in input: \"" + line + "\"", 0);
                    return solution;
                }
                string name = nodeNamePattern.Match(line).Value;
                if (nodeNames.Contains(name))
                {
                    solution.WriteSolution(1, "ERROR: Duplicate data in input: \"" + name + "\"", 0);
                    return solution;
                }
                MatchCollection numbers = numberPattern.Matches(line);
                int[] values = new int[numbers.Count];
                for (int j = 0; j < numbers.Count; j++)
                {
                    values[j] = int.Parse(numbers[j].Value);
                }
                nodeNames.Add(name);
                useds.Add(values[3]);
                availables.Add(values[4]);
            }

            int partOneSolution = 0;
            for (int a = 0; a < nodeNames.Count - 1; a++)
            {
                for (int b = a + 1; b < nodeNames.Count; b++)
                {
                    if (useds[a] > 0 && useds[a] <= availables[b])
                    {
                        partOneSolution++;
                    }
                    if (useds[b] > 0 && useds[b] <= availables[a])
                    {
                        partOneSolution++;
                    }
                }
            }

            solution.WriteSolution(1, partOneSolution, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }
    }
}
