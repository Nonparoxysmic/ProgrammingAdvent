// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2015
{
    static class Day13
    {
        public static void Solve()
        {
            string input1Path = @"Day13\Puzzle\Input1.txt";
            string[] input1 = null;
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }
            if (input1.Length < 12)
            {
                Print.PrintErrorAndExit("Day 13: Input too short.");
            }

            List<Guest> guests = new List<Guest>();

            foreach (string line in input1)
            {
                string[] words = line.Split();
                if (words.Length != 11) Print.PrintErrorAndExit("Day 13: Cannot process \"" + line + "\" in Input1.txt");
                Guest guest = guests.FirstOrDefault(x => x.name == words[0]);
                if (guest == null)
                {
                    guest = new Guest(words[0]);
                    guests.Add(guest);
                }
                string friend = words[10].Trim('.');
                if (guest.friends.ContainsKey(friend))
                {
                    Print.PrintErrorAndExit("Day 13: Duplicate data in " + input1Path + Environment.NewLine + "\"" + line + "\"");
                }
                if (!int.TryParse(words[3], out int happiness))
                {
                    Print.PrintErrorAndExit("Day 13: Cannot parse \"" + words[3] + "\" as an integer.");
                }
                if (words[2] == "lose") happiness *= -1;
                guest.friends.Add(friend, happiness);
            }

            int[] permutation = new int[guests.Count];
            for (int i = 0; i < guests.Count; i++) permutation[i] = i;

            int bestTotalHappiness = 0;
            while (true)
            {
                int permutationHappiness = TotalHappiness(guests, permutation);

                bestTotalHappiness = Math.Max(bestTotalHappiness, permutationHappiness);

                if (!Day09.TryNextPermutation(permutation, out permutation)) break;
            }

            Console.WriteLine("Day 13 Part One Answer: " + bestTotalHappiness);
        }

        static int TotalHappiness(List<Guest> guests, int[] permutation)
        {
            if (guests.Count != permutation.Length) throw new ArgumentOutOfRangeException();
            int sum = PairHappiness(guests[permutation[0]], guests[permutation[permutation.Length - 1]]);
            for (int i = 0; i < permutation.Length - 1; i++)
            {
                sum += PairHappiness(guests[permutation[i]], guests[permutation[i + 1]]);
            }
            return sum;
        }

        static int PairHappiness(Guest guest1, Guest guest2)
        {
            if (!guest1.friends.ContainsKey(guest2.name) || !guest2.friends.ContainsKey(guest1.name))
            {
                Print.PrintErrorAndExit("Day 13: Missing data between " + guest1.name + " and " + guest2.name);
            }
            return guest1.friends[guest2.name] + guest2.friends[guest1.name];
        }
    }

    class Guest
    {
        public string name;
        public Dictionary<string, int> friends;

        public Guest (string name)
        {
            this.name = name;
            friends = new Dictionary<string, int>();
        }
    }
}
