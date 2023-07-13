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
        if (bounds is null || bounds.Count < 2)
        {
            return output.WriteAnswers(partOneAnswer, "Failed to image the beam.");
        }
        BoundsFunction boundsFunction = new(bounds);
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

    private class BoundsFunction
    {
        private readonly int lowerX0;
        private readonly int lowerY0;
        private readonly int lowerLoopDeltaX;
        private readonly int[] lowerLoopOffsets;
        private readonly int upperX0;
        private readonly int upperY0;
        private readonly int upperLoopDeltaX;
        private readonly int[] upperLoopOffsets;

        public BoundsFunction(SortedDictionary<int, (int, int)> bounds)
        {
            (lowerX0, lowerY0, lowerLoopDeltaX, lowerLoopOffsets) = BoundsParamenters(bounds, 0);
            (upperX0, upperY0, upperLoopDeltaX, upperLoopOffsets) = BoundsParamenters(bounds, 1);
        }

        public int LowerBound(int y)
        {
            if (y < lowerY0)
            {
                return -1;
            }
            return lowerX0 + (y - lowerY0) / lowerLoopOffsets.Length * lowerLoopDeltaX
                + lowerLoopOffsets[(y - lowerY0) % lowerLoopOffsets.Length];
        }

        public int UpperBound(int y)
        {
            if (y < upperY0)
            {
                return -1;
            }
            return upperX0 + (y - upperY0) / upperLoopOffsets.Length * upperLoopDeltaX
                + upperLoopOffsets[(y - upperY0) % upperLoopOffsets.Length];
        }

        private static (int, int, int, int[]) BoundsParamenters(SortedDictionary<int, (int, int)> bounds, int index)
        {
            int[] measuredX = new int[bounds.Count];
            (int, int)[] values = bounds.Values.ToArray();
            if (index <= 0)
            {
                for (int i = 0; i < bounds.Count; i++)
                {
                    measuredX[i] = values[i].Item1;
                }
            }
            else
            {
                for (int i = 0; i < bounds.Count; i++)
                {
                    measuredX[i] = values[i].Item2;
                }
            }

            int[] differences = new int[measuredX.Length];
            for (int i = 1; i < differences.Length; i++)
            {
                differences[i] = measuredX[i] - measuredX[i - 1];
            }
            int position = -1, length = -1;
            for (int i = 2; i < differences.Length; i++)
            {
                if (differences[i] != differences[1])
                {
                    position = i;
                    break;
                }
            }
            if (position < 0)
            {
                position = 1;
                length = differences.Length - 1;
            }
            else
            {
                bool foundNext = false;
                for (int i = position; i < differences.Length; i++)
                {
                    if (differences[i] != differences[position])
                    {
                        foundNext = true;
                    }
                    if (foundNext && differences[i] == differences[position])
                    {
                        length = i - position;
                        break;
                    }
                }
                if (length < 0)
                {
                    length = differences.Length - position;
                }
            }

            int outputX0 = measuredX[position];
            int[] keys = bounds.Keys.ToArray();
            int outputY0 = keys[position];
            int loopDeltaX = measuredX[position + length] - outputX0;
            int[] loopOffsets = new int[length];
            for (int i = 0; i < loopOffsets.Length; i++)
            {
                loopOffsets[i] = measuredX[position + i] - measuredX[position];
            }

            return (outputX0, outputY0, loopDeltaX, loopOffsets);
        }
    }
}
