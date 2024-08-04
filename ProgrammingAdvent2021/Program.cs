// ProgrammingAdvent2021 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using ProgrammingAdvent2021.Common;
using ProgrammingAdvent2021.Utilities;

Console.Title = "ProgrammingAdvent2021 by Nonparoxysmic";

Console.Write("Press E to test examples, any other key to solve");
char key = (char)Console.ReadKey(true).Key;
Console.Clear();
if (key.ToUpper() == 'E')
{
    Examples.Initialize();
    Examples.TestExamples();
}
else
{
    // TODO: Solve puzzles
    Console.WriteLine("Solving not yet implemented.");
}
