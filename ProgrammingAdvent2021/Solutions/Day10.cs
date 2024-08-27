// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day10 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        List<string> incompleteLines = new(input.Length);
        int totalSyntaxErrorScore = 0;
        foreach (string line in input)
        {
            char? error = FirstIncorrectClosingCharacter(line);
            switch (error)
            {
                case ')':
                    totalSyntaxErrorScore += 3;
                    break;
                case ']':
                    totalSyntaxErrorScore += 57;
                    break;
                case '}':
                    totalSyntaxErrorScore += 1197;
                    break;
                case '>':
                    totalSyntaxErrorScore += 25137;
                    break;
                case null:
                    incompleteLines.Add(line);
                    break;
                default:
                    break;
            }
        }
        return ($"{totalSyntaxErrorScore}", "n/a");
    }

    private static char? FirstIncorrectClosingCharacter(string line)
    {
        Stack<char> openingCharacters = [];
        foreach (char c in line)
        {
            switch (c)
            {
                case '(':
                case '[':
                case '{':
                case '<':
                    openingCharacters.Push(c);
                    break;
                case ')':
                    if (openingCharacters.Pop() != '(')
                    {
                        return c;
                    }
                    break;
                case ']':
                    if (openingCharacters.Pop() != '[')
                    {
                        return c;
                    }
                    break;
                case '}':
                    if (openingCharacters.Pop() != '{')
                    {
                        return c;
                    }
                    break;
                case '>':
                    if (openingCharacters.Pop() != '<')
                    {
                        return c;
                    }
                    break;
            }
        }
        return null;
    }
}
