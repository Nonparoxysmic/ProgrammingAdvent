// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Drawing;
using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day11 : Day
{
    private static readonly Regex _validCharacters = new("^[0-9,-]+$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0 || inputLines[0].Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (!_validCharacters.IsMatch(inputLines[0]))
        {
            errorMessage = $"Input \"{inputLines[0].Left(20, true)}\" contains invalid characters.";
            return false;
        }
        string[] terms = inputLines[0].Split(',');
        if (terms.Length == 0)
        {
            errorMessage = "Insufficient input.";
            return false;
        }
        foreach (string term in terms)
        {
            if (!long.TryParse(term, out long _))
            {
                errorMessage = $"Term \"{term.Left(20, true)}\" cannot be parsed as an Int64.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines)
    {
        PuzzleAnswers output = new();
        Day11Robot robot = new(inputLines[0]);
        if (!robot.Run(out string error))
        {
            return output.WriteError(error);
        }
        int panelsPainted = robot.PanelsPainted;
        robot = new(inputLines[0], 1);
        if (!robot.Run(out string error2))
        {
            return output.WriteAnswers(panelsPainted, error2);
        }
        return output.WriteAnswers(panelsPainted, robot.IdentifierPainted);
    }

    private class Day11Robot
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Direction { get; private set; }
        public int PanelsPainted => _panelsPainted.Count;
        public string IdentifierPainted { get; private set; } = string.Empty;

        private readonly Day09.Day09Program _program;
        private readonly HashSet<(int, int)> _whitePanels = new();
        private readonly HashSet<(int, int)> _panelsPainted = new();
        private readonly (int dX, int dY)[] steps = new (int, int)[]
        {
            (-1, 0),
            (0, -1),
            (1, 0),
            (0, 1)
        };

        public Day11Robot(string intcode, int startingPanel = 0)
        {
            _program = new Day09.Day09Program(intcode);
            if (startingPanel != 0)
            {
                _whitePanels.Add((X, Y));
            }
        }

        public bool Run(out string error)
        {
            Direction = 1;
            int timeout = 0;
            while (timeout++ < 250_000)
            {
                while (_program.Tick()) { }
                if (_program.Status == Day09.Day09Program.ProgramStatus.Error)
                {
                    error = _program.Error;
                    return false;
                }
                if (_program.Status == Day09.Day09Program.ProgramStatus.Halted)
                {
                    if (!HandleOutput(out string outputError))
                    {
                        error = outputError;
                        return false;
                    }
                    IdentifierPainted = ReadIdentifier(_whitePanels);
                    error = string.Empty;
                    return true;
                }
                if (_program.Status == Day09.Day09Program.ProgramStatus.Waiting)
                {
                    if (!HandleOutput(out string outputError))
                    {
                        error = outputError;
                        return false;
                    }
                    _program.EnqueueInput(_whitePanels.Contains((X, Y)) ? 1 : 0);
                    continue;
                }
            }
            error = "Robot ran too long.";
            return false;
        }

        private bool HandleOutput(out string error)
        {
            if (_program.OutputCount != 0 && _program.OutputCount != 2)
            {
                error = "Unexpected program outputs.";
                return false;
            }
            if (_program.OutputCount == 0)
            {
                error = string.Empty;
                return true;
            }
            long colorToPaint = _program.DequeueOutput();
            long turnDirection = _program.DequeueOutput();
            if (colorToPaint < 0 || colorToPaint > 1
                            || turnDirection < 0 || turnDirection > 1)
            {
                error = "Program produced invalid output.";
                return false;
            }
            if (colorToPaint == 0)
            {
                _whitePanels.Remove((X, Y));
            }
            else
            {
                _whitePanels.Add((X, Y));
            }
            _panelsPainted.Add((X, Y));
            if (turnDirection == 0)
            {
                Direction += 3;
            }
            else
            {
                Direction++;
            }
            Direction %= 4;
            X += steps[Direction].dX;
            Y += steps[Direction].dY;
            error = string.Empty;
            return true;
        }

        private static string ReadIdentifier(HashSet<(int, int)> whitePanels)
        {
            if (whitePanels is null || whitePanels.Count == 0)
            {
                return "ERROR: No white panels.";
            }
            Rectangle boundary = Boundary(whitePanels);
            if (boundary.Height != 6 || boundary.Width < 38 || boundary.Width > 40)
            {
                return "ERROR: Painted panels are not the size of 8 letters.";
            }
            char[,] panels = new char[boundary.Width, boundary.Height];
            for (int y = 0; y < boundary.Height; y++)
            {
                for (int x = 0; x < boundary.Width; x++)
                {
                    if (whitePanels.Contains((boundary.X + x, boundary.Y + y)))
                    {
                        panels[x, y] = '#';
                    }
                    else
                    {
                        panels[x, y] = '.';
                    }
                }
            }
            if (!TryReadLetters(panels, out string identifier))
            {
                return $"ERROR: Could not read all letters in identifier \"{identifier}\".";
            }
            return identifier;
        }

        private static Rectangle Boundary(HashSet<(int, int)> whitePanels)
        {
            int xMin = int.MaxValue, yMin = int.MaxValue;
            int xMax = int.MinValue, yMax = int.MinValue;
            foreach ((int panelX, int panelY) in whitePanels)
            {
                xMin = Math.Min(xMin, panelX);
                yMin = Math.Min(yMin, panelY);
                xMax = Math.Max(xMax, panelX);
                yMax = Math.Max(yMax, panelY);
            }
            return new Rectangle(xMin, yMin, xMax - xMin + 1, yMax - yMin + 1);
        }

        public static bool TryReadLetters(char[,] points, out string result)
        {
            if (points.GetLength(1) != 6 || points.GetLength(0) < 3)
            {
                result = string.Empty;
                return false;
            }
            int xPos = 1;
            for (int y = 0; y < 6; y++)
            {
                if (points[0, y] == '#')
                {
                    xPos = 0;
                    break;
                }
            }
            List<char> parsedLetters = new();
            if (FirstLetterIsI(points, xPos))
            {
                parsedLetters.Add('I');
                xPos += 4;
            }
            bool foundUnknownLetters = false;
            while (xPos < points.GetLength(0))
            {
                char[] bits = new char[30];
                int i = 0;
                for (int y = 0; y < 6; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        if (xPos + x >= points.GetLength(0))
                        {
                            bits[i++] = '0';
                        }
                        else
                        {
                            bits[i++] = points[xPos + x, y] == '#' ? '1' : '0';
                        }
                    }
                }
                if (!Day08.TryGetLetter(string.Join(null, bits), out char c))
                {
                    foundUnknownLetters = true;
                }
                parsedLetters.Add(c);
                xPos += 5;
            }
            result = string.Join(null, parsedLetters);
            return !foundUnknownLetters;
        }

        private static bool FirstLetterIsI(char[,] points, int x)
        {
            if (points.GetLength(0) < x + 3)
            {
                return false;
            }
            return points[x, 0] == '#' && points[x + 1, 0] == '#' && points[x + 2, 0] == '#'
                && points[x, 1] == '.' && points[x + 1, 1] == '#' && points[x + 2, 1] == '.'
                && points[x, 2] == '.' && points[x + 1, 2] == '#' && points[x + 2, 2] == '.'
                && points[x, 3] == '.' && points[x + 1, 3] == '#' && points[x + 2, 3] == '.'
                && points[x, 4] == '.' && points[x + 1, 4] == '#' && points[x + 2, 4] == '.'
                && points[x, 5] == '#' && points[x + 1, 5] == '#' && points[x + 2, 5] == '#';
        }
    }
}
