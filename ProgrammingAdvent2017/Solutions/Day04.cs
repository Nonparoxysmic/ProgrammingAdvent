// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Diagnostics;
using System.Linq;
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

            int noRepeats = 0;
            int noAnagrams = 0;
            string[] inputLines = input.ToLines();
            foreach (string line in inputLines)
            {
                string[] words = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length == 0) { continue; }
                if (words.Length == 1)
                {
                    noRepeats++;
                    noAnagrams++;
                    continue;
                }
                bool wordsMatch = false;
                bool hasAnagrams = false;
                for (int i = 0; i < words.Length - 1; i++)
                {
                    for (int j = i + 1; j < words.Length; j++)
                    {
                        if (words[i] == words[j])
                        {
                            wordsMatch = true;
                        }
                        if (AreAnagrams(words[i], words[j]))
                        {
                            hasAnagrams = true;
                        }
                    }
                }
                if (!wordsMatch) { noRepeats++; }
                if (!hasAnagrams) { noAnagrams++; }
            }
            
            sw.Stop();
            output.WriteAnswers(noRepeats, noAnagrams, sw);
            return output;
        }

        internal static bool AreAnagrams(string stringOne, string stringTwo)
        {
            if (stringOne.Length != stringTwo.Length) { return false; }
            if (stringOne == stringTwo) { return true; }
            char[] charsOne = stringOne.ToCharArray();
            char[] charsTwo = stringTwo.ToCharArray();
            Array.Sort(charsOne);
            Array.Sort(charsTwo);
            if (charsOne.SequenceEqual(charsTwo)) { return true; }
            return false;
        }
    }
}
