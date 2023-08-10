// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day20 : Day
{
    private static readonly Regex _validCharacters = new("^[ #.A-Z]+$");
    private static readonly Vector2Int[] _directions = new Vector2Int[]
    {
        new Vector2Int(-1,  0),
        new Vector2Int( 0, -1),
        new Vector2Int( 1,  0),
        new Vector2Int( 0,  1)
    };

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
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

        char[,] map = inputLines.ToCharArray2D();
        if (!TryFindInteriorSpace(map, out Rectangle interior))
        {
            errorMessage = "Unable to find interior space.";
            return false;
        }
        Dictionary<(int, int), string> outerLabels = OuterLabels(map);
        Dictionary<(int, int), string> innerLabels = InnerLabels(map, interior);

        List<string> outerList = outerLabels.Values.ToList();
        if (outerList.Count == 0)
        {
            errorMessage = "Maze contains no entrances.";
            return false;
        }
        for (int i = 1; i < outerList.Count; i++)
        {
            if (outerList[i] == outerList[i - 1])
            {
                errorMessage = $"Duplicate outer label \"{outerList[i]}\".";
                return false;
            }
        }
        List<string> innerList = innerLabels.Values.ToList();
        for (int i = 1; i < innerList.Count; i++)
        {
            if (innerList[i] == innerList[i - 1])
            {
                errorMessage = $"Duplicate inner label \"{innerList[i]}\".";
                return false;
            }
        }
        foreach (string label in new string[] { "AA", "ZZ" })
        {
            if (innerList.Contains(label))
            {
                errorMessage = $"Label \"{label}\" is in an invalid position.";
                return false;
            }
            if (!outerList.Contains(label))
            {
                errorMessage = $"Label \"{label}\" is missing.";
                return false;
            }
            outerList.Remove(label);
        }

        outerList.Sort();
        innerList.Sort();
        if (outerList.Count != innerList.Count)
        {
            errorMessage = "Mismatched number of labels on outer edge versus inner edge.";
            return false;
        }
        for (int i = 0; i < outerList.Count; i++)
        {
            if (outerList[i] != innerList[i])
            {
                errorMessage = "Labels on inner edge do not match labels on outer edge.";
                return false;
            }
            if (!AreCapitalLetters(outerList[i][0], outerList[i][1]))
            {
                errorMessage = $"Invalid label \"{outerList[i]}\".";
                return false;
            }
        }

        int width = map.GetLength(0);
        int height = map.GetLength(1);
        if (map[2, 2] != '#' || map[2, height - 3] != '#'
            || map[width - 3, 2] != '#' || map[width - 3, height - 3] != '#')
        {
            errorMessage = "All four corners are not '#'.";
            return false;
        }
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x < 2 || x >= width - 2 || y < 2 || y >= height - 2)
                {
                    if (map[x, y] != ' ')
                    {
                        errorMessage = $"Unexpected character {map[x, y]} at {x},{y}.";
                        return false;
                    }
                }
                else if (x < interior.X || x >= interior.Right || y < interior.Y || y >= interior.Bottom)
                {
                    if (map[x, y] != '#' && map[x, y] != '.' && map[x, y] != '*')
                    {
                        errorMessage = $"Unexpected character {map[x, y]} at {x},{y}.";
                        return false;
                    }
                }
                else if (map[x, y] != ' ')
                {
                    errorMessage = $"Unexpected character {map[x, y]} at {x},{y}.";
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

        char[,] map = inputLines.ToCharArray2D();
        TryFindInteriorSpace(map, out Rectangle interior);
        Dictionary<(int, int), string> outerLabels = OuterLabels(map);
        Dictionary<(int, int), string> innerLabels = InnerLabels(map, interior);
        PruneDeadEnds(map);

        return output.WriteAnswers(null, null);
    }

    private static bool TryFindInteriorSpace(char[,] map, [NotNullWhen(true)] out Rectangle interior)
    {
        int centerX = map.GetLength(0) / 2;
        int centerY = map.GetLength(1) / 2;
        int interiorX = centerX, interiorY = centerY, width = 0, height = 0;
        for (int y = centerY; y >= 4; y--)
        {
            if (map[centerX, y] == '#' || map[centerX, y] == '.')
            {
                interiorY = y + 1;
                break;
            }
        }
        for (int y = centerY; y < map.GetLength(1) - 4; y++)
        {
            if (map[centerX, y] == '#' || map[centerX, y] == '.')
            {
                height = y - interiorY;
                break;
            }
        }
        for (int x = centerX; x >= 4; x--)
        {
            if (map[x, centerY] == '#' || map[x, centerY] == '.')
            {
                interiorX = x + 1;
                break;
            }
        }
        for (int x = centerX; x < map.GetLength(0) - 4; x++)
        {
            if (map[x, centerY] == '#' || map[x, centerY] == '.')
            {
                width = x - interiorX;
                break;
            }
        }
        if (interiorX == centerX || interiorY == centerY || width < 5 || height < 5)
        {
            interior = Rectangle.Empty;
            return false;
        }
        interior = new Rectangle(interiorX, interiorY, width, height);
        return true;
    }

    private static Dictionary<(int, int), string> OuterLabels(char[,] map)
    {
        char labelClear = ' ';
        char portal = '*';
        Dictionary<(int, int), string> labels = new();
        int limitX = map.GetLength(0) - 3;
        int limitY = map.GetLength(1) - 3;
        for (int y = 3; y < limitY; y++)
        {
            // Left edge
            if (map[2, y] == '.')
            {
                labels.Add((2, y), $"{map[0, y]}{map[1, y]}");
                map[0, y] = labelClear;
                map[1, y] = labelClear;
                map[2, y] = portal;
            }
            // Right edge
            if (map[limitX, y] == '.')
            {
                labels.Add((limitX, y), $"{map[limitX + 1, y]}{map[limitX + 2, y]}");
                map[limitX + 1, y] = labelClear;
                map[limitX + 2, y] = labelClear;
                map[limitX, y] = portal;
            }
        }
        for (int x = 3; x < limitX; x++)
        {
            // Top edge
            if (map[x, 2] == '.')
            {
                labels.Add((x, 2), $"{map[x, 0]}{map[x, 1]}");
                map[x, 0] = labelClear;
                map[x, 1] = labelClear;
                map[x, 2] = portal;
            }
            // Bottom edge
            if (map[x, limitY] == '.')
            {
                labels.Add((x, limitY), $"{map[x, limitY + 1]}{map[x, limitY + 2]}");
                map[x, limitY + 1] = labelClear;
                map[x, limitY + 2] = labelClear;
                map[x, limitY] = portal;
            }
        }
        return labels;
    }

    private static Dictionary<(int, int), string> InnerLabels(char[,] map, Rectangle interior)
    {
        char labelClear = ' ';
        char portal = '*';
        Dictionary<(int, int), string> labels = new();
        int limitX = interior.Right;
        int limitY = interior.Bottom;
        for (int y = interior.Y; y < limitY; y++)
        {
            // Left edge
            if (map[interior.X - 1, y] == '.')
            {
                labels.Add((interior.X - 1, y), $"{map[interior.X, y]}{map[interior.X + 1, y]}");
                map[interior.X, y] = labelClear;
                map[interior.X + 1, y] = labelClear;
                map[interior.X - 1, y] = portal;
            }
            // Right edge
            if (map[limitX, y] == '.')
            {
                labels.Add((limitX, y), $"{map[limitX - 2, y]}{map[limitX - 1, y]}");
                map[limitX - 2, y] = labelClear;
                map[limitX - 1, y] = labelClear;
                map[limitX, y] = portal;
            }
        }
        for (int x = interior.X; x < limitX; x++)
        {
            // Top edge
            if (map[x, interior.Y - 1] == '.')
            {
                labels.Add((x, interior.Y - 1), $"{map[x, interior.Y]}{map[x, interior.Y + 1]}");
                map[x, interior.Y] = labelClear;
                map[x, interior.Y + 1] = labelClear;
                map[x, interior.Y - 1] = portal;
            }
            // Bottom edge
            if (map[x, limitY] == '.')
            {
                labels.Add((x, limitY), $"{map[x, limitY - 2]}{map[x, limitY - 1]}");
                map[x, limitY - 2] = labelClear;
                map[x, limitY - 1] = labelClear;
                map[x, limitY] = portal;
            }
        }
        return labels;
    }

    private static bool AreCapitalLetters(char one, char two)
    {
        return 'A' <= one && one <= 'Z' && 'A' <= two && two <= 'Z';
    }

    private static void PruneDeadEnds(char[,] map)
    {
        for (int y = 2; y < map.GetLength(1) - 2; y++)
        {
            for (int x = 2; x < map.GetLength(0) - 2; x++)
            {
                PruneDeadEnd(map, x, y);
            }
        }
    }

    private static void PruneDeadEnd(char[,] map, int x, int y)
    {
        if (map[x, y] != '.')
        {
            return;
        }
        int walls = 0;
        int openX = -1;
        int openY = -1;
        foreach (Vector2Int step in _directions)
        {
            if (map[x + step.X, y + step.Y] == '#' || map[x + step.X, y + step.Y] == ' ')
            {
                walls++;
            }
            else
            {
                openX = x + step.X;
                openY = y + step.Y;
            }
        }
        if (walls == 4)
        {
            map[x, y] = '#';
            return;
        }
        if (walls == 3)
        {
            map[x, y] = '#';
            PruneDeadEnd(map, openX, openY);
        }
    }
}
