// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Numerics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day24 : Day
{
    private static readonly Regex _validLine = new("^[#.]{5}$");
    private static readonly uint _maskGrid = 0x1FFFFFF;
    private static readonly uint _maskL = 0b01111_01111_01111_01111_01111;
    private static readonly uint _maskR = 0b11110_11110_11110_11110_11110;

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        if (inputLines.Length != 5)
        {
            errorMessage = "Input has the wrong number of lines.";
            return false;
        }
        foreach (string line in inputLines)
        {
            if (line.Length == 0)
            {
                errorMessage = "Input contains an empty line.";
                return false;
            }
            if (!_validLine.IsMatch(line))
            {
                errorMessage = $"Input line \"{line.Left(20, true)}\" is not valid.";
                return false;
            }
        }
        if (inputLines[2][2] != '.')
        {
            errorMessage = "Middle tile is not empty.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        uint initialState = ReadInput(inputLines);
        uint currentState = initialState;
        HashSet<uint> seenStates = new();
        while (seenStates.Add(currentState))
        {
            currentState = NextState(currentState);
        }
        uint partOneAnswer = currentState;

        int steps = 200;
        if (exampleModifier is not null && int.TryParse(exampleModifier, out int exampleSteps))
        {
            steps = exampleSteps;
        }
        BugMap map = new(initialState);
        map.Simulate(steps);
        int partTwoAnswer = map.CountBugs();

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static uint ReadInput(string[] inputLines)
    {
        uint output = 0;
        int i = 0;
        for (int y = 0; y < inputLines.Length; y++)
        {
            for (int x = 0; x < inputLines[y].Length; x++)
            {
                if (inputLines[y][x] == '#')
                {
                    output |= 1U << i;
                }
                i++;
            }
        }
        return output;
    }

    private static uint NextState(uint currentState)
    {
        //   0  1  2  3  4
        //   5  6  7  8  9
        //  10 11 12 13 14
        //  15 16 17 18 19
        //  20 21 22 23 24

        uint up = currentState >> 5;
        uint down = (currentState << 5) & _maskGrid;
        uint left = (currentState >> 1) & _maskL;
        uint right = (currentState << 1) & _maskR;

        uint neighbors0 = 0;
        uint neighbors1 = 0;
        uint neighbors2 = 0;
        uint carry0 = neighbors0 & up;
        uint carry1 = neighbors1 & carry0;
        neighbors0 ^= up;
        neighbors1 ^= carry0;
        neighbors2 |= carry1;
        carry0 = neighbors0 & down;
        carry1 = neighbors1 & carry0;
        neighbors0 ^= down;
        neighbors1 ^= carry0;
        neighbors2 |= carry1;
        carry0 = neighbors0 & left;
        carry1 = neighbors1 & carry0;
        neighbors0 ^= left;
        neighbors1 ^= carry0;
        neighbors2 |= carry1;
        carry0 = neighbors0 & right;
        carry1 = neighbors1 & carry0;
        neighbors0 ^= right;
        neighbors1 ^= carry0;
        neighbors2 |= carry1;

        uint survivingBugs = currentState & neighbors0 & ~neighbors1 & ~neighbors2;
        uint newBugs = ~currentState & (neighbors0 ^ neighbors1) & ~neighbors2;
        return survivingBugs | newBugs;
    }

    private class BugMap
    {
        private readonly Dictionary<int, MapLayer> _layers = new();
        private int _lowestLayer;
        private int _highestLayer;

        public BugMap(uint initialState)
        {
            _layers.Add(-2, new MapLayer());
            _layers.Add(-1, new MapLayer());
            _layers.Add(0, new MapLayer(initialState));
            _layers.Add(1, new MapLayer());
            _layers.Add(2, new MapLayer());
            _lowestLayer = -2;
            _highestLayer = 2;
        }

        public void Simulate(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                ExpandMap();
                CalculateNeighbors();
                UpdateBugs();
            }
        }

        public int CountBugs()
        {
            int sum = 0;
            for (int i = _lowestLayer; i <= _highestLayer; i++)
            {
                sum += _layers[i].CountBugs();
            }
            return sum;
        }

        private void ExpandMap()
        {
            if (_layers[_lowestLayer].State > 0)
            {
                _layers.Add(_lowestLayer - 2, new MapLayer());
                _layers.Add(_lowestLayer - 1, new MapLayer());
                _lowestLayer -= 2;
            }
            else if (_layers[_lowestLayer + 1].State > 0)
            {
                _layers.Add(_lowestLayer - 1, new MapLayer());
                _lowestLayer--;
            }
            if (_layers[_highestLayer].State > 0)
            {
                _layers.Add(_highestLayer + 2, new MapLayer());
                _layers.Add(_highestLayer + 1, new MapLayer());
                _highestLayer += 2;
            }
            else if (_layers[_highestLayer - 1].State > 0)
            {
                _layers.Add(_highestLayer + 1, new MapLayer());
                _highestLayer++;
            }
        }

        private void CalculateNeighbors()
        {
            for (int i = _lowestLayer + 1; i < _highestLayer; i++)
            {
                _layers[i].CalculateNeighbors(_layers[i - 1].State, _layers[i + 1].State);
            }
        }

        private void UpdateBugs()
        {
            for (int i = _lowestLayer + 1; i < _highestLayer; i++)
            {
                _layers[i].UpdateBugs();
            }
        }
    }

    private class MapLayer
    {

        private static readonly uint _T = 0b11111;
        private static readonly uint _B = 0b11111 << 20;
        private static readonly uint _L = 0b00001_00001_00001_00001_00001;
        private static readonly uint _R = 0b10000_10000_10000_10000_10000;

        public uint State { get; set; }

        private uint neighbors0;
        private uint neighbors1;
        private uint neighbors2;

        public MapLayer(uint state = 0)
        {
            State = state;
        }

        public int CountBugs()
        {
            return BitOperations.PopCount(State);
        }

        public void CalculateNeighbors(uint lowerState, uint upperState)
        {
            InitializeNeighbors(upperState);
            uint up    = (State >> 5)             | (_B * ((lowerState >> 17) & 1));
            uint down  = (State << 5) & _maskGrid | (_T * ((lowerState >>  7) & 1));
            uint left  = (State >> 1) & _maskL    | (_R * ((lowerState >> 13) & 1));
            uint right = (State << 1) & _maskR    | (_L * ((lowerState >> 11) & 1));
            AddNeighbors(up);
            AddNeighbors(down);
            AddNeighbors(left);
            AddNeighbors(right);
        }

        public void UpdateBugs()
        {
            uint survivingBugs = State & neighbors0 & ~neighbors1;
            uint newBugs = ~State & (neighbors0 ^ neighbors1) & ~(1U << 12);
            State = (survivingBugs | newBugs) & ~neighbors2;
        }

        private void InitializeNeighbors(uint upperState)
        {
            neighbors0 = 0;
            neighbors1 = 0;
            neighbors2 = 0;
            GetUpperStateNeighbors(upperState, _T, 7);
            GetUpperStateNeighbors(upperState, _B, 17);
            GetUpperStateNeighbors(upperState, _L, 11);
            GetUpperStateNeighbors(upperState, _R, 13);
        }

        private void AddNeighbors(uint bits)
        {
            uint carry0 = neighbors0 & bits;
            uint carry1 = neighbors1 & carry0;
            neighbors0 ^= bits;
            neighbors1 ^= carry0;
            neighbors2 |= carry1;
        }

        private void GetUpperStateNeighbors(uint upperState, uint mask, int bit)
        {
            uint current = (uint)BitOperations.PopCount(upperState & mask);
            neighbors0 |= (current & 1U) << bit;
            neighbors1 |= (current & 2U) << (bit - 1);
            neighbors2 |= (current & 4U) << (bit - 2);
        }
    }
}
