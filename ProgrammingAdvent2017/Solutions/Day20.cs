// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2017
// https://adventofcode.com/2017

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2017.Program;

namespace ProgrammingAdvent2017.Solutions
{
    internal class Day20 : Day
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
            int smallestAcceleration = int.MaxValue;
            int smallestAccelerationParticle = -1;
            for (int i = 0; i < inputLines.Length; i++)
            {
                if (!Regex.IsMatch(inputLines[i],
                    @"^p=<-?\d+,-?\d+,-?\d+>, v=<-?\d+,-?\d+,-?\d+>, a=<-?\d+,-?\d+,-?\d+>$"))
                {
                    output.WriteError($"Invalid line in input: {inputLines[i]}", sw);
                    return output;
                }
                MatchCollection matches = Regex.Matches(inputLines[i], @"(?<=[<,])-?\d+(?=[,>])");

                int accelX = int.Parse(matches[6].Value);
                int accelY = int.Parse(matches[7].Value);
                int accelZ = int.Parse(matches[8].Value);
                int acceleration = Math.Abs(accelX) + Math.Abs(accelY) + Math.Abs(accelZ);
                if (acceleration < smallestAcceleration)
                {
                    smallestAcceleration = acceleration;
                    smallestAccelerationParticle = i;
                }
            }

            sw.Stop();
            output.WriteAnswers(smallestAccelerationParticle, null, sw);
            return output;
        }
    }
}
