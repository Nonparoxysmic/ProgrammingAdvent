// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day21 : Day
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

        return output.WriteAnswers(null, null);
    }

    public class UserComputer
    {
        readonly Day09.Day09Program _program;

        public UserComputer(string code)
        {
            _program = new(code);
        }

        public void Run(int timeout = 20)
        {
            Console.WriteLine("Running program...");
            Console.ForegroundColor = ConsoleColor.Green;
            int i = 0;
            while (++i < timeout)
            {
                while (_program.Tick()) { }
                if (_program.Status == Day09.Day09Program.ProgramStatus.Error)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: " + _program.Error);
                    Console.WriteLine();
                    Console.ResetColor();
                    return;
                }
                while (_program.OutputCount > 0)
                {
                    long output = _program.DequeueOutput();
                    if (output == '\n' || (' ' <= output && output <= '~'))
                    {
                        Console.Write((char)output);
                    }
                    else
                    {
                        Console.Write(output);
                    }
                }
                if (_program.Status == Day09.Day09Program.ProgramStatus.Waiting)
                {
                    string input = Console.ReadLine() ?? string.Empty;
                    foreach (char c in input)
                    {
                        _program.EnqueueInput(c);
                    }
                    _program.EnqueueInput(10);
                    continue;
                }
                if (_program.Status == Day09.Day09Program.ProgramStatus.Halted)
                {
                    break;
                }
            }
            Console.ResetColor();
            if (i >= timeout)
            {
                Console.WriteLine("Program ran too long and was timed out.");
            }
            else
            {
                Console.WriteLine("Program successfully halted.");
            }
            Console.WriteLine();
        }
    }
}
