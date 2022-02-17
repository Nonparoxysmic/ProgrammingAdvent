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
    internal class Day08 : Day
    {
        private Dictionary<string, int> registerValues;

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
                string pattern = @"^[a-z]+( inc | dec )-?\d+ if [a-z]+ (>|<|>=|==|<=|!=) -?\d+$";
                if (!Regex.IsMatch(line, pattern))
                {
                    output.WriteError($"Invalid line: \"{line}\"", sw);
                    return output;
                }
            }
            registerValues = new Dictionary<string, int>();
            foreach (string line in inputLines)
            {
                string name = Regex.Match(line, @"^[a-z]+(?= )").Value;
                if (!registerValues.ContainsKey(name))
                {
                    registerValues.Add(name, 0);
                }
            }

            int largestValueDuringProcess = 0;
            foreach (string line in inputLines)
            {
                try
                {
                    ProcessInstruction(line, ref largestValueDuringProcess);
                }
                catch
                {
                    output.WriteError($"Cannot process instruction \"{line}\"", sw);
                    return output;
                }
            }
            int largestValueAfterProcess = int.MinValue;
            foreach (KeyValuePair<string, int> kvp in registerValues)
            {
                largestValueAfterProcess = Math.Max(largestValueAfterProcess, kvp.Value);
            }

            sw.Stop();
            output.WriteAnswers(largestValueAfterProcess, largestValueDuringProcess, sw);
            return output;
        }

        private void ProcessInstruction(string line, ref int largestValue)
        {
            string[] parts = line.Split();
            int conditionValue = int.Parse(parts[6]);
            bool doModify = false;
            switch (parts[5])
            {
                case ">":
                    if (registerValues[parts[4]] > conditionValue) { doModify = true; }
                    break;
                case "<":
                    if (registerValues[parts[4]] < conditionValue) { doModify = true; }
                    break;
                case ">=":
                    if (registerValues[parts[4]] >= conditionValue) { doModify = true; }
                    break;
                case "<=":
                    if (registerValues[parts[4]] <= conditionValue) { doModify = true; }
                    break;
                case "==":
                    if (registerValues[parts[4]] == conditionValue) { doModify = true; }
                    break;
                case "!=":
                    if (registerValues[parts[4]] != conditionValue) { doModify = true; }
                    break;
            }
            if (doModify)
            {
                int changeValue = int.Parse(parts[2]);
                switch (parts[1])
                {
                    case "inc":
                        registerValues[parts[0]] += changeValue;
                        largestValue = Math.Max(largestValue, registerValues[parts[0]]);
                        break;
                    case "dec":
                        registerValues[parts[0]] -= changeValue;
                        largestValue = Math.Max(largestValue, registerValues[parts[0]]);
                        break;
                }
            }
        }
    }
}
