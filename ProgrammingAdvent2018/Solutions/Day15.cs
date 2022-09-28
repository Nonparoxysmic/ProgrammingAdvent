// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day15 : Day
    {
        private readonly Regex validLine = new Regex(@"^[#\.GE]+$");

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string[] inputLines = input.ToLines();

            if (!ValidateInput(inputLines, out string inputError))
            {
                output.WriteError(inputError, sw);
                return output;
            }
            List<Unit> units = new List<Unit>();
            BattleSquare[,] map = InputToMap(inputLines, units);
            Battle battle = new Battle(map, units);
            if (!battle.CalculateOutcome(out int partOneAnswer, out string battleError))
            {
                output.WriteError(battleError, sw);
                return output;
            }

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        private bool ValidateInput(string[] inputLines, out string message)
        {
            if (inputLines.Length == 0)
            {
                message = "No input.";
                return false;
            }
            if (inputLines[0].Length == 0)
            {
                message = "First line is empty.";
                return false;
            }
            int goblinUnits = 0;
            int elfUnits = 0;
            for (int y = 0; y < inputLines.Length; y++)
            {
                if (inputLines[y].Length != inputLines[0].Length)
                {
                    message = "Input lines must be the same length.";
                    return false;
                }
                Match lineMatch = validLine.Match(inputLines[y]);
                if (!lineMatch.Success)
                {
                    message = $"Invalid characters in line {y}.";
                    return false;
                }
                for (int x = 0; x < inputLines[y].Length; x++)
                {
                    if (inputLines[y][x] == 'G')
                    {
                        goblinUnits++;
                    }
                    if (inputLines[y][x] == 'E')
                    {
                        elfUnits++;
                    }
                }
            }
            if (goblinUnits + elfUnits == 0)
            {
                message = "No combat units on the map.";
                return false;
            }
            if (goblinUnits == 0)
            {
                message = "No Goblin combat units on the map.";
                return false;
            }
            if (elfUnits == 0)
            {
                message = "No Elf combat units on the map.";
                return false;
            }
            message = string.Empty;
            return true;
        }

        private BattleSquare[,] InputToMap(string[] inputLines, List<Unit> units)
        {
            BattleSquare[,] map = new BattleSquare[inputLines[0].Length, inputLines.Length];
            for (int y = 0; y < inputLines.Length; y++)
            {
                for (int x = 0; x < inputLines[y].Length; x++)
                {
                    switch (inputLines[y][x])
                    {
                        case '.':
                            map[x, y] = new BattleSquare(x, y, null);
                            break;
                        case 'G':
                        case 'E':
                            Unit newUnit = new Unit(inputLines[y][x], x, y);
                            units.Add(newUnit);
                            map[x, y] = new BattleSquare(x, y, newUnit);
                            break;
                        case '#':
                        default:
                            break;
                    }
                }
            }
            for (int y = 0; y < inputLines.Length - 1; y++)
            {
                for (int x = 0; x < inputLines[y].Length - 1; x++)
                {
                    BattleSquare current = map[x, y];
                    if (current == null)
                    {
                        continue;
                    }
                    BattleSquare right = map[x + 1, y];
                    if (right != null)
                    {
                        current.AdjacentSquares.Add(right);
                        right.AdjacentSquares.Add(current);
                    }
                    BattleSquare down = map[x, y + 1];
                    if (down != null)
                    {
                        current.AdjacentSquares.Add(down);
                        down.AdjacentSquares.Add(current);
                    }
                }
            }
            for (int y = 0; y < inputLines.Length; y++)
            {
                for (int x = 0; x < inputLines[y].Length; x++)
                {
                    if (map[x, y] != null)
                    {
                        map[x, y].AdjacentSquares.Sort();
                    }
                }
            }
            return map;
        }

        internal void Debug_DrawMap(BattleSquare[,] map)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y] == null)
                    {
                        Debug.Write('#');
                    }
                    else if (map[x, y].Occupant == null)
                    {
                        Debug.Write('.');
                    }
                    else if (map[x, y].Occupant.Type == UnitType.Goblin)
                    {
                        Debug.Write('G');
                    }
                    else if (map[x, y].Occupant.Type == UnitType.Elf)
                    {
                        Debug.Write('E');
                    }
                    else
                    {
                        Debug.Write('?');
                    }
                }
                Debug.WriteLine("");
            }
        }

        internal void Debug_Tests()
        {
            (string, int)[] examples = new (string, int)[]
            {
                ("#######\xA#.G...#\xA#...EG#\xA#.#.#G#\xA#..G#E#\xA#.....#\xA#######", 27730),
                ("#######\xA#G..#E#\xA#E#E.E#\xA#G.##.#\xA#...#E#\xA#...E.#\xA#######", 36334),
                ("#######\xA#E..EG#\xA#.#G.E#\xA#E.##E#\xA#G..#.#\xA#..E#.#\xA#######", 39514),
                ("#######\xA#E.G#.#\xA#.#G..#\xA#G.#.G#\xA#G..#.#\xA#...E.#\xA#######", 27755),
                ("#######\xA#.E...#\xA#.#..G#\xA#.###.#\xA#E#G#G#\xA#...#G#\xA#######", 28944),
                ("#########\xA#G......#\xA#.E.#...#\xA#..##..G#\xA#...##..#\xA#...#...#\xA#.G...G.#\xA#.....G.#\xA#########", 18740)
            };
            for (int i = 0; i < examples.Length; i++)
            {
                string exampleInput = examples[i].Item1;
                int exampleOutput = examples[i].Item2;
                string[] lines = exampleInput.ToLines();
                if (!ValidateInput(lines, out string inputError))
                {
                    Debug.WriteLine($"DEBUG {i}: " + inputError);
                    continue;
                }
                List<Unit> units = new List<Unit>();
                BattleSquare[,] map = InputToMap(lines, units);
                Battle battle = new Battle(map, units);
                if (!battle.CalculateOutcome(out int calculatedAnswer, out string battleError))
                {
                    Debug.WriteLine($"DEBUG {i}: " + battleError);
                    continue;
                }
                if (calculatedAnswer == exampleOutput)
                {
                    Debug.WriteLine($"DEBUG {i}: MATCH");
                }
                else
                {
                    Debug.WriteLine($"DEBUG {i}: EXPECTED {exampleOutput}, ACTUAL {calculatedAnswer}");
                }
            }
        }

        internal enum UnitType
        {
            None,
            Goblin,
            Elf
        }

        internal class Unit : IComparable
        {
            public UnitType Type { get; set; }
            public UnitType EnemyType { get; set; }
            public bool IsDead { get; set; }
            public int HP { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

            public Unit(char type, int x, int y)
            {
                if (type == 'G')
                {
                    Type = UnitType.Goblin;
                    EnemyType = UnitType.Elf;
                }
                else if (type == 'E')
                {
                    Type = UnitType.Elf;
                    EnemyType = UnitType.Goblin;
                }
                else
                {
                    Type = UnitType.None;
                    EnemyType = UnitType.None;
                }
                IsDead = false;
                HP = 200;
                X = x;
                Y = y;
            }

            public int CompareTo(object obj)
            {
                if (obj == null)
                {
                    return -1;
                }
                if (obj is Unit otherUnit)
                {
                    if (X == otherUnit.X && Y == otherUnit.Y)
                    {
                        return 0;
                    }
                    if (Y < otherUnit.Y || (Y == otherUnit.Y && X < otherUnit.X))
                    {
                        return -1;
                    }
                }
                return 1;
            }
        }

        internal class BattleSquare : IComparable
        {
            public int X { get; private set; }
            public int Y { get; private set; }
            public Unit Occupant { get; set; }
            public List<BattleSquare> AdjacentSquares { get; private set; }

            public BattleSquare(int x, int y, Unit occupant)
            {
                X = x;
                Y = y;
                Occupant = occupant;
                AdjacentSquares = new List<BattleSquare>(4);
            }

            public int CompareTo(object obj)
            {
                if (obj == null)
                {
                    return -1;
                }
                if (obj is BattleSquare otherNode)
                {
                    if (X == otherNode.X && Y == otherNode.Y)
                    {
                        return 0;
                    }
                    if (Y < otherNode.Y || (Y == otherNode.Y && X < otherNode.X))
                    {
                        return -1;
                    }
                }
                return 1;
            }
        }

        internal class Battle
        {
            private readonly BattleSquare[,] _map;
            private readonly List<Unit> _units;

            public Battle(BattleSquare[,] map, List<Unit> units)
            {
                _map = map;
                _units = units;
            }

            public bool CalculateOutcome(out int answer, out string errorMessage)
            {
                int roundLimit = 60;
                List<Unit> goblins = _units.Where(u => u.Type == UnitType.Goblin).ToList();
                List<Unit> elves = _units.Where(u => u.Type == UnitType.Elf).ToList();
                answer = -1;
                for (int round = 0; round < roundLimit; round++)
                {
                    foreach (Unit unit in _units)
                    {
                        // If this unit is dead, skip to next unit.
                        if (unit.IsDead)
                        {
                            continue;
                        }

                        // Identify all possible targets.
                        List<Unit> targets;
                        if (unit.Type == UnitType.Goblin)
                        {
                            targets = elves;
                        }
                        else
                        {
                            targets = goblins;
                        }

                        // If no targets remain, combat ends. Calculate answer and return.
                        if (targets.Count == 0)
                        {
                            answer = 0;
                            foreach (Unit remainingUnit in _units)
                            {
                                if (!remainingUnit.IsDead)
                                {
                                    answer += remainingUnit.HP;
                                }
                            }
                            answer *= round;
                            errorMessage = string.Empty;
                            return true;
                        }

                        // Identify potential targets.
                        bool adjacentToTarget = false;
                        foreach (BattleSquare square in _map[unit.X, unit.Y].AdjacentSquares)
                        {
                            if (square.Occupant?.Type == unit.EnemyType)
                            {
                                adjacentToTarget = true;
                                break;
                            }
                        }
                        bool hasOpenTargets = false;
                        if (!adjacentToTarget)
                        {
                            foreach (Unit target in targets)
                            {
                                if (target.IsDead)
                                {
                                    continue;
                                }
                                foreach (BattleSquare square in _map[target.X, target.Y].AdjacentSquares)
                                {
                                    if (square.Occupant == null)
                                    {
                                        hasOpenTargets = true;
                                        break;
                                    }
                                }
                                if (hasOpenTargets)
                                {
                                    break;
                                }
                            }
                        }
                        // If the unit is not already adjacent to a target, and there are
                        // no open squares which are adjacent to a target, the unit ends its turn.
                        if (!adjacentToTarget && !hasOpenTargets)
                        {
                            continue;
                        }

                        // If not adjacent to a target, move.
                        if (!adjacentToTarget)
                        {
                            // Identify the nearest open square adjacent to a target. (Usual tie-breaker)

                            // Take one step toward the chosen square along the shortest path.
                            // (Usual tie-breaker for first step)

                        }

                        // Then, if now adjacent to a target, attack.
                        List<Unit> adjacentTargets = new List<Unit>();
                        foreach (BattleSquare square in _map[unit.X, unit.Y].AdjacentSquares)
                        {
                            if (square.Occupant?.Type == unit.EnemyType)
                            {
                                adjacentTargets.Add(square.Occupant);
                            }
                        }
                        if (adjacentTargets.Count > 0)
                        {
                            // Select adjacent target with the fewest hit points.
                            if (!SelectTarget(adjacentTargets, out Unit target))
                            {
                                errorMessage = "Unknown error when selecting an adjacent target.";
                                return false;
                            }
                            // Deal damage equal to attack power to the selected target.
                            target.HP -= 3;
                            // If this reduces its hit points to 0 or fewer, the selected target dies.
                            if (target.HP <= 0)
                            {
                                target.HP = 0;
                                target.IsDead = true;
                                target.Type = UnitType.None;
                                target.EnemyType = UnitType.None;
                                _map[target.X, target.Y].Occupant = null;
                            }
                        }

                        targets.RemoveAll(u => u.IsDead);
                    }
                    _units.RemoveAll(u => u.IsDead);
                    _units.Sort();
                }
                if (answer < 0)
                {
                    errorMessage = $"Combat did not end after {roundLimit} rounds.";
                    return false;
                }
                errorMessage = "Unknown error";
                return false;
            }

            private bool SelectTarget(List<Unit> adjacentTargets, out Unit selectedTarget)
            {
                int lowestHP = int.MaxValue;
                foreach (Unit target in adjacentTargets)
                {
                    lowestHP = Math.Min(lowestHP, target.HP);
                }
                foreach (Unit target in adjacentTargets)
                {
                    if (target.HP == lowestHP)
                    {
                        selectedTarget = target;
                        return true;
                    }
                }
                selectedTarget = null;
                return false;
            }
        }
    }
}
