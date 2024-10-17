// ProgrammingAdvent2022 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using ProgrammingAdvent2022.Common;
using ProgrammingAdvent2022.Solutions;

Console.Title = "ProgrammingAdvent2022 by Nonparoxysmic";
ExampleSource.Initialize();
InputManager.Update();
while (true)
{
    Console.WriteLine("Test (E)xamples | (S)olve | E(X)it Program");
    Console.Write("> ");
    string input = Console.ReadLine()?.Trim() ?? string.Empty;
    if (input.Length == 0)
    {
        Console.WriteLine("Enter a command.");
    }
    else if (input.Length == 1)
    {
        if (input[0] == 'E' || input[0] == 'e')
        {
            TestAllExamples();
        }
        else if (input[0] == 'S' || input[0] == 's')
        {
            SolveAll();
        }
        else if (input[0] == 'X' || input[0] == 'x')
        {
            Console.WriteLine("Goodbye.");
            Console.WriteLine();
            return;
        }
        else
        {
            Console.WriteLine("Unknown command.");
        }
    }
    else
    {
        if (!int.TryParse(input[1..], out int day))
        {
            Console.WriteLine("Not a valid number.");
            continue;
        }
        if (day < 1 || day > 25)
        {
            Console.WriteLine($"{day} is not a valid Day.");
            continue;
        }
        if (input[0] == 'E' || input[0] == 'e')
        {
            TestDay(day);
        }
        else if (input[0] == 'S' || input[0] == 's')
        {
            SolveDay(day);
        }
        else if (input[0] == 'X' || input[0] == 'x')
        {
            Console.WriteLine("Goodbye.");
            Console.WriteLine();
            return;
        }
        else
        {
            Console.WriteLine("Unknown command.");
        }
    }
    Console.WriteLine();
}

static void TestAllExamples()
{
    Console.CursorVisible = false;
    Console.WriteLine("Testing examples for all days...");
    List<(ConsoleColor, string)> messages = [];
    Console.Write("Progress: [                         ]");
    Console.CursorLeft -= 26;
    for (int day = 1; day <= 25; day++)
    {
        if (!ExampleSource.TryGetExamples(day,
            out (string Input, string? PartOne, string? PartTwo, string? Modifier)[]? examples))
        {
            messages.Add((ConsoleColor.Red, $"Could not load examples for Day {day}"));
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(" ");
            continue;
        }
        if (examples.Length == 0)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(" ");
            continue;
        }
        if (!SolutionSource.TryGetSolution(day, out Day? solution))
        {
            Console.ResetColor();
            Console.Write(" ");
            continue;
        }
        int fail = 0;
        for (int i = 0; i < examples.Length; i++)
        {
            string input = examples[i].Input;
            string? modifier = examples[i].Modifier;
            PuzzleAnswers calculated = solution.Solve(input.ToLines(), modifier);
            if (calculated.IsError)
            {
                fail++;
                messages.Add((ConsoleColor.Red, $"Day {day} Example {i + 1} returned an error:"));
                messages.Add((ConsoleColor.Red, $"    {calculated.ErrorMessage}"));
                continue;
            }
            if (examples[i].PartOne is not null)
            {
                if (calculated.PartOneAnswer != examples[i].PartOne)
                {
                    if (calculated.PartOneAnswer == "Solution not yet implemented.")
                    {
                        fail++;
                        messages.Add((ConsoleColor.Red, $"Day {day} Example {i + 1} Part One not implemented"));
                    }
                    else
                    {
                        fail++;
                        messages.Add((ConsoleColor.Red, $"Day {day} Example {i + 1} Part One:"));
                        messages.Add((ConsoleColor.Red, $"    Should be \"{examples[i].PartOne}\", got \"{calculated.PartOneAnswer}\""));
                    }
                }
            }
            if (examples[i].PartTwo is not null)
            {
                if (calculated.PartTwoAnswer != examples[i].PartTwo)
                {
                    if (calculated.PartTwoAnswer == "Solution not yet implemented.")
                    {
                        fail++;
                        messages.Add((ConsoleColor.Yellow, $"Day {day} Example {i + 1} Part Two not implemented"));
                    }
                    else
                    {
                        fail++;
                        messages.Add((ConsoleColor.Red, $"Day {day} Example {i + 1} Part Two:"));
                        messages.Add((ConsoleColor.Red, $"    Should be \"{examples[i].PartTwo}\", got \"{calculated.PartTwoAnswer}\""));
                    }
                }
            }
        }
        if (fail == examples.Length)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(" ");
        }
        else if (fail > 0)
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Write(" ");
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(" ");
        }
    }
    Console.ResetColor();
    Console.CursorLeft++;
    Console.WriteLine();
    foreach ((ConsoleColor color, string message) in messages)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
    }
    Console.ResetColor();
    Console.CursorVisible = true;
}

