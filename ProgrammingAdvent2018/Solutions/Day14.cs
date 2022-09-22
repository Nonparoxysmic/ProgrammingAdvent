// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day14 : Day
    {
        private SequenceGenerator sequenceGenerator;

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
            if (!int.TryParse(input, out int inputValue))
            {
                output.WriteError("Cannot parse input as an integer.", sw);
                return output;
            }

            sequenceGenerator = new SequenceGenerator();
            string partOneAnswer = GetScores(inputValue, 10);
            if (!GetPosition(input, out int partTwoAnswer))
            {
                output.WriteError("Part Two: answer not found in search space.", sw);
                return output;
            }

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
            return output;
        }

        private string GetScores(int position, int length)
        {
            sequenceGenerator.JumpToPosition(position);
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                output.Append(sequenceGenerator.Next());
            }
            return output.ToString();
        }

        private bool GetPosition(string pattern, out int position)
        {
            if (sequenceGenerator.Length < pattern.Length)
            {
                sequenceGenerator.Generate(pattern.Length);
            }
            int[] patternNumbers = pattern.Select(c => c - 48).ToArray();
            if (SearchSequence(patternNumbers, out int position1))
            {
                position = position1;
                return true;
            }
            int timeout = 65536;
            for (int i = 0; i < timeout; i++)
            {
                int startingIndex = sequenceGenerator.Length - patternNumbers.Length - 1;
                sequenceGenerator.Generate(1000);
                if (SearchSequence(patternNumbers, startingIndex, out int position2))
                {
                    position = position2;
                    return true;
                }
            }
            position = -1;
            return false;
        }

        private bool SearchSequence(int[] pattern, out int position)
        {
            return SearchSequence(pattern, 0, out position);
        }

        private bool SearchSequence(int[] pattern, int startingIndex, out int position)
        {
            for (int i = startingIndex; i < sequenceGenerator.Length - pattern.Length; i++)
            {
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (sequenceGenerator[i + j] != pattern[j])
                    {
                        break;
                    }
                    if (j == pattern.Length - 1)
                    {
                        position = i;
                        return true;
                    }
                }
            }
            position = -1;
            return false;
        }

        private class SequenceGenerator
        {
            public int Length { get => numbers.Count; }

            private int current;
            private int position0;
            private int position1;
            private readonly List<int> numbers;

            public SequenceGenerator()
            {
                current = 0;
                position0 = 0;
                position1 = 1;
                numbers = new List<int> { 3, 7 };
            }

            public int this[int i]
            {
                get
                {
                    return numbers[i];
                }
            }

            public void Generate(int length)
            {
                int target = numbers.Count + length;
                while (numbers.Count < target)
                {
                    int next = numbers[position0] + numbers[position1];
                    if (next > 9)
                    {
                        numbers.Add(1);
                        numbers.Add(next - 10);
                    }
                    else
                    {
                        numbers.Add(next);
                    }
                    position0 = (position0 + numbers[position0] + 1) % numbers.Count;
                    position1 = (position1 + numbers[position1] + 1) % numbers.Count;
                }
            }

            public void JumpToPosition(int position)
            {
                while (numbers.Count <= position)
                {
                    int next = numbers[position0] + numbers[position1];
                    if (next > 9)
                    {
                        numbers.Add(1);
                        numbers.Add(next - 10);
                    }
                    else
                    {
                        numbers.Add(next);
                    }
                    position0 = (position0 + numbers[position0] + 1) % numbers.Count;
                    position1 = (position1 + numbers[position1] + 1) % numbers.Count;
                }
                current = position;
            }

            public int Next()
            {
                if (current >= numbers.Count)
                {
                    Generate(1000);
                }
                return numbers[current++];
            }
        }
    }
}
