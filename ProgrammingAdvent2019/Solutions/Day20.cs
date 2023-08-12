// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2019
// https://adventofcode.com/2019

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text.RegularExpressions;
using ProgrammingAdvent2019.Common;

namespace ProgrammingAdvent2019.Solutions;

internal class Day20 : Day
{
    private static readonly Regex _validCharacters = new("^[ #.A-Z]+$");
    private static readonly Vector2Int[] _directions = new Vector2Int[]
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
        foreach (string line in inputLines)
        {
            if (line.Length == 0)
            {
                errorMessage = "Input contains an empty line.";
                return false;
            }
            if (line.Length != inputLines[0].Length)
            {
                errorMessage = "All lines in input are not the same length.";
                return false;
            }
            if (!_validCharacters.IsMatch(line))
            {
                errorMessage = $"Input \"{line.Left(20, true)}\" contains invalid characters.";
                return false;
            }
        }
        if (inputLines.Length < 15 || inputLines[0].Length < 15)
        {
            errorMessage = "Input is too small.";
            return false;
        }

        char[,] map = inputLines.ToCharArray2D();
        if (!TryFindInteriorSpace(map, out Rectangle interior))
        {
            errorMessage = "Unable to find interior space.";
            return false;
        }
        Dictionary<(int, int), string> outerLabels = OuterLabels(map);
        Dictionary<(int, int), string> innerLabels = InnerLabels(map, interior);

        List<string> outerList = outerLabels.Values.ToList();
        if (outerList.Count == 0)
        {
            errorMessage = "Maze contains no entrances.";
            return false;
        }
        for (int i = 1; i < outerList.Count; i++)
        {
            if (outerList[i] == outerList[i - 1])
            {
                errorMessage = $"Duplicate outer label \"{outerList[i]}\".";
                return false;
            }
        }
        List<string> innerList = innerLabels.Values.ToList();
        for (int i = 1; i < innerList.Count; i++)
        {
            if (innerList[i] == innerList[i - 1])
            {
                errorMessage = $"Duplicate inner label \"{innerList[i]}\".";
                return false;
            }
        }
        foreach (string label in new string[] { "AA", "ZZ" })
        {
            if (innerList.Contains(label))
            {
                errorMessage = $"Label \"{label}\" is in an invalid position.";
                return false;
            }
            if (!outerList.Contains(label))
            {
                errorMessage = $"Label \"{label}\" is missing.";
                return false;
            }
            outerList.Remove(label);
        }

        outerList.Sort();
        innerList.Sort();
        if (outerList.Count != innerList.Count)
        {
            errorMessage = "Mismatched number of labels on outer edge versus inner edge.";
            return false;
        }
        for (int i = 0; i < outerList.Count; i++)
        {
            if (outerList[i] != innerList[i])
            {
                errorMessage = "Labels on inner edge do not match labels on outer edge.";
                return false;
            }
            if (!AreCapitalLetters(outerList[i][0], outerList[i][1]))
            {
                errorMessage = $"Invalid label \"{outerList[i]}\".";
                return false;
            }
        }

