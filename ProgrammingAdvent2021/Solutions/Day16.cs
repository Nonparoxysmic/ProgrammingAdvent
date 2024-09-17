// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using ProgrammingAdvent2021.Common;

namespace ProgrammingAdvent2021.Solutions;

internal class Day16 : Day
{
    protected override (string, string) CalculateAnswers(string[] input)
    {
        if (input.Length == 0 || !GeneratedRegex.ValidHexadecimal.Match(input[0]).Success)
        {
            return ("Invalid input", "n/a");
        }
        Packet.AllPackets.Clear();
        BitManager bitManager = new(input[0]);
        ulong rootVersion = bitManager.GetNextBits(3);
        ulong rootTypeID = bitManager.GetNextBits(3);
        if (rootTypeID == 4)
        {
            return ($"{rootVersion}", "n/a");
        }
        Packet root = OperatorPacket(rootVersion, rootTypeID, bitManager);
        int versionSum = Packet.AllPackets.Sum(packet => packet.Version);
        ulong expressionValue = root.Evaluate();
        return ($"{versionSum}", $"{expressionValue}");
    }

    private static Packet OperatorPacket(ulong version, ulong typeID, BitManager bitManager)
    {
        ulong lengthTypeID = bitManager.GetNextBits(1);
        uint bits = 7;
        List<Packet> subPackets = [];
        if (lengthTypeID == 0)
        {
            ulong subPacketBits = bitManager.GetNextBits(15);
            bits += 15;
            uint subPacketBitsFound = 0;
            while (subPacketBitsFound < subPacketBits)
            {
                Packet next = GetNextSubPacket(bitManager);
                subPackets.Add(next);
                bits += next.Bits;
                subPacketBitsFound += next.Bits;
            }
        }
        else
        {
            ulong subPacketCount = bitManager.GetNextBits(11);
            bits += 11;
            for (uint i = 0; i < subPacketCount; i++)
            {
                Packet next = GetNextSubPacket(bitManager);
                subPackets.Add(next);
                bits += next.Bits;
            }
        }
        return new Packet(version, typeID, subPackets, bits);
    }

    private static Packet LiteralValuePacket(ulong version, ulong typeID, BitManager bitManager)
    {
        ulong value = 0;
        uint bits = 6;
        while (true)
        {
            ulong lastGroupFlag = bitManager.GetNextBits(1);
            value <<= 4;
            value |= bitManager.GetNextBits(4);
            bits += 5;
            if (lastGroupFlag == 0)
            {
                break;
            }
        }
        return new Packet(version, typeID, value, bits);
    }

    private static Packet GetNextSubPacket(BitManager bitManager)
    {
        ulong version = bitManager.GetNextBits(3);
        ulong typeID = bitManager.GetNextBits(3);
        Packet next;
        if (typeID == 4)
        {
            next = LiteralValuePacket(version, typeID, bitManager);
        }
        else
        {
            next = OperatorPacket(version, typeID, bitManager);
        }
        return next;
    }

    private class BitManager
    {
        public uint BitsProcessed { get; private set; }

        private readonly byte[] _bytes;
        private int _currentByte;
        private int _currentBitIndex = 7;

        public BitManager(string hex)
        {
            List<byte> bytes = [];
            for (int i = 0; i < hex.Length - 1; i += 2)
            {
                byte b = Convert.ToByte(hex[i..(i + 2)], 16);
                bytes.Add(b);
            }
            if (hex.Length % 2 == 1)
            {
                byte b = Convert.ToByte(hex[^1].ToString(), 16);
                bytes.Add(b);
            }
            _bytes = [.. bytes];
        }

        public ulong GetNextBits(int length)
        {
            ulong result = 0;
            for (int i = 0; i < length && _currentByte < _bytes.Length; i++)
            {
                result <<= 1;
                result |= (uint)(_bytes[_currentByte] >> _currentBitIndex) & 1U;

                BitsProcessed++;

                _currentBitIndex--;
                if (_currentBitIndex < 0)
                {
                    _currentBitIndex = 7;
                    _currentByte++;
                }
            }
            return result;
        }
    }

    private class Packet
    {
        public static List<Packet> AllPackets { get; } = [];

        public int Version { get; }
        public ulong TypeID { get; }
        public ulong? Value { get; }
        public List<Packet> Children { get; }
        public uint Bits { get; }

        public Packet(ulong version, ulong typeID, ulong value, uint bits)
        {
            Version = (int)version;
            TypeID = typeID;
            Value = value;
            Children = [];
            Bits = bits;
            AllPackets.Add(this);
        }

        public Packet(ulong version, ulong typeID, List<Packet> children, uint bits)
        {
            Version = (int)version;
            TypeID = typeID;
            Children = children;
            Bits = bits;
            AllPackets.Add(this);
        }

        public ulong Evaluate()
        {
            switch (TypeID)
            {
                case 0:
                    ulong sum = (ulong)Children.Sum(c => (decimal)c.Evaluate());
                    return sum;
                case 1:
                    var product = Children.Aggregate(1UL, (product, p) => product * p.Evaluate());
                    return product;
                case 2:
                    ulong min = Children.Min(c => c.Evaluate());
                    return min;
                case 3:
                    ulong max = Children.Max(c => c.Evaluate());
                    return max;
                case 4:
                    if (Value is not null)
                    {
                        return (ulong)Value;
                    }
                    break;
                case 5:
                    ulong gt = Children[0].Evaluate() > Children[1].Evaluate() ? 1UL : 0;
                    return gt;
                case 6:
                    ulong lt = Children[0].Evaluate() < Children[1].Evaluate() ? 1UL : 0;
                    return lt;
                case 7:
                    ulong equals = Children[0].Evaluate() == Children[1].Evaluate() ? 1UL : 0;
                    return equals;
            }
            throw new InvalidOperationException();
        }
    }
}
