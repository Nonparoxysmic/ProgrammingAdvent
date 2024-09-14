// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day15 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        int width = input[0].Length;
        int height = input.Length;
        int[,] map = new int[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, y] = input[y][x] - '0';
            }
        }
        int partOneAnswer = PathLength(map);
        return ($"{partOneAnswer}", "n/a");
    }

    private static int PathLength(int[,] map)
    {
        Node.Initialize(0, 0, map.GetLength(0) - 1, map.GetLength(1) - 1);
        HashSet<Node> openNodes = [];
        openNodes.Add(Node.GetNode(0, 0));
        while (openNodes.Count != 0)
        {
            Node current = openNodes.Aggregate((a, b) => a.Score < b.Score ? a : b);
            if (current.DistanceFromEnd == 0)
            {
                return current.PathLengthFromStart;
            }
            openNodes.Remove(current);
            foreach (Node neighbor in current.GetNeighbors())
            {
                int pathThroughCurrent = current.PathLengthFromStart + map[neighbor.X, neighbor.Y];
                if (pathThroughCurrent < neighbor.PathLengthFromStart)
                {
                    neighbor.PathLengthFromStart = pathThroughCurrent;
                    openNodes.Add(neighbor);
                }
            }
        }
        return -1;
    }

    private class Node
    {
        public static (int X, int Y) End { get; private set; }
        public static Dictionary<(int, int), Node> Nodes { get; } = [];

        public int X { get; }
        public int Y { get; }
        public int DistanceFromEnd { get; }
        public int PathLengthFromStart { get; set; } = 1_000_000_000;
        public int Score => PathLengthFromStart + DistanceFromEnd;

        public Node(int x, int y)
        {
            X = x;
            Y = y;
            DistanceFromEnd = End.X - X + End.Y - Y;
        }

        public static void Initialize(int startX, int startY, int endX, int endY)
        {
            Nodes.Clear();
            End = (endX, endY);
            Node start = new(startX, startY)
            {
                PathLengthFromStart = 0
            };
            Nodes.Add((startX, startY), start);
        }

        public static Node GetNode(int x, int y)
        {
            if (Nodes.TryGetValue((x, y), out Node? node))
            {
                return node;
            }
            Node created = new(x, y);
            Nodes.Add((x, y), created);
            return created;
        }

        public List<Node> GetNeighbors()
        {
            List<Node> neigbors = [];
            if (X > 1)
            {
                neigbors.Add(GetNode(X - 1, Y));
            }
            if (Y > 1)
            {
                neigbors.Add(GetNode(X, Y - 1));
            }
            if (X < End.X)
            {
                neigbors.Add(GetNode(X + 1, Y));
            }
            if (Y < End.Y)
            {
                neigbors.Add(GetNode(X, Y + 1));
            }
            return neigbors;
        }
    }
}
