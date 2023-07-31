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
    private static readonly Regex _validOuterLabel = new(@"^([A-Z]{2}\.|  #)$");
    private static readonly Regex _validInnerLabel = new(@"^([A-Z]{2}\.|[A-Z ]{2}#)$");
    private static readonly Vector2Int[] _steps = new Vector2Int[]
    {
        new Vector2Int(-1,  0),
        new Vector2Int( 0, -1),
        new Vector2Int( 1,  0),
        new Vector2Int( 0,  1)
    };

    private readonly Dictionary<int, int> _portals = new();

    public override bool ValidateInput(string[] inputLines, out string errorMessage)
    {
        // Check overall input.
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
        // Create map.
        char[,] map = inputLines.ToCharArray2D();
        // Check the corners.
        (int X, int Y)[] corner = new (int, int)[]
        {
            (0, 0),
            (map.GetLength(0) - 2, 0),
            (0, map.GetLength(1) - 2),
            (map.GetLength(0) - 2, map.GetLength(1) - 2)
        };
        for (int i = 0; i < 4; i++)
        {
            for (int y = corner[i].Y; y < corner[i].Y + 2; y++)
            {
                for (int x = corner[i].X; x < corner[i].X + 2; x++)
                {
                    if (map[x, y] != ' ')
                    {
                        errorMessage = $"Unexpected character '{map[x, y]}' at {x},{y}.";
                        return false;
                    }
                }
            }
        }
        // Keep track of labels found.
        List<string> labels = new();
        // Check the outer labels.
        int ox = map.GetLength(0) - 3;
        for (int y = 2; y < map.GetLength(1) - 3; y++)
        {
            string left = string.Join(null, new char[] { map[0, y], map[1, y], map[2, y] });
            if (!_validOuterLabel.IsMatch(left))
            {
                errorMessage = $"Invalid horizontal data \"{left}\".";
                return false;
            }
            string right = string.Join(null, new char[] { map[ox + 1, y], map[ox + 2, y], map[ox, y] });
            if (!_validOuterLabel.IsMatch(right))
            {
                errorMessage = $"Invalid horizontal data \"{right}\".";
                return false;
            }
            if (left != "  #")
            {
                labels.Add(left);
            }
            if (right != "  #")
            {
                labels.Add(right);
            }
        }
        int oy = map.GetLength(1) - 3;
        for (int x = 2; x < map.GetLength(0) - 3; x++)
        {
            string top = string.Join(null, new char[] { map[x, 0], map[x, 1], map[x, 2] });
            if (!_validOuterLabel.IsMatch(top))
            {
                errorMessage = $"Invalid vertical data \"{top}\".";
                return false;
            }
            string bottom = string.Join(null, new char[] { map[x, oy + 1], map[x, oy + 2], map[x, oy] });
            if (!_validOuterLabel.IsMatch(bottom))
            {
                errorMessage = $"Invalid vertical data \"{bottom}\".";
                return false;
            }
            if (top != "  #")
            {
                labels.Add(top);
            }
            if (bottom != "  #")
            {
                labels.Add(bottom);
            }
        }
        // Check the inner labels.
        Rectangle interior = InteriorSpace(map);
        if (interior.Width < 5 || interior.Height < 5)
        {
            errorMessage = "Inner area of maze is too small.";
            return false;
        }
        int ix = interior.X + interior.Width;
        for (int y = interior.Y; y < interior.Y + interior.Height; y++)
        {
            string left = string.Join(null, new char[] { map[interior.X, y], map[interior.X + 1, y], map[interior.X - 1, y] });
            if (!_validInnerLabel.IsMatch(left))
            {
                errorMessage = $"Invalid horizontal data \"{left}\".";
                return false;
            }
            string right = string.Join(null, new char[] { map[ix - 2, y], map[ix - 1, y], map[ix, y] });
            if (!_validInnerLabel.IsMatch(right))
            {
                errorMessage = $"Invalid horizontal data \"{right}\".";
                return false;
            }
            if (left[2] == '.')
            {
                labels.Add(left);
                map[interior.X, y] = ' ';
                map[interior.X + 1, y] = ' ';
            }
            if (right[2] == '.')
            {
                labels.Add(right);
                map[ix - 2, y] = ' ';
                map[ix - 1, y] = ' ';
            }
        }
        int iy = interior.Y + interior.Height;
        for (int x = interior.X; x < interior.X + interior.Width; x++)
        {
            string top = string.Join(null, new char[] { map[x, interior.Y], map[x, interior.Y + 1], map[x, interior.Y - 1] });
            if (!_validInnerLabel.IsMatch(top))
            {
                errorMessage = $"Invalid vertical data \"{top}\".";
                return false;
            }
            string bottom = string.Join(null, new char[] { map[x, iy - 2], map[x, iy - 1], map[x, iy] });
            if (!_validInnerLabel.IsMatch(bottom))
            {
                errorMessage = $"Invalid vertical data \"{bottom}\".";
                return false;
            }
            if (top[2] == '.')
            {
                labels.Add(top);
                map[x, interior.Y] = ' ';
                map[x, interior.Y + 1] = ' ';
            }
            if (bottom[2] == '.')
            {
                labels.Add(bottom);
                map[x, iy - 2] = ' ';
                map[x, iy - 1] = ' ';
            }
        }
        // Check for stray characters in the interior.
        for (int y = interior.Y; y < interior.Y + interior.Height; y++)
        {
            for (int x = interior.X; x < interior.X + interior.Width; x++)
            {
                if (map[x, y] != ' ')
                {
                    errorMessage = $"Unexpected character '{map[x, y]}' at {x},{y}.";
                    return false;
                }
            }
        }
        // Check the set of labels.
        if (labels.Count == 0)
        {
            errorMessage = "Input contains no labels.";
            return false;
        }
        if (labels.Count % 2 == 1)
        {
            errorMessage = "Input contains an odd number of labels.";
            return false;
        }
        if (!labels.Contains("AA."))
        {
            errorMessage = "Input is missing label \"AA\".";
            return false;
        }
        if (!labels.Contains("ZZ."))
        {
            errorMessage = "Input is missing label \"ZZ\".";
            return false;
        }
        labels.Sort();
        bool hasMismatchedLabels = false;
        if (labels[0] != "AA." || labels[^1] != "ZZ." || labels[^2] == labels[^1])
        {
            hasMismatchedLabels = true;
        }
        else
        {
            for (int i = 1; i < labels.Count - 1; i += 2)
            {
                if (labels[i] != labels[i + 1])
                {
                    hasMismatchedLabels = true;
                    break;
                }
                if (labels[i] == labels[i - 1])
                {
                    hasMismatchedLabels = true;
                    break;
                }
                if (labels[i][0] == ' ' || labels[i][1] == ' ')
                {
                    errorMessage = $"Invalid label \"{labels[i]}\"";
                    return false;
                }
                if (labels[i + 1][0] == ' ' || labels[i + 1][1] == ' ')
                {
                    errorMessage = $"Invalid label \"{labels[i + 1]}\"";
                    return false;
                }
            }
        }
        if (hasMismatchedLabels)
        {
            errorMessage = "Input has mismatched labels.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static Rectangle InteriorSpace(char[,] map)
    {
        Rectangle output = new();
        (int centerX, int centerY) = (map.GetLength(0) / 2, map.GetLength(1) / 2);
        for (int y = centerY; y >= 0; y--)
        {
            if (map[centerX, y] == '#' || map[centerX, y] == '.')
            {
                output.Y = y + 1;
                break;
            }
        }
        for (int y = centerY; y < map.GetLength(1); y++)
        {
            if (map[centerX, y] == '#' || map[centerX, y] == '.')
            {
                output.Height = y - output.Y;
                break;
            }
        }
        for (int x = centerX; x >= 0; x--)
        {
            if (map[x, centerY] == '#' || map[x, centerY] == '.')
            {
                output.X = x + 1;
                break;
            }
        }
        for (int x = centerX; x < map.GetLength(0); x++)
        {
            if (map[x, centerY] == '#' || map[x, centerY] == '.')
            {
                output.Width = x - output.X;
                break;
            }
        }
        return output;
    }

    protected override PuzzleAnswers CalculateAnswers(string[] inputLines, string? exampleModifier = null)
    {
        PuzzleAnswers output = new();

        char[,] map = inputLines.ToCharArray2D();
        FindPortals(map);
        PruneDeadEnds(map);
        MazeNode entrance = BuildGraph(map);
        int partOneAnswer = ShortestPathLength(entrance, "ZZ");

        return output.WriteAnswers(partOneAnswer, null);
    }

    private void FindPortals(char[,] map)
    {
        _portals.Clear();
        // Add the outer portals.
        int ox = map.GetLength(0) - 3;
        int oy = map.GetLength(1) - 3;
        for (int y = 2; y < oy; y++)
        {
            if (map[2, y] == '.')
            {
                int coord = 2 << 16 | y;
                int label = map[0, y] << 16 | map[1, y];
                _portals.Add(coord, label);
                map[2, y] = '*';
            }
            if (map[ox, y] == '.')
            {
                int coord = ox << 16 | y;
                int label = map[ox + 1, y] << 16 | map[ox + 2, y];
                _portals.Add(coord, label);
                map[ox, y] = '*';
            }
        }
        for (int x = 2; x < ox; x++)
        {
            if (map[x, 2] == '.')
            {
                int coord = x << 16 | 2;
                int label = map[x, 0] << 16 | map[x, 1];
                _portals.Add(coord, label);
                map[x, 2] = '*';
            }
            if (map[x, oy] == '.')
            {
                int coord = x << 16 | oy;
                int label = map[x, oy + 1] << 16 | map[x, oy + 2];
                _portals.Add(coord, label);
                map[x, oy] = '*';
            }
        }
        // Add the inner portals.
        Rectangle interior = InteriorSpace(map);
        int ix = interior.X + interior.Width;
        int iy = interior.Y + interior.Height;
        for (int y = interior.Y; y < iy; y++)
        {
            if (map[interior.X - 1, y] == '.')
            {
                int coord = (interior.X - 1) << 16 | y;
                int label = map[interior.X, y] << 16 | map[interior.X + 1, y];
                _portals.Add(coord, label);
                map[interior.X - 1, y] = '*';
            }
            if (map[ix, y] == '.')
            {
                int coord = ix << 16 | y;
                int label = map[ix - 2, y] << 16 | map[ix - 1, y];
                _portals.Add(coord, label);
                map[ix, y] = '*';
            }
        }
        for (int x = interior.X; x < ix; x++)
        {
            if (map[x, interior.Y - 1] == '.')
            {
                int coord = x << 16 | (interior.Y - 1);
                int label = map[x, interior.Y] << 16 | map[x, interior.Y + 1];
                _portals.Add(coord, label);
                map[x, interior.Y - 1] = '*';
            }
            if (map[x, iy] == '.')
            {
                int coord = x << 16 | iy;
                int label = map[x, iy - 2] << 16 | map[x, iy - 1];
                _portals.Add(coord, label);
                map[x, iy] = '*';
            }
        }
    }

    private static void PruneDeadEnds(char[,] map)
    {
        for (int y = 3; y < map.GetLength(1) - 3; y++)
        {
            for (int x = 3; x < map.GetLength(0) - 3; x++)
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
        foreach (Vector2Int step in _steps)
        {
            if (map[x + step.X, y + step.Y] == '#')
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

    private MazeNode BuildGraph(char[,] map)
    {
        MazeNode.AllNodes.Clear();
        int AA = 'A' << 16 | 'A';
        int mazeStart = _portals.Where(kvp => kvp.Value == AA).First().Key;
        MazeNode root = new(mazeStart >> 16, mazeStart & 0xFFFF, _portals[mazeStart]);
        FindNeighbors(root, map);
        return root;
    }

    private void FindNeighbors(MazeNode source, char[,] map)
    {
        int lookX, lookY;
        foreach (Vector2Int step in _steps)
        {
            lookX = source.X + step.X;
            lookY = source.Y + step.Y;
            if (map[lookX, lookY] != '.' && map[lookX, lookY] != '*' && map[lookX, lookY] != '+')
            {
                continue;
            }
            (MazeNode neighbor, int distance) = FindNextNode(lookX, lookY, map, source);
            if (source.Neighbors.ContainsKey(neighbor))
            {
                source.Neighbors[neighbor] = Math.Min(source.Neighbors[neighbor], distance);
                neighbor.Neighbors[source] = Math.Min(neighbor.Neighbors[source], distance);
            }
            else
            {
                source.Neighbors.Add(neighbor, distance);
                neighbor.Neighbors.Add(source, distance);
                FindNeighbors(neighbor, map);
            }
        }
        (int remoteX, int remoteY) = OtherPortalCoordinates(source);
        if (remoteX >= 0 && remoteY >= 0)
        {
            if (MazeNode.FindNode(remoteX, remoteY, out MazeNode? remote))
            {
                if (!source.Neighbors.ContainsKey(remote))
                {
                    source.Neighbors.Add(remote, 1);
                    remote.Neighbors.Add(source, 1);
                    FindNeighbors(remote, map);
                }
            }
            else
            {
                MazeNode newRemote = new(remoteX, remoteY, map[remoteX, remoteY]);
                source.Neighbors.Add(newRemote, 1);
                newRemote.Neighbors.Add(source, 1);
                FindNeighbors(newRemote, map);
            }
        }
    }

    private (MazeNode, int) FindNextNode(int x, int y, char[,] map, MazeNode source)
    {
        if (map[x, y] == '*' || map[x, y] == '+')
        {
            if (MazeNode.FindNode(x, y, out MazeNode? node))
            {
                return (node, 1);
            }
            int label = map[x, y] == '+' ? 0 : _portals[x << 16 | y];
            MazeNode next = new(x, y, label);
            return (next, 1);
        }

        int openDirections = 0;
        int openX = -1, openY = -1;
        foreach (Vector2Int step in _steps)
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
            if (MazeNode.FindNode(x, y, out MazeNode? node))
            {
                return (node, 1);
            }
            MazeNode next = new(x, y, 0);
            return (next, 1);
        }
        else
        {
            // This should never happen because dead ends should have been removed.
            throw new InvalidOperationException();
        }
    }

    private (int, int) OtherPortalCoordinates(MazeNode local)
    {
        int originCoords = local.X << 16 | local.Y;
        var matchingPortals = _portals.Where(kvp => kvp.Key != originCoords && kvp.Value == local.Label);
        if (!matchingPortals.Any())
        {
            return (-1, -1);
        }
        int code = matchingPortals.First().Key;
        return (code >> 16, code & 0xFFFF);
    }

    private static int ShortestPathLength(MazeNode entrance, string goal)
    {
        entrance.DijkstraDistance = 0;
        List<MazeNode> unvisitedNodes = new(MazeNode.AllNodes);
        int goalLabel = goal[0] << 16 | goal[1];
        int timeout = 0;
        while (timeout++ < 1000)
        {
            MazeNode? current = unvisitedNodes.OrderBy(n => n.DijkstraDistance).FirstOrDefault();
            if (current is null || current.DijkstraDistance == int.MaxValue)
            {
                break;
            }
            if (current.Label == goalLabel)
            {
                return current.DijkstraDistance;
            }
            var unvisitedNeighbors = current.Neighbors.Where(n => !n.Key.DijkstraVisited);
            foreach (var kvp in unvisitedNeighbors)
            {
                int distance = current.DijkstraDistance + kvp.Value;
                kvp.Key.DijkstraDistance = Math.Min(kvp.Key.DijkstraDistance, distance);
            }
            current.DijkstraVisited = true;
            unvisitedNodes.Remove(current);
        }
        return int.MaxValue;
    }

    private class MazeNode
    {
        public static HashSet<MazeNode> AllNodes = new();

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Label { get; private set; }
        public Dictionary<MazeNode, int> Neighbors { get; private set; } = new();

        public bool DijkstraVisited { get; set; } = false;
        public int DijkstraDistance { get; set; } = int.MaxValue;

        public MazeNode(int x, int y, int label)
        {
            X = x;
            Y = y;
            Label = label;
            AllNodes.Add(this);
        }

        public static bool FindNode(int x, int y, [NotNullWhen(true)] out MazeNode? node)
        {
            node = AllNodes.Where(n => n.X == x && n.Y == y).FirstOrDefault();
            return node is not null;
        }
    }
}
