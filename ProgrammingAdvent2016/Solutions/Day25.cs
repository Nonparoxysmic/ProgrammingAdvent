// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace ProgrammingAdvent2016
{
    public class Day25 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        readonly string[] expectedInput = new string[]
        { 
            "cpy a d",
            "cpy 7 c",
            "cpy ### b",
            "inc d",
            "dec b",
            "jnz b -2",
            "dec c",
            "jnz c -5",
            "cpy d a",
            "jnz 0 0",
            "cpy a b",
            "cpy 0 a",
            "cpy 2 c",
            "jnz b 2",
            "jnz 1 6",
            "dec b",
            "dec c",
            "jnz c -4",
            "inc a",
            "jnz 1 -7",
            "cpy 2 b",
            "jnz c 2",
            "jnz 1 4",
            "dec b",
            "dec c",
            "jnz 1 -4",
            "jnz 0 0",
            "out b",
            "jnz a -19",
            "jnz 1 -21"
        };

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

            for (int i = 0; i < inputLines.Length; i++)
            {
                if ((i != 2 && inputLines[i] != expectedInput[i])
                    || (i == 2 && !Regex.IsMatch(inputLines[2], @"^cpy \d+ b$")))
                {
                    solution.WriteSolution(1, "ERROR: Unexpected input \"" + inputLines[i] + "\".", 0);
                    return solution;
                }
            }
            int inputValue = int.Parse(Regex.Match(inputLines[2], @"\d+(?= )").Value);

            int answer = FindAnswer(inputValue);
            solution.WriteSolution(1, answer, stopwatch.ElapsedMilliseconds);
            solution.WriteSolution(2, "Transmit the Signal", solution.PartOneMilliseconds());

            stopwatch.Reset();
            return solution;
        }

        int FindAnswer(int inputValue)
        {
            for (int i = 1; i < int.MaxValue; i++)
            {
                string result = GetPattern(i, inputValue);
                if (result.Length % 2 == 1) { continue; }
                for (int j = 0; j < result.Length - 1; j += 2)
                {
                    if (!(result[j] == '0' && result[j + 1] == '1'))
                    {
                        break;
                    }
                    if (j == result.Length - 2)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        string GetPattern(int initialA, int inputValue)
        {
            var sb = new StringBuilder();
            int a = initialA + inputValue * 7, b, c;
            do
            {
                b = a;
                a = 0;
                while (true)
                {
                    c = 2;
                    do
                    {
                        if (b == 0) { goto BreakLoops; }
                        b--;
                        c--;
                    } while (c != 0);
                    a++;
                }
                BreakLoops:
                b = 2 - c;
                sb.Append(b);
            } while (a != 0);
            return sb.ToString();
        }
    }
}
