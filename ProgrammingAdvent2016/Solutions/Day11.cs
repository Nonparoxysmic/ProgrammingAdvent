// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ProgrammingAdvent2016
{
    public class Day11 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            string[] inputLines = input.ToLines();
            if (inputLines.Length < 4 
                || inputLines[0].Length < 30 || inputLines[1].Length < 30 
                || inputLines[2].Length < 30 || inputLines[3].Length < 30
                || inputLines[0].Substring(0, 25) != "The first floor contains "
                || inputLines[1].Substring(0, 26) != "The second floor contains "
                || inputLines[2].Substring(0, 25) != "The third floor contains "
                || inputLines[3].Substring(0, 26) != "The fourth floor contains ")
            {
                solution.WriteSolution(1, "ERROR: Invalid input.", 0);
                return solution;
            }
            stopwatch.Start();

            InitialState initialState = new InitialState();
            List<string> elements = new List<string>();
            for (int floor = 0; floor < 4; floor++)
            {
                inputLines[floor] = inputLines[floor].Substring(25 + floor % 2).TrimEnd('.');
                string[] terms = inputLines[floor].Split(new string[] { ",", "and" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string term in terms)
                {
                    string[] words = term.Split();
                    if (words.Length < 3) continue;
                    string element = "";
                    ItemType type = ItemType.Microchip;
                    for (int i = 1; i < words.Length; i++)
                    {
                        if (words[i] == "generator")
                        {
                            element = words[i - 1];
                            type = ItemType.Generator;
                            break;
                        }
                        else if (words[i] == "microchip")
                        {
                            if (words[i - 1].Length > 11)
                            {
                                element = words[i - 1].Substring(0, words[i - 1].Length - 11);
                            }
                            break;
                        }
                    }
                    if (element != "")
                    {
                        if (!elements.Contains(element)) elements.Add(element);
                        initialState.AddItem(floor, elements.IndexOf(element), type);
                    }
                }
            }
            if (!initialState.ValidateInitialState(elements.Count) || elements.Count > 16 || elements.Count < 2)
            {
                solution.WriteSolution(1, "ERROR: Invalid initial state.", stopwatch.ElapsedMilliseconds);
                return solution;
            }

            int partOneSolution = NumberOfSteps(initialState);
            solution.WriteSolution(1, partOneSolution, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        int NumberOfSteps(InitialState initialState)
        {
            int output = -1;
            // Create initial search node.
            Dictionary<ulong, SearchNode> allNodes = new Dictionary<ulong, SearchNode>();
            SearchNode startingNode = initialState.ToSearchNode();
            ulong startingNodeKey = startingNode.GetKey();
            allNodes.Add(startingNodeKey, startingNode);
            // Initialize node lists.
            List<ulong> openList = new List<ulong>();
            List<ulong> closedList = new List<ulong>();
            openList.Add(startingNodeKey);
            // While there are nodes on the open list...
            while (openList.Count > 0)
            {
                // Get a node from the open list with the lowest score.
                SearchNode currentNode = null;
                int lowestScore = int.MaxValue;
                foreach (ulong key in openList)
                {
                    SearchNode node = allNodes[key];
                    if (node.score < lowestScore)
                    {
                        lowestScore = node.score;
                        currentNode = node;
                    }
                }
                // If the node is the goal state, set output and break.
                if (currentNode.IsGoal())
                {
                    output = currentNode.stepsFromStart;
                    break;
                }
                // Move current node from open list to closed list.
                ulong currentNodeKey = currentNode.GetKey();
                openList.Remove(currentNodeKey);
                closedList.Add(currentNodeKey);
                // Generate the valid states that can be reached in one move.
                Dictionary<ulong, SearchNode> possibleMoves = currentNode.GetPossibleMoves();
                // For each possible move:
                foreach (KeyValuePair<ulong, SearchNode> move in possibleMoves)
                {
                    // Add to collection of all known nodes if not already included.
                    if (!allNodes.ContainsKey(move.Key))
                    {
                        allNodes.Add(move.Key, move.Value);
                    }
                    // If the move is already on the closed list, skip it.
                    if (closedList.Contains(move.Key)) continue;
                    // If it's not on the open list, or has been reached in fewer steps:
                    if (!openList.Contains(move.Key) 
                        || currentNode.stepsFromStart + 1 < allNodes[move.Key].stepsFromStart)
                    {
                        // Recalculate its values.
                        allNodes[move.Key].stepsFromStart = currentNode.stepsFromStart + 1;
                        allNodes[move.Key].score = allNodes[move.Key].stepsFromStart + allNodes[move.Key].minimumStepsToGoal;
                        // Add to open list if not already.
                        if (!openList.Contains(move.Key))
                        {
                            openList.Add(move.Key);
                        }
                    }
                }
            }
            return output;
        }
    }

    class SearchNode
    {
        public int elevatorFloor;
        public string items;
        public int stepsFromStart;
        public int minimumStepsToGoal;
        public int score;

        public SearchNode(int elevatorFloor, string items)
        {
            this.elevatorFloor = elevatorFloor;
            this.items = items;
            minimumStepsToGoal = EstimateMinimumStepsToGoal();
        }

        public SearchNode(int elevatorFloor, string items, int stepsFromStart)
        {
            this.elevatorFloor = elevatorFloor;
            this.items = items;
            this.stepsFromStart = stepsFromStart;
            minimumStepsToGoal = EstimateMinimumStepsToGoal();
            score = stepsFromStart + minimumStepsToGoal;
        }

        public int EstimateMinimumStepsToGoal()
        {
            List<int> values = new List<int>();
            foreach (char c in items)
            {
                switch (c)
                {
                    case '0':
                        values.Add(0);
                        break;
                    case '1':
                        values.Add(1);
                        break;
                    case '2':
                        values.Add(2);
                        break;
                }
            }
            if (values.Count == 0) return 0;
            if (values.Count == 1) return 3 - values[0];
            values.Sort();
            int lowestFloor = values[0];
            values.Reverse();
            int output = Math.Abs(elevatorFloor - lowestFloor);
            for (int i = 0; i < values.Count - 1; i += 2)
            {
                if (i != 0)
                {
                    output += 3 - Math.Min(values[i], values[i + 1]);
                }
                switch (Math.Min(values[i], values[i + 1]))
                {
                    case 0:
                        output += 3;
                        break;
                    case 1:
                        output += 2;
                        break;
                    case 2:
                        output += 1;
                        break;
                }
            }
            if (values.Count % 2 == 1)
            {
                int lastValue = values[values.Count - 1];
                output += 2 * (3 - lastValue);
            }
            return output;
        }

        public ulong GetKey()
        {
            byte[] hash = AdventMD5.ComputeHash(Encoding.UTF8.GetBytes(items + elevatorFloor));
            ulong output = 0;
            for (int i = 0; i < 8; i++)
            {
                output |= (ulong)hash[i] << i * 8;
            }
            return output;
        }

        public bool IsGoal()
        {
            if (elevatorFloor != 3 || items.Contains("0") || items.Contains("1") || items.Contains("2"))
            {
                return false;
            }
            return true;
        }

        public Dictionary<ulong, SearchNode> GetPossibleMoves()
        {
            Dictionary<ulong, SearchNode> output = new Dictionary<ulong, SearchNode>();
            List<int> moveDirections = new List<int>();
            if (elevatorFloor > 0) moveDirections.Add(-1);
            if (elevatorFloor < 3) moveDirections.Add(1);
            foreach (int direction in moveDirections)
            {
                for (int i = 0; i < items.Length; i += 2)
                {
                    if (items[i] == elevatorFloor.ToString()[0])
                    {
                        SearchNode oneItem = MakeMove(direction, i);
                        if (oneItem != null)
                        {
                            ulong key = oneItem.GetKey();
                            if (!output.ContainsKey(key))
                            {
                                output.Add(key, oneItem);
                            }
                        }
                    }
                    for (int j = i + 2; j < items.Length; j += 2)
                    {
                        if ((items[i] == elevatorFloor.ToString()[0]) 
                            && (items[j] == elevatorFloor.ToString()[0]))
                        {
                            SearchNode twoItem = MakeMove(direction, i, j);
                            if (twoItem != null)
                            {
                                ulong key = twoItem.GetKey();
                                if (!output.ContainsKey(key))
                                {
                                    output.Add(key, twoItem);
                                }
                            }
                        }
                    }
                }
            }
            return output;
        }

        SearchNode MakeMove(int elevatorChange, int itemOne, int itemTwo = -1)
        {
            char newFloor = (elevatorFloor + elevatorChange).ToString()[0];
            StringBuilder newItems = new StringBuilder(items);
            newItems[itemOne] = newFloor;
            if (itemTwo >= 0) newItems[itemTwo] = newFloor;
            string[] itemPairs = newItems.ToString().Split(',');
            Array.Sort(itemPairs);
            string newItemString = string.Join(",", itemPairs);
            if (IsValidState(newItemString))
            {
                return new SearchNode(elevatorFloor + elevatorChange, newItemString, stepsFromStart + 1);
            }
            else return null;
        }

        bool IsValidState(string items)
        {
            string[] itemPairs = items.Split(',');
            ulong microchips = 0;
            ulong generators = 0;
            for (int i = 0; i < itemPairs.Length; i++)
            {
                int microchipFloor = itemPairs[i][0] - 48;
                int generatorFloor = itemPairs[i][2] - 48;
                microchips |= 1UL << (microchipFloor * 16 + i);
                generators |= 1UL << (generatorFloor * 16 + i);
            }
            ulong unprotectedChips = microchips & ~generators;
            if (unprotectedChips == 0) return true;
            for (int floor = 0; floor < 4; floor++)
            {
                int numberUnprotectedChips = (int)(unprotectedChips >> floor * 16) & 0xFFFF;
                int numberGenerators = (int)(generators >> floor * 16) & 0xFFFF;
                if (numberUnprotectedChips > 0 && numberGenerators > 0) return false;
            }
            return true;
        }
    }

    class InitialState
    {
        readonly List<FacilityItem> facilityItems = new List<FacilityItem>();
        int numberOfElements = 0;

        public void AddItem(int floor, int element, ItemType type)
        {
            if (floor < 0 || floor > 3) return;
            facilityItems.Add(new FacilityItem(floor, element, type));
            numberOfElements = Math.Max(numberOfElements, element + 1);
        }

        public bool ValidateInitialState(int numberOfElements)
        {
            for (int element = 0; element < numberOfElements; element++)
            {
                bool microchipFound = false;
                bool generatorFound = false;
                foreach (FacilityItem item in facilityItems)
                {
                    if (item.element == element)
                    {
                        switch (item.type)
                        {
                            case ItemType.Microchip:
                                if (microchipFound) return false;
                                microchipFound = true;
                                break;
                            case ItemType.Generator:
                                if (generatorFound) return false;
                                generatorFound = true;
                                break;
                        }
                    }
                }
                if (!microchipFound || !generatorFound) return false;
            }
            return true;
        }

        public SearchNode ToSearchNode()
        {
            List<string> itemPairs = new List<string>();
            for (int element = 0; element < numberOfElements; element++)
            {
                int microchipFloor = -999;
                int generatorFloor = -999;
                foreach (FacilityItem item in facilityItems)
                {
                    if (item.element == element)
                    {
                        if (item.type == ItemType.Microchip)
                        {
                            microchipFloor = item.floor;
                        }
                        else generatorFloor = item.floor;
                    }
                }
                if (microchipFloor + generatorFloor < 0)
                {
                    Console.WriteLine("ERROR: missing item in initial state.");
                }
                itemPairs.Add(microchipFloor.ToString() + "+" + generatorFloor);
            }
            itemPairs.Sort();
            string newItems = string.Join(",", itemPairs);
            return new SearchNode(0, newItems, 0);
        }
    }

    class FacilityItem
    {
        public int floor;
        public int element;
        public ItemType type;

        public FacilityItem(int floor, int element, ItemType type)
        {
            this.floor = floor;
            this.element = element;
            this.type = type;
        }
    }

    enum ItemType
    {
        Microchip,
        Generator
    }
}
