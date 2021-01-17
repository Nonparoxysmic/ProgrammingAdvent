// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace AdventOfCode2015
{
    static class Day09
    {
        public static void Solve()
        {
            string input1Path = @"Day09\Puzzle\Input1.txt";
            string[] input1 = new string[0];
            try
            {
                input1 = File.ReadAllLines(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }

            List<string> locations = new List<string>();

            DataTable distanceTable = new DataTable("Distances");
            distanceTable.Columns.Add(new DataColumn() { DataType = typeof(string), ColumnName = "Location" });
            distanceTable.PrimaryKey = new DataColumn[1] { distanceTable.Columns["Location"] };

            foreach (string line in input1)
            {
                string[] data = line.Split();
                if ((data.Length != 5) || (data[1] != "to") || (data[3] != "="))
                {
                    PrintErrorInInput("\"" + line + "\"");
                    continue;
                }
                if (!int.TryParse(data[4], out int distance))
                {
                    PrintErrorInInput("as an integer \"" + data[4] + "\" in \"" + line + "\"");
                    continue;
                }

                if (!locations.Contains(data[0]))
                {
                    AddRowAndColumn(data[0], distanceTable);
                    locations.Add(data[0]);
                }
                if (!locations.Contains(data[2]))
                {
                    AddRowAndColumn(data[2], distanceTable);
                    locations.Add(data[2]);
                }

                SetValue(data[0], data[2], distance, distanceTable);
            }
            foreach (string location in locations)
            {
                SetValue(location, location, 0, distanceTable);
            }

            int shortestDistance = int.MaxValue;

            int[] permutation = new int[locations.Count];
            for (int i = 0; i < permutation.Length; i++) permutation[i] = i;
            while (true)
            {
                int permutationDistance = 0;
                for (int i = 0; i < permutation.Length - 1; i++)
                {
                    permutationDistance += GetValue(locations[permutation[i]], locations[permutation[i + 1]], distanceTable);
                }

                shortestDistance = Math.Min(shortestDistance, permutationDistance);

                if (!TryNextPermutation(permutation, out permutation))
                {
                    break;
                }
            }

            Console.WriteLine("Day 9 Part One Answer: " + shortestDistance);
        }

        static void AddRowAndColumn(string name, DataTable table)
        {
            table.Columns.Add(new DataColumn() { DataType = typeof(int), ColumnName = name });
            DataRow row = table.NewRow();
            row["Location"] = name;
            table.Rows.Add(row);
        }

        static void SetValue(string name1, string name2, int value, DataTable table)
        {
            DataRow row = table.Rows.Find(name1);
            row[name2] = value;
            row = table.Rows.Find(name2);
            row[name1] = value;
        }

        static int GetValue(string name1, string name2, DataTable table)
        {
            DataRow row = table.Rows.Find(name1);
            return (int)row[name2];
        }

        // https://en.wikipedia.org/wiki/Permutation#Generation_in_lexicographic_order
        static bool TryNextPermutation(int[] input, out int[] output)
        {
            if (input.Length < 2)
            {
                output = input;
                return false;
            }
            // 1. Find the largest index k such that a[k] < a[k + 1].
            int kilo = -1;
            for (int i = input.Length - 2; i >= 0; i--)
            {
                if (input[i] < input[i + 1])
                {
                    kilo = i;
                    break;
                }
            }
            // 1. If no such index exists, the permutation is the last permutation.
            if (kilo < 0)
            {
                output = input;
                return false;
            }
            // 2. Find the largest index l greater than k such that a[k] < a[l].
            int lima = -1;
            for (int i = input.Length - 1; i > kilo; i--)
            {
                if (input[kilo] < input[i])
                {
                    lima = i;
                    break;
                }
            }
            // 3. Swap the value of a[k] with that of a[l].
            SwapValues(kilo, lima, input);
            // 4. Reverse the sequence from a[k + 1] up to and including the final element a[n].
            int start = kilo + 1;
            int end = input.Length - 1;
            while (end > start)
            {
                SwapValues(start, end, input);
                start++;
                end--;
            }

            output = input;
            return true;
        }

        static void SwapValues(int a, int b, int[] array)
        {
            int temp = array[a];
            array[a] = array[b];
            array[b] = temp;
        }

        static void PrintErrorInInput(string text)
        {
            Console.WriteLine("Day 9: Cannot process {0} in Input1.txt", text);
        }
    }
}
