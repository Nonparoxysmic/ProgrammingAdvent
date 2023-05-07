// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day03 : Day
{
    private static readonly Regex _validCharacters = new("^[0-9,LDRU]+$");
    private static readonly Regex _validTerm = new("^[LDRU](?<Steps>[0-9]{1,4})$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (inputLines.Length < 2)
        {
            errorMessage = "Insufficient input.";
            return false;
        }
        for (int i = 0; i < 2; i++)
        {
            if (inputLines[i].Length == 0)
            {
                errorMessage = "Empty input line.";
                return false;
            }
            if (!_validCharacters.IsMatch(inputLines[i]))
            {
                errorMessage = $"Input \"{inputLines[i].Left(20, true)}\" contains invalid characters.";
                return false;
            }
            string[] terms = inputLines[i].Split(',');
            foreach (string term in terms)
            {
                if (!_validTerm.IsMatch(term))
                {
                    errorMessage = $"Invalid term \"{term.Left(20, true)}\" in input.";
                    return false;
                }
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();
        Dictionary<Vector2Int, int> firstWire = WireData(inputLines[0]);
        Dictionary<Vector2Int, int> secondWire = WireData(inputLines[1]);
        IEnumerable<Vector2Int> intersections = firstWire.Keys.Intersect(secondWire.Keys);
        if (!intersections.Any())
        {
            output.WriteError("Wires do not intersect.");
            return output;
        }
        int partOneAnswer = intersections.Select(v => v.TaxicabMagnitude()).Min();
        int partTwoAnswer = intersections.Select(v => firstWire[v] + secondWire[v]).Min();
        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static Dictionary<Vector2Int, int> WireData(string input)
    {
        Dictionary<Vector2Int, int> output = new();
        string[] terms = input.Split(',');
        int x = 0, y = 0, totalSteps = 0;
        foreach (string term in terms)
        {
            int steps = int.Parse(_validTerm.Match(term).Groups["Steps"].Value);
            (int stepX, int stepY) = term[0] switch
            {
                'L' => (-1,  0),
                'D' => ( 0, -1),
                'R' => ( 1,  0),
                'U' => ( 0,  1),
                _ => (0, 0)
            };
            for (int i = 0; i < steps; i++)
            {
                x += stepX;
                y += stepY;
                totalSteps++;
                Vector2Int position = new(x, y);
                if (!output.ContainsKey(position))
                {
                    output.Add(position, totalSteps);
                }
            }
        }
        return output;
    }
}
