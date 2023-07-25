// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day18 : Day
{
    private static readonly Regex _validCharacters = new("^[a-zA-Z@.#]+$");
    private static readonly Vector2Int[] _steps = new Vector2Int[]
    {
        new Vector2Int(-1,  0),
        new Vector2Int( 0, -1),
        new Vector2Int( 1,  0),
        new Vector2Int( 0,  1)
    };

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        if (inputLines.Length == 0)
        {
            errorMessage = "No input.";
            return false;
        }
        HashSet<char> keys = new();
        bool entranceFound = false;
        foreach (string line in inputLines)
        {
            if (line.Length == 0)
            {
                errorMessage = "Input contains an empty line.";
                return false;
            }
            if (line.Length != inputLines[0].Length)
            {
                errorMessage = "Input lines are not all the same length.";
                return false;
            }
            if (!_validCharacters.IsMatch(line))
            {
                errorMessage = $"Input \"{line.Left(20, true)}\" contains invalid characters.";
                return false;
            }
            foreach (char c in line)
            {
                if ('a' <= c && c <= 'z')
                {
                    if (!keys.Add(c))
                    {
                        errorMessage = $"Input contains duplicate key '{c}'.";
                        return false;
                    }
                }
                if (c == '@')
                {
                    if (entranceFound)
                    {
                        errorMessage = "Input contains multiple entrances.";
                        return false;
                    }
                    else
                    {
                        entranceFound = true;
                    }
                }
            }
        }
        if (!entranceFound)
        {
            errorMessage = "Input contains no entrance.";
            return false;
        }
        if (keys.Count == 0)
        {
            errorMessage = "Input does not contain any keys.";
            return false;
        }
        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        (char[,] map, int allKeys, Vector2Int entrance) = ProcessInput(inputLines);
        MazeNode entranceNode = BuildGraph(map, entrance, out bool partTwoAllowed);

        int keysCollected = 0;
        Dictionary<ulong, int> cache = new();
        int partOneAnswer = StepsToCollectAllKeys(entranceNode, ref keysCollected, allKeys, cache);
        if (partOneAnswer == int.MaxValue)
        {
            return output.WriteError("Unable to collect all keys.");
        }

        if (!partTwoAllowed)
        {
            return output.WriteAnswers(partOneAnswer, "Invalid input for Part Two");
        }
        MazeNode[] entranceNodes = SplitEntrance(entranceNode);
        keysCollected = 0;
        cache.Clear();
        int partTwoAnswer = RobotStepsToCollectAllKeys(entranceNodes, ref keysCollected, allKeys, cache, 15);
        if (partTwoAnswer == int.MaxValue)
        {
            return output.WriteAnswers(partOneAnswer, "ERROR: Unable to collect all keys.");
        }

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static (char[,], int, Vector2Int) ProcessInput(string[] input)
    {
        char[,] map = new char[input[0].Length, input.Length];
        int keys = 0;
        int entranceX = -1;
        int entranceY = -1;
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[0].Length; x++)
            {
                map[x, y] = input[y][x];
                if (map[x, y] == '@')
                {
                    entranceX = x;
                    entranceY = y;
                }
                if ('a' <= map[x, y] && map[x, y] <= 'z')
                {
                    keys |= 1 << (map[x, y] - 'a');
                }
            }
        }
        PruneDeadEnds(map);
        return (map, keys, new Vector2Int(entranceX, entranceY));
    }

    private static void PruneDeadEnds(char[,] map)
    {
        for (int y = 1; y < map.GetLength(1) - 1; y++)
        {
            for (int x = 1; x < map.GetLength(0) - 1; x++)
            {
                PruneDeadEnd(map, x, y);
            }
        }
    }

    private static void PruneDeadEnd(char[,] map, int x, int y)
    {
        if (('a' <= map[x, y] && map[x, y] <= 'z') || map[x, y] == '@')
        {
            return;
        }
        int openNeighbors = 0;
        int openX = -1;
        int openY = -1;
        foreach (Vector2Int step in _steps)
        {
            if (map[x + step.X, y + step.Y] != '#')
            {
                openNeighbors++;
                openX = x + step.X;
                openY = y + step.Y;
            }
        }
        if (openNeighbors == 0)
        {
            map[x, y] = '#';
            return;
        }
        if (openNeighbors == 1)
        {
            map[x, y] = '#';
            PruneDeadEnd(map, openX, openY);
        }
    }

    private static MazeNode BuildGraph(char[,] map, Vector2Int entrance, out bool partTwoAllowed)
    {
        MazeNode.AllNodes.Clear();
        MazeNode.KeyNodes.Clear();
        MazeNode root = new(entrance.X, entrance.Y, '@');

        if (map[entrance.X - 1, entrance.Y - 1] == '.'
            && map[entrance.X, entrance.Y - 1] == '.'
            && map[entrance.X + 1, entrance.Y - 1] == '.'
            && map[entrance.X - 1, entrance.Y] == '.'
            && map[entrance.X + 1, entrance.Y] == '.'
            && map[entrance.X - 1, entrance.Y + 1] == '.'
            && map[entrance.X, entrance.Y + 1] == '.'
            && map[entrance.X + 1, entrance.Y + 1] == '.')
        {
            // Actual input can be divided into four quadrants.
            partTwoAllowed = true;
            map[entrance.X - 1, entrance.Y - 1] = '@';
            map[entrance.X, entrance.Y - 1] = '#';
            map[entrance.X + 1, entrance.Y - 1] = '@';
            map[entrance.X - 1, entrance.Y] = '#';
            map[entrance.X, entrance.Y] = '#';
            map[entrance.X + 1, entrance.Y] = '#';
            map[entrance.X - 1, entrance.Y + 1] = '@';
            map[entrance.X, entrance.Y + 1] = '#';
            map[entrance.X + 1, entrance.Y + 1] = '@';

            root.CreateEntranceNeighbors();
            foreach (MazeNode neighbor in root.Neighbors)
            {
                MapToNodes(map, neighbor);
            }
        }
        else
        {
            // Example inputs do not divide into quadrants.
            partTwoAllowed = false;
            MapToNodes(map, root);
        }

        return root;
    }

    private static void MapToNodes(char[,] map, MazeNode node)
    {
        map[node.X, node.Y] = '#';
        for (int direction = 0; direction < 4; direction++)
        {
            Vector2Int step = _steps[direction];
            if (map[node.X + step.X, node.Y + step.Y] == '#')
            {
                continue;
            }
            MazeCrawler crawler = new(node.X + step.X, node.Y + step.Y, map, node);
            crawler.FindNextNode();
        }
    }

    private static void Dijkstra(MazeNode source)
    {
        source.KeyDistances.Clear();
        MazeNode.DijkstraReset();
        source.DijkstraDistance = 0;
        List<MazeNode> unvisitedNodes = new(MazeNode.AllNodes);
        int timeout = 0;
        while (timeout++ < 1000)
        {
            MazeNode? current = unvisitedNodes.OrderBy(n => n.DijkstraDistance).FirstOrDefault();
            if (current is null || current.DijkstraDistance == int.MaxValue)
            {
                break;
            }
            if (current.HasKey)
            {
                source.KeyDistances[current.Feature] = (current.DijkstraDistance, current.DijkstraDoors);
            }
            else
            {
                IEnumerable<MazeNode> unvisitedNeighbors = current.Neighbors.Where(n => !n.DijkstraVisited);
                foreach (MazeNode neighbor in unvisitedNeighbors)
                {
                    int distance = current.DijkstraDistance + EdgeInfo.GetDistance(current, neighbor);
                    neighbor.DijkstraDistance = Math.Min(neighbor.DijkstraDistance, distance);
                    int doors = current.DijkstraDoors | EdgeInfo.GetDoors(current, neighbor);
                    neighbor.DijkstraDoors = doors;
                }
            }
            current.DijkstraVisited = true;
            unvisitedNodes.Remove(current);
        }
    }

    private static int StepsToCollectAllKeys(MazeNode currentNode, ref int keysCollected, int allKeys, Dictionary<ulong, int> cache)
    {
        int uncollectedKeys = allKeys & ~keysCollected;
        if (uncollectedKeys == 0)
        {
            return 0;
        }

        ulong state = ((ulong)currentNode.Feature << 32) | (uint)uncollectedKeys;
        if (cache.ContainsKey(state))
        {
            return cache[state];
        }

        Dijkstra(currentNode);

        int shortestPath = int.MaxValue;
        for (int letter = 0; letter < 26; letter++)
        {
            if ((uncollectedKeys & 1 << letter) == 0)
            {
                continue;
            }
            char key = (char)('a' + letter);
            if (!currentNode.KeyDistances.ContainsKey(key))
            {
                continue;
            }
            (int distance, int doors) = currentNode.KeyDistances[key];
            bool canOpenDoors = (doors & ~keysCollected) == 0;
            if (canOpenDoors)
            {
                MazeNode nextNode = MazeNode.KeyNodes[key];
                keysCollected |= 1 << (nextNode.Feature - 'a');
                nextNode.DijkstraKeyCollected = true;
                int nextSteps = StepsToCollectAllKeys(nextNode, ref keysCollected, allKeys, cache);
                keysCollected &= ~(1 << (nextNode.Feature - 'a'));
                nextNode.DijkstraKeyCollected = false;
                if (nextSteps < int.MaxValue)
                {
                    shortestPath = Math.Min(shortestPath, distance + nextSteps);
                }
            }
        }
        cache[state] = shortestPath;
        return shortestPath;
    }

    private static MazeNode[] SplitEntrance(MazeNode entranceNode)
    {
        if (entranceNode.Neighbors.Count != 4)
        {
            throw new ArgumentOutOfRangeException(nameof(entranceNode));
        }
        MazeNode[] output = new MazeNode[4];
        for (int i = 0; i < 4; i++)
        {
            output[i] = entranceNode.Neighbors.ElementAt(i);
        }
        for (int i = 0; i < 4; i++)
        {
            output[i].Feature = '@';
            entranceNode.Neighbors.Remove(output[i]);
            output[i].Neighbors.Remove(entranceNode);
            EdgeInfo.RemoveEdge(entranceNode, output[i]);
            for (int j = i + 1; j < 4; j++)
            {
                output[i].Neighbors.Remove(output[j]);
                output[j].Neighbors.Remove(output[i]);
                EdgeInfo.RemoveEdge(output[i], output[j]);
            }
        }
        return output;
    }

    private static int RobotStepsToCollectAllKeys(MazeNode[] currentNodes, ref int keysCollected, int allKeys, Dictionary<ulong, int> cache, int update)
    {
        int uncollectedKeys = allKeys & ~keysCollected;
        if (uncollectedKeys == 0)
        {
            return 0;
        }

        ulong state = ((ulong)currentNodes[3].Feature << 54)
            | ((ulong)currentNodes[2].Feature << 48)
            | ((ulong)currentNodes[1].Feature << 40)
            | ((ulong)currentNodes[0].Feature << 32)
            | (uint)uncollectedKeys;
        if (cache.ContainsKey(state))
        {
            return cache[state];
        }

        for (int i = 0; i < 4; i++)
        {
            if ((update & (1 << i)) > 0)
            {
                Dijkstra(currentNodes[i]);
            }
        }

        int shortestPath = int.MaxValue;
        for (int letter = 0; letter < 26; letter++)
        {
            if ((uncollectedKeys & 1 << letter) == 0)
            {
                continue;
            }
            char key = (char)('a' + letter);
            for (int robot = 0; robot < 4; robot++)
            {
                if (!currentNodes[robot].KeyDistances.ContainsKey(key))
                {
                    continue;
                }
                (int distance, int doors) = currentNodes[robot].KeyDistances[key];
                bool canOpenDoors = (doors & ~keysCollected) == 0;
                if (canOpenDoors)
                {
                    MazeNode nextNode = MazeNode.KeyNodes[key];
                    keysCollected |= 1 << (nextNode.Feature - 'a');
                    nextNode.DijkstraKeyCollected = true;
                    Dictionary<char, (int, int)> keyCache = new(currentNodes[robot].KeyDistances);
                    MazeNode previousNode = currentNodes[robot];
                    currentNodes[robot] = nextNode;
                    int nextSteps = RobotStepsToCollectAllKeys(currentNodes, ref keysCollected, allKeys, cache, 1 << robot);
                    currentNodes[robot] = previousNode;
                    currentNodes[robot].KeyDistances = keyCache;
                    keysCollected &= ~(1 << (nextNode.Feature - 'a'));
                    nextNode.DijkstraKeyCollected = false;
                    if (nextSteps < int.MaxValue)
                    {
                        shortestPath = Math.Min(shortestPath, distance + nextSteps);
                    }
                }
            }
        }
        cache[state] = shortestPath;
        return shortestPath;
    }

    private class MazeNode
    {
        public static List<MazeNode> AllNodes = new();
        public static Dictionary<char, MazeNode> KeyNodes = new();

        public int X { get; private set; }
        public int Y { get; private set; }
        public char Feature { get; set; }
        public HashSet<MazeNode> Neighbors { get; private set; } = new();
        public Dictionary<char, (int, int)> KeyDistances { get; set; } = new();

        public bool HasKey
        {
            get => ('a' <= Feature && Feature <= 'z') && !DijkstraKeyCollected;
        }
        public bool IsEntranceOrKey
        {
            get => Feature == '@' || ('a' <= Feature && Feature <= 'z');
        }

        public bool DijkstraKeyCollected { get; set; }
        public bool DijkstraVisited { get; set; }
        public int DijkstraDistance { get; set; }
        public int DijkstraDoors { get; set; }

        public MazeNode(int x, int y, char feature)
        {
            X = x;
            Y = y;
            Feature = feature;
            AllNodes.Add(this);
            if ('a' <= Feature && Feature <= 'z')
            {
                KeyNodes.Add(Feature, this);
            }
        }

        public MazeNode CreateNeighbor(int x, int y, int distance, int doors, char feature = '.')
        {
            MazeNode neighbor = new(x, y, feature);
            Neighbors.Add(neighbor);
            neighbor.Neighbors.Add(this);
            EdgeInfo.AddEdge(this, neighbor, distance, doors);
            return neighbor;
        }

        public static void ConnectNodes(MazeNode A, MazeNode B)
        {
            A.Neighbors.Add(B);
            B.Neighbors.Add(A);
            EdgeInfo.AddEdge(A, B, Math.Abs(A.X - B.X) + Math.Abs(A.Y - B.Y), 0);
        }

        public void CreateEntranceNeighbors()
        {
            MazeNode NW = CreateNeighbor(X - 1, Y - 1, 2, 0);
            MazeNode NE = CreateNeighbor(X + 1, Y - 1, 2, 0);
            MazeNode SW = CreateNeighbor(X - 1, Y + 1, 2, 0);
            MazeNode SE = CreateNeighbor(X + 1, Y + 1, 2, 0);
            ConnectNodes(NW, NE);
            ConnectNodes(NW, SW);
            ConnectNodes(NE, SE);
            ConnectNodes(SW, SE);
        }

        public static void DijkstraReset()
        {
            foreach (MazeNode node in AllNodes)
            {
                node.DijkstraVisited = false;
                node.DijkstraDistance = int.MaxValue;
                node.DijkstraDoors = 0;
            }
        }
    }

    private class MazeCrawler
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Steps { get; private set; }
        public int DoorsPassed { get; private set; }

        private readonly char[,] _map;
        private readonly MazeNode _parentNode;

        public MazeCrawler(int x, int y, char[,] map, MazeNode parentNode)
        {
            X = x;
            Y = y;
            Steps = 1;
            _map = map;
            _parentNode = parentNode;
        }

        public void FindNextNode()
        {
            if (_map[X, Y] == '@')
            {
                return;
            }
            if ('a' <= _map[X, Y] && _map[X, Y] <= 'z')
            {
                MazeNode newNode = _parentNode.CreateNeighbor(X, Y, Steps, DoorsPassed, _map[X, Y]);
                MapToNodes(_map, newNode);
                return;
            }
            if ('A' <= _map[X, Y] && _map[X, Y] <= 'Z')
            {
                DoorsPassed |= 1 << (_map[X, Y] - 'A');
            }
            _map[X, Y] = '#';
            int openDirections = 0;
            int openX = -1, openY = -1;
            for (int direction = 0; direction < 4; direction++)
            {
                Vector2Int step = _steps[direction];
                if (_map[X + step.X, Y + step.Y] == '#')
                {
                    continue;
                }
                openDirections++;
                openX = X + step.X;
                openY = Y + step.Y;
            }
            if (openDirections == 1)
            {
                X = openX;
                Y = openY;
                Steps++;
                FindNextNode();
            }
            else if (openDirections > 1)
            {
                MazeNode junction = _parentNode.CreateNeighbor(X, Y, Steps, DoorsPassed);
                MapToNodes(_map, junction);
            }
        }
    }

    private static class EdgeInfo
    {
        static readonly Dictionary<(MazeNode, MazeNode), (int, int)> _edges = new();

        public static void AddEdge(MazeNode A, MazeNode B, int distance, int doors)
        {
            _edges.Add((A, B), (distance, doors));
        }

        public static bool RemoveEdge(MazeNode A, MazeNode B)
        {
            return _edges.Remove((A, B)) | _edges.Remove((B, A));
        }

        public static int GetDistance(MazeNode A, MazeNode B)
        {
            if (_edges.ContainsKey((A, B)))
            {
                return _edges[(A, B)].Item1;
            }
            if (_edges.ContainsKey((B, A)))
            {
                return _edges[(B, A)].Item1;
            }
            return int.MaxValue;
        }

        public static int GetDoors(MazeNode A, MazeNode B)
        {
            if (_edges.ContainsKey((A, B)))
            {
                return _edges[(A, B)].Item2;
            }
            if (_edges.ContainsKey((B, A)))
            {
                return _edges[(B, A)].Item2;
            }
            return 0;
        }
    }
}
