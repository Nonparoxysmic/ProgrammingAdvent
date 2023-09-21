// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2020
// https://adventofcode.com/2020

using System.Text.RegularExpressions;
using ProgrammingAdvent2020.Common;

namespace ProgrammingAdvent2020.Solutions;

internal class Day12 : Day
{
    private static readonly Regex _validLine = new("^([LR](?<degrees>90|180|270)|[NSEWF](?<units>[0-9]{1,3}))$");
    private static readonly Vector2Int[] _directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),  // East
        new Vector2Int(0, -1), // North
        new Vector2Int(-1, 0), // West
        new Vector2Int(0, 1)   // South
    };

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
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        Instruction[] instructions = ReadInput(input);
        int partOneAnswer = PartOneAnswer(instructions);
        int partTwoAnswer = PartTwoAnswer(instructions);

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static Instruction[] ReadInput(string[] input)
    {
        Instruction[] output = new Instruction[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            Match match = _validLine.Match(input[i]);
            char action = match.Value[0];
            int value = action switch
            {
                'L' or 'R' => int.Parse(match.Groups["degrees"].Value),
                _ => int.Parse(match.Groups["units"].Value)
            };
            output[i] = new(action, value);
        }
        return output;
    }

    private static int PartOneAnswer(Instruction[] instructions)
    {
        Vector2Int position = Vector2Int.Zero;
        int direction = 0;
        foreach (Instruction instruction in instructions)
        {
            position += instruction.MoveDistance * _directions[instruction.MoveDirection]
                + instruction.Forward * _directions[direction % 4];
            direction += instruction.Turn;
        }
        return position.TaxicabMagnitude();
    }

    private static int PartTwoAnswer(Instruction[] instructions)
    {
        Vector2Int position = Vector2Int.Zero;
        Vector2Int waypoint = new(10, -1);
        foreach (Instruction instruction in instructions)
        {
            waypoint += instruction.MoveDistance * _directions[instruction.MoveDirection];
            waypoint = instruction.RotateWaypoint(waypoint);
            position += instruction.Forward * waypoint;
        }
        return position.TaxicabMagnitude();
    }

    private class Instruction
    {
        public char Action { get; private set; }
        public int Value { get; private set; }
        public int Forward { get; private set; }
        public int Turn { get; private set; }
        public int MoveDistance { get; private set; }
        public int MoveDirection { get; private set; }

        private readonly int[,] _rotationMatrix;

        public Instruction(char action, int value)
        {
            Action = action;
            Value = value;
            Forward = Action switch
            {
                'F' => Value,
                _ => 0
            };
            Turn = Action switch
            {
                'L' => Value / 90,
                'R' => (360 - Value) / 90,
                _ => 0
            };
            MoveDistance = Action switch
            {
                'N' or 'S' or 'E' or 'W' => Value,
                _ => 0
            };
            MoveDirection = Action switch
            {
                'E' => 0,
                'N' => 1,
                'W' => 2,
                _ => 3
            };
            _rotationMatrix = Turn switch
            {
                3 => new int[,] { {  0,  1 }, { -1,  0 } },
                2 => new int[,] { { -1,  0 }, {  0, -1 } },
                1 => new int[,] { {  0, -1 }, {  1,  0 } },
                _ => new int[,] { {  1,  0 }, {  0,  1 } }
            };
        }

        public Vector2Int RotateWaypoint(Vector2Int waypoint)
        {
            int x = waypoint.X * _rotationMatrix[0, 0] + waypoint.Y * _rotationMatrix[1, 0];
            int y = waypoint.X * _rotationMatrix[0, 1] + waypoint.Y * _rotationMatrix[1, 1];
            return new Vector2Int(x, y);
        }
    }
}
