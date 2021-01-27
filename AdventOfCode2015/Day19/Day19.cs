// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2015
{
    static class Day19
    {
        public static void Solve()
        {
            string input1Path = @"Day19\Puzzle\Input1.txt";
            string[] input1 = null;
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }
            if (input1.Length < 3) Print.PrintErrorAndExit("Day 19: Cannot process " + input1Path);

            Dictionary<string, List<string>> replacements = new Dictionary<string, List<string>>();

            for (int i = 0; i < input1.Length - 2; i++)
            {
                string[] words = input1[i].Split();
                if (words.Length != 3 || words[1] != "=>")
                {
                    Print.PrintErrorAndExit("Day 19: \"" + input1[i] + "\" is not a valid replacement.");
                }
                if (!replacements.ContainsKey(words[0]))
                {
                    replacements.Add(words[0], new List<string>());
                }
                replacements.TryGetValue(words[0], out List<string> outputs);
                if (!outputs.Contains(words[2])) outputs.Add(words[2]);
            }
            if (replacements.Count == 0) Print.PrintErrorAndExit("Day 19: No replacements found in " + input1Path);

            int maxInputLength = 0;
            foreach (KeyValuePair<string, List<string>> replacement in replacements)
            {
                maxInputLength = Math.Max(maxInputLength, replacement.Key.Length);
            }

            List<string> molecules = new List<string>();

            string startingMolecule = input1[input1.Length - 1];
            for (int pos = 0; pos < startingMolecule.Length; pos++)
            {
                for (int len = 1; len <= maxInputLength; len++)
                {
                    if (len > startingMolecule.Length - pos) continue;
                    string match = startingMolecule.Substring(pos, len);
                    if (replacements.ContainsKey(match))
                    {
                        GetSingleReplacementMolecules(molecules, startingMolecule, replacements, match, pos, len);
                    }
                }
            }
            molecules = molecules.Distinct().ToList();

            Console.WriteLine("Day 19 Part One Answer: " + molecules.Count);

            Console.WriteLine("Day 19 Part Two not attempted.");
        }

        static void GetSingleReplacementMolecules(List<string> molecules, string startingMolecule, Dictionary<string, List<string>> replacements, string match, int pos, int len)
        {
            foreach (string output in replacements[match])
            {
                string newMolecule = startingMolecule.Substring(0, pos) + output + startingMolecule.Substring(pos + len);
                molecules.Add(newMolecule);
            }
        }
    }
}
