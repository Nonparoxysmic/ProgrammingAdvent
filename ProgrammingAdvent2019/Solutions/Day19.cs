// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day19 : Day
{
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

        if (!ScanBeam(inputLines[0], 50, out ScanResult scanResult))
        {
            return output.WriteError(scanResult.ErrorMessage);
        }
        if (!scanResult.FoundBounds)
        {
            return output.WriteAnswers(scanResult.PointsAffected, "ERROR: Beam not found.");
        }

        int upperRightX = scanResult.UpperX;
        int lowerLeftX = scanResult.LowerX;
        for (int i = 0; i < 98; i++)
        {
            if (!AdvanceBound(inputLines[0], lowerLeftX, scanResult.BoundsY + i, 1, out int newX))
            {
                return output.WriteAnswers(scanResult.PointsAffected, "ERROR: Unable to follow the beam.");
            }
            lowerLeftX = newX;
        }

        int currentY = scanResult.BoundsY;
        int timeout = 0;
        while (timeout++ < 10_000 && upperRightX < lowerLeftX + 100)
        {
            if (!AdvanceBound(inputLines[0], lowerLeftX, currentY + 99, 1, out int newLowerX))
            {
                return output.WriteAnswers(scanResult.PointsAffected, "ERROR: Unable to follow the beam.");
            }
            lowerLeftX = newLowerX;
            if (!AdvanceBound(inputLines[0], upperRightX, currentY, 0, out int newUpperX))
            {
                return output.WriteAnswers(scanResult.PointsAffected, "ERROR: Unable to follow the beam.");
            }
            upperRightX = newUpperX;
            currentY++;
        }
        int partTwoAnswer = 10_000 * lowerLeftX + currentY;

        return output.WriteAnswers(scanResult.PointsAffected, partTwoAnswer);
    }

    private static bool TestPoint(int x, int y, string code, out int result, out string error)
    {
        Day09.Day09Program program = new(code);
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

    private static bool ScanBeam(string code, int range, out ScanResult scanResult)
    {
        scanResult = new()
        {
            PointsAffected = 1
        };
        // Scan outward from the emitter until finding the beam.
        int nearPointsFound = 0;
        int rangeScanned = 0;
        for (int r = 1; r < range; r++)
        {
            for (int y = 0; y < r; y++)
            {
                if (!TestPoint(r, y, code, out int result, out string message))
                {
                    scanResult.Error(message);
                    return false;
                }
                if (result == 1)
                {
                    nearPointsFound++;
                }
            }
            for (int x = 0; x <= r; x++)
            {
                if (!TestPoint(x, r, code, out int result, out string message))
                {
                    scanResult.Error(message);
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
            scanResult.PointsAffected += nearPointsFound;
            return true;
        }
        // Finish scanning the remainder of these rows.
        for (int y = 0; y <= rangeScanned; y++)
        {
            for (int x = rangeScanned + 1; x < range; x++)
            {
                if (!TestPoint(x, y, code, out int result, out string message))
                {
                    scanResult.Error(message);
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
        scanResult.PointsAffected += nearPointsFound;
        // Find the width of the beam in the next row.
        int lowerBoundX = int.MaxValue;
        int upperBoundX = -1;
        for (int testX = 0; testX < 1000; testX++)
        {
            if (!TestPoint(testX, rangeScanned + 1, code, out int result, out string message))
            {
                scanResult.Error(message);
                return false;
            }
            if (result == 1 && lowerBoundX == int.MaxValue)
            {
                lowerBoundX = testX;
                upperBoundX = int.MinValue;
            }
            if (result == 0 && upperBoundX == int.MinValue)
            {
                upperBoundX = testX;
                break;
            }
        }
        if (lowerBoundX != int.MaxValue && upperBoundX > 0)
        {
            scanResult.SetBounds(rangeScanned + 1, lowerBoundX, upperBoundX);
        }
        if (lowerBoundX >= range)
        {
            return true;
        }
        scanResult.PointsAffected += Math.Min(range, upperBoundX) - lowerBoundX;
        if (rangeScanned == range - 1)
        {
            return true;
        }
        // Scan the remaining rows.
        for (int y = rangeScanned + 2; y < range; y++)
        {
            // Update lower bound.
            for (int x = lowerBoundX; x < lowerBoundX + 1000; x++)
            {
                if (!TestPoint(x, y, code, out int result, out string message))
                {
                    scanResult.Error(message);
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
            if (upperBoundX <= range)
            {
                for (int x = Math.Max(lowerBoundX, upperBoundX); x < upperBoundX + 1000; x++)
                {
                    if (!TestPoint(x, y, code, out int result, out string message))
                    {
                        scanResult.Error(message);
                        return false;
                    }
                    if (result == 0)
                    {
                        upperBoundX = x;
                        break;
                    }
                }
                scanResult.SetBounds(y, lowerBoundX, upperBoundX);
            }
            // Count the points in this row.
            scanResult.PointsAffected += Math.Min(range, upperBoundX) - lowerBoundX;
        }
        return true;
    }

    private static bool AdvanceBound(string code, int x, int y, int stop, out int newX)
    {
        for (int i = x; i < x + 1000; i++)
        {
            if (!TestPoint(i, y + 1, code, out int result, out _))
            {
                newX = -1;
                return false;
            }
            if (result == stop)
            {
                newX = i;
                return true;
            }
        }
        newX = -1;
        return false;
    }

    private class ScanResult
    {
        public int PointsAffected { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public bool FoundBounds { get; set; }
        public int BoundsY { get; set; }
        public int LowerX { get; set; }
        public int UpperX { get; set; }

        public void Error(string errorMessage)
        {
            PointsAffected = -1;
            ErrorMessage = errorMessage;
        }

        public void SetBounds(int y, int lowerX, int upperX)
        {
            BoundsY = y;
            LowerX = lowerX;
            UpperX = upperX;
            FoundBounds = true;
        }
    }
}
