// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Diagnostics;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day04 : Day
    {
        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int numberValid = 0;
            string[] inputLines = input.ToLines();
            foreach (string line in inputLines)
            {
                string[] words = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length == 0) { continue; }
                if (words.Length == 1)
                {
                    numberValid++;
                    continue;
                }
                bool wordsMatch = false;
                for (int i = 0; i < words.Length - 1; i++)
                {
                    for (int j = i + 1; j < words.Length; j++)
                    {
                        if (words[i] == words[j])
                        {
                            wordsMatch = true;
                        }
                    }
                }
                if (!wordsMatch) { numberValid++; }
            }
            
            sw.Stop();
            output.WriteAnswers(numberValid, null, sw);
            return output;
        }
    }
}
