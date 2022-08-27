// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day10 : Day
    {
        private readonly Regex validLine = new Regex(@"^position=< *(-?[0-9]+), +(-?[0-9]+)> velocity=< *(-?[0-9]+), +(-?[0-9]+)>$");

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
            Point[] points = new Point[inputLines.Length];
            for (int i = 0; i < inputLines.Length; i++)
            {
                Match validLineMatch = validLine.Match(inputLines[i]);
                if (!validLineMatch.Success)
                {
                    output.WriteError($"Invalid line \"{inputLines[i]}\" in input.", sw);
                    return output;
                }
                int posX = int.Parse(validLineMatch.Groups[1].Value);
                int posY = int.Parse(validLineMatch.Groups[2].Value);
                int velX = int.Parse(validLineMatch.Groups[3].Value);
                int velY = int.Parse(validLineMatch.Groups[4].Value);
                points[i] = new Point(posX, posY, velX, velY);
            }


            sw.Stop();
            output.WriteAnswers(null, null, sw);
            return output;
        }

        private class Point
        {
            public Vector2Int Position { get; set; }
            public Vector2Int Velocity { get; private set; }

            public Point(int positionX, int positionY, int velocityX, int velocityY)
            {
                Position = new Vector2Int(positionX, positionY);
                Velocity = new Vector2Int(velocityX, velocityY);
            }
        }
    }
}
