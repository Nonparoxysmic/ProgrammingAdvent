// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

namespace ProgrammingAdvent2021.Solutions;

internal class Day18 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        List<Node> numbers = input.Select(line => CreateTree(line)).ToList();
        int finalSumMagnitude = numbers.Aggregate((sum, next) => sum + next).CalculateMagnitude();
        return ($"{finalSumMagnitude}", "n/a");
    }

    private static Node CreateTree(string line)
    {
        Node? current = null;
        for (int i = 0; i < line.Length; i++)
        {
            switch (line[i])
            {
                case '[':
                    current ??= new();
                    current.AddChildren();
                    current = current.Left;
                    break;
                case ']':
                    current = current?.Parent;
                    break;
                case ',':
                    current = current?.Parent?.Right;
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    if (current is null)
                    {
                        throw new InvalidOperationException();
                    }
                    current.Value = line[i] - '0';
                    for (int j = i + 1; j < line.Length; j++)
                    {
                        if ('0' <= line[j] && line[j] <= '9')
                        {
                            current.Value *= 10;
                            current.Value += line[j] - '0';
                        }
                        else
                        {
                            i = j - 1;
                            break;
                        }
                    }
                    break;
                default:
                    throw new InvalidDataException();
            }
        }
        if (current is null)
        {
            throw new InvalidOperationException();
        }
        return current;
    }

    private class Node
    {
        public Node? Parent { get; set; }
        public Node? Left { get; set; }
        public Node? Right { get; set; }
        public int? Value { get; set; }
        public int Magnitude { get; private set; }

        public int CalculateMagnitude()
        {
            if (Value is not null)
            {
                Magnitude = (int)Value;
            }
            else if (Left is not null && Right is not null)
            {
                Magnitude = 3 * Left.CalculateMagnitude() + 2 * Right.CalculateMagnitude();
            }
            else
            {
                throw new InvalidOperationException();
            }
            return Magnitude;
        }

        public Node Copy(Node? parent = null)
        {
            Node newNode = new()
            {
                Parent = parent,
                Value = Value,
                Magnitude = Magnitude
            };
            newNode.Left = Left?.Copy(parent: newNode);
            newNode.Right = Right?.Copy(parent: newNode);
            return newNode;
        }

        public void AddChildren()
        {
            Left = new();
            Right = new();
            Left.Parent = this;
            Right.Parent = this;
        }

        public static Node operator +(Node a, Node b)
        {
            Node newRoot = new();
            Node newLeft = a.Copy();
            Node newRight = b.Copy();
            newRoot.Left = newLeft;
            newRoot.Right = newRight;
            newLeft.Parent = newRoot;
            newRight.Parent = newRoot;
            while (true)
            {
                if (TryExplode(newRoot))
                {
                    continue;
                }
                if (TrySplit(newRoot))
                {
                    continue;
                }
                break;
            }
            return newRoot;
        }

        public override string ToString()
        {
            if (Value is not null)
            {
                return ((int)Value).ToString();
            }
            if (Left is not null && Right is not null)
            {
                return $"[{Left},{Right}]";
            }
            throw new InvalidOperationException();
        }

        public bool HoldsValuePair()
        {
            return Left is not null && Left.Value is not null && Right is not null && Right.Value is not null;
        }

        private static bool TrySplit(Node node)
        {
            if (node.Value is not null && node.Value >= 10)
            {
                Node left = new();
                Node right = new();
                left.Parent = node;
                right.Parent = node;
                node.Left = left;
                node.Right = right;
                left.Value = node.Value / 2;
                right.Value = (node.Value + 1) / 2;
                node.Value = null;
                return true;
            }
            if (node.Left is not null && TrySplit(node.Left))
            {
                return true;
            }
            return node.Right is not null && TrySplit(node.Right);
        }

        private static Node? _previousValue;
        private static Node? _explodingPair;
        private static Node? _nextValue;

        private static bool TryExplode(Node root)
        {
            _previousValue = null;
            _explodingPair = null;
            _nextValue = null;
            if (FindNodesForExploding(root))
            {
                if (_previousValue is not null)
                {
                    _previousValue.Value += _explodingPair?.Left?.Value;
                }
                if (_nextValue is not null)
                {
                    _nextValue.Value += _explodingPair?.Right?.Value;
                }
                if (_explodingPair is null)
                {
                    throw new InvalidOperationException();
                }
                _explodingPair.Left = null;
                _explodingPair.Right = null;
                _explodingPair.Value = 0;
                return true;
            }
            return false;
        }

        private static bool FindNodesForExploding(Node node, int depth = 0)
        {
            if (depth >= 4 && node.HoldsValuePair() && _explodingPair is null)
            {
                _explodingPair = node;
                return false;
            }
            if (node.Value is not null)
            {
                if (_explodingPair is null)
                {
                    _previousValue = node;
                }
                else
                {
                    _nextValue = node;
                    return true;
                }
            }
            if (node.Left is not null && FindNodesForExploding(node.Left, depth + 1))
            {
                return true;
            }
            if (node.Right is not null && FindNodesForExploding(node.Right, depth + 1))
            {
                return true;
            }
            if (depth == 0)
            {
                return _explodingPair is not null;
            }
            return false;
        }
    }
}
