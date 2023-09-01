// ProgrammingAdvent2020 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using ProgrammingAdvent2020.Commands;
using ProgrammingAdvent2020.Common;

Console.Title = "ProgrammingAdvent2020 by Nonparoxysmic";

ExampleSource.Initialize();
InputManager.Update();
while (true)
{
    Console.WriteLine();
    Console.Write("> ");
    string input = Console.ReadLine() ?? string.Empty;
    string[] terms = input.Split(null as char[], StringSplitOptions.RemoveEmptyEntries);
    if (terms.Length == 0)
    {
        continue;
    }
    if (int.TryParse(terms[0], out int numberInput) && 1 <= numberInput && numberInput <= 25)
    {
        terms = terms.Prepend("solve").ToArray();
    }
    string commandText = terms[0].ToLower();
    if (commandText is "exit" or "quit" or "x" or "q")
    {
        break;
    }
    Command command = Command.ParseCommand(commandText);
    if (terms.Length > 1 && terms[1] is "help" or "h" or "?" or "\"help\"")
    {
        command.PrintHelp();
    }
    else
    {
        command.Execute(terms.TakeLast(terms.Length - 1).ToArray());
    }
}
