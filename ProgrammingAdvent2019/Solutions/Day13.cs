// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day13 : Day
{
    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
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
        Day13Arcade arcade = new(inputLines[0]);
        if (!arcade.Run(out string error))
        {
            return output.WriteError(error);
        }
        return output.WriteAnswers(arcade.BlocksOnScreen, null);
    }

    private class Day13Arcade
    {
        public int BlocksOnScreen
        {
            get
            {
                return _tiles.Values.Count(t => t == 2);
            }
        }

        private readonly Day09.Day09Program _program;
        private readonly Dictionary<(long, long), long> _tiles = new();

        public Day13Arcade(string intcode)
        {
            _program = new Day09.Day09Program(intcode);
        }

        public bool Run(out string error)
        {
            int timeout = 0;
            while (timeout++ < 1000)
            {
                while (_program.Tick()) { }
                if (_program.Status == Day09.Day09Program.ProgramStatus.Error)
                {
                    error = _program.Error;
                    return false;
                }
                if (_program.Status == Day09.Day09Program.ProgramStatus.Halted)
                {
                    while (_program.OutputCount >= 3)
                    {
                        long x = _program.DequeueOutput();
                        long y = _program.DequeueOutput();
                        long tile = _program.DequeueOutput();
                        _tiles[(x, y)] = tile;
                    }
                    if (_program.OutputCount > 0)
                    {
                        error = "Unexpected outputs.";
                        return false;
                    }
                    error = string.Empty;
                    return true;
                }
                if (_program.Status == Day09.Day09Program.ProgramStatus.Waiting)
                {
                    error = "Program is stuck waiting for input.";
                    return false;
                }
            }
            error = "Game ran too long.";
            return false;
        }
    }
}
