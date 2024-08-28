// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day10 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        List<long> incompleteLineScores = new(input.Length);
        int totalSyntaxErrorScore = 0;
        foreach (string line in input)
        {
            char? error = FirstIncorrectClosingCharacter(line, out Stack<char> openingCharacters);
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
                    incompleteLineScores.Add(ScoreCompletionString(openingCharacters));
                    break;
                default:
                    break;
            }
        }
        incompleteLineScores.Sort();
        long middleScore = incompleteLineScores[incompleteLineScores.Count / 2];
        return ($"{totalSyntaxErrorScore}", $"{middleScore}");
    }

    private static char? FirstIncorrectClosingCharacter(string line,
        out Stack<char> openingCharacters)
    {
        openingCharacters = [];
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

    private static long ScoreCompletionString(Stack<char> openingCharacters)
    {
        long score = 0;
        while (openingCharacters.Count > 0)
        {
            score *= 5;
            score += openingCharacters.Pop() switch
            {
                '(' => 1,
                '[' => 2,
                '{' => 3,
                '<' => 4,
                _ => 0
            };
        }
        return score;
    }
}
