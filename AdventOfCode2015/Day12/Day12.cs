// Advent of Code 2015
// https://adventofcode.com/2015
// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/AdventOfCode

using System;
using System.IO;
using System.Text.Json;

namespace AdventOfCode2015
{
    static class Day12
    {
        public static void Solve()
        {
            string input1Path = @"Day12\Puzzle\Input1.json";
            string input1 = "";
            try
            {
                input1 = File.ReadAllText(input1Path);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to load input file " + input1Path + Environment.NewLine + e.GetType());
            }

            JsonDocument jd = null;
            try
            {
                jd = JsonDocument.Parse(input1);
            }
            catch (Exception e)
            {
                Print.PrintErrorAndExit("Unable to parse JSON in " + input1Path + Environment.NewLine + e.GetType());
            }
            
            JsonElement root = jd.RootElement;

            int sum = AddNumbersInJSON(root);

            Console.WriteLine("Day 12 Part One Answer: " + sum);
        }

        static int AddNumbersInJSON(JsonElement element)
        {
            int sum = 0;

            JsonValueKind valueKind = element.ValueKind;
            switch (valueKind)
            {
                case JsonValueKind.Object:
                    JsonElement.ObjectEnumerator objectEnumerator = element.EnumerateObject();
                    foreach (JsonProperty jp in objectEnumerator)
                    {
                        sum += AddNumbersInJSON(element.GetProperty(jp.Name));
                    }
                    break;
                case JsonValueKind.Array:
                    JsonElement.ArrayEnumerator arrayEnumerator = element.EnumerateArray();
                    foreach (JsonElement je in arrayEnumerator)
                    {
                        sum += AddNumbersInJSON(je);
                    }
                    break;
                case JsonValueKind.Number:
                    if (element.TryGetInt32(out int value))
                    {
                        sum += value;
                    }
                    else
                    {
                        Print.PrintErrorAndExit("Cannot get \"" + element + "\" as a 32-bit integer.");
                    }
                    break;
                default:
                    break;
            }

            return sum;
        }
    }
}
