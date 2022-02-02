// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day06 : Day
    {
        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (input == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputs = input.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (inputs.Length != 16)
            {
                output.WriteError("Incorrect number of input values.", sw);
                return output;
            }
            int[] inputValues = new int[16];
            for (int i = 0; i < 16; i++)
            {
                if (!int.TryParse(inputs[i], out inputValues[i]))
                {
                    output.WriteError($"Input value \"{inputs[i]}\" is not an integer.", sw);
                    return output;
                }
                if (inputValues[i] < 0)
                {
                    output.WriteError("Input values can not be negative.", sw);
                    return output;
                }
            }

            int steps = StepsToRepeat(inputValues, out int loopLength);

            sw.Stop();
            output.WriteAnswers(steps, loopLength, sw);
            return output;
        }

        private int StepsToRepeat(int[] input, out int loopLength)
        {
            List<string> seenStates = new List<string>
            {
                string.Join(',', input)
            };
            int count = 0;
            while (true)
            {
                RedistributeBlocks(input);
                count++;
                string state = string.Join(',', input);
                if (seenStates.Contains(state))
                {
                    loopLength = count - seenStates.IndexOf(state);
                    break;
                }
                seenStates.Add(state);
            }
            return count;
        }

        private void RedistributeBlocks(int[] input)
        {
            int pos = Array.IndexOf(input, input.Max());
            int blocks = input[pos];
            input[pos] = 0;
            while (blocks > 0)
            {
                pos++;
                if (pos >= input.Length) { pos = 0; }
                input[pos]++;
                blocks--;
            }
        }
    }
}
