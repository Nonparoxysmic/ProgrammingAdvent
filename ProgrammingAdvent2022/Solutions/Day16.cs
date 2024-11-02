// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Text.RegularExpressions;
using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal partial class Day16 : Day
{
    public static readonly Regex ValidInputLine = ValidInputLineRegex();

    [GeneratedRegex("^Valve (?<valve>[A-Z]{2}) has flow rate=(?<flowRate>[0-9]{1,2}); tunnels* leads* to valves* (?<tunnels>[A-Z]{2}(, [A-Z]{2})*)$")]
    private static partial Regex ValidInputLineRegex();

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        int[,] distanceMatrix = ParseInput(input, out int[] flowRates);

        int mostPressureReleased = MostPressureReleased(30, distanceMatrix, flowRates);

        return result.WriteAnswers(mostPressureReleased, null);
    }

    private static int[,] ParseInput(string[] input, out int[] flowRates)
    {
        List<string> valveNames = input.Select(line => line[6..8]).ToList();
        valveNames.Sort();
        Dictionary<string, int> valveIndices = [];
        for (int i = 0; i < valveNames.Count; i++)
        {
            valveIndices.Add(valveNames[i], i);
        }
        int[,] edgeWeights = new int[valveNames.Count, valveNames.Count];
        edgeWeights.Fill(int.MaxValue / 4 - 2);
        flowRates = new int[valveNames.Count];
        foreach (string line in input)
        {
            Match match = ValidInputLine.Match(line);
            if (match.Success)
            {
                int valve = valveIndices[match.Groups["valve"].Value];
                edgeWeights[valve, valve] = 0;
                flowRates[valve] = int.Parse(match.Groups["flowRate"].Value);
                foreach (string tunnel in match.Groups["tunnels"].Value
                    .Split([' ', ','], StringSplitOptions.RemoveEmptyEntries))
                {
                    int connection = valveIndices[tunnel];
                    edgeWeights[valve, connection] = 1;
                    edgeWeights[connection, valve] = 1;
                }
            }
        }
        int[,] distanceMatrix = FloydWarshall(edgeWeights);
        distanceMatrix = RemoveUnnecessaryValves(distanceMatrix, flowRates, out int[] newFlowRates);
        for (int i = 0; i < distanceMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < distanceMatrix.GetLength(1); j++)
            {
                if (distanceMatrix[i, j] > 0)
                {
                    distanceMatrix[i, j]++;
                }
            }
        }
        flowRates = newFlowRates;
        return distanceMatrix;
    }

    private static int[,] FloydWarshall(int[,] weights)
    {
        int size = weights.GetLength(0);
        for (int k = 0; k < size; k++)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (weights[i, j] > weights[i, k] + weights[k, j])
                    {
                        weights[i, j] = weights[i, k] + weights[k, j];
                    }
                }
            }
        }
        return weights;
    }

    private static int[,] RemoveUnnecessaryValves(int[,] matrix, int[] flowRates, out int[] newFlowRates)
    {
        List<int> neededIndices = [0];
        for (int i = 1; i < flowRates.Length; i++)
        {
            if (flowRates[i] > 0)
            {
                neededIndices.Add(i);
            }
        }
        int[,] result = new int[neededIndices.Count, neededIndices.Count];
        for (int i = 0; i < neededIndices.Count; i++)
        {
            for (int j = 0; j < neededIndices.Count; j++)
            {
                result[i, j] = matrix[neededIndices[i], neededIndices[j]];
            }
        }
        newFlowRates = neededIndices.Select(i => flowRates[i]).ToArray();
        return result;
    }

    private static int MostPressureReleased(int minutes, int[,] distanceMatrix, int[] flowRates)
    {
        Search search = new(minutes, distanceMatrix, flowRates);
        for (int i = 0; i < search.ValveCount; i++)
        {
            if (flowRates[i] > 0)
            {
                int timeToOpen = search.DistanceMatrix[0, i];
                if (timeToOpen < search.Minutes)
                {
                    search.TryValve(i, timeToOpen);
                }
            }
        }
        return search.MostPressureReleased;
    }

    private class Search
    {
        public int Minutes { get; }
        public int[,] DistanceMatrix { get; }
        public int[] FlowRates { get; }
        public int ValveCount { get; }
        public int MostPressureReleased { get; private set; }

        private int minutesPassed;
        private int pressureReleased;
        private readonly Stack<int> valveOpenOrder;
        private readonly bool[] valvesOpened;

        public Search(int minutes, int[,] distanceMatrix, int[] flowRates)
        {
            Minutes = minutes;
            DistanceMatrix = distanceMatrix;
            FlowRates = flowRates;
            ValveCount = flowRates.Length;
            MostPressureReleased = 0;
            minutesPassed = 0;
            pressureReleased = 0;
            valveOpenOrder = [];
            valvesOpened = new bool[flowRates.Length];
            valvesOpened[0] = true;
        }

        public void TryValve(int valveIndex, int time)
        {
            OpenValve(valveIndex, time);

            MostPressureReleased = Math.Max(MostPressureReleased, pressureReleased);

            for (int i = 0; i < ValveCount; i++)
            {
                if (!valvesOpened[i])
                {
                    int timeToOpen = DistanceMatrix[valveIndex, i];
                    if (minutesPassed + timeToOpen < Minutes)
                    {
                        TryValve(i, timeToOpen);
                    }
                }
            }

            UnopenValve(valveIndex, time);
        }

        private void OpenValve(int valveIndex, int time)
        {
            minutesPassed += time;
            pressureReleased += FlowRates[valveIndex] * (Minutes - minutesPassed);
            valveOpenOrder.Push(valveIndex);
            valvesOpened[valveIndex] = true;
        }

        private void UnopenValve(int valveIndex, int time)
        {
            valvesOpened[valveIndex] = false;
            valveOpenOrder.Pop();
            pressureReleased -= FlowRates[valveIndex] * (Minutes - minutesPassed);
            minutesPassed -= time;
        }
    }
}