        int width = map.GetLength(0);
        int height = map.GetLength(1);
        if (map[2, 2] != '#' || map[2, height - 3] != '#'
            || map[width - 3, 2] != '#' || map[width - 3, height - 3] != '#')
        {
            errorMessage = "All four corners are not '#'.";
            return false;
        }
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x < 2 || x >= width - 2 || y < 2 || y >= height - 2)
                {
                    if (map[x, y] != ' ')
                    {
                        errorMessage = $"Unexpected character {map[x, y]} at {x},{y}.";
                        return false;
                    }
                }
                else if (x < interior.X || x >= interior.Right || y < interior.Y || y >= interior.Bottom)
                {
                    if (map[x, y] != '#' && map[x, y] != '.' && map[x, y] != '*')
                    {
                        errorMessage = $"Unexpected character {map[x, y]} at {x},{y}.";
                        return false;
                    }
                }
                else if (map[x, y] != ' ')
                {
                    errorMessage = $"Unexpected character {map[x, y]} at {x},{y}.";
                    return false;
                }
            }
        }

        errorMessage = string.Empty;
        return true;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        char[,] map = inputLines.ToCharArray2D();
        TryFindInteriorSpace(map, out Rectangle interior);
        Dictionary<(int, int), string> outerLabels = OuterLabels(map);
        Dictionary<(int, int), string> innerLabels = InnerLabels(map, interior);
        PruneDeadEnds(map);

        MazeGraph graph = new(map, outerLabels, innerLabels);
        int partOneAnswer = graph.ShortestPath(0);
        int partTwoAnswer = graph.ShortestPath(1);

        return output.WriteAnswers(partOneAnswer, partTwoAnswer);
    }

    private static bool TryFindInteriorSpace(char[,] map, [NotNullWhen(true)] out Rectangle interior)
    {
        int centerX = map.GetLength(0) / 2;
        int centerY = map.GetLength(1) / 2;
        int interiorX = centerX, interiorY = centerY, width = 0, height = 0;
        for (int y = centerY; y >= 4; y--)
        {
            if (map[centerX, y] == '#' || map[centerX, y] == '.')
            {
                interiorY = y + 1;
                break;
            }
        }
        for (int y = centerY; y < map.GetLength(1) - 4; y++)
        {
            if (map[centerX, y] == '#' || map[centerX, y] == '.')
            {
                height = y - interiorY;
                break;
            }
        }
        for (int x = centerX; x >= 4; x--)
        {
            if (map[x, centerY] == '#' || map[x, centerY] == '.')
            {
                interiorX = x + 1;
                break;
            }
        }
        for (int x = centerX; x < map.GetLength(0) - 4; x++)
        {
            if (map[x, centerY] == '#' || map[x, centerY] == '.')
            {
                width = x - interiorX;
                break;
            }
        }
        if (interiorX == centerX || interiorY == centerY || width < 5 || height < 5)
        {
            interior = Rectangle.Empty;
            return false;
        }
        interior = new Rectangle(interiorX, interiorY, width, height);
        return true;
    }

    private static Dictionary<(int, int), string> OuterLabels(char[,] map)
    {
        char labelClear = ' ';
        char portal = '*';
        Dictionary<(int, int), string> labels = new();
        int limitX = map.GetLength(0) - 3;
        int limitY = map.GetLength(1) - 3;
        for (int y = 3; y < limitY; y++)
        {
            // Left edge
            if (map[2, y] == '.')
            {
                labels.Add((2, y), $"{map[0, y]}{map[1, y]}");
                map[0, y] = labelClear;
                map[1, y] = labelClear;
                map[2, y] = portal;
            }
            // Right edge
            if (map[limitX, y] == '.')
            {
                labels.Add((limitX, y), $"{map[limitX + 1, y]}{map[limitX + 2, y]}");
                map[limitX + 1, y] = labelClear;
                map[limitX + 2, y] = labelClear;
                map[limitX, y] = portal;
            }
        }
        for (int x = 3; x < limitX; x++)
        {
            // Top edge
            if (map[x, 2] == '.')
            {
                labels.Add((x, 2), $"{map[x, 0]}{map[x, 1]}");
                map[x, 0] = labelClear;
                map[x, 1] = labelClear;
                map[x, 2] = portal;
            }
            // Bottom edge
            if (map[x, limitY] == '.')
            {
                labels.Add((x, limitY), $"{map[x, limitY + 1]}{map[x, limitY + 2]}");
                map[x, limitY + 1] = labelClear;
                map[x, limitY + 2] = labelClear;
                map[x, limitY] = portal;
            }
        }
        return labels;
    }

    private static Dictionary<(int, int), string> InnerLabels(char[,] map, Rectangle interior)
    {
        char labelClear = ' ';
        char portal = '*';
        Dictionary<(int, int), string> labels = new();
        int limitX = interior.Right;
        int limitY = interior.Bottom;
        for (int y = interior.Y; y < limitY; y++)
        {
            // Left edge
            if (map[interior.X - 1, y] == '.')
            {
                labels.Add((interior.X - 1, y), $"{map[interior.X, y]}{map[interior.X + 1, y]}");
                map[interior.X, y] = labelClear;
                map[interior.X + 1, y] = labelClear;
                map[interior.X - 1, y] = portal;
            }
            // Right edge
            if (map[limitX, y] == '.')
            {
                labels.Add((limitX, y), $"{map[limitX - 2, y]}{map[limitX - 1, y]}");
                map[limitX - 2, y] = labelClear;
                map[limitX - 1, y] = labelClear;
                map[limitX, y] = portal;
            }
        }
        for (int x = interior.X; x < limitX; x++)
        {
            // Top edge
            if (map[x, interior.Y - 1] == '.')
            {
                labels.Add((x, interior.Y - 1), $"{map[x, interior.Y]}{map[x, interior.Y + 1]}");
                map[x, interior.Y] = labelClear;
                map[x, interior.Y + 1] = labelClear;
                map[x, interior.Y - 1] = portal;
            }
            // Bottom edge
            if (map[x, limitY] == '.')
            {
                labels.Add((x, limitY), $"{map[x, limitY - 2]}{map[x, limitY - 1]}");
                map[x, limitY - 2] = labelClear;
                map[x, limitY - 1] = labelClear;
                map[x, limitY] = portal;
            }
        }
        return labels;
    }

    private static bool AreCapitalLetters(char one, char two)
    {
        return 'A' <= one && one <= 'Z' && 'A' <= two && two <= 'Z';
    }

    private static void PruneDeadEnds(char[,] map)
    {
        for (int y = 2; y < map.GetLength(1) - 2; y++)
        {
            for (int x = 2; x < map.GetLength(0) - 2; x++)
            {
                PruneDeadEnd(map, x, y);
            }
        }
    }

    private static void PruneDeadEnd(char[,] map, int x, int y)
    {
        if (map[x, y] != '.')
        {
            return;
        }
        int walls = 0;
        int openX = -1;
        int openY = -1;
        foreach (Vector2Int step in _directions)
        {
            if (map[x + step.X, y + step.Y] == '#' || map[x + step.X, y + step.Y] == ' ')
            {
                walls++;
            }
            else
            {
                openX = x + step.X;
                openY = y + step.Y;
            }
        }
        if (walls == 4)
        {
            map[x, y] = '#';
            return;
        }
        if (walls == 3)
        {
            map[x, y] = '#';
            PruneDeadEnd(map, openX, openY);
        }
    }

    private class MazeGraph
    {
        private readonly Dictionary<(int, int), (int, int)> _innerPortalDestinations;
        private readonly Dictionary<(int, int), (int, int)> _outerPortalDestinations;

        private readonly Dictionary<int, MazeLayer> _layers = new();
        private readonly MazeLayer _template;

        private readonly Vector2Int _mazeStart;

        public MazeGraph(char[,] map, Dictionary<(int, int), string> outerLabels, Dictionary<(int, int), string> innerLabels)
        {
            _innerPortalDestinations = new();
            _outerPortalDestinations = new();
            _mazeStart = Vector2Int.Zero;
            foreach (var outerKVP in outerLabels)
            {
                foreach (var innerKVP in innerLabels)
                {
                    if (outerKVP.Value == innerKVP.Value)
                    {
                        _outerPortalDestinations.Add(outerKVP.Key, innerKVP.Key);
                        _innerPortalDestinations.Add(innerKVP.Key, outerKVP.Key);
                    }
                }
                if (outerKVP.Value == "AA")
                {
                    _mazeStart = new Vector2Int(outerKVP.Key.Item1, outerKVP.Key.Item2);
                }
            }
            if (_mazeStart == Vector2Int.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(outerLabels));
            }
            _template = new(map, outerLabels, innerLabels);
            _layers.Add(0, new MazeLayer(_template, 0));
        }

        public int ShortestPath(int recursion)
        {
            foreach (MazeLayer layer in _layers.Values)
            {
                layer.SearchReset();
            }
            MazeNode entrance = GetNode(_mazeStart.X, _mazeStart.Y, 0);
            entrance.SearchDistance = 0;
            HashSet<MazeNode> seenNodes = new() { entrance };
            int timeout = 0;
            while (++timeout < 32_768)
            {
                MazeNode? current = seenNodes.OrderBy(n => n.SearchDistance).FirstOrDefault();
                if (current is null || current.SearchDistance == int.MaxValue)
                {
                    break;
                }
                if (current.Label == "ZZ" && current.Z == 0)
                {
                    return current.SearchDistance;
                }
                foreach (var kvp in current.NeighborDistances)
                {
                    MazeNode neighbor = GetNode(kvp.Key >> 16, kvp.Key & 0xFFFF, current.Z);
                    if (neighbor.SearchVisited)
                    {
                        continue;
                    }
                    seenNodes.Add(neighbor);
                    int newDistance = current.SearchDistance + kvp.Value;
                    neighbor.SearchDistance = Math.Min(neighbor.SearchDistance, newDistance);
                }
                MazeNode? portalNeighbor = null;
                if (_outerPortalDestinations.ContainsKey((current.X, current.Y)))
                {
                    int Z = current.Z - recursion;
                    if (Z >= 0)
                    {
                        (int X, int Y) = _outerPortalDestinations[(current.X, current.Y)];
                        portalNeighbor = GetNode(X, Y, Z);
                    }
                }
                else if (_innerPortalDestinations.ContainsKey((current.X, current.Y)))
                {
                    int Z = current.Z + recursion;
                    (int X, int Y) = _innerPortalDestinations[(current.X, current.Y)];
                    portalNeighbor = GetNode(X, Y, Z);
                }
                if (portalNeighbor is not null && !portalNeighbor.SearchVisited)
                {
                    seenNodes.Add(portalNeighbor);
                    int newDistance = current.SearchDistance + 1;
                    portalNeighbor.SearchDistance = Math.Min(portalNeighbor.SearchDistance, newDistance);
                }
                current.SearchVisited = true;
                seenNodes.Remove(current);
            }
            return int.MaxValue;
        }

        private MazeNode GetNode(int x, int y, int z)
        {
            if (!_layers.ContainsKey(z))
            {
                _layers.Add(z, new MazeLayer(_template, z));
            }
            return _layers[z].GetNode(x, y);
        }
    }

    private class MazeLayer
    {
        public Dictionary<int, MazeNode> Nodes { get; private set; }

        private readonly Dictionary<(int, int), string> _labels = new();

        public MazeLayer(char[,] map, Dictionary<(int, int), string> outerLabels, Dictionary<(int, int), string> innerLabels)
        {
            Nodes = new();
            _labels = new(outerLabels);
            foreach (var kvp in innerLabels)
            {
                _labels.Add(kvp.Key, kvp.Value);
            }
            (int startX, int startY) = outerLabels.Where(kvp => kvp.Value == "AA").First().Key;
            MazeNode entrance = new(startX, startY, "AA");
            Nodes.Add(startX << 16 | startY, entrance);
            FindNeighbors(entrance, map);
            foreach ((int X, int Y) inner in innerLabels.Keys)
            {
                MazeNode innerPortal = GetNode(inner.X, inner.Y);
                if (HasMatchingPortal(innerPortal, out (int X, int Y) outer))
                {
                    MazeNode outerPortal = GetNode(outer.X, outer.Y);
                    innerPortal.NeighborDistances.Remove(outer.X << 16 | outer.Y);
                    outerPortal.NeighborDistances.Remove(inner.X << 16 | inner.Y);
                }
            }
        }

        public MazeLayer(MazeLayer template, int depth)
        {
            Nodes = new();
            foreach (var kvp in template.Nodes)
            {
                Nodes.Add(kvp.Key, new(kvp.Value, depth));
            }
        }

        public void SearchReset()
        {
            foreach (MazeNode node in Nodes.Values)
            {
                node.SearchVisited = false;
                node.SearchDistance = int.MaxValue;
            }
        }

        public MazeNode GetNode(int x, int y)
        {
            int key = x << 16 | y;
            return Nodes[key];
        }

        private void FindNeighbors(MazeNode source, char[,] map)
        {
            int sourcePosition = source.X << 16 | source.Y;
            int lookX, lookY;
            foreach (Vector2Int step in _directions)
            {
                lookX = source.X + step.X;
                lookY = source.Y + step.Y;
                if (map[lookX, lookY] != '.' && map[lookX, lookY] != '*' && map[lookX, lookY] != '+')
                {
                    continue;
                }
                (MazeNode neighbor, int distance) = FindNextNode(lookX, lookY, map, source);
                int neighborPosition = neighbor.X << 16 | neighbor.Y;
                if (source.NeighborDistances.ContainsKey(neighborPosition))
                {
                    source.NeighborDistances[neighborPosition] = Math.Min(source.NeighborDistances[neighborPosition], distance);
                    neighbor.NeighborDistances[sourcePosition] = Math.Min(neighbor.NeighborDistances[sourcePosition], distance);
                }
                else
                {
                    source.NeighborDistances.Add(neighborPosition, distance);
                    neighbor.NeighborDistances.Add(sourcePosition, distance);
                    FindNeighbors(neighbor, map);
                }
            }

            if (HasMatchingPortal(source, out (int X, int Y) remote))
            {
                int remotePosition = remote.X << 16 | remote.Y;
                if (Nodes.ContainsKey(remotePosition))
                {
                    if (!source.NeighborDistances.ContainsKey(remotePosition))
                    {
                        MazeNode remoteNode = Nodes[remotePosition];
                        source.NeighborDistances.Add(remotePosition, 1);
                        remoteNode.NeighborDistances.Add(sourcePosition, 1);
                        FindNeighbors(remoteNode, map);
                    }
                }
                else
                {
                    MazeNode newRemote = new(remote.X, remote.Y, source.Label);
                    Nodes.Add(remotePosition, newRemote);
                    source.NeighborDistances.Add(remotePosition, 1);
                    newRemote.NeighborDistances.Add(sourcePosition, 1);
                    FindNeighbors(newRemote, map);
                }
            }
        }

        private bool HasMatchingPortal(MazeNode local, out (int, int) destination)
        {
            var matchingPortals = _labels.Where(kvp => kvp.Key != (local.X, local.Y) && kvp.Value == local.Label);
            if (!matchingPortals.Any())
            {
                destination = (-1, -1);
                return false;
            }
            destination = matchingPortals.First().Key;
            return true;
        }

        private (MazeNode, int) FindNextNode(int x, int y, char[,] map, MazeNode source)
        {
            if (map[x, y] == '*' || map[x, y] == '+')
            {
                if (Nodes.ContainsKey(x << 16 | y))
                {
                    return (Nodes[x << 16 | y], 1);
                }
                string label = string.Empty;
                if (map[x, y] == '*')
                {
                    label = _labels[(x, y)];
                }
                MazeNode next = new(x, y, label);
                Nodes.Add(x << 16 | y, next);
                return (next, 1);
            }

            int openDirections = 0;
            int openX = -1, openY = -1;
            foreach (Vector2Int step in _directions)
            {
                int lookX = x + step.X;
                int lookY = y + step.Y;
                if (lookX == source.X && lookY == source.Y)
                {
                    continue;
                }
                if (map[lookX, lookY] != '.' &&
                    map[lookX, lookY] != '*' &&
                    map[lookX, lookY] != '+')
                {
                    continue;
                }
                openDirections++;
                openX = x + step.X;
                openY = y + step.Y;
            }
            if (openDirections == 1)
            {
                map[x, y] = 'X';
                (MazeNode next, int distance) = FindNextNode(openX, openY, map, source);
                return (next, distance + 1);
            }
            else if (openDirections > 1)
            {
                map[x, y] = '+';
                if (Nodes.ContainsKey(x << 16 | y))
                {
                    return (Nodes[x << 16 | y], 1);
                }
                MazeNode next = new(x, y, string.Empty);
                Nodes.Add(x << 16 | y, next);
                return (next, 1);
            }
            else
            {
                // This should never happen because dead ends should have been removed.
                throw new InvalidOperationException();
            }
        }
    }

    private class MazeNode
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        public string Label { get; private set; }

        public Dictionary<int, int> NeighborDistances { get; private set; }

        public bool SearchVisited { get; set; } = false;
        public int SearchDistance { get; set; } = int.MaxValue;

        public MazeNode(int x, int y, string label)
        {
            X = x;
            Y = y;
            Z = 0;
            Label = label;
            NeighborDistances = new();
        }

        public MazeNode(MazeNode nodeToCopy, int depth)
        {
            X = nodeToCopy.X;
            Y = nodeToCopy.Y;
            Z = depth;
            Label = nodeToCopy.Label;
            NeighborDistances = new();
            foreach (var kvp in nodeToCopy.NeighborDistances)
            {
                NeighborDistances.Add(kvp.Key, kvp.Value);
            }
        }
    }
}
