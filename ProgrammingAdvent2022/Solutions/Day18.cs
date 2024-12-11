// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Text.RegularExpressions;
using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal partial class Day18 : Day
{
    public static readonly Regex ValidInputLine = ValidInputLineRegex();

    [GeneratedRegex("^(?<x>[0-9]{1,2}),(?<y>[0-9]{1,2}),(?<z>[0-9]{1,2})$")]
    private static partial Regex ValidInputLineRegex();

    private static readonly Vector3Int[] _directions =
    [
        Vector3Int.UnitX, Vector3Int.UnitY, Vector3Int.UnitZ,
        -Vector3Int.UnitX, -Vector3Int.UnitY, -Vector3Int.UnitZ,
    ];

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        int[,,] map = ParseInput(input);
        int surfaceArea = AllSurfaces(map);

        return result.WriteAnswers(surfaceArea, null);
    }

    private static int[,,] ParseInput(string[] input)
    {
        IEnumerable<Vector3Int> cubePositions = input.Select(line => ParseInputLine(line));
        int maxCoord = 1;
        foreach (Vector3Int cube in cubePositions)
        {
            maxCoord = MoreMath.Max(maxCoord, cube.X, cube.Y, cube.Z);
        }
        int[,,] map = new int[maxCoord + 3, maxCoord + 3, maxCoord + 3];
        foreach (Vector3Int cube in cubePositions)
        {
            map[cube.X + 1, cube.Y + 1, cube.Z + 1] = 1;
        }
        return map;
    }

    private static Vector3Int ParseInputLine(string line)
    {
        Match match = ValidInputLine.Match(line);
        if (!match.Success)
        {
            throw new InvalidOperationException();
        }
        int x = int.Parse(match.Groups["x"].Value);
        int y = int.Parse(match.Groups["y"].Value);
        int z = int.Parse(match.Groups["z"].Value);
        return new Vector3Int(x, y, z);
    }

    private static int AllSurfaces(int[,,] map)
    {
        int surfaces = 0;
        int size = map.GetLength(0);
        for (int z = 0; z < size; z++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (x > 0)
                    {
                        if (map[x, y, z] + map[x - 1, y, z] == 1)
                        {
                            surfaces++;
                        }
                    }
                    if (y > 0)
                    {
                        if (map[x, y, z] + map[x, y - 1, z] == 1)
                        {
                            surfaces++;
                        }
                    }
                    if (z > 0)
                    {
                        if (map[x, y, z] + map[x, y, z - 1] == 1)
                        {
                            surfaces++;
                        }
                    }
                }
            }
        }
        return surfaces;
    }
}
