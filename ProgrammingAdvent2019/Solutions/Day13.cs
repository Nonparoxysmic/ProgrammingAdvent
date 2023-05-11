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
        Day13Arcade arcadeDemo = new(inputLines[0]);
        if (!arcadeDemo.Run(out string error1))
        {
            return output.WriteError(error1);
        }
        Day13Arcade arcadeGame = new(inputLines[0], 2);
        if (!arcadeGame.Run(out string error2))
        {
            return output.WriteError(error2);
        }
        return output.WriteAnswers(arcadeDemo.BlocksOnScreen, arcadeGame.Score);
    }

    private class Day13Arcade
    {
        public long Score { get; private set; }
        public int BlocksOnScreen
        {
            get
            {
                int sum = 0;
                for (int y = 0; y < _display.GetLength(1); y++)
                {
                    for (int x = 0; x < _display.GetLength(0); x++)
                    {
                        if (_display[x, y] == '2')
                        {
                            sum++;
                        }
                    }
                }
                return sum;
            }
        }

        private readonly Day09.Day09Program _program;
        private readonly char[,] _display = new char[44, 20];
        private readonly bool isDemo = true;
        private long _ballPositionX;
        private long _paddlePositionX;

        public Day13Arcade(string intcode, int quarters = 0)
        {
            _program = new Day09.Day09Program(intcode);
            if (quarters != 0)
            {
                _program.WriteMemory(0, quarters);
                isDemo = false;
            }
        }

        public bool Run(out string error)
        {
            int timeout = 0;
            while (timeout++ < 1_000_000)
            {
                while (_program.Tick()) { }
                if (_program.Status == Day09.Day09Program.ProgramStatus.Error)
                {
                    error = _program.Error;
                    return false;
                }
                if (_program.Status == Day09.Day09Program.ProgramStatus.Halted)
                {
                    if (!UpdateDisplay())
                    {
                        error = "Program produced invalid outputs.";
                        return false;
                    }
                    error = string.Empty;
                    return true;
                }
                if (_program.Status == Day09.Day09Program.ProgramStatus.Waiting)
                {
                    if (!UpdateDisplay())
                    {
                        error = "Program produced invalid outputs.";
                        return false;
                    }
                    if (isDemo)
                    {
                        error = "Program is stuck waiting for input.";
                        return false;
                    }
                    _program.EnqueueInput(Math.Sign(_ballPositionX - _paddlePositionX));
                }
            }
            error = "Game ran too long.";
            return false;
        }

        private bool UpdateDisplay()
        {
            while (_program.OutputCount >= 3)
            {
                long x = _program.DequeueOutput();
                long y = _program.DequeueOutput();
                long tile = _program.DequeueOutput();
                if (x == -1 && y == 0)
                {
                    Score = tile;
                    continue;
                }
                if (x < 0 || y < 0 || x >= 44 || y >= 20)
                {
                    return false;
                }
                _display[x, y] = (char)(tile + '0');
                if (_display[x, y] == '4')
                {
                    _ballPositionX = x;
                }
                else if (_display[x, y] == '3')
                {
                    _paddlePositionX = x;
                }
            }
            return _program.OutputCount == 0;
        }

        //private void DebugPrintDisplay()
        //{
        //    for (int y = 0; y < _display.GetLength(1); y++)
        //    {
        //        for (int x = 0; x < _display.GetLength(0); x++)
        //        {
        //            Console.Write(_display[x, y] == '0' ? ' ' : _display[x, y]);
        //        }
        //        Console.WriteLine();
        //    }
        //    Console.WriteLine($"Score: {Score}");
        //}
    }
}
