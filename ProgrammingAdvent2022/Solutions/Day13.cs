// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day13 : Day
{
    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        List<Packet> packets = ParseInput(input);

        // Part One
        int sumOfCorrectIndices = 0;
        for (int i = 0; i < packets.Count / 2; i++)
        {
            if (packets[2 * i] < packets[2 * i + 1])
            {
                sumOfCorrectIndices += i + 1;
            }
        }

        // Part Two
        Packet divider1 = new("[[2]]");
        Packet divider2 = new("[[6]]");
        packets.Add(divider1);
        packets.Add(divider2);
        packets.Sort();
        int decoderKey = (packets.IndexOf(divider1) + 1) * (packets.IndexOf(divider2) + 1);

        return result.WriteAnswers(sumOfCorrectIndices, decoderKey);
    }

    private static List<Packet> ParseInput(string[] input)
    {
        List<Packet> packets = [];
        for (int i = 0; i < input.Length - 1; i += 3)
        {
            if (string.IsNullOrEmpty(input[i]) || string.IsNullOrEmpty(input[i + 1]))
            {
                break;
            }
            packets.Add(new Packet(input[i]));
            packets.Add(new Packet(input[i + 1]));
        }
        return packets;
    }

    private static Node CreateTree(string line)
    {
        Node current = new();
        for (int i = 1; i < line.Length - 1; i++)
        {
            switch (line[i])
            {
                case '[':
                    current = current.AddChild();
                    break;
                case ']':
                    if (current.Parent is not null)
                    {
                        current = current.Parent;
                    }
                    break;
                case ',':
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
                    current = current.AddChild();
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
                    if (current.Parent is not null)
                    {
                        current = current.Parent;
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

    private class Packet(string data = "") : IComparable
    {
        public Node RootNode { get; } = CreateTree(data);

        public static bool operator <(Packet a, Packet b)
        {
            return a.RootNode < b.RootNode;
        }

        public static bool operator >(Packet a, Packet b)
        {
            return a.RootNode > b.RootNode;
        }

        public int CompareTo(object? obj)
        {
            if (obj is Packet other)
            {
                if (this < other)
                {
                    return -1;
                }
                if (this > other)
                {
                    return 1;
                }
            }
            return 0;
        }
    }

    private class Node
    {
        public Node? Parent { get; set; }
        public List<Node> Children { get; } = [];
        public int? Value { get; set; }

        public Node AddChild()
        {
            Node child = new()
            {
                Parent = this
            };
            Children.Add(child);
            return child;
        }

        public static bool operator <(Node a, Node b)
        {
            if (a.Value is not null && b.Value is not null)
            {
                return a.Value < b.Value;
            }
            List<Node> listA = a.Children;
            if (a.Value is not null)
            {
                Node newNode = new()
                {
                    Value = a.Value
                };
                listA = [newNode];
            }
            List<Node> listB = b.Children;
            if (b.Value is not null)
            {
                Node newNode = new()
                {
                    Value = b.Value
                };
                listB = [newNode];
            }
            for (int i = 0; i < listB.Count; i++)
            {
                if (i > listA.Count - 1)
                {
                    return true;
                }
                if (listA[i] < listB[i])
                {
                    return true;
                }
                else if (listA[i] > listB[i])
                {
                    return false;
                }
            }
            return false;
        }

        public static bool operator >(Node a, Node b)
        {
            if (a.Value is not null && b.Value is not null)
            {
                return a.Value > b.Value;
            }
            List<Node> listA = a.Children;
            if (a.Value is not null)
            {
                Node newNode = new()
                {
                    Value = a.Value
                };
                listA = [newNode];
            }
            List<Node> listB = b.Children;
            if (b.Value is not null)
            {
                Node newNode = new()
                {
                    Value = b.Value
                };
                listB = [newNode];
            }
            for (int i = 0; i < listA.Count; i++)
            {
                if (i > listB.Count - 1)
                {
                    return true;
                }
                if (listA[i] > listB[i])
                {
                    return true;
                }
                else if (listA[i] < listB[i])
                {
                    return false;
                }
            }
            return false;
        }
    }
}
