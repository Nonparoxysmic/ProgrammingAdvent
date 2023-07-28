// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Drawing;
using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day20 : Day
{
    private static readonly Regex _validCharacters = new("^[ #.A-Z]+$");
    private static readonly Regex _validOuterLabel = new(@"^([A-Z]{2}\.|  #)$");
    private static readonly Regex _validInnerLabel = new(@"^([A-Z]{2}\.|[A-Z ]{2}#)$");

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        // Check overall input.
        if (inputLines.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in inputLines)
        {
            if (line.Length == 0)
            {
                errorMessage = "Input contains an empty line.";
                return false;
            }
            if (line.Length != inputLines[0].Length)
            {
                errorMessage = "All lines in input are not the same length.";
                return false;
            }
            if (!_validCharacters.IsMatch(line))
            {
                errorMessage = $"Input \"{line.Left(20, true)}\" contains invalid characters.";
                return false;
            }
        }
        if (inputLines.Length < 15 || inputLines[0].Length < 15)
        {
            errorMessage = "Input is too small.";
            return false;
        }
        // Create map.
        char[,] map = inputLines.ToCharArray2D();
        // Check the corners.
        (int X, int Y)[] corner = new (int, int)[]
        {
            (0, 0),
            (map.GetLength(0) - 2, 0),
            (0, map.GetLength(1) - 2),
            (map.GetLength(0) - 2, map.GetLength(1) - 2)
        };
        for (int i = 0; i < 4; i++)
        {
            for (int y = corner[i].Y; y < corner[i].Y + 2; y++)
            {
                for (int x = corner[i].X; x < corner[i].X + 2; x++)
                {
                    if (map[x, y] != ' ')
                    {
                        errorMessage = $"Unexpected character '{map[x, y]}' at {x},{y}.";
                        return false;
                    }
                }
            }
        }
        // Keep track of labels found.
        List<string> labels = new();
        // Check the outer labels.
        int ox = map.GetLength(0) - 3;
        for (int y = 2; y < map.GetLength(1) - 3; y++)
        {
            string left = string.Join(null, new char[] { map[0, y], map[1, y], map[2, y] });
            if (!_validOuterLabel.IsMatch(left))
            {
                errorMessage = $"Invalid horizontal data \"{left}\".";
                return false;
            }
            string right = string.Join(null, new char[] { map[ox + 1, y], map[ox + 2, y], map[ox, y] });
            if (!_validOuterLabel.IsMatch(right))
            {
                errorMessage = $"Invalid horizontal data \"{right}\".";
                return false;
            }
            if (left != "  #")
            {
                labels.Add(left);
            }
            if (right != "  #")
            {
                labels.Add(right);
            }
        }
        int oy = map.GetLength(1) - 3;
        for (int x = 2; x < map.GetLength(0) - 3; x++)
        {
            string top = string.Join(null, new char[] { map[x, 0], map[x, 1], map[x, 2] });
            if (!_validOuterLabel.IsMatch(top))
            {
                errorMessage = $"Invalid vertical data \"{top}\".";
                return false;
            }
            string bottom = string.Join(null, new char[] { map[x, oy + 1], map[x, oy + 2], map[x, oy] });
            if (!_validOuterLabel.IsMatch(bottom))
            {
                errorMessage = $"Invalid vertical data \"{bottom}\".";
                return false;
            }
            if (top != "  #")
            {
                labels.Add(top);
            }
            if (bottom != "  #")
            {
                labels.Add(bottom);
            }
        }
        // Check the inner labels.
        Rectangle interior = InteriorSpace(map);
        if (interior.Width < 5 || interior.Height < 5)
        {
            errorMessage = "Inner area of maze is too small.";
            return false;
        }
        int ix = interior.X + interior.Width;
        for (int y = interior.Y; y < interior.Y + interior.Height; y++)
        {
            string left = string.Join(null, new char[] { map[interior.X, y], map[interior.X + 1, y], map[interior.X - 1, y] });
            if (!_validInnerLabel.IsMatch(left))
            {
                errorMessage = $"Invalid horizontal data \"{left}\".";
                return false;
            }
            string right = string.Join(null, new char[] { map[ix - 2, y], map[ix - 1, y], map[ix, y] });
            if (!_validInnerLabel.IsMatch(right))
            {
                errorMessage = $"Invalid horizontal data \"{right}\".";
                return false;
            }
            if (left[2] == '.')
            {
                labels.Add(left);
                map[interior.X, y] = ' ';
                map[interior.X + 1, y] = ' ';
            }
            if (right[2] == '.')
            {
                labels.Add(right);
                map[ix - 2, y] = ' ';
                map[ix - 1, y] = ' ';
            }
        }
        int iy = interior.Y + interior.Height;
        for (int x = interior.X; x < interior.X + interior.Width; x++)
        {
            string top = string.Join(null, new char[] { map[x, interior.Y], map[x, interior.Y + 1], map[x, interior.Y - 1] });
            if (!_validInnerLabel.IsMatch(top))
            {
                errorMessage = $"Invalid vertical data \"{top}\".";
                return false;
            }
            string bottom = string.Join(null, new char[] { map[x, iy - 2], map[x, iy - 1], map[x, iy] });
            if (!_validInnerLabel.IsMatch(bottom))
            {
                errorMessage = $"Invalid vertical data \"{bottom}\".";
                return false;
            }
            if (top[2] == '.')
            {
                labels.Add(top);
                map[x, interior.Y] = ' ';
                map[x, interior.Y + 1] = ' ';
            }
            if (bottom[2] == '.')
            {
                labels.Add(bottom);
                map[x, iy - 2] = ' ';
                map[x, iy - 1] = ' ';
            }
        }
        // Check for stray characters in the interior.
        for (int y = interior.Y; y < interior.Y + interior.Height; y++)
        {
            for (int x = interior.X; x < interior.X + interior.Width; x++)
            {
                if (map[x, y] != ' ')
                {
                    errorMessage = $"Unexpected character '{map[x, y]}' at {x},{y}.";
                    return false;
                }
            }
        }
        // Check the set of labels.
        if (labels.Count == 0)
        {
            errorMessage = "Input contains no labels.";
            return false;
        }
        if (labels.Count % 2 == 1)
        {
            errorMessage = "Input contains an odd number of labels.";
            return false;
        }
        if (!labels.Contains("AA."))
        {
            errorMessage = "Input is missing label \"AA\".";
            return false;
        }
        if (!labels.Contains("ZZ."))
        {
            errorMessage = "Input is missing label \"ZZ\".";
            return false;
        }
        labels.Sort();
        bool hasMismatchedLabels = false;
        if (labels[0] != "AA." || labels[^1] != "ZZ." || labels[^2] == labels[^1])
        {
            hasMismatchedLabels = true;
        }
        else
        {
            for (int i = 1; i < labels.Count - 1; i += 2)
            {
                if (labels[i] != labels[i + 1])
                {
                    hasMismatchedLabels = true;
                    break;
                }
                if (labels[i] == labels[i - 1])
                {
                    hasMismatchedLabels = true;
                    break;
                }
                if (labels[i][0] == ' ' || labels[i][1] == ' ')
                {
                    errorMessage = $"Invalid label \"{labels[i]}\"";
                    return false;
                }
                if (labels[i + 1][0] == ' ' || labels[i + 1][1] == ' ')
                {
                    errorMessage = $"Invalid label \"{labels[i + 1]}\"";
                    return false;
                }
            }
        }
        if (hasMismatchedLabels)
        {
            errorMessage = "Input has mismatched labels.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static Rectangle InteriorSpace(char[,] map)
    {
        Rectangle output = new();
        (int centerX, int centerY) = (map.GetLength(0) / 2, map.GetLength(1) / 2);
        for (int y = centerY; y >= 0; y--)
        {
            if (map[centerX, y] == '#' || map[centerX, y] == '.')
            {
                output.Y = y + 1;
                break;
            }
        }
        for (int y = centerY; y < map.GetLength(1); y++)
        {
            if (map[centerX, y] == '#' || map[centerX, y] == '.')
            {
                output.Height = y - output.Y;
                break;
            }
        }
        for (int x = centerX; x >= 0; x--)
        {
            if (map[x, centerY] == '#' || map[x, centerY] == '.')
            {
                output.X = x + 1;
                break;
            }
        }
        for (int x = centerX; x < map.GetLength(0); x++)
        {
            if (map[x, centerY] == '#' || map[x, centerY] == '.')
            {
                output.Width = x - output.X;
                break;
            }
        }
        return output;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        return output.WriteAnswers(null, null);
    }
}
