// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day13 : Day
{
    private static readonly Regex _busIDs = new("^(x|[1-9][0-9]{0,3})(,(x|[1-9][0-9]{0,3}))*$");

    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length < 2)
        {
            errorMessage = "Missing input.";
            return false;
        }
        if (!int.TryParse(input[0], out int timestamp) || timestamp < 1)
        {
            errorMessage = $"Invalid line \"{input[0].Left(20, true)}\" in input.";
            return false;
        }
        if (!_busIDs.IsMatch(input[1]))
        {
            errorMessage = $"Invalid line \"{input[1].Left(20, true)}\" in input.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        int startTime = int.Parse(input[0]);
        int[] busesInService = Buses(input[1]);
        int partOneAnswer = PartOneAnswer(busesInService, startTime);

        return output.WriteAnswers(partOneAnswer, null);
    }

    private static int[] Buses(string busIDs)
    {
        List<int> buses = new();
        string[] terms = busIDs.Split(',');
        foreach (string term in terms)
        {
            if (int.TryParse(term, out int ID))
            {
                buses.Add(ID);
            }
        }
        return buses.ToArray();
    }

    private static int PartOneAnswer(int[] busesInService, int startTime)
    {
        int nextBusID = 0;
        int nextBusTime = int.MaxValue;
        foreach (int bus in busesInService)
        {
            int nextTime = (startTime / bus + 1) * bus;
            if (nextTime < nextBusTime)
            {
                nextBusTime = nextTime;
                nextBusID = bus;
            }
        }
        return nextBusID * (nextBusTime - startTime);
    }
}
