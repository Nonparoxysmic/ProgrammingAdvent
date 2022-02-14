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
    internal class Day11 : Day
    {
        internal readonly Dictionary<string, HexCoord> directionOffsets
            = new Dictionary<string, HexCoord>()
            {
                { "n",  new HexCoord( 0, -1) },
                { "ne", new HexCoord( 1, -1) },
                { "se", new HexCoord( 1,  0) },
                { "s",  new HexCoord( 0,  1) },
                { "sw", new HexCoord(-1,  1) },
                { "nw", new HexCoord(-1,  0) }
            };

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
            string[] steps = input.Split(',');

            HexCoord targetHex = new HexCoord(0, 0);
            int furthestDistance = 0;
            foreach (string step in steps)
            {
                if (!Regex.IsMatch(step, @"^(n|ne|se|s|sw|nw)$"))
                {
                    output.WriteError($"Invalid input \"{step}\".", sw);
                    return output;
                }
                targetHex += directionOffsets[step];
                furthestDistance = Math.Max(furthestDistance, targetHex.StepsToOrigin());
            }

            int targetDistance = targetHex.StepsToOrigin();

            sw.Stop();
            output.WriteAnswers(targetDistance, furthestDistance, sw);
            return output;
        }
    }

    internal class HexCoord
    {
        //  Coordinate System:
        //
        //       (+s)      (-r)
        //          \      /
        //           +----+
        //          /      \
        //  (-q) --+        +-- (+q)
        //          \      /
        //           +----+
        //          /      \
        //       (+r)      (-s)
        //
        //  q + r + s = 0
        //
        //  https://www.redblobgames.com/grids/hexagons/

        internal int Q { get; set; }
        internal int R { get; set; }

        public HexCoord(int q, int r)
        {
            Q = q;
            R = r;
        }

        public static HexCoord operator +(HexCoord a, HexCoord b)
        {
            return new HexCoord(a.Q + b.Q, a.R + b.R);
        }

        internal int StepsToOrigin()
        {
            return Math.Max(Math.Max(Math.Abs(Q), Math.Abs(R)), Math.Abs(Q + R));
        }
    }
}
