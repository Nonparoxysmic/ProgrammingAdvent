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
        Bus[] buses = Buses(input[1]);
        int partOneAnswer = PartOneAnswer(buses.Select(b => (int)b.ID), startTime);
        ulong? partTwoAnswer = PartTwoAnswer(buses);
        if (partTwoAnswer is null)
        {
            return output.WriteError("Did not find the answer.");
        }

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static Bus[] Buses(string busIDs)
    {
        List<Bus> buses = new();
        string[] terms = busIDs.Split(',');
        for (ulong i = 0; i < (ulong)terms.Length; i++)
        {
            if (ulong.TryParse(terms[i], out ulong ID))
            {
                buses.Add(new Bus(ID, i));
            }
        }
        return buses.ToArray();
    }

    private static int PartOneAnswer(IEnumerable<int> busesInService, int startTime)
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

    private static ulong? PartTwoAnswer(Bus[] buses)
    {
        ulong current = 0;
        ulong step = buses[0].ID;
        for (int nextBus = 1; nextBus < buses.Length; nextBus++)
        {
            int timeout = 0;
            while ((current + buses[nextBus].Index) % buses[nextBus].ID != 0)
            {
                current += step;
                if (timeout++ > 2048)
                {
                    return null;
                }
            }
            step *= buses[nextBus].ID;
        }
        return current;
    }

    private class Bus
    {
        public ulong ID { get; private set; }
        public ulong Index { get; private set; }

        public Bus(ulong id, ulong index)
        {
            ID = id;
            Index = index;
        }
    }
}
