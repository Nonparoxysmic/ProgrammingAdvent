// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Drawing;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day15 : Day
{
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
        char[,] map = ExploreMap(inputLines[0]);
        return output.WriteAnswers(null, null);
    }

    private static char[,] ExploreMap(string intcode)
    {
        Dictionary<(int, int), char> map = new()
        {
            [(0, 0)] = 'D'
        };
        Day15Robot robot = new(intcode);
        if (!robot.Initialize(out string _))
        {
            return new char[0, 0];
        }
        robot.Explore(map);
        Rectangle boundary = Boundary(map);
        char[,] output = new char[boundary.Width, boundary.Height];
        output.Fill('#');
        foreach (KeyValuePair<(int, int), char> kvp in map)
        {
            int x = kvp.Key.Item1 - boundary.X;
            int y = kvp.Key.Item2 - boundary.Y;
            output[x, y] = kvp.Value;
        }
        return output;
    }

    private static Rectangle Boundary(Dictionary<(int, int), char> map)
    {
        int xMin = int.MaxValue, yMin = int.MaxValue;
        int xMax = int.MinValue, yMax = int.MinValue;
        foreach (KeyValuePair<(int, int), char> kvp in map)
        {
            xMin = Math.Min(xMin, kvp.Key.Item1);
            yMin = Math.Min(yMin, kvp.Key.Item2);
            xMax = Math.Max(xMax, kvp.Key.Item1);
            yMax = Math.Max(yMax, kvp.Key.Item2);
        }
        return new Rectangle(xMin, yMin, xMax - xMin + 1, yMax - yMin + 1);
    }

    private static void DebugPrintMap(char[,] map)
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (map[x, y] == '#')
                {
                    Console.Write("██");
                }
                else if (map[x, y] == '.')
                {
                    Console.Write("  ");
                }
                else if (map[x, y] == 'O')
                {
                    Console.Write("O2");
                }
                else if (map[x, y] == 'D')
                {
                    Console.Write("RD");
                }
                else
                {
                    Console.Write(map[x, y]);
                    Console.Write(map[x, y]);
                }
            }
            Console.WriteLine();
        }
    }

    private class Day15Robot
    {
        public int X { get; private set; }
        public int Y { get; private set; }

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

        public void Explore(Dictionary<(int, int), char> map)
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
            (int X, int Y) look, Dictionary<(int, int), char> map)
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
                    map.Add(look, '#');
                    return;
                }
                if (result == 1)
                {
                    map.Add(look, '.');
                }
                if (result == 2)
                {
                    map.Add(look, 'O');
                }
                robot.X = look.X;
                robot.Y = look.Y;
                robot.Explore(map);
            }
        }
    }
}
