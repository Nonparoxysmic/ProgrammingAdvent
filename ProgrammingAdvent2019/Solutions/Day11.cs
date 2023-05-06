// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

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
        return output.WriteAnswers(robot.PanelsPainted, null);
    }

    private class Day11Robot
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Direction { get; private set; }
        public int PanelsPainted => _panelsPainted.Count;

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

        public Day11Robot(string intcode)
        {
            _program = new Day09.Day09Program(intcode);
        }

        public bool Run(out string error)
        {
            Direction = 3;
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
                Direction++;
            }
            else
            {
                Direction += 3;
            }
            Direction %= 4;
            X += steps[Direction].dX;
            Y += steps[Direction].dY;
            error = string.Empty;
            return true;
        }
    }
}
