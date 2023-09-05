// ProgrammingAdvent2020 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;

namespace ProgrammingAdvent2020.Commands;

internal abstract class Command
{
    public static readonly Command Clear = new ClearCommand();
    public static readonly Command Help = new HelpCommand();
    public static readonly Command Input = new InputCommand();
    public static readonly Command Solve = new SolveCommand();
    public static readonly Command Test = new TestCommand();
    public static readonly Command Unrecognized = new UnrecognizedCommand();

    protected static readonly Regex _numberRange
        = new("^(?<first>0?[1-9]|1[0-9]|2[0-5])-(?<last>0?[1-9]|1[0-9]|2[0-5])$");

    public static Command ParseCommand(string command)
    {
        return command switch
        {
            "clear" or "c" => Clear,
            "help" or "h" or "?" or "\"help\"" => Help,
            "input" or "i" => Input,
            "solve" or "s" => Solve,
            "test" or "t" => Test,
            _ => Unrecognized
        };
    }

    public abstract void Execute(string[] args);

    public abstract void PrintHelp();
}
