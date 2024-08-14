// ProgrammingAdvent2021 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using ProgrammingAdvent2021.Common;
using ProgrammingAdvent2021.Solutions;
using ProgrammingAdvent2021.Utilities;

Console.Title = "ProgrammingAdvent2021 by Nonparoxysmic";

bool solve = true;
Console.Write("Press E to test examples, any other key to solve");
char key1 = (char)Console.ReadKey(true).Key;
Console.Clear();
if (key1.ToUpper() == 'E')
{
    solve = false;
    Examples.Initialize();
    Examples.TestExamples();
    Console.WriteLine();
    Console.Write("Press S to solve, any other key to exit");
    char key2 = (char)Console.ReadKey(true).Key;
    Console.WriteLine();
    Console.WriteLine();
    if (key2.ToUpper() == 'S')
    {
        solve = true;
    }
}
if (solve)
{
    for (int dayNumber = 1; dayNumber <= 25; dayNumber++)
    {
        if (Reflection.DayTypes.ContainsKey(dayNumber) && Day.TryGetSolution(dayNumber, out Day? solution))
        {
            if (SystemIO.TryReadInputFile(dayNumber, out string[]? lines))
            {
                solution.Solve(lines);
            }
            else
            {
                Console.WriteLine($"Day {dayNumber:00}: No input.");
            }
        }
        else
        {
            Console.WriteLine($"Day {dayNumber:00}: No solution implemented.");
        }
    }
}
