// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Drawing;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day15 : Day
{
    private static int StepsToOxygenSystem { get; set; }
    private static (int X, int Y) OxygenSystemPosition { get; set; }

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0 || inputLines[0].Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (!ValidateIntcodeInput(inputLines, out errorMessage))
        {
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();
        StepsToOxygenSystem = -1;
        OxygenSystemPosition = (-1, -1);
        char[,] map = ExploreMap(inputLines[0], out int[,] _);
        if (StepsToOxygenSystem < 0 || OxygenSystemPosition.X < 0)
        {
            return output.WriteError("Did not find oxygen system.");
        }
        int timeToFullOxygen = SpreadTime(map, OxygenSystemPosition);
        return output.WriteAnswers(StepsToOxygenSystem, timeToFullOxygen);
    }

    private static char[,] ExploreMap(string intcode, out int[,] distances)
    {
        Dictionary<(int, int), (char, int)> map = new()
        {
            [(0, 0)] = ('D', 0)
        };
        Day15Robot robot = new(intcode);
        if (!robot.Initialize(out string _))
        {
            distances = new int[0, 0];
            return new char[0, 0];
        }
        robot.Explore(map);
        Rectangle boundary = Boundary(map);
        char[,] charMap = new char[boundary.Width, boundary.Height];
        distances = new int[boundary.Width, boundary.Height];
        charMap.Fill('#');
        foreach (KeyValuePair<(int X, int Y), (char, int)> kvp in map)
        {
            int x = kvp.Key.X - boundary.X;
            int y = kvp.Key.Y - boundary.Y;
            charMap[x, y] = kvp.Value.Item1;
            distances[x, y] = kvp.Value.Item2;
            if (charMap[x, y] == 'O')
            {
                OxygenSystemPosition = (x, y);
            }
        }
        return charMap;
    }

    private static Rectangle Boundary(Dictionary<(int, int), (char, int)> map)
    {
        int xMin = int.MaxValue, yMin = int.MaxValue;
        int xMax = int.MinValue, yMax = int.MinValue;
        foreach (KeyValuePair<(int X, int Y), (char, int)> kvp in map)
        {
            xMin = Math.Min(xMin, kvp.Key.X);
            yMin = Math.Min(yMin, kvp.Key.Y);
            xMax = Math.Max(xMax, kvp.Key.X);
            yMax = Math.Max(yMax, kvp.Key.Y);
        }
        return new Rectangle(xMin, yMin, xMax - xMin + 1, yMax - yMin + 1);
    }

    private static int SpreadTime(char[,] map, (int X, int Y) Position)
    {
        return SpreadTime(map, Position.X, Position.Y);
    }

    private static int SpreadTime(char[,] map, int x, int y, int depth = 0)
    {
        map[x, y] = 'O';
        int max = -1;
        for (int direction = 1; direction <= 4; direction++)
        {
            (int lookX, int lookY) = direction switch
            {
                1 => (x, y - 1),
                2 => (x, y + 1),
                3 => (x - 1, y),
                4 => (x + 1, y),
                _ => (x, y)
            };
            if (map[lookX, lookY] == '.')
            {
                max = Math.Max(max, SpreadTime(map, lookX, lookY, depth + 1));
            }
        }
        return max < 0 ? depth : max;
    }

    //private static void DebugPrintMap(char[,] map, int[,]? distances = null)
    //{
    //    for (int y = 0; y < map.GetLength(1); y++)
    //    {
    //        for (int x = 0; x < map.GetLength(0); x++)
    //        {
    //            if (map[x, y] == '#')
    //            {
    //                Console.Write("██");
    //            }
    //            else if (map[x, y] == '.')
    //            {
    //                int distance = -1;
    //                if (distances is not null)
    //                {
    //                    try
    //                    {
    //                        distance = distances[x, y];
    //                    }
    //                    catch { }
    //                }
    //                if (distance > 0)
    //                {
    //                    Console.Write($"{distance % 100:00}");
    //                }
    //                else
    //                {
    //                    Console.Write("  ");
    //                }
    //            }
    //            else if (map[x, y] == 'O')
    //            {
    //                Console.Write("O2");
    //            }
    //            else if (map[x, y] == 'D')
    //            {
    //                Console.Write("RD");
    //            }
    //            else
    //            {
    //                Console.Write(map[x, y]);
    //                Console.Write(map[x, y]);
    //            }
    //        }
    //        Console.WriteLine();
    //    }
    //}

    private class Day15Robot
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public int StepsTaken { get; private set; }

        private readonly Day09.Day09Program _program;

        public Day15Robot(string intcode)
        {
            _program = new Day09.Day09Program(intcode);
        }

        public Day15Robot(Day15Robot robotToCopy)
        {
            _program = new(robotToCopy._program);
            X = robotToCopy.X;
            Y = robotToCopy.Y;
            StepsTaken = robotToCopy.StepsTaken;
        }

        public bool Initialize(out string error)
        {
            while (_program.Tick()) { }
            if (_program.Status == Day09.Day09Program.ProgramStatus.Error)
            {
                error = _program.Error;
                return false;
            }
            if (_program.Status == Day09.Day09Program.ProgramStatus.Halted)
            {
                error = "Program halted unexpectedly.";
                return false;
            }
            error = string.Empty;
            return true;
        }

        public void Explore(Dictionary<(int, int), (char, int)> map)
        {
            for (int direction = 1; direction <= 4; direction++)
            {
                (int X, int Y) look = direction switch
                {
                    1 => (X, Y - 1),
                    2 => (X, Y + 1),
                    3 => (X - 1, Y),
                    4 => (X + 1, Y),
                    _ => (X, Y)
                };
                if (map.ContainsKey((look.X, look.Y)))
                {
                    continue;
                }
                Day15Robot robotCopy = new(this);
                ExploreDirection(robotCopy, direction, look, map);
            }
        }

        private static  void ExploreDirection(Day15Robot robot, int direction,
            (int X, int Y) look, Dictionary<(int, int), (char, int)> map)
        {
            robot._program.EnqueueInput(direction);
            while (robot._program.Tick()) { }
            if (robot._program.Status == Day09.Day09Program.ProgramStatus.Error)
            {
                return;
            }
            if (robot._program.Status == Day09.Day09Program.ProgramStatus.Halted)
            {
                return;
            }
            if (robot._program.Status == Day09.Day09Program.ProgramStatus.Waiting)
            {
                long result = robot._program.DequeueOutput();
                if (result == 0)
                {
                    map.Add(look, ('#', 0));
                    return;
                }
                robot.X = look.X;
                robot.Y = look.Y;
                robot.StepsTaken++;
                if (result == 1)
                {
                    map.Add(look, ('.', robot.StepsTaken));
                }
                if (result == 2)
                {
                    map.Add(look, ('O', robot.StepsTaken));
                    StepsToOxygenSystem = robot.StepsTaken;
                }
                robot.Explore(map);
            }
        }
    }
}
