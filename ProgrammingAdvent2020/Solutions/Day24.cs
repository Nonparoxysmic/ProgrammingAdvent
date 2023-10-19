// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day24 : Day
{
    //  Coordinate System:
    //
    //           (-r)
    //        NW      NE
    //  (+s)    \    /    (+q)
    //           \  /
    //     W ----    ---- E
    //           /  \
    //  (-q)    /    \    (-s)
    //        SW      SE
    //           (+r)
    //
    //  q + r + s = 0
    //
    //  https://www.redblobgames.com/grids/hexagons/

    private static readonly Dictionary<string, Vector2Int> _directions = new()
    {
        //                       q   r
        {  "e",  new Vector2Int( 1,  0) },
        { "se",  new Vector2Int( 0,  1) },
        { "sw",  new Vector2Int(-1,  1) },
        {  "w",  new Vector2Int(-1,  0) },
        { "nw",  new Vector2Int( 0, -1) },
        { "ne",  new Vector2Int( 1, -1) }
    };

    private static readonly Regex _validLine = new("^(e|se|sw|w|nw|ne)+$");

    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in input)
        {
            if (line.Length == 0)
            {
                break;
            }
            if (line.Length > 64)
            {
                errorMessage = $"Input line \"{line.Left(20, true)}\" is too long.";
                return false;
            }
            if (!_validLine.IsMatch(line))
            {
                errorMessage = $"Invalid line \"{line.Left(20, true)}\" in input.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        HashSet<Vector2Int> blackTiles = new();
        foreach (string line in input)
        {
            Match match = _validLine.Match(line);
            if (match.Success)
            {
                IEnumerable<string> directions = match.Groups[1].Captures.Select(c => c.Value);
                Vector2Int position = Vector2Int.Zero;
                foreach (string direction in directions)
                {
                    position += _directions[direction];
                }
                if (blackTiles.Contains(position))
                {
                    blackTiles.Remove(position);
                }
                else
                {
                    blackTiles.Add(position);
                }
            }
        }

        return output.WriteAnswers(blackTiles.Count, null);
    }
}
