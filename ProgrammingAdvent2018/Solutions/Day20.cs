// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day20 : Day
    {
        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            input = input.Trim();
            if (input.Length < 3)
            {
                output.WriteError("Input is too short.", sw);
                return output;
            }
            if (input[0] != '^' || input[^1] != '$')
            {
                output.WriteError("Input must begin with '^' and end with '$'.", sw);
                return output;
            }
            for (int i = 1; i < input.Length - 1; i++)
            {
                switch (input[i])
                {
                    case 'N':
                    case 'S':
                    case 'E':
                    case 'W':
                    case '|':
                    case '(':
                    case ')':
                        continue;
                    default:
                        output.WriteError($"Input contains invalid character '{input[i]}'.", sw);
                        return output;
                }
            }
            try
            {
                Regex inputRegex = new Regex(input);
            }
            catch
            {
                output.WriteError("Input is not a valid regular expression.", sw);
                return output;
            }

            MapArray<int> distanceMap = CreateDistanceMap(input);

            (int partOneAnswer, int partTwoAnswer) = FindAnswers(distanceMap, 1000);

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
            return output;
        }

        private MapArray<int> CreateDistanceMap(string input)
        {
            MapArray<int> distanceMap = new MapArray<int>(300, 300, 100, (-150, -150));
            StringBuilder pathTraveled = new StringBuilder();
            Stack<int> lastBranchDistances = new Stack<int>();
            int x = 0, y = 0;
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case 'N':
                    case 'S':
                    case 'E':
                    case 'W':
                        if (pathTraveled.Length > 0 && IsBacktracking(input[i], pathTraveled[^1]))
                        {
                            Reverse(ref pathTraveled, ref x, ref y);
                            break;
                        }
                        Step(input[i], ref pathTraveled, ref x, ref y);
                        distanceMap[x, y] = pathTraveled.Length;
                        break;
                    case '|':
                        while (pathTraveled.Length > lastBranchDistances.Peek())
                        {
                            Reverse(ref pathTraveled, ref x, ref y);
                        }
                        break;
                    case '(':
                        lastBranchDistances.Push(pathTraveled.Length);
                        break;
                    case ')':
                        int startOfBranch = lastBranchDistances.Pop();
                        while (pathTraveled.Length > startOfBranch)
                        {
                            Reverse(ref pathTraveled, ref x, ref y);
                        }
                        break;
                }
            }
            return distanceMap;
        }

        private void Step(char direction, ref StringBuilder pathTraveled, ref int x, ref int y)
        {
            switch (direction)
            {
                case 'N':
                    y--;
                    break;
                case 'S':
                    y++;
                    break;
                case 'E':
                    x++;
                    break;
                case 'W':
                    x--;
                    break;
            }
            pathTraveled.Append(direction);
        }

        private void Reverse(ref StringBuilder pathTraveled, ref int x, ref int y)
        {
            switch (pathTraveled[^1])
            {
                case 'N':
                    y++;
                    break;
                case 'S':
                    y--;
                    break;
                case 'E':
                    x--;
                    break;
                case 'W':
                    x++;
                    break;
            }
            pathTraveled.Remove(pathTraveled.Length - 1, 1);
        }

        private (int, int) FindAnswers(MapArray<int> distanceMap, int minimumDistance)
        {
            int largest = -1, count = 0;
            for (int y = distanceMap.Position.Y; y < distanceMap.Position.Y + distanceMap.Height; y++)
            {
                for (int x = distanceMap.Position.X; x < distanceMap.Position.X + distanceMap.Width; x++)
                {
                    if (distanceMap[x, y] >= minimumDistance)
                    {
                        count++;
                        largest = Math.Max(largest, distanceMap[x, y]);
                    }
                }
            }
            return (largest, count);
        }

        private bool IsBacktracking(char a, char b)
        {
            if ((a == 'N' && b == 'S') ||
                (a == 'S' && b == 'N') ||
                (a == 'E' && b == 'W') ||
                (a == 'W' && b == 'E'))
            {
                return true;
            }
            return false;
        }
    }
}
