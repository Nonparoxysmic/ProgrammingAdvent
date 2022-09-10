// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day10 : Day
    {
        private readonly Regex validLine = new Regex(@"^position=< *(-?[0-9]+), +(-?[0-9]+)> velocity=< *(-?[0-9]+), +(-?[0-9]+)>$");

        internal override PuzzleAnswers Solve(string input)
        {
            PuzzleAnswers output = new PuzzleAnswers();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (input.Trim() == "")
            {
                output.WriteError("No input.", sw);
                return output;
            }
            string[] inputLines = input.ToLines();
            if (inputLines.Length < 2)
            {
                output.WriteError("Insufficient input.", sw);
                return output;
            }
            Point[] points = new Point[inputLines.Length];
            for (int i = 0; i < inputLines.Length; i++)
            {
                Match validLineMatch = validLine.Match(inputLines[i]);
                if (!validLineMatch.Success)
                {
                    output.WriteError($"Invalid line \"{inputLines[i]}\" in input.", sw);
                    return output;
                }
                int posX = int.Parse(validLineMatch.Groups[1].Value);
                int posY = int.Parse(validLineMatch.Groups[2].Value);
                int velX = int.Parse(validLineMatch.Groups[3].Value);
                int velY = int.Parse(validLineMatch.Groups[4].Value);
                points[i] = new Point(posX, posY, velX, velY);
            }

            int timeOfInterest = TimeOfInterest(points);
            for (int i = 0; i < points.Length; i++)
            {
                points[i].Position += timeOfInterest * points[i].Velocity;
            }

            int boundingBoxSize = int.MaxValue;
            int t;
            for (t = 0; t < 6000; t++)
            {
                int newBoundingBoxSize = BoundingBoxSize(points);
                if (newBoundingBoxSize > boundingBoxSize)
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        points[i].Position -= points[i].Velocity;
                    }
                    break;
                }
                boundingBoxSize = newBoundingBoxSize;
                for (int i = 0; i < points.Length; i++)
                {
                    points[i].Position += points[i].Velocity;
                }
            }

            SimpleBitmap partOneAnswer = DrawPoints(points);

            int partTwoAnswer = timeOfInterest + t - 1;

            sw.Stop();
            output.WriteAnswers(partOneAnswer, partTwoAnswer, sw);
            return output;
        }

        private int TimeOfInterest(Point[] points)
        {
            if (points.Length < 2) { return 0; }
            Point pointA = null;
            Point pointB = null;
            int maxVelocityDifference = int.MinValue;
            for (int i = 0; i < points.Length - 1; i++)
            {
                for (int j = 0; j < points.Length; j++)
                {
                    if (Math.Sign(points[i].Velocity.X) == 0
                        || Math.Sign(points[j].Velocity.X) == 0
                        || (Math.Sign(points[i].Velocity.X) == Math.Sign(points[j].Velocity.X)
                        && Math.Sign(points[i].Velocity.Y) == Math.Sign(points[j].Velocity.Y)))
                    {
                        continue;
                    }
                    int difference = (points[i].Velocity - points[j].Velocity).TaxicabMagnitude();
                    if (difference > maxVelocityDifference)
                    {
                        maxVelocityDifference = difference;
                        pointA = points[i];
                        pointB = points[j];
                    }
                }
            }
            return TimeOfCloseApproach(pointA, pointB);
        }

        private int TimeOfCloseApproach(Point pointA, Point pointB)
        {
            if (pointA == null || pointB == null) { return 0; }
            float c = pointA.Position.X;
            float d = pointA.Position.Y;
            float e = pointA.Velocity.X;
            float f = pointA.Velocity.Y;
            float g = pointB.Position.X;
            float h = pointB.Position.Y;
            float i = pointB.Velocity.X;
            float j = pointB.Velocity.Y;
            float num1 = h - d + (c - g) * (j - f) / (i - e);
            float num2 = (e - i) / (j - f) - (j - f) / (i - e);
            int t = (int)(((num1 / num2) + c - g) / (i - e)) - 30;
            return t >= 0 ? t : 0;
        }

        private int BoundingBoxSize(Point[] points)
        {
            int xMin = int.MaxValue;
            int xMax = int.MinValue;
            int yMin = int.MaxValue;
            int yMax = int.MinValue;
            foreach (Point point in points)
            {
                xMin = Math.Min(xMin, point.Position.X);
                xMax = Math.Max(xMax, point.Position.X);
                yMin = Math.Min(yMin, point.Position.Y);
                yMax = Math.Max(yMax, point.Position.Y);
            }
            return xMax + yMax - xMin - yMin;
        }

        private SimpleBitmap DrawPoints(Point[] points)
        {
            int xMin = int.MaxValue;
            int xMax = int.MinValue;
            int yMin = int.MaxValue;
            int yMax = int.MinValue;
            foreach (Point point in points)
            {
                xMin = Math.Min(xMin, point.Position.X);
                xMax = Math.Max(xMax, point.Position.X);
                yMin = Math.Min(yMin, point.Position.Y);
                yMax = Math.Max(yMax, point.Position.Y);
            }
            xMin--;
            xMax++;
            yMin--;
            yMax++;
            SimpleBitmap output = new SimpleBitmap(xMax - xMin + 1, yMax - yMin + 1, Color.Black);
            foreach (Point point in points)
            {
                output.SetPixel(point.Position.X - xMin, point.Position.Y - yMin, Color.White);
            }
            return SimpleBitmap.Scale(output, 2);
        }

        private class Point
        {
            public Vector2Int Position { get; set; }
            public Vector2Int Velocity { get; private set; }

            public Point(int positionX, int positionY, int velocityX, int velocityY)
            {
                Position = new Vector2Int(positionX, positionY);
                Velocity = new Vector2Int(velocityX, velocityY);
            }
        }
    }
}
