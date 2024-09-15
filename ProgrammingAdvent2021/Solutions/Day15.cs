// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021


using ProgrammingAdvent2021.Utilities;

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
        int[,] fullMap = Expand(map, width, height);
        int partTwoAnswer = PathLength(fullMap);
        return ($"{partOneAnswer}", $"{partTwoAnswer}");
    }

    private static int PathLength(int[,] map)
    {
        Node.Initialize(0, 0, map.GetLength(0) - 1, map.GetLength(1) - 1);
        MinHeap openNodes = new();
        openNodes.Insert(Node.GetNode(0, 0));
        while (openNodes.Count != 0)
        {
            Node current = openNodes.FindMin();
            if (current.IsEnd)
            {
                return current.PathLengthFromStart;
            }
            openNodes.DeleteMin();
            foreach (Node neighbor in current.GetNeighbors())
            {
                int pathThroughCurrent = current.PathLengthFromStart + map[neighbor.X, neighbor.Y];
                if (pathThroughCurrent < neighbor.PathLengthFromStart)
                {
                    neighbor.PathLengthFromStart = pathThroughCurrent;
                    if (neighbor.Open)
                    {
                        openNodes.DecreaseKey(neighbor);
                    }
                    else
                    {
                        openNodes.Insert(neighbor);
                    }
                }
            }
        }
        return -1;
    }

    private static int[,] Expand(int[,] map, int originalWidth, int originalHeight)
    {
        int[,] fullMap = new int[originalWidth * 5, originalHeight * 5];
        for (int tileY = 0; tileY < 5; tileY++)
        {
            for (int tileX = 0; tileX < 5; tileX++)
            {
                int modifier = tileX + tileY;
                for (int y = 0; y < originalHeight; y++)
                {
                    for (int x = 0; x < originalWidth; x++)
                    {
                        int newValue = (map[x, y] - 1 + modifier) % 9 + 1;
                        fullMap[tileX * originalWidth + x, tileY * originalHeight + y] = newValue;
                    }
                }
            }
        }
        return fullMap;
    }

    private class Node
    {
        public static (int X, int Y) End { get; private set; }
        public static Dictionary<(int, int), Node> Nodes { get; } = [];

        public int X { get; }
        public int Y { get; }
        public bool Open { get; set; }
        public bool IsEnd { get; }
        public int PathLengthFromStart { get; set; } = 1_000_000_000;
        public int Score => PathLengthFromStart;

        public Node(int x, int y)
        {
            X = x;
            Y = y;
            IsEnd = End.X == X && End.Y == Y;
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

    private class MinHeap
    {
        public int Count { get; private set; }

        private readonly Node[] _nodes = new Node[2000];
        private int _endIndex;

        public void Insert(Node node)
        {
            Count++;
            node.Open = true;
            _nodes[_endIndex] = node;
            UpHeap(_endIndex);
            _endIndex++;
        }

        public Node FindMin()
        {
            return _nodes[0];
        }

        public void DeleteMin()
        {
            _nodes[0].Open = false;
            Count--;
            if (Count == 0)
            {
                _endIndex = 0;
                return;
            }
            _endIndex--;
            _nodes[0] = _nodes[_endIndex];
            DownHeap(0);
        }

        public void DecreaseKey(Node node)
        {
            for (int i = 0; i < _endIndex; i++)
            {
                if (node == _nodes[i])
                {
                    UpHeap(i);
                    return;
                }
            }
        }

        private void UpHeap(int index)
        {
            if (index == 0)
            {
                return;
            }
            int parentIndex = (index - 1) / 2;
            if (_nodes[parentIndex].Score > _nodes[index].Score)
            {
                _nodes.Swap(index, parentIndex);
                UpHeap(parentIndex);
            }
        }

        private void DownHeap(int index)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;
            int lowest = index;
            if (leftChildIndex < _endIndex && _nodes[leftChildIndex].Score < _nodes[lowest].Score)
            {
                lowest = leftChildIndex;
            }
            if (rightChildIndex < _endIndex && _nodes[rightChildIndex].Score < _nodes[lowest].Score)
            {
                lowest = rightChildIndex;
            }
            if (lowest != index)
            {
                _nodes.Swap(index, lowest);
                DownHeap(lowest);
            }
        }
    }
}
