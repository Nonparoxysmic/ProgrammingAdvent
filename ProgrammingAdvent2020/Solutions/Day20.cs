// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day20 : Day
{
    private static readonly int[] _monsterPattern = new int[] { -1, 1, 6, 7, 12, 13 };
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

        List<Tile> tiles = ReadInput(input);
        MatchEdges(tiles);

        if (tiles.Any(tile => tile.AdjacentTiles < 2 || tile.AdjacentTiles > 4))
        {
            return output.WriteError("Found tiles with an invalid number of neighbors.");
        }
        int edgeTileCount = tiles.Count(t => t.AdjacentTiles == 3);
        int expectedEdges = ((int)Math.Sqrt(tiles.Count) - 2) * 4;
        if (edgeTileCount != expectedEdges)
        {
            return output.WriteError($"Did not find the correct number of edge pieces ({expectedEdges}).");
        }

        IEnumerable<Tile> cornerTiles = tiles.Where(t => t.AdjacentTiles == 2);
        if (cornerTiles.Count() != 4)
        {
            return output.WriteError("Did not find four corner pieces.");
        }
        long partOneAnswer = cornerTiles.Aggregate(1L, (a, tile) => a * tile.ID);

        char[,] image = AssembleImage(tiles);
        List<Vector2Int> potentialMonsters = GetPointsOfInterest(image);
        Vector2Int orientation = CorrectOrientation(image, potentialMonsters);
        if (orientation != Vector2Int.Zero)
        {
            foreach (Vector2Int point in potentialMonsters)
            {
                if (IsSeaMonster(point, image, orientation))
                {
                    DrawSeaMonster(point, image, orientation);
                }
            }
        }
        int partTwoAnswer = 0;
        for (int y = 1; y < image.GetLength(1) - 1; y++)
        {
            for (int x = 1; x < image.GetLength(0) - 1; x++)
            {
                partTwoAnswer += image[x, y] == '#' ? 1 : 0;
            }
        }

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static List<Tile> ReadInput(string[] input)
    {
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
        return tiles;
    }

    private static void MatchEdges(List<Tile> tiles)
    {
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
            tile.T = edgeCounts[tile.T] == 1 ? 0 : tile.T;
            tile.B = edgeCounts[tile.B] == 1 ? 0 : tile.B;
            tile.L = edgeCounts[tile.L] == 1 ? 0 : tile.L;
            tile.R = edgeCounts[tile.R] == 1 ? 0 : tile.R;
        }
    }

    private static char[,] AssembleImage(List<Tile> tiles)
    {
        List<Tile> cornerTiles = tiles.Where(t => t.AdjacentTiles == 2).ToList();
        List<Tile> edgeTiles = tiles.Where(t => t.AdjacentTiles == 3).ToList();
        List<Tile> middleTiles = tiles.Where(t => t.AdjacentTiles == 4).ToList();

        int size = (int)Math.Sqrt(tiles.Count);
        Tile[,] map = new Tile[size, size];
        Tile current;
        uint top, left;

        // Top row
        current = cornerTiles.First();
        map[0, 0] = current.Transform(0, 0);
        cornerTiles.Remove(current);
        for (int x = 1; x < size - 1; x++)
        {
            left = map[x - 1, 0].R;
            current = edgeTiles.First(t => t.ContainsEdge(left));
            map[x, 0] = current.Transform(0, left);
            edgeTiles.Remove(current);
        }
        left = map[size - 2, 0].R;
        current = cornerTiles.First(t => t.ContainsEdge(left));
        map[size - 1, 0] = current.Transform(0, left);
        cornerTiles.Remove(current);
        // Middle rows
        for (int y = 1; y < size - 1; y++)
        {
            top = map[0, y - 1].B;
            current = edgeTiles.First(t => t.ContainsEdge(top));
            map[0, y] = current.Transform(top, 0);
            edgeTiles.Remove(current);
            for (int x = 1; x < size - 1; x++)
            {
                top = map[x, y - 1].B;
                left = map[x - 1, y].R;
                current = middleTiles.First(t => t.ContainsEdge(top) && t.ContainsEdge(left));
                map[x, y] = current.Transform(top, left);
                middleTiles.Remove(current);
            }
            top = map[size - 1, y - 1].B;
            left = map[size - 2, y].R;
            current = edgeTiles.First(t => t.ContainsEdge(top) && t.ContainsEdge(left));
            map[size - 1, y] = current.Transform(top, left);
            edgeTiles.Remove(current);
        }
        // Bottom row
        top = map[0, size - 2].B;
        current = cornerTiles.First(t => t.ContainsEdge(top));
        map[0, size - 1] = current.Transform(top, 0);
        cornerTiles.Remove(current);
        for (int x = 1; x < size - 1; x++)
        {
            top = map[x, size - 2].B;
            left = map[x - 1, size - 1].R;
            current = edgeTiles.First(t => t.ContainsEdge(top) && t.ContainsEdge(left));
            map[x, size - 1] = current.Transform(top, left);
            edgeTiles.Remove(current);
        }
        top = map[size - 1, size - 2].B;
        left = map[size - 2, size - 1].R;
        current = cornerTiles.First(t => t.ContainsEdge(top) && t.ContainsEdge(left));
        map[size - 1, size - 1] = current.Transform(top, left);
        cornerTiles.Remove(current);

        char[,] output = new char[size * 8 + 2, size * 8 + 2];
        output.Fill('█');
        for (int mapRow = 0; mapRow < size; mapRow++)
        {
            for (int mapCol = 0; mapCol < size; mapCol++)
            {
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        output[mapRow * 8 + x + 1, mapCol * 8 + y + 1] = map[mapRow, mapCol].Pixels[x, y];
                    }
                }
            }
        }
        return output;
    }

    private static List<Vector2Int> GetPointsOfInterest(char[,] image)
    {
        List<Vector2Int> poi = new();
        for (int y = 2; y < image.GetLength(1) - 2; y++)
        {
            for (int x = 2; x < image.GetLength(0) - 2; x++)
            {
                if (image[x, y] == '.') { continue; }
                int neighbors = 0;
                if (image[x - 1, y] == '#') { neighbors++; }
                if (image[x, y - 1] == '#') { neighbors++; }
                if (image[x + 1, y] == '#') { neighbors++; }
                if (image[x, y + 1] == '#') { neighbors++; }
                if (neighbors > 2)
                {
                    poi.Add(new Vector2Int(x, y));
                }
            }
        }
        return poi;
    }

    private static Vector2Int CorrectOrientation(char[,] image, List<Vector2Int> potentialMonsters)
    {
        Vector2Int[] possibleOrientations = new Vector2Int[]
        {
            new Vector2Int(-2, 1),
            new Vector2Int(-1, 2),
            new Vector2Int(1, 2),
            new Vector2Int(2, 1),
            new Vector2Int(2, -1),
            new Vector2Int(1, -2),
            new Vector2Int(-1, -2),
            new Vector2Int(-2, -1),
        };
        foreach (Vector2Int point in potentialMonsters)
        {
            foreach (Vector2Int orientation in possibleOrientations)
            {
                if (IsSeaMonster(point, image, orientation))
                {
                    return orientation;
                }
            }
        }
        return Vector2Int.Zero;
    }

    private static bool IsSeaMonster(Vector2Int point, char[,] image, Vector2Int orientation)
    {
        Vector2Int tailDir = orientation / 2;
        Vector2Int tail = point + 18 * tailDir;
        if (tail.X < 1 || tail.X >= image.GetLength(0) - 1 ||
            tail.Y < 1 || tail.Y >= image.GetLength(1) - 1)
        {
            return false;
        }
        if (image[tail.X, tail.Y] != '#')
        {
            return false;
        }
        foreach (int i in _monsterPattern)
        {
            if (image[point.X + i * tailDir.X, point.Y + i * tailDir.Y] != '#')
            {
                return false;
            }
        }
        Vector2Int crown = point + 2 * tailDir - orientation;
        if (image[crown.X, crown.Y] != '#')
        {
            return false;
        }
        for (int i = 0; i < 16; i += 3)
        {
            Vector2Int surface = point + orientation + i * tailDir;
            if (image[surface.X, surface.Y] != '#')
            {
                return false;
            }
        }
        return true;
    }

    private static void DrawSeaMonster(Vector2Int point, char[,] image, Vector2Int orientation)
    {
        Vector2Int tailDir = orientation / 2;
        Vector2Int tail = point + 18 * tailDir;
        image[tail.X, tail.Y] = 'O';
        foreach (int i in _monsterPattern)
        {
            image[point.X + i * tailDir.X, point.Y + i * tailDir.Y] = 'O';
        }
        Vector2Int crown = point + 2 * tailDir - orientation;
        image[crown.X, crown.Y] = 'O';
        for (int i = 0; i < 16; i += 3)
        {
            Vector2Int surface = point + orientation + i * tailDir;
            image[surface.X, surface.Y] = 'O';
        }
        image[point.X, point.Y] = 'O';
    }

    private class Tile
    {
        public int ID { get; private set; }
        public int AdjacentTiles { get; set; }

        public uint T { get; set; }
        public uint B { get; set; }
        public uint L { get; set; }
        public uint R { get; set; }

        public char[,] Pixels { get; set; }

        public Tile(int idNumber, string[] data)
        {
            ID = idNumber;

            T = EncodeEdge(data[0].ToCharArray());
            B = EncodeEdge(data[9].ToCharArray());
            L = EncodeEdge(data.Select(s => s[0]).ToArray());
            R = EncodeEdge(data.Select(s => s[9]).ToArray());

            Pixels = new char[8, 8];
            for (int y = 1; y < 9; y++)
            {
                for (int x = 1; x < 9; x++)
                {
                    Pixels[x - 1, y - 1] = data[y][x];
                }
            }
        }

        public Tile(Tile tileToCopy)
        {
            ID = tileToCopy.ID;
            AdjacentTiles = tileToCopy.AdjacentTiles;
            T = tileToCopy.T;
            B = tileToCopy.B;
            L = tileToCopy.L;
            R = tileToCopy.R;
            Pixels = new char[8, 8];
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Pixels[x, y] = tileToCopy.Pixels[x, y];
                }
            }
        }

        public Tile Transform(uint top, uint left)
        {
            if (T == top && L == left)
            {
                return this;
            }
            Tile output = new(this);
            Func<int, int, Vector2Int> transform;
            if (T == top && R == left)
            {
                // Flip horizontally
                transform = (x, y) => new Vector2Int(7 - x, y);
                (output.L, output.R) = (R, L);
            }
            else if (B == top && L == left)
            {
                // Flip vertically
                transform = (x, y) => new Vector2Int(x, 7 - y);
                (output.T, output.B) = (B, T);
            }
            else if (B == top && R == left)
            {
                // Flip both
                transform = (x, y) => new Vector2Int(7 - x, 7 - y);
                (output.L, output.R) = (R, L);
                (output.T, output.B) = (B, T);
            }
            else if (L == top && T == left)
            {
                // Swap x and y
                transform = (x, y) => new Vector2Int(y, x);
                (output.T, output.L) = (L, T);
                (output.B, output.R) = (R, B);
            }
            else if (L == top && B == left)
            {
                // 90 degree rotation
                transform = (x, y) => new Vector2Int(y, 7 - x);
                (output.T, output.B, output.L, output.R) = (L, R, B, T);
            }
            else if (R == top && T == left)
            {
                // 90 degree rotation
                transform = (x, y) => new Vector2Int(7 - y, x);
                (output.T, output.B, output.L, output.R) = (R, L, T, B);

            }
            else // R == top && B == left
            {
                transform = (x, y) => new Vector2Int(7 - y, 7 - x);
                (output.T, output.B, output.L, output.R) = (R, L, B, T);
            }

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Vector2Int source = transform(x, y);
                    output.Pixels[x, y] = Pixels[source.X, source.Y];
                }
            }
            return output;
        }

        public bool ContainsEdge(uint edge)
        {
            return edge == T || edge == B || edge == L || edge == R;
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
