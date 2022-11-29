// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Collections.Generic;
using System.Diagnostics;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day25 : Day
    {
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
            List<(int X, int Y, int Z, int W)> points = new List<(int, int, int, int)>();
            foreach (string line in inputLines)
            {
                string[] terms = line.Split(',');
                if (terms.Length != 4)
                {
                    output.WriteError($"Invalid line: \"{line}\"", sw);
                    return output;
                }
                int[] values = new int[4];
                for (int i = 0; i < 4; i++)
                {
                    if (!int.TryParse(terms[i], out values[i]))
                    {
                        output.WriteError($"Invalid line: \"{line}\"", sw);
                        return output;
                    }
                }
                points.Add((values[0], values[1], values[2], values[3]));
            }

            Dictionary<int, int> constellationIDs = new Dictionary<int, int>();
            for (int i = 0; i < points.Count; i++)
            {
                constellationIDs.Add(i, i);
            }
            for (int i = 0; i < points.Count - 1; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    if (Distance(points[i], points[j]) <= 3 && constellationIDs[i] != constellationIDs[j])
                    {
                        int oldID = constellationIDs[j];
                        for (int k = 0; k < points.Count; k++)
                        {
                            if (constellationIDs[k] == oldID)
                            {
                                constellationIDs[k] = constellationIDs[i];
                            }
                        }
                    }
                }
            }
            HashSet<int> constellations = new HashSet<int>();
            for (int i = 0; i < points.Count; i++)
            {
                constellations.Add(constellationIDs[i]);
            }

            sw.Stop();
            output.WriteAnswers(constellations.Count, "Trigger the Underflow", sw);
            return output;
        }

        private int Distance((int X, int Y, int Z, int W) one, (int X, int Y, int Z, int W) two)
        {
            return Math.Abs(one.X - two.X)
                + Math.Abs(one.Y - two.Y)
                + Math.Abs(one.Z - two.Z)
                + Math.Abs(one.W - two.W);
        }
    }
}
