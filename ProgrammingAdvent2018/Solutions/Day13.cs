// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingAdvent2018.Program;

namespace ProgrammingAdvent2018.Solutions
{
    internal class Day13 : Day
    {
        private readonly Regex validLine = new Regex(@"^[ |\-/\\+^v<>]{2,}$");

        //  Direction    x   y      i
        // --------------------------
        //  ^ up       ( 0, -1)    [0]
        //  v down     ( 0,  1)    [1]
        //  < left     (-1,  0)    [2]
        //  > right    ( 1,  0)    [3]
        private readonly int[] moveX = new int[] { 0, 0, -1, 1 };
        private readonly int[] moveY = new int[] { -1, 1, 0, 0 };

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
            if (inputLines.Length < 2 || inputLines[0].Length < 2)
            {
                output.WriteError("Insufficient input.", sw);
                return output;
            }

            TrackMap tracks = new TrackMap(inputLines[0].Length, inputLines.Length);
            List<Cart> cartList = new List<Cart>();
            for (int y = 0; y < inputLines.Length; y++)
            {
                if (inputLines[y].Length != inputLines[0].Length)
                {
                    output.WriteError("Input lines must be the same length.", sw);
                    return output;
                }
                Match lineMatch = validLine.Match(inputLines[y]);
                if (!lineMatch.Success)
                {
                    output.WriteError($"Invalid characters in line {y}.", sw);
                    return output;
                }
                for (int x = 0; x < inputLines[y].Length; x++)
                {
                    char c = inputLines[y][x];
                    if (c == '^' || c == 'v' || c == '<' || c == '>')
                    {
                        int direction;
                        switch (c)
                        {
                            case '^':
                                direction = 0;
                                c = '|';
                                break;
                            case 'v':
                                direction = 1;
                                c = '|';
                                break;
                            case '<':
                                direction = 2;
                                c = '-';
                                break;
                            case '>':
                                direction = 3;
                                c = '-';
                                break;
                            default:
                                throw new ApplicationException();
                        }
                        cartList.Add(new Cart(x, y, direction));
                    }
                    tracks[x, y] = c;
                }
            }
            if (cartList.Count < 2)
            {
                output.WriteError("Not enough carts.", sw);
                return output;
            }
            Cart[] carts = cartList.ToArray();

            string partOneAnswer = "";
            bool collisionFound = false;
            for (int tick = 0; tick < 65536; tick++)
            {
                foreach (Cart cart in carts)
                {
                    cart.X += moveX[cart.Direction];
                    cart.Y += moveY[cart.Direction];
                    char newTrack = tracks[cart.X, cart.Y];
                    if (newTrack == '\0')
                    {
                        int startingX = cart.ID & 0b1111_1111_1111_1111;
                        int startingY = cart.ID >> 16;
                        output.WriteError($"Cart starting at {startingX}, {startingY} left the map.", sw);
                        return output;
                    }
                    if (newTrack == ' ')
                    {
                        int startingX = cart.ID & 0b1111_1111_1111_1111;
                        int startingY = cart.ID >> 16;
                        output.WriteError($"Cart starting at {startingX}, {startingY} left the tracks.", sw);
                        return output;
                    }
                    if (FindFirstCollision(carts, out partOneAnswer))
                    {
                        collisionFound = true;
                        break;
                    }
                    cart.Turn(newTrack);
                }
                if (collisionFound)
                {
                    break;
                }
                Array.Sort(carts);
            }
            if (partOneAnswer == "")
            {
                output.WriteError("No collisions found.", sw);
                return output;
            }

            sw.Stop();
            output.WriteAnswers(partOneAnswer, null, sw);
            return output;
        }

        private bool FindFirstCollision(Cart[] carts, out string coordinates)
        {
            for (int j = 1; j < carts.Length; j++)
            {
                for (int i = 0; i < j; i++)
                {
                    if (carts[i].X == carts[j].X && carts[i].Y == carts[j].Y)
                    {
                        coordinates = $"{carts[i].X},{carts[i].Y}";
                        return true;
                    }
                }
            }
            coordinates = "";
            return false;
        }

        private class TrackMap
        {
            public int Width { get; private set; }
            public int Height { get; private set; }

            private readonly char[,] tracks;

            public TrackMap(int width, int height)
            {
                Width = width;
                Height = height;
                tracks = new char[width + 2, height + 2];
            }

            public char this[int x, int y]
            {
                get => GetValue(x, y);
                set => SetValue(x, y, value);
            }

            private char GetValue(int x, int y)
            {
                return tracks[x + 1, y + 1];
            }

            private void SetValue(int x, int y, char c)
            {
                tracks[x + 1, y + 1] = c;
            }
        }

        private class Cart : IComparable
        {
            public int ID { get; private set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Direction { get; set; }

            private readonly int[] leftTurns = new int[] { 2, 3, 1, 0 };
            private readonly int[] rightTurns = new int[] { 3, 2, 0, 1 };

            private int currentTurn;

            public Cart(int x, int y, int direction)
            {
                ID = y << 16 | x;
                X = x;
                Y = y;
                Direction = direction;
                currentTurn = 0;
            }

            public void Turn(char track)
            {
                if (track == '/')
                {
                    Direction = 3 - Direction;
                }
                else if (track == '\\')
                {
                    Direction = (Direction + 2) % 4;
                }
                else if (track == '+')
                {
                    if (currentTurn == 0)
                    {
                        Direction = leftTurns[Direction];
                    }
                    else if (currentTurn == 2)
                    {
                        Direction = rightTurns[Direction];
                    }
                    currentTurn = ++currentTurn % 3;
                }
            }

            public int CompareTo(object obj)
            {
                if (obj == null)
                {
                    return 1;
                }
                if (obj is Cart otherCart)
                {
                    if (X == otherCart.X && Y == otherCart.Y)
                    {
                        return 0;
                    }
                    if (Y < otherCart.Y || (Y == otherCart.Y && X < otherCart.X))
                    {
                        return -1;
                    }
                }
                return 1;
            }
        }
    }
}
