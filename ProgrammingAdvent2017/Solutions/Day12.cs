// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day12 : Day
    {
        VillageProgram[] programs;

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
            string[] inputLines = input.ToLines();
            programs = new VillageProgram[inputLines.Length];
            for (int i = 0; i < inputLines.Length; i++)
            {
                if (!Regex.IsMatch(inputLines[i], @"^\d+ <-> \d+(, \d+(?=,|$))*$")
                    || Regex.Match(inputLines[i], @"^\d+(?= )").Value != i.ToString())
                {
                    output.WriteError($"Invalid input \"{inputLines[i]}\".", sw);
                    return output;
                }
                programs[i] = new VillageProgram(i, inputLines[i]);
            }

            HashSet<int> searched = new HashSet<int>() { 0 };
            int connectedToZero = CountConnectedPrograms(0, searched);

            int groups = 1;
            for (int i = 0; i < programs.Length; i++)
            {
                if (searched.Add(i))
                {
                    groups++;
                    CountConnectedPrograms(i, searched);
                }
            }

            sw.Stop();
            output.WriteAnswers(connectedToZero, groups, sw);
            return output;
        }

        private int CountConnectedPrograms(int id, HashSet<int> searched)
        {
            foreach (int n in programs[id].Neighbors)
            {
                if (searched.Add(n))
                {
                    CountConnectedPrograms(n, searched);
                }
            }
            return searched.Count;
        }
    }

    internal class VillageProgram
    {
        internal int ID { get; set; }
        internal int[] Neighbors { get; set; }

        public VillageProgram(int id, string inputLine)
        {
            ID = id;
            string[] neighbors = Regex.Match(inputLine, @"(?<= <-> ).+$").Value
                .Split(new char[] { ',', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            List<int> neighborIDs = new List<int>();
            foreach (string neighbor in neighbors)
            {
                neighborIDs.Add(int.Parse(neighbor));
            }
            Neighbors = neighborIDs.ToArray();
        }
    }
}
