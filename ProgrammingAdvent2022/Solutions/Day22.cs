// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

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

        return result.WriteAnswers(password, null);
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

    private static int FollowPath(string path, Tile start)
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
                    if (current.Neighbors[facing].Contents == '#')
                    {
                        break;
                    }
                    current = current.Neighbors[facing];
                }
                continue;
            }
            break;
        }
        return 1000 * current.Row + 4 * current.Column + facing;
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

        public Tile(char contents, int row, int column)
        {
            Contents = contents;
            Row = row;
            Column = column;
            Neighbors = [this, this, this, this];
        }
    }
}
