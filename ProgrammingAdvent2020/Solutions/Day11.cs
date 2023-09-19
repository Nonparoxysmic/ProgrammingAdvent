// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Numerics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day11 : Day
{
    private static readonly Regex _validLine = new("^[.L]{1,100}$");

    public override bool ValidateInput(string[] input, out string errorMessage)
    {
        if (input.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        foreach (string line in input)
        {
            if (!_validLine.IsMatch(line))
            {
                errorMessage = $"Invalid line \"{line.Left(20, true)}\" in input.";
                return false;
            }
            if (line.Length != input[0].Length)
            {
                errorMessage = "All lines in input must be the same length.";
                return false;
            }
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        char[,] map = input.ToCharArray2D();
        WaitingArea waitingArea = new(map);
        while (waitingArea.Update()) { }
        if (!waitingArea.Stabilized)
        {
            return output.WriteError("Seat states did not stabilize.");
        }

        return output.WriteAnswers(waitingArea.OccupiedSeatCount, null);
    }

    private class WaitingArea
    {
        public bool Stabilized { get; private set; }
        public int OccupiedSeatCount => CountOccupiedSeats();
        public int Time { get; private set; }

        private readonly int _timeout;
        private readonly int _width;
        private readonly int _height;
        private readonly Tile[,] _tiles;

        public WaitingArea(char[,] map, int timeout = 1024)
        {
            _timeout = timeout;
            _width = ((map.GetLength(0) - 1) / 8) + 3;
            _height = ((map.GetLength(1) - 1) / 8) + 3;
            _tiles = new Tile[_width, _height];
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (x == 0 || y == 0 || x == _width - 1 || y == _height - 1)
                    {
                        _tiles[x, y] = new(x, y);
                    }
                    else
                    {
                        _tiles[x, y] = new(x, y, map);
                    }
                }
            }
        }

        public bool Update()
        {
            if (Stabilized)
            {
                return false;
            }
            if (Time >= _timeout)
            {
                return false;
            }
            Time++;
            for (int y = 1; y < _height - 1; y++)
            {
                for (int x = 1; x < _width - 1; x++)
                {
                    _tiles[x, y].CountNeighbors(_tiles);
                }
            }
            bool seatsChanged = false;
            for (int y = 1; y < _height - 1; y++)
            {
                for (int x = 1; x < _width - 1; x++)
                {
                    seatsChanged |= _tiles[x, y].Update();
                }
            }
            if (!seatsChanged)
            {
                Stabilized = true;
            }
            return seatsChanged;
        }

        private int CountOccupiedSeats()
        {
            int sum = 0;
            for (int y = 1; y < _height - 1; y++)
            {
                for (int x = 1; x < _width - 1; x++)
                {
                    sum += BitOperations.PopCount(_tiles[x, y].OccupiedSeats);
                }
            }
            return sum;
        }

        //public void PrintMap()
        //{
        //    for (int y = 1; y < _height - 1; y++)
        //    {
        //        for (int dy = 0; dy < 8; dy++)
        //        {
        //            for (int x = 1; x < _width - 1; x++)
        //            {
        //                for (int dx = 0; dx < 8; dx++)
        //                {
        //                    if (((_tiles[x, y].OccupiedSeats >> (dy * 8 + dx)) & 1UL) == 1)
        //                    {
        //                        Console.Write('#');
        //                    }
        //                    else if (((_tiles[x, y].Seats >> (dy * 8 + dx)) & 1UL) == 1)
        //                    {
        //                        Console.Write('L');
        //                    }
        //                    else
        //                    {
        //                        Console.Write('.');
        //                    }
        //                }
        //            }
        //            Console.WriteLine();
        //        }
        //    }
        //}

        private class Tile
        {
            //   0  1  2  3  4  5  6  7
            //   8  9 10 11 12 13 14 15
            //  16 17 18 19 20 21 22 23
            //  24 25 26 27 28 29 30 31
            //  32 33 34 35 36 37 38 39
            //  40 41 42 43 44 45 46 47
            //  48 49 50 51 52 53 54 55
            //  56 57 58 59 60 61 62 63

            private static readonly ulong T = 0xFF;
            private static readonly ulong B = T << 56;
            private static readonly ulong L = 0x0101_0101_0101_0101;
            private static readonly ulong R = 0x8080_8080_8080_8080;

            public int X { get; private set; }
            public int Y { get; private set; }
            public ulong Seats { get; private set; }
            public ulong OccupiedSeats { get; private set; }

            private ulong _previousOccupiedSeats;
            private ulong _neighbors0, _neighbors1, _neighbors2;

            public Tile(int x, int y)
            {
                X = x;
                Y = y;
            }

            public Tile(int x, int y, char[,] map)
            {
                X = x;
                Y = y;
                int mapMinX = (X - 1) * 8;
                int mapMaxX = Math.Min(mapMinX + 7, map.GetLength(0) - 1);
                int mapMinY = (Y - 1) * 8;
                int mapMaxY = Math.Min(mapMinY + 7, map.GetLength(1) - 1);
                for (int j = mapMinY; j <= mapMaxY; j++)
                {
                    for (int i = mapMinX; i <= mapMaxX; i++)
                    {
                        if (map[i, j] == '.')
                        {
                            continue;
                        }
                        Seats |= 1UL << ((j - mapMinY) * 8 + i - mapMinX);
                    }
                }
            }

            public void CountNeighbors(Tile[,] tiles)
            {
                //  00 10 20
                //  01    21
                //  02 12 22

                ulong n00 = tiles[X - 1, Y - 1].OccupiedSeats & B & R;
                ulong n10 = tiles[X    , Y - 1].OccupiedSeats & B;
                ulong n20 = tiles[X + 1, Y - 1].OccupiedSeats & B & L;
                ulong n01 = tiles[X - 1, Y    ].OccupiedSeats & R;
                ulong n21 = tiles[X + 1, Y    ].OccupiedSeats & L;
                ulong n02 = tiles[X - 1, Y + 1].OccupiedSeats & T & R;
                ulong n12 = tiles[X    , Y + 1].OccupiedSeats & T;
                ulong n22 = tiles[X + 1, Y + 1].OccupiedSeats & T & L;

                ulong s11 = OccupiedSeats;
                ulong s10 = (s11 << 8) | (n10 >> 56);
                ulong s12 = (s11 >> 8) | (n12 << 56);
                ulong s01 = ((s11 & ~R) << 1) | (n01 >> 7);
                ulong s21 = ((s11 & ~L) >> 1) | (n21 << 7);
                ulong s20 = ((s11 & ~L) << 7) | (n10 >> 57) | (n21 << 15) | (n20 >> 49);
                ulong s02 = ((s11 & ~R) >> 7) | (n12 << 57) | (n01 >> 15) | (n02 << 49);
                ulong s00 = ((s11 & ~R) << 9) | ((n10 & ~R) >> 55) | (n01 << 1) | (n00 >> 63);
                ulong s22 = ((s11 & ~L) >> 9) | ((n12 & ~L) << 55) | (n21 >> 1) | (n22 << 63);

                _neighbors0 = 0;
                _neighbors1 = 0;
                _neighbors2 = 0;

                AddNeighbors(s00);
                AddNeighbors(s01);
                AddNeighbors(s02);
                AddNeighbors(s10);
                AddNeighbors(s12);
                AddNeighbors(s20);
                AddNeighbors(s21);
                AddNeighbors(s22);
            }

            public bool Update()
            {
                _previousOccupiedSeats = OccupiedSeats;

                ulong newOccupied = Seats & ~OccupiedSeats & ~_neighbors0 & ~_neighbors1 & ~_neighbors2;
                ulong seatsToEmpty = OccupiedSeats & _neighbors2;
                OccupiedSeats = (OccupiedSeats | newOccupied) & ~seatsToEmpty;

                return OccupiedSeats != _previousOccupiedSeats;
            }

            private void AddNeighbors(ulong seats)
            {
                ulong carry0 = _neighbors0 & seats;
                ulong carry1 = _neighbors1 & carry0;
                _neighbors0 ^= seats;
                _neighbors1 ^= carry0;
                _neighbors2 |= carry1;
            }
        }
    }
}
