// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day20 : Day
{
    private static readonly Regex _titleLine = new("^Tile (?<tileID>[0-9]{4}):$");
    private static readonly Regex _dataLine = new("^[.#]{10}$");

    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        HashSet<int> tileIDs = new();
        for (int i = 0; i < input.Length - 10; i += 12)
        {
            Match titleMatch = _titleLine.Match(input[i]);
            if (!titleMatch.Success)
            {
                errorMessage = $"Expected valid tile ID number, found \"{input[i].Left(20, true)}\".";
                return false;
            }
            int tileID = int.Parse(titleMatch.Groups["tileID"].Value);
            if (!tileIDs.Add(tileID))
            {
                errorMessage = $"Input contains duplicate tile ID number {tileID}.";
                return false;
            }
            for (int j = i + 1; j < i + 11; j++)
            {
                Match dataMatch = _dataLine.Match(input[j]);
                if (!dataMatch.Success)
                {
                    errorMessage = $"Expected valid camera data, found \"{input[j].Left(20, true)}\".";
                    return false;
                }
            }
        }
        if (tileIDs.Count < 4 || Math.Sqrt(tileIDs.Count) % 1 != 0)
        {
            errorMessage = "Number of tiles is not a perfect square greater than one.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        List<Tile> tiles = new();
        for (int i = 0; i < input.Length - 10; i += 12)
        {
            Match titleMatch = _titleLine.Match(input[i]);
            int tileID = int.Parse(titleMatch.Groups["tileID"].Value);
            string[] data = new string[10];
            for (int j = 0; j < 10; j++)
            {
                data[j] = input[i + j + 1];
            }
            tiles.Add(new Tile(tileID, data));
        }

        Dictionary<uint, int> edgeCounts = new();
        foreach (Tile tile in tiles)
        {
            edgeCounts[tile.T] = edgeCounts.GetValueOrDefault(tile.T) + 1;
            edgeCounts[tile.B] = edgeCounts.GetValueOrDefault(tile.B) + 1;
            edgeCounts[tile.L] = edgeCounts.GetValueOrDefault(tile.L) + 1;
            edgeCounts[tile.R] = edgeCounts.GetValueOrDefault(tile.R) + 1;
        }
        foreach (Tile tile in tiles)
        {
            tile.AdjacentTiles += edgeCounts[tile.T] + edgeCounts[tile.B]
                + edgeCounts[tile.L] + edgeCounts[tile.R] - 4;
        }
        IEnumerable<Tile> cornerTiles = tiles.Where(t => t.AdjacentTiles == 2);
        if (cornerTiles.Count() != 4)
        {
            return output.WriteError("Did not find four corner pieces.");
        }
        long partOneAnswer = cornerTiles.Aggregate(1L, (a, tile) => a * tile.ID);

        return output.WriteAnswers(partOneAnswer, null);
    }

    private class Tile
    {
        public int ID { get; private set; }
        public int AdjacentTiles { get; set; }

        public uint T { get; private set; }
        public uint B { get; private set; }
        public uint L { get; private set; }
        public uint R { get; private set; }

        public Tile(int idNumber, string[] data)
        {
            ID = idNumber;

            T = EncodeEdge(data[0].ToCharArray());
            B = EncodeEdge(data[9].ToCharArray());
            L = EncodeEdge(data.Select(s => s[0]).ToArray());
            R = EncodeEdge(data.Select(s => s[9]).ToArray());
        }

        private static uint EncodeEdge(char[] edge)
        {
            IEnumerable<char> bits = edge.Select(c => c == '.' ? '0' : '1');
            string binary = string.Join(null, bits);
            uint code = Convert.ToUInt32(binary, 2);
            uint reverseCode = 0;
            uint bitsToReverse = code;
            for (int i = 0; i < 10; i++)
            {
                reverseCode <<= 1;
                reverseCode |= bitsToReverse & 1;
                bitsToReverse >>= 1;
            }
            return Math.Min(code, reverseCode);
        }
    }
}
