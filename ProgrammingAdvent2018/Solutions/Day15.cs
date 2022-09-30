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
            BattleSquare[,] map = InputToMap(inputLines, units, 3);
            Battle battle = new Battle(map, units);
            if (!battle.CalculateOutcome(out int partOneAnswer, out string battleError))
            {
                output.WriteError(battleError, sw);
                return output;
            }

            int partTwoResult = -1;
            int powerLimit = 201;
            for (int elfAttackPower = 4; elfAttackPower < powerLimit; elfAttackPower++)
            {
                List<Unit> testUnits = new List<Unit>();
                BattleSquare[,] testMap = InputToMap(inputLines, testUnits, elfAttackPower);
                Battle testBattle = new Battle(testMap, testUnits, true);
                if (testBattle.CalculateOutcome(out int calculatedResult, out string _))
                {
                    partTwoResult = calculatedResult;
                    break;
                }
            }
            string partTwoAnswer;
            if (partTwoResult < 0)
            {
                partTwoAnswer = "The Elves cannot win.";
            }
            else
            {
                partTwoAnswer = partTwoResult.ToString();
            }

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
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

        private BattleSquare[,] InputToMap(string[] inputLines, List<Unit> units, int elfAttackPower)
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
                            Unit newUnit = new Unit(inputLines[y][x], x, y, elfAttackPower);
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

        internal static void Debug_DrawMap(BattleSquare[,] map)
        {
            Debug.Write(' ');
            for (int x = 0; x < map.GetLength(0); x++)
            {
                Debug.Write(x % 10);
            }
            Debug.WriteLine("");
            for (int y = 0; y < map.GetLength(1); y++)
            {
                Debug.Write(y % 10);
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
            (string, int, int)[] examples = new (string, int, int)[]
            {
                ("#######\xA#.G...#\xA#...EG#\xA#.#.#G#\xA#..G#E#\xA#.....#\xA#######", 27730, 4988),
                ("#######\xA#G..#E#\xA#E#E.E#\xA#G.##.#\xA#...#E#\xA#...E.#\xA#######", 36334, int.MinValue),
                ("#######\xA#E..EG#\xA#.#G.E#\xA#E.##E#\xA#G..#.#\xA#..E#.#\xA#######", 39514, 31284),
                ("#######\xA#E.G#.#\xA#.#G..#\xA#G.#.G#\xA#G..#.#\xA#...E.#\xA#######", 27755, 3478),
                ("#######\xA#.E...#\xA#.#..G#\xA#.###.#\xA#E#G#G#\xA#...#G#\xA#######", 28944, 6474),
                ("#########\xA#G......#\xA#.E.#...#\xA#..##..G#\xA#...##..#\xA#...#...#\xA#.G...G.#\xA#.....G.#\xA#########", 18740, 1140)
            };
            for (int i = 0; i < examples.Length; i++)
            {
                string exampleInput = examples[i].Item1;
                string[] lines = exampleInput.ToLines();
                if (!ValidateInput(lines, out string inputError))
                {
                    Debug.WriteLine($"DEBUG {i}: " + inputError);
                    continue;
                }
                List<Unit> units = new List<Unit>();
                BattleSquare[,] map = InputToMap(lines, units, 3);
                Battle battle = new Battle(map, units);
                if (!battle.CalculateOutcome(out int calculatedPartOneAnswer, out string battleError))
                {
                    Debug.WriteLine($"DEBUG {i} PART 1: " + battleError);
                    continue;
                }
                int examplePartOneOutput = examples[i].Item2;
                if (calculatedPartOneAnswer == examplePartOneOutput)
                {
                    Debug.WriteLine($"DEBUG {i} PART 1: MATCH");
                }
                else
                {
                    Debug.WriteLine($"DEBUG {i} PART 1: EXPECTED {examplePartOneOutput}, ACTUAL {calculatedPartOneAnswer}");
                }
                if (i == 1)
                {
                    // No Part Two for this example.
                    Debug.WriteLine($"DEBUG {i} PART 2: N/A");
                    continue;
                }
                int calculatedPartTwoAnswer = -1;
                int powerLimit = 201;
                for (int elfAttackPower = 4; elfAttackPower < powerLimit; elfAttackPower++)
                {
                    List<Unit> testUnits = new List<Unit>();
                    BattleSquare[,] testMap = InputToMap(lines, testUnits, elfAttackPower);
                    Battle testBattle = new Battle(testMap, testUnits, true);
                    if (testBattle.CalculateOutcome(out int calculatedResult, out string _))
                    {
                        calculatedPartTwoAnswer = calculatedResult;
                        break;
                    }
                }
                if (calculatedPartTwoAnswer < 0)
                {
                    Debug.WriteLine($"DEBUG {i} PART 2: NO ANSWER FOUND BELOW ATTACK POWER {powerLimit}");
                    continue;
                }
                int examplePartTwoOutput = examples[i].Item3;
                if (calculatedPartTwoAnswer == examplePartTwoOutput)
                {
                    Debug.WriteLine($"DEBUG {i} PART 2: MATCH");
                }
                else
                {
                    Debug.WriteLine($"DEBUG {i} PART 2: EXPECTED {examplePartTwoOutput}, ACTUAL {calculatedPartTwoAnswer}");
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
            public int AttackPower { get; set; }

            public Unit(char type, int x, int y, int elfAttackPower)
            {
                if (type == 'G')
                {
                    Type = UnitType.Goblin;
                    EnemyType = UnitType.Elf;
                    AttackPower = 3;
                }
                else if (type == 'E')
                {
                    Type = UnitType.Elf;
                    EnemyType = UnitType.Goblin;
                    AttackPower = elfAttackPower;
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
            public bool Searched { get; set; }
            public int SearchValue { get; set; }

            public BattleSquare(int x, int y, Unit occupant)
            {
                X = x;
                Y = y;
                Occupant = occupant;
                AdjacentSquares = new List<BattleSquare>(4);
                Searched = false;
                SearchValue = int.MaxValue;
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

        private class Battle
        {
            private readonly BattleSquare[,] _map;
            private readonly List<Unit> _units;
            private readonly bool _failOnElfDeath;

            public Battle(BattleSquare[,] map, List<Unit> units) : this(map, units, false) { }

            public Battle(BattleSquare[,] map, List<Unit> units, bool failOnElfDeath)
            {
                _map = map;
                _units = units;
                _failOnElfDeath = failOnElfDeath;
            }

            public bool CalculateOutcome(out int answer, out string errorMessage)
            {
                int roundLimit = 2048;
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
                        else if (unit.Type == UnitType.Elf)
                        {
                            targets = goblins;
                        }
                        else
                        {
                            errorMessage = "Unknown error when identifying targets.";
                            return false;
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
                            // Identify the nearest reachable open square adjacent to a target.
                            if (NearestOpenSquare(unit, out (int, int) coordinates))
                            {
                                // Take one step toward the chosen square along the shortest path.
                                (int, int) move = GetMoveToward(coordinates, unit);
                                _map[unit.X, unit.Y].Occupant = null;
                                unit.X = move.Item1;
                                unit.Y = move.Item2;
                                _map[unit.X, unit.Y].Occupant = unit;
                            }
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
                            target.HP -= unit.AttackPower;
                            // If this reduces its hit points to 0 or fewer, the selected target dies.
                            if (target.HP <= 0)
                            {
                                target.HP = 0;
                                target.IsDead = true;
                                if (target.Type == UnitType.Elf && _failOnElfDeath)
                                {
                                    answer = int.MinValue;
                                    errorMessage = "An Elf has died.";
                                    return false;
                                }
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
                    errorMessage = $"Combat did not end in the first {roundLimit} rounds.";
                    return false;
                }
                errorMessage = "Unknown error.";
                return false;
            }

            private bool NearestOpenSquare(Unit unit, out (int, int) coordinates)
            {
                int maxSearchDistance = int.MaxValue - 1;
                HashSet<BattleSquare> pending = new HashSet<BattleSquare>();
                HashSet<BattleSquare> searched = new HashSet<BattleSquare>();
                HashSet<BattleSquare> targetOpenSquares = new HashSet<BattleSquare>();
                BattleSquare currentSquare = _map[unit.X, unit.Y];
                currentSquare.SearchValue = 0;
                while (true)
                {
                    // Search adjacent squares.
                    foreach (BattleSquare adjacent in currentSquare.AdjacentSquares)
                    {
                        if (!adjacent.Searched)
                        {
                            if (adjacent.Occupant == null)
                            {
                                // Add adjacent empty square to be searched.
                                adjacent.SearchValue = Math.Min(adjacent.SearchValue, currentSquare.SearchValue + 1);
                                pending.Add(adjacent);
                            }
                            else if (adjacent.Occupant.Type == unit.EnemyType)
                            {
                                // Current square is adjacent to an enemy.
                                targetOpenSquares.Add(currentSquare);
                                // Don't search beyond the closest enemy.
                                maxSearchDistance = Math.Min(maxSearchDistance, currentSquare.SearchValue);
                            }
                        }
                    }
                    currentSquare.Searched = true;
                    pending.Remove(currentSquare);
                    searched.Add(currentSquare);
                    
                    // Choose a square with the lowest value to search next.
                    if (pending.Count == 0)
                    {
                        break;
                    }
                    int closest = int.MaxValue;
                    foreach (BattleSquare square in pending)
                    {
                        closest = Math.Min(closest, square.SearchValue);
                    }
                    foreach (BattleSquare square in pending)
                    {
                        if (square.SearchValue == closest)
                        {
                            currentSquare = square;
                            break;
                        }
                    }

                    // If all the squares within range have been searched, the algorithm has finished.
                    if (currentSquare.SearchValue > maxSearchDistance)
                    {
                        break;
                    }
                }

                // Reset search values.
                foreach (BattleSquare square in pending)
                {
                    square.Searched = false;
                    square.SearchValue = int.MaxValue;
                }
                foreach (BattleSquare square in searched)
                {
                    square.Searched = false;
                    square.SearchValue = int.MaxValue;
                }

                // If there are no paths to a target, return false.
                if (targetOpenSquares.Count == 0)
                {
                    coordinates = (0, 0);
                    return false;
                }

                // Select the candidate square that is first in top-to-bottom, left-to-right order.
                BattleSquare[] nearestTargetOpenSquares = targetOpenSquares.ToArray();
                Array.Sort(nearestTargetOpenSquares);
                coordinates = (nearestTargetOpenSquares[0].X, nearestTargetOpenSquares[0].Y);
                return true;
            }

            private (int, int) GetMoveToward((int, int) coordinates, Unit unit)
            {
                int maxSearchDistance = int.MaxValue - 1;
                HashSet<BattleSquare> pending = new HashSet<BattleSquare>();
                HashSet<BattleSquare> searched = new HashSet<BattleSquare>();
                BattleSquare currentSquare = _map[coordinates.Item1, coordinates.Item2];
                currentSquare.SearchValue = 0;
                while (true)
                {
                    // Search adjacent squares.
                    foreach (BattleSquare adjacent in currentSquare.AdjacentSquares)
                    {
                        if (!adjacent.Searched)
                        {
                            if (adjacent.Occupant == null)
                            {
                                // Add adjacent empty square to be searched.
                                adjacent.SearchValue = Math.Min(adjacent.SearchValue, currentSquare.SearchValue + 1);
                                pending.Add(adjacent);
                            }
                            else if (adjacent.Occupant == unit)
                            {
                                // Don't search beyond the moving unit.
                                maxSearchDistance = Math.Min(maxSearchDistance, currentSquare.SearchValue);
                            }
                        }
                    }
                    currentSquare.Searched = true;
                    pending.Remove(currentSquare);
                    searched.Add(currentSquare);

                    // Choose a square with the lowest value to search next.
                    if (pending.Count == 0)
                    {
                        break;
                    }
                    int closest = int.MaxValue;
                    foreach (BattleSquare square in pending)
                    {
                        closest = Math.Min(closest, square.SearchValue);
                    }
                    foreach (BattleSquare square in pending)
                    {
                        if (square.SearchValue == closest)
                        {
                            currentSquare = square;
                            break;
                        }
                    }

                    // If all the squares within range have been searched, the algorithm has finished.
                    if (currentSquare.SearchValue > maxSearchDistance)
                    {
                        break;
                    }
                }

                // Choose potential moves that are closest to the target coordinates.
                List<BattleSquare> potentialMoves = new List<BattleSquare>();
                int minimumDistance = int.MaxValue;
                foreach (BattleSquare square in _map[unit.X, unit.Y].AdjacentSquares)
                {
                    if (square.Occupant != null)
                    {
                        continue;
                    }
                    if (square.SearchValue < minimumDistance)
                    {
                        potentialMoves.Clear();
                        minimumDistance = square.SearchValue;
                    }
                    if (square.SearchValue <= minimumDistance)
                    {
                        potentialMoves.Add(square);
                    }
                }

                // Reset search values.
                foreach (BattleSquare square in pending)
                {
                    square.Searched = false;
                    square.SearchValue = int.MaxValue;
                }
                foreach (BattleSquare square in searched)
                {
                    square.Searched = false;
                    square.SearchValue = int.MaxValue;
                }

                if (potentialMoves.Count == 0)
                {
                    // There are no adjacent open squares. (This shouldn't happen.)
                    return (unit.X, unit.Y);
                }

                // Select the move that is first in top-to-bottom, left-to-right order.
                potentialMoves.Sort();
                return (potentialMoves[0].X, potentialMoves[0].Y);
            }

            private bool SelectTarget(List<Unit> adjacentTargets, out Unit selectedTarget)
            {
                // Select the first unit in the list with the lowest HP.
                // The list should already be sorted in top-to-bottom, left-to-right order.
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