static void SolveAll()
{
    Console.CursorVisible = false;
    Console.WriteLine("Solving all days...");
    List<(ConsoleColor, string)> messages = [];
    List<PuzzleAnswers> goodAnswers = [];
    Console.Write("Progress: [                         ]");
    Console.CursorLeft -= 26;
    for (int day = 1; day <= 25; day++)
    {
        if (!SolutionSource.TryGetSolution(day, out Day? solution))
        {
            Console.ResetColor();
            Console.Write(" ");
            continue;
        }
        if (InputManager.TryGetInput(day, out string[]? input))
        {
            PuzzleAnswers answers = solution.Solve(input);
            if (answers.IsError)
            {
                messages.Add((ConsoleColor.Red, $"Day {day}: {answers.ErrorMessage}"));
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(" ");
            }
            else if (answers.PartOneAnswer != "Solution not yet implemented."
                && answers.PartTwoAnswer != "Solution not yet implemented.")
            {
                goodAnswers.Add(answers);
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write(" ");
            }
            else
            {
                goodAnswers.Add(answers);
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.Write(" ");
            }
        }
        else
        {
            messages.Add((ConsoleColor.Red, $"Could not load input for Day {day}"));
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(" ");
        }
    }
    Console.ResetColor();
    Console.CursorLeft++;
    Console.WriteLine();
    foreach ((ConsoleColor color, string message) in messages)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
    }
    Console.ResetColor();
    Console.WriteLine("Day__Part One__________________________________Part Two____________________________________ms");
    foreach (PuzzleAnswers answers in goodAnswers)
    {
        string output = $" {answers.DayNumber:00}  {answers.PartOneAnswer,-40}  "
                + $"{answers.PartTwoAnswer,-40}  {answers.ElapsedMilliseconds,4}";
        Console.WriteLine(output);
    }
    Console.CursorVisible = true;
}

static void TestDay(int day)
{
    if (!ExampleSource.TryGetExamples(day,
            out (string Input, string? PartOne, string? PartTwo, string? Modifier)[]? examples))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"Could not load examples for Day {day}");
        Console.ResetColor();
        return;
    }
    if (examples.Length == 0)
    {
        Console.WriteLine($"No examples for Day {day}");
        return;
    }
    if (!SolutionSource.TryGetSolution(day, out Day? solution))
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Day {day} solution not available.");
        Console.ResetColor();
        return;
    }
    for (int i = 0; i < examples.Length; i++)
    {
        string input = examples[i].Input;
        string? modifier = examples[i].Modifier;
        PuzzleAnswers calculated = solution.Solve(input.ToLines(), modifier);
        if (calculated.IsError)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Day {day} Example {i + 1} returned an error:");
            Console.WriteLine($"    {calculated.ErrorMessage}");
            Console.ResetColor();
            continue;
        }
        if (examples[i].PartOne is not null)
        {
            if (calculated.PartOneAnswer != examples[i].PartOne)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Day {day} Example {i + 1}:");
                Console.WriteLine($"    Should be \"{examples[i].PartOne}\", got \"{calculated.PartOneAnswer}\"");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Day {day} Example {i + 1} Part One: SUCCESS");
                Console.ResetColor();
            }
        }
        if (examples[i].PartTwo is not null)
        {
            if (calculated.PartTwoAnswer != examples[i].PartTwo)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Day {day} Example {i + 1}:");
                Console.WriteLine($"    Should be \"{examples[i].PartTwo}\", got \"{calculated.PartTwoAnswer}\"");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Day {day} Example {i + 1} Part Two: SUCCESS");
                Console.ResetColor();
            }
        }
    }
}

static void SolveDay(int day)
{
    if (!SolutionSource.TryGetSolution(day, out Day? solution))
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Day {day} solution not available.");
        Console.ResetColor();
        return;
    }
    if (InputManager.TryGetInput(day, out string[]? input))
    {
        PuzzleAnswers answers = solution.Solve(input);
        if (answers.IsError)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Day {day} Error: {answers.ErrorMessage}");
            Console.ResetColor();
        }
        else
        {
            string output = $"Day {day:00} Part One: {answers.PartOneAnswer}{Environment.NewLine}"
                + $"Day {day:00} Part Two: {answers.PartTwoAnswer}{Environment.NewLine}"
                + $"Day {day:00} Time: {answers.ElapsedMilliseconds} ms";
            Console.WriteLine(output);
        }
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Could not load input for Day {day}");
        Console.ResetColor();
    }
}
