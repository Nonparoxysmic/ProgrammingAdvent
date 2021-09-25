// Advent of Code 2016
// https://adventofcode.com/2016
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ProgrammingAdvent2016
{
    public class Day17 : Day
    {
        readonly PuzzleSolution solution = new PuzzleSolution();
        readonly Stopwatch stopwatch = new Stopwatch();

        public override PuzzleSolution Solution()
        {
            return solution;
        }

        public override PuzzleSolution FindSolution(string input)
        {
            if (input.Length == 0)
            {
                solution.WriteSolution(1, "ERROR: No input.", 0);
                return solution;
            }
            stopwatch.Start();

            string partOneSolution = FindShortestPath(input);
            solution.WriteSolution(1, partOneSolution, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
            return solution;
        }

        string FindShortestPath(string input)
        {
            VaultPathNode.passcodeLength = input.Length;

            var startingNode = new VaultPathNode(3, 3, input);
            var openList = new List<VaultPathNode> { startingNode };
            while (openList.Count > 0)
            {
                // Get a node on the open list with the lowest score.
                VaultPathNode currentNode = null;
                int lowestScore = int.MaxValue;
                foreach (var node in openList)
                {
                    if (node.score < lowestScore)
                    {
                        lowestScore = node.score;
                        currentNode = node;
                    }
                }
                // If the current node is the goal node, return the path.
                if (currentNode.x == 0 && currentNode.y == 0)
                {
                    return currentNode.path.Substring(input.Length);
                }
                // Remove current node from open list.
                openList.Remove(currentNode);
                // Add possible next states from this node to the open list.
                foreach (char dir in currentNode.directionsOpen)
                {
                    int deltaX = 0;
                    int deltaY = 0;
                    if (dir == 'U' && currentNode.y < 3) { deltaY = 1; }
                    if (dir == 'D' && currentNode.y > 0) { deltaY = -1; }
                    if (dir == 'L' && currentNode.x < 3) { deltaX = 1; }
                    if (dir == 'R' && currentNode.x > 0) { deltaX = -1; }
                    if (deltaX == 0 && deltaY == 0) { continue; }
                    openList.Add(new VaultPathNode(currentNode.x + deltaX, currentNode.y + deltaY, currentNode.path + dir));
                }
            }
            // The goal node was not found, so there is no path.
            return "ERROR";
        }
    }

    class VaultPathNode
    {
        public static int passcodeLength;

        public int x;
        public int y;
        public string path;
        public char[] directionsOpen;
        public int score;

        public VaultPathNode(int x, int y, string path)
        {
            this.x = x;
            this.y = y;
            this.path = path;
            directionsOpen = DirectionsOpen(path);
            score = path.Length - passcodeLength + x + y;
        }

        char[] DirectionsOpen(string path)
        {
            var open = new List<char>();
            byte[] hash = AdventMD5.ComputeHash(Encoding.UTF8.GetBytes(path));
            if ((hash[0] >>  4) > 0xA) { open.Add('U'); }
            if ((hash[0] & 0xF) > 0xA) { open.Add('D'); }
            if ((hash[1] >>  4) > 0xA) { open.Add('L'); }
            if ((hash[1] & 0xF) > 0xA) { open.Add('R'); }
            return open.ToArray();
        }
    }
}
