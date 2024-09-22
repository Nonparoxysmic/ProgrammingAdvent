// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2021
// https://adventofcode.com/2021

using System.Security.Cryptography;
using System.Text.RegularExpressions;
using ProgrammingAdvent2021.Common;

namespace ProgrammingAdvent2021.Solutions;

internal class Day19 : Day
{
    private static readonly MD5 md5 = MD5.Create();

    protected override (string, string) CalculateAnswers(string[] input)
    {
        int currentScanner = -1;
        List<Scanner> scanners = [];
        foreach (string line in input)
        {
            Match scannerMatch = GeneratedRegex.ValidDay19Scanner.Match(line);
            if (scannerMatch.Success)
            {
                int newScanner = int.Parse(scannerMatch.Groups["scanner"].Value);
                if (currentScanner + 1 != newScanner)
                {
                    return ("Scanners out of order.", "n/a");
                }
                currentScanner = newScanner;
                scanners.Add(new Scanner(currentScanner));
                continue;
            }
            Match coordinatesMatch = GeneratedRegex.ValidDay19Coordinates.Match(line);
            if (coordinatesMatch.Success)
            {
                int x = int.Parse(coordinatesMatch.Groups["x"].Value);
                int y = int.Parse(coordinatesMatch.Groups["y"].Value);
                int z = int.Parse(coordinatesMatch.Groups["z"].Value);
                scanners[currentScanner].AddBeacon(x, y, z);
            }
        }
        foreach (Scanner scanner in scanners)
        {
            scanner.HashBeacons();
        }
        for (int i = 0; i < scanners.Count - 1; i++)
        {
            for (int j = i + 1; j < scanners.Count; j++)
            {
                int possibleOverlap = scanners[i].PossibleOverlap(scanners[j]);
                if (possibleOverlap >= 9)
                {
                    scanners[i].Connections.Add(scanners[j]);
                    scanners[j].Connections.Add(scanners[i]);
                }
            }
        }
        if (!BuildTree(scanners, scanners[0]))
        {
            return ("Solution not found", "n/a");
        }
        FindScannerPositions(scanners, scanners[0]);

        // TODO: solve this

        return ("Solution not yet implemented", "n/a");
    }

    private static int Distance(Beacon a, Beacon b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);
    }

    private static int Hash(params int[] numbers)
    {
        byte[] bytes = new byte[numbers.Length * 4];
        for (int i = 0; i < numbers.Length; i++)
        {
            byte[] current = BitConverter.GetBytes(numbers[i]);
            Array.Copy(current, 0, bytes, i * 4, 4);
        }
        byte[] hash = md5.ComputeHash(bytes);
        return BitConverter.ToInt32(hash, 0);
    }

    private static bool BuildTree(List<Scanner> scanners, Scanner scanner, int depth = 0)
    {
        foreach (Scanner connection in scanner.Connections)
        {
            if (connection.ID == 0) { continue; }
            if (connection.Parent is null)
            {
                connection.Parent = scanner;
                scanner.Children.Add(connection);
            }
        }
        foreach (Scanner child in scanner.Children)
        {
            BuildTree(scanners, child, depth + 1);
        }
        if (depth == 0)
        {
            foreach (Scanner s in scanners)
            {
                if (s.Parent is null && s.ID != 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static void FindScannerPositions(List<Scanner> scanners, Scanner scanner)
    {
        if (scanner.ID == 0)
        {
            // TODO: scanner position is the origin
        }
        foreach (Scanner child in scanner.Children)
        {
            // TODO: set child position based on scanner position

            FindScannerPositions(scanners, child);
        }
    }

    private readonly struct Beacon(int x, int y, int z)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public int Z { get; } = z;
    }

    private class Scanner(int id)
    {
        public int ID { get; } = id;
        public Scanner? Parent { get; set; }
        public List<Scanner> Children { get; } = [];
        public HashSet<Scanner> Connections { get; } = [];
        public int[] Hashes { get; private set; } = [];

        private readonly List<Beacon> _beacons = [];

        public void AddBeacon(int x, int y, int z)
        {
            _beacons.Add(new Beacon(x, y, z));
        }

        public void HashBeacons()
        {
            Hashes = new int[_beacons.Count];
            Dictionary<(int, int), int> distances = [];
            for (int i = 0; i < _beacons.Count - 1; i++)
            {
                for (int j = i + 1; j < _beacons.Count; j++)
                {
                    distances.Add((i, j), Distance(_beacons[i], _beacons[j]));
                    distances.Add((j, i), Distance(_beacons[i], _beacons[j]));
                }
            }
            for (int i = 0; i < _beacons.Count; i++)
            {
                List<int> d = [];
                for (int j = 0; j < _beacons.Count; j++)
                {
                    if (i == j) { continue; }
                    d.Add(distances[(i, j)]);
                }
                d.Sort();
                Hashes[i] = Hash(d[0], d[1]);
            }
        }

        public int PossibleOverlap(Scanner other)
        {
            return Hashes.Intersect(other.Hashes).Count();
        }
    }
}
