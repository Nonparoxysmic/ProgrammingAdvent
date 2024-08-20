// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day07 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        int[] initialPositions = input[0].Split(',').Select(int.Parse).ToArray();
        Array.Sort(initialPositions);
        int len = initialPositions.Length;

        int partOnePosition = len % 2 == 1
            ? initialPositions[len / 2]
            : (initialPositions[len / 2] + initialPositions[len / 2 - 1]) / 2;
        int partOneAnswer = initialPositions
            .Aggregate(0, (total, next) => total + Math.Abs(partOnePosition - next));

        (int, int)[] positionData = initialPositions.Select(pos => (pos, 1)).ToArray();
        int upperBound = initialPositions.Max();
        int lowerBound = initialPositions.Min();
        int totalCost = 0;
        while (upperBound > lowerBound)
        {
            int upperCost = CostToMove(upperBound, positionData);
            int lowerCost = CostToMove(lowerBound, positionData);
            if (lowerCost < upperCost)
            {
                MakeMove(lowerBound, 1, positionData);
                lowerBound++;
                totalCost += lowerCost;
            }
            else
            {
                MakeMove(upperBound, -1, positionData);
                upperBound--;
                totalCost += upperCost;
            }
        }

        return ($"{partOneAnswer}", $"{totalCost}");
    }

    private static int CostToMove(int position, (int, int)[] positionData)
    {
        return positionData.Where(d => d.Item1 == position).Sum(d => d.Item2);
    }

    private static void MakeMove(int position, int direction, (int, int)[] positionData)
    {
        int newPosition = position + direction;
        for (int i = 0; i < positionData.Length; i++)
        {
            if (positionData[i].Item1 == position)
            {
                positionData[i] = (newPosition, positionData[i].Item2 + 1);
            }
        }
    }
}
