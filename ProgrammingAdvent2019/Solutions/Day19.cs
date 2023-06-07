// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day19 : Day
{
    private string _code = string.Empty;

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0 || inputLines[0].Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (!ValidateIntcodeInput(inputLines, out errorMessage))
        {
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();
        _code = inputLines[0];
        if (!ScanBeam(50, out int partOneAnswer, out SortedDictionary<int, (int, int)>? bounds, out string error))
        {
            return output.WriteError(error);
        }
        return output.WriteAnswers(partOneAnswer, null);
    }

    private bool TestPoint(int x, int y, out int result, out string error)
    {
        Day09.Day09Program program = new(_code);
        program.EnqueueInput(x);
        program.EnqueueInput(y);
        while (program.Tick()) { }
        if (program.Status == Day09.Day09Program.ProgramStatus.Error)
        {
            result = -1;
            error = $"Program encountered an error: {program.Error}";
            return false;
        }
        if (program.OutputCount != 1)
        {
            result = -1;
            error = "Program has an unexpected number of outputs.";
            return false;
        }
        result = (int)program.DequeueOutput();
        error = string.Empty;
        return true;
    }

    private bool ScanBeam(int range, out int pointsAffected, out SortedDictionary<int, (int, int)>? bounds, out string error)
    {
        bounds = null;
        pointsAffected = 1;
        // Scan outward from the emitter until finding the direction of the beam.
        int nearPointsFound = 0;
        int rangeScanned = 0;
        for (int r = 1; r < range; r++)
        {
            for (int y = 0; y < r; y++)
            {
                if (!TestPoint(r, y, out int result, out error))
                {
                    pointsAffected = -1;
                    return false;
                }
                if (result == 1)
                {
                    nearPointsFound++;
                }
            }
            for (int x = 0; x <= r; x++)
            {
                if (!TestPoint(x, r, out int result, out error))
                {
                    pointsAffected = -1;
                    return false;
                }
                if (result == 1)
                {
                    nearPointsFound++;
                }
            }
            if (nearPointsFound > 0)
            {
                rangeScanned = r;
                break;
            }
        }
        if (rangeScanned == 0 || rangeScanned == range)
        {
            pointsAffected += nearPointsFound;
            error = string.Empty;
            return true;
        }
        // Finish scanning the remainder of these rows.
        for (int y = 0; y <= rangeScanned; y++)
        {
            for (int x = rangeScanned + 1; x < range; x++)
            {
                if (!TestPoint(x, y, out int result, out error))
                {
                    pointsAffected = -1;
                    return false;
                }
                if (result == 1)
                {
                    nearPointsFound++;
                }
                else
                {
                    break;
                }
            }
        }
        pointsAffected += nearPointsFound;
        // Find the width of the beam in the next row.
        int lowerBoundX = int.MaxValue, upperBoundX = -1;
        for (int testX = 0; testX < 1000; testX++)
        {
            if (!TestPoint(testX, rangeScanned + 1, out int result, out error))
            {
                pointsAffected = -1;
                return false;
            }
            if (result == 1 && lowerBoundX == int.MaxValue)
            {
                lowerBoundX = testX;
                upperBoundX = int.MinValue;
            }
            if (result == 0 && upperBoundX == int.MinValue)
            {
                upperBoundX = testX - 1;
                break;
            }
        }
        if (lowerBoundX >= range)
        {
            error = string.Empty;
            return true;
        }
        pointsAffected += Math.Min(range - 1, upperBoundX) - lowerBoundX + 1;
        if (rangeScanned + 1 == range)
        {
            error = string.Empty;
            return true;
        }
        bounds = new()
        {
            { rangeScanned + 1, (lowerBoundX, upperBoundX) }
        };
        // Scan the remaining rows.
        for (int y = rangeScanned + 2; y < range; y++)
        {
            // Update lower bound.
            for (int x = lowerBoundX; x < lowerBoundX + 1000; x++)
            {
                if (!TestPoint(x, y, out int result, out error))
                {
                    pointsAffected = -1;
                    return false;
                }
                if (result == 1)
                {
                    lowerBoundX = x;
                    break;
                }
            }
            if (lowerBoundX >= range)
            {
                break;
            }
            // Update upper bound.
            if (upperBoundX < range)
            {
                for (int x = Math.Max(lowerBoundX, upperBoundX); x < upperBoundX + 1000; x++)
                {
                    if (!TestPoint(x, y, out int result, out error))
                    {
                        pointsAffected = -1;
                        return false;
                    }
                    if (result == 0)
                    {
                        upperBoundX = x - 1;
                        break;
                    }
                }
                bounds.Add(y, (lowerBoundX, upperBoundX));
            }
            // Count the points in this row.
            pointsAffected += Math.Min(range - 1, upperBoundX) - lowerBoundX + 1;
        }
        error = string.Empty;
        return true;
    }
}
