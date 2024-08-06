// ProgrammingAdvent2021 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using ProgrammingAdvent2021.Common;
using ProgrammingAdvent2021.Solutions;
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
