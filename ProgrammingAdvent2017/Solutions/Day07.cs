// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day07 : Day
    {
        private string[] names;
        private TowerProgram[] programs;

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
            foreach (string line in inputLines)
            {
                if (!Regex.IsMatch(line, @"^[a-z]+ \(\d+\)( -> [a-z, ]+$)*$"))
                {
                    output.WriteError($"Invalid line: \"{line}\"", sw);
                    return output;
                }
            }
            List<string> nameList = new List<string>();
            foreach (string line in inputLines)
            {
                nameList.Add(Regex.Match(line, @"^[a-z]+(?= )").Value);
            }
            names = nameList.ToArray();

            programs = new TowerProgram[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                int weight = int.Parse(Regex.Match(inputLines[i], @"(?<=\()\d+(?=\))").Value);
                programs[i] = new TowerProgram(i, names[i], weight);
            }
            for (int i = 0; i < inputLines.Length; i++)
            {
                if (Regex.IsMatch(inputLines[i], @"(?<=( -> ))[a-z, ]+$"))
                {
                    string[] childNames =
                        Regex.Match(inputLines[i], @"(?<=( -> ))[a-z, ]+$").Value
                        .Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    List<TowerProgram> children = new List<TowerProgram>();
                    foreach (string childName in childNames)
                    {
                        int childID = Array.IndexOf(names, childName);
                        if (childID < 0)
                        {
                            output.WriteError($"\"{childName}\" is not a valid name.", sw);
                            return output;
                        }
                        children.Add(programs[childID]);
                        programs[childID].parent = programs[i];
                    }
                    programs[i].children = children.ToArray();
                }
            }

            string baseProgramName = names[programs[0].GetBaseProgramID()];

            sw.Stop();
            output.WriteAnswers(baseProgramName, null, sw);
            return output;
        }
    }

    internal class TowerProgram
    {
        public readonly int idNumber;
        public readonly int selfWeight;
        public int totalWeight = -1;
        public readonly string name;
        public TowerProgram parent;
        public TowerProgram[] children;

        public TowerProgram(int id, string name, int weight)
        {
            idNumber = id;
            this.name = name;
            selfWeight = weight;
        }

        public int GetBaseProgramID()
        {
            if (parent == null) { return idNumber; }
            return parent.GetBaseProgramID();
        }
    }
}
