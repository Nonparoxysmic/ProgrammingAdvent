// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day16 : Day
    {
        private readonly Regex instructionLine = new Regex(@"^([0-9]|1[0-5]) ([0-9]+) ([0-9]+) ([0-3])$");
        private readonly Regex bforeLine = new Regex(@"^Before: \[([0-9]+), ([0-9]+), ([0-9]+), ([0-9]+)]$");
        private readonly Regex afterLine = new Regex(@"^After:  \[([0-9]+), ([0-9]+), ([0-9]+), ([0-9]+)]$");

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
            if (inputLines.Length < 4)
            {
                output.WriteError("Input too short.", sw);
                return output;
            }

            int lastAfter = -1;
            for (int i = 0; i <= inputLines.Length - 4; i += 4)
            {
                Match before = bforeLine.Match(inputLines[i]);
                Match instruction = instructionLine.Match(inputLines[i + 1]);
                Match after = afterLine.Match(inputLines[i + 2]);
                if (!before.Success || !instruction.Success || !after.Success)
                {
                    break;
                }
                if (!TryParseRegisters(before, out ulong[] registersBefore))
                {
                    output.WriteError($"Cannot parse register values \"{inputLines[i]}\".", sw);
                    return output;
                }
                if (!int.TryParse(instruction.Groups[1].Value, out int _)
                    || !ulong.TryParse(instruction.Groups[2].Value, out ulong A)
                    || !ulong.TryParse(instruction.Groups[3].Value, out ulong B)
                    || !int.TryParse(instruction.Groups[4].Value, out int C))
                {
                    output.WriteError($"Cannot parse instruction \"{inputLines[i + 1]}\".", sw);
                    return output;
                }
                if (!TryParseRegisters(after, out ulong[] registersAfter))
                {
                    output.WriteError($"Cannot parse register values \"{inputLines[i + 2]}\".", sw);
                    return output;
                }


                lastAfter = i + 2;
            }
            if (lastAfter < 0)
            {
                output.WriteError("Input must begin with a sample.", sw);
                return output;
            }

            sw.Stop();
            output.WriteAnswers(null, null, sw);
            return output;
        }

        private bool TryParseRegisters(Match match, out ulong[] registerValues)
        {
            if (match.Groups.Count < 5)
            {
                registerValues = Array.Empty<ulong>();
                return false;
            }
            if (!ulong.TryParse(match.Groups[1].Value, out ulong first) ||
                !ulong.TryParse(match.Groups[2].Value, out ulong second) ||
                !ulong.TryParse(match.Groups[3].Value, out ulong third) ||
                !ulong.TryParse(match.Groups[4].Value, out ulong fourth))
            {
                registerValues = Array.Empty<ulong>();
                return false;
            }
            registerValues = new ulong[] { first, second, third, fourth };
            return true;
        }
    }
}
