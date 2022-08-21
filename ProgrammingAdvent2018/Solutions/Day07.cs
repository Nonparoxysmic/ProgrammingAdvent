// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day07 : Day
    {
        private readonly Regex validLine = new Regex(@"^Step ([A-Z]) must be finished before step ([A-Z]) can begin\.$");

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (input.Trim() == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();
            SortedDictionary<char, int> stepPrerequisites1 = new SortedDictionary<char, int>();
            foreach (string line in inputLines)
            {
                Match validMatch = validLine.Match(line);
                if (!validMatch.Success)
                {
                    output.WriteError($"Invalid line \"{line}\" in input.", sw);
                    return output;
                }
                char prereq = validMatch.Groups[1].Value[0];
                char step = validMatch.Groups[2].Value[0];
                if (stepPrerequisites1.ContainsKey(step))
                {
                    stepPrerequisites1[step] |= CharToFlag(prereq);
                }
                else
                {
                    stepPrerequisites1.Add(step, CharToFlag(prereq));
                }
                if (!stepPrerequisites1.ContainsKey(prereq))
                {
                    stepPrerequisites1.Add(prereq, 0);
                }
            }
            SortedDictionary<char, int> stepPrerequisites2 = new SortedDictionary<char, int>(stepPrerequisites1);

            // Part One
            StringBuilder stepOrder = new StringBuilder();
            while (stepPrerequisites1.Count > 0)
            {
                if (!NextStep(stepPrerequisites1, out char nextStep))
                {
                    output.WriteError("No valid order of steps available.", sw);
                    return output;
                }
                stepOrder.Append(nextStep);
                stepPrerequisites1.Remove(nextStep);
                foreach (char key in stepPrerequisites1.Keys.ToList())
                {
                    stepPrerequisites1[key] &= ~CharToFlag(nextStep);
                }
            }

            // Part Two
            int workerCount = 5;  // 2 for example
            int stepTimeModifier = -4;  // -64 for example
            int[] countdowns = new int[workerCount];
            char[] stepsInProgress = new char[workerCount];
            int pendingSteps = 0;
            int t;
            // For each time step...
            for (t = 0; t < int.MaxValue; t++)
            {
                // If all of the steps are done, stop.
                if (pendingSteps <= 0 && !NextStep(stepPrerequisites2, out char _))
                {
                    break;
                }
                // While there are available workers, assign available steps.
                int availableWorker = Array.IndexOf(countdowns, 0);
                while (availableWorker >= 0)
                {
                    // If there are no steps available, move on.
                    if (!NextStep(stepPrerequisites2, out char nextStep))
                    {
                        break;
                    }
                    // Assign the step to the worker.
                    stepsInProgress[availableWorker] = nextStep;
                    countdowns[availableWorker] = nextStep + stepTimeModifier;
                    stepPrerequisites2.Remove(nextStep);
                    pendingSteps++;
                    // Check for another available worker.
                    availableWorker = Array.IndexOf(countdowns, 0);
                }
                // Decrement the work countdown for each worker.
                for (int i = 0; i < countdowns.Length; i++)
                {
                    if (countdowns[i] == 1)
                    {
                        // When a step is completed, remove it as a prerequisite.
                        pendingSteps--;
                        foreach (char key in stepPrerequisites2.Keys.ToList())
                        {
                            stepPrerequisites2[key] &= ~CharToFlag(stepsInProgress[i]);
                        }
                        countdowns[i] = 0;
                    }
                    else if (countdowns[i] > 0) { countdowns[i]--; }
                }
            }

            sw.Stop();
            output.WriteAnswers(stepOrder, t, sw);
            return output;
        }

        private int CharToFlag(char c)
        {
            return 1 << (c - '@');
        }

        private bool NextStep(SortedDictionary<char, int> stepPrerequisites, out char nextStep)
        {
            foreach (var kvp in stepPrerequisites)
            {
                if (kvp.Value == 0)
                {
                    nextStep = kvp.Key;
                    return true;
                }
            }
            nextStep = '\0';
            return false;
        }
    }
}
