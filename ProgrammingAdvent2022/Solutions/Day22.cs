// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Text;
using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day22 : Day
{
    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        if (!TryParseInput(input, out Section?[,] sections, out string path))
        {
            return result.WriteError("Could not parse input.");
        }

        Tile start = StartTile(sections);
        BuildGraph(sections);
        int password = FollowPath(path, start);
        BuildCube(sections);
        int actualPassword = FollowPath(path, start);

        return result.WriteAnswers(password, actualPassword);
    }

    private static bool TryParseInput(string[] input, out Section?[,] sections, out string path)
    {
        path = input[^1];
        if (path.Length == 0)
        {
            sections = new Section[0, 0];
            return false;
        }
        int height = input.Length - 2;
        int width = input[0..^2].Select(x => x.Length).Max();
        int sectionSize;
        if (Math.Max(width, height) * 3 == Math.Min(width, height) * 4)
        {
            sectionSize = Math.Max(width, height) / 4;
        }
        else if (Math.Max(width, height) * 2 == Math.Min(width, height) * 5)
        {
            sectionSize = Math.Max(width, height) / 5;
        }
        else
        {
            sections = new Section[0, 0];
            return false;
        }
        sections = new Section[width / sectionSize, height / sectionSize];
        for (int y = 0; y < height / sectionSize; y++)
        {
            for (int x = 0; x < width / sectionSize; x++)
            {
                sections[x, y] = ReadSection(input, x, y, sectionSize);
            }
        }
        int validSections = sections.Cast<Section?>().Where(s => s is not null).Count();
        if (validSections != 6)
        {
            return false;
        }
        return true;
    }

    private static Section? ReadSection(string[] input, int sectionX, int sectionY, int sectionSize)
    {
        char[,] map = new char[sectionSize, sectionSize];
        for (int y = 0; y < sectionSize; y++)
        {
            if (input[y + sectionY * sectionSize].Length < (sectionX + 1) * sectionSize)
            {
                return null;
            }
            for (int x = 0; x < sectionSize; x++)
            {
                map[x, y] = input[y + sectionY * sectionSize][x + sectionX * sectionSize];
                if (map[x, y] == ' ')
                {
                    return null;
                }
            }
        }
        return new Section(map, sectionX * sectionSize, sectionY * sectionSize);
    }

    private static void BuildGraph(Section?[,] sections)
    {
        for (int y = 0; y < sections.GetLength(1); y++)
        {
            for (int x = 0; x < sections.GetLength(0); x++)
            {
                Section? current = sections[x, y];
                if (current is not null)
                {
                    int size = current.Size;
                    Section up = NextSection(sections, x, y, 0, -1);
                    Section dn = NextSection(sections, x, y, 0, 1);
                    Section lf = NextSection(sections, x, y, -1, 0);
                    Section rt = NextSection(sections, x, y, 1, 0);
                    for (int i = 0; i < size; i++)
                    {
                        current.Tiles[i, 0].Neighbors[3] = up.Tiles[i, size - 1];
                        up.Tiles[i, size - 1].Neighbors[1] = current.Tiles[i, 0];

                        current.Tiles[i, size - 1].Neighbors[1] = dn.Tiles[i, 0];
                        dn.Tiles[i, 0].Neighbors[3] = current.Tiles[i, size - 1];

                        current.Tiles[0, i].Neighbors[2] = lf.Tiles[size - 1, i];
                        lf.Tiles[size - 1, i].Neighbors[0] = current.Tiles[0, i];

                        current.Tiles[size - 1, i].Neighbors[0] = rt.Tiles[0, i];
                        rt.Tiles[0, i].Neighbors[2] = current.Tiles[size - 1, i];
                    }
                }
            }
        }
    }

    private static Tile StartTile(Section?[,] sections)
    {
        for (int y = 0; y < sections.GetLength(1); y++)
        {
            for (int x = 0; x < sections.GetLength(0); x++)
            {
                Section? current = sections[x, y];
                if (current is not null)
                {
                    return current.Tiles[0, 0];
                }
            }
        }
        throw new InvalidOperationException();
    }

    private static Section NextSection(Section?[,] sections, int xPos, int yPos, int xDir, int yDir)
    {
        int x = xPos, y = yPos;
        Section? current;
        while (true)
        {
            x += xDir;
            y += yDir;
            x = (x + sections.GetLength(0)) % sections.GetLength(0);
            y = (y + sections.GetLength(1)) % sections.GetLength(1);
            current = sections[x, y];
            if (current is not null)
            {
                return current;
            }
        }
    }

    private static int FollowPath(string path, Tile start, bool ignoreWalls = false)
    {
        Tile current = start;
        int facing = 0;
        int i = 0;
        while (true)
        {
            if (i >= path.Length)
            {
                break;
            }
            char c = path[i++];
            if (c == 'L')
            {
                facing--;
                facing = (facing + 4) % 4;
                continue;
            }
            if (c == 'R')
            {
                facing++;
                facing %= 4;
                continue;
            }
            if ('0' <= c && c <= '9')
            {
                int steps = c - '0';
                while (i < path.Length && '0' <= path[i] && path[i] <= '9')
                {
                    steps *= 10;
                    steps += path[i++] - '0';
                }
                for (int j = 0; j < steps; j++)
                {
                    if (current.Neighbors[facing].Contents == '#' && !ignoreWalls)
                    {
                        break;
                    }
                    int newFacing = (facing + current.NeighborRotation[facing]) % 4;
                    current = current.Neighbors[facing];
                    facing = newFacing;
                }
                continue;
            }
            break;
        }
        return 1000 * current.Row + 4 * current.Column + facing;
    }

    private static void BuildCube(Section?[,] sectionArray)
    {
        DisconnectEdges(sectionArray);
        IEnumerable<Section> sections = sectionArray.Cast<Section>().Where(s => s is not null);
        bool allConnected = false;
        while (!allConnected)
        {
            allConnected = true;
            foreach (Section section in sections)
            {
                Tile topLeft = section.Tiles[0, 0];
                if (HasAdjacentSections(topLeft, 2))
                {
                    ConnectCorner(topLeft, 2, section.Size);
                }
                else
                {
                    allConnected = false;
                }

                Tile topRight = section.Tiles[section.Size - 1, 0];
                if (HasAdjacentSections(topRight, 3))
                {
                    ConnectCorner(topRight, 3, section.Size);
                }
                else
                {
                    allConnected = false;
                }

                Tile bottomLeft = section.Tiles[0, section.Size - 1];
                if (HasAdjacentSections(bottomLeft, 1))
                {
                    ConnectCorner(bottomLeft, 1, section.Size);
                }
                else
                {
                    allConnected = false;
                }

                Tile bottomRight = section.Tiles[section.Size - 1, section.Size - 1];
                if (HasAdjacentSections(bottomRight, 0))
                {
                    ConnectCorner(bottomRight, 0, section.Size);
                }
                else
                {
                    allConnected = false;
                }
            }
        }
    }

    private static bool HasAdjacentSections(Tile corner, int direction)
    {
        return corner.Neighbors[direction] != corner && corner.Neighbors[(direction + 1) % 4] != corner;
    }

    private static void ConnectCorner(Tile corner, int direction1, int size)
    {
        if (CornerIsConnected(corner, direction1))
        {
            return;
        }
        int direction2 = (direction1 + 1) % 4;
        Tile left = corner.Neighbors[direction1];
        Tile right = corner.Neighbors[direction2];
        direction1 = UpdateFacing(corner, direction1);
        direction2 = UpdateFacing(corner, direction2);
        int neighbor1 = (direction1 + 1) % 4;
        int neighbor2 = (direction2 + 3) % 4;
        for (int i = 0; i < size; i++)
        {
            left.Neighbors[neighbor1] = right;
            left.NeighborRotation[neighbor1] = (neighbor2 - neighbor1 + 6) % 4;
            right.Neighbors[neighbor2] = left;
            right.NeighborRotation[neighbor2] = (neighbor1 - neighbor2 + 6) % 4;
            left = left.Neighbors[direction1];
            right = right.Neighbors[direction2];
        }
    }

    private static int UpdateFacing(Tile corner, int direction)
    {
        Tile neighbor = corner.Neighbors[direction];
        char[] path = Enumerable.Repeat('R', direction).Append('1').ToArray();
        int result = FollowPath(string.Join(null, path), corner, true);
        return (result - 1000 * neighbor.Row - 4 * neighbor.Column) % 4;
    }

    private static bool CornerIsConnected(Tile corner, int direction)
    {
        StringBuilder sb = new();
        for (int i = 0; i < direction; i++)
        {
            sb.Append('R');
        }
        sb.Append("1R1R1");
        int finalTurns = (direction + 3) % 4;
        for (int i = 0; i < finalTurns; i++)
        {
            sb.Append('L');
        }
        return FollowPath(sb.ToString(), corner, true) == 1000 * corner.Row + 4 * corner.Column;
    }

    private static void DisconnectEdges(Section?[,] sections)
    {
        int width = sections.GetLength(0);
        int height = sections.GetLength(1);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Section? current = sections[x, y];
                if (current is null)
                {
                    continue;
                }
                if (x == 0 || sections[x - 1, y] == null)
                {
                    // disconnect left edge
                    for (int i = 0; i < current.Size; i++)
                    {
                        current.Tiles[0, i].DisconnectNeighbor(2);
                    }
                }
                if (y == 0 || sections[x, y - 1] == null)
                {
                    // disconnect top edge
                    for (int i = 0; i < current.Size; i++)
                    {
                        current.Tiles[i, 0].DisconnectNeighbor(3);
                    }
                }
                if (x == width - 1 || sections[x + 1, y] == null)
                {
                    // disconnect right edge
                    for (int i = 0; i < current.Size; i++)
                    {
                        current.Tiles[current.Size - 1, i].DisconnectNeighbor(0);
                    }
                }
                if (y == height - 1 || sections[x, y + 1] == null)
                {
                    // disconnect bottom edge
                    for (int i = 0; i < current.Size; i++)
                    {
                        current.Tiles[i, current.Size - 1].DisconnectNeighbor(1);
                    }
                }
            }
        }
    }

    private class Section
    {
        public Tile[,] Tiles;
        public int Size;

        public Section(char[,] map, int xPos, int yPos)
        {
            Size = map.GetLength(0);
            Tiles = new Tile[Size, Size];
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    Tiles[x, y] = new Tile(map[x, y], y + yPos + 1, x + xPos + 1);
                }
            }
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x > 0)
                    {
                        Tiles[x, y].Neighbors[2] = Tiles[x - 1, y];
                        Tiles[x - 1, y].Neighbors[0] = Tiles[x, y];
                    }
                    if (y > 0)
                    {
                        Tiles[x, y].Neighbors[3] = Tiles[x, y - 1];
                        Tiles[x, y - 1].Neighbors[1] = Tiles[x, y];
                    }
                }
            }
        }
    }

    private class Tile
    {
        public char Contents { get; }
        public int Row { get; }
        public int Column { get; }
        public Tile[] Neighbors { get; }
        public int[] NeighborRotation { get; }

        public Tile(char contents, int row, int column)
        {
            Contents = contents;
            Row = row;
            Column = column;
            Neighbors = [this, this, this, this];
            NeighborRotation = [0, 0, 0, 0];
        }

        public void DisconnectNeighbor(int index)
        {
            Neighbors[index] = this;
        }
    }
}
