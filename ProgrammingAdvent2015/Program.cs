using System;
using System.Diagnostics;

namespace ProgrammingAdvent2015
{
    static class Program
    {
        static void Main()
        {
            Console.Title = "ProgrammingAdvent2015 by Nonparoxysmic";
            Stopwatch sw = new Stopwatch();

            sw.Start();
            Day01.Solve();
            Day02.Solve();
            Day03.Solve();
            sw.Stop();
            Day04.Solve();
            sw.Start();
            Day05.Solve();
            Day06.Solve();
            Day07.Solve();
            Day08.Solve();
            Day09.Solve();
            Day10.Solve();
            Day11.Solve();
            Day12.Solve();
            Day13.Solve();
            Day14.Solve();
            Day15.Solve();
            Day16.Solve();
            Day17.Solve();
            Day18.Solve();
            Day19.Solve();
            Day20.Solve();
            Day21.Solve();
            Day22.Solve();
            Day23.Solve();
            Day24.Solve();
            Day25.Solve();
            sw.Stop();

            Console.WriteLine("Total Time: {0:F1} seconds (not including Day 4)", sw.ElapsedMilliseconds * 0.001);

            Console.ReadKey(true);
        }
    }
}
