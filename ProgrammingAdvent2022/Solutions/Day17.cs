// Puzzle solution by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

using System.Drawing;
using ProgrammingAdvent2022.Common;

namespace ProgrammingAdvent2022.Solutions;

internal class Day17 : Day
{
    protected override PuzzleAnswers CalculateAnswers(string[] input, string? exampleModifier = null)
    {
        PuzzleAnswers result = new();

        JetPattern jets = new(input[0]);
        int towerHeight = TowerHeight(2022, jets);

        return result.WriteAnswers(towerHeight, null);
    }

    private static int TowerHeight(int numberOfRocks, JetPattern jets)
    {
        Queue<Rock> rocks = [];
        int height = 0;
        for (int i = 0; i < numberOfRocks; i++)
        {
            Rock nextRock = new(Rock.Types[i % 5], new Vector2Int(2, height + 3));
            while (true)
            {
                char jetPush = jets.Next();
                if (jetPush == '<')
                {
                    Push(nextRock, -1, rocks);
                }
                if (jetPush == '>')
                {
                    Push(nextRock, 1, rocks);
                }
                if (!TryFall(nextRock, rocks))
                {
                    break;
                }
            }
            height = Math.Max(height, nextRock.Position.Y + nextRock.BoundingBoxSize.Y);
            rocks.Enqueue(nextRock);
            if (rocks.Count >= 30)
            {
                rocks.Dequeue();
            }
        }
        return height;
    }

    private static void Push(Rock rock, int direction, Queue<Rock> rocks)
    {
        if (rock.Position.X + direction < 0
            || rock.Position.X + rock.BoundingBoxSize.X + direction > 7)
        {
            return;
        }
        TryMove(rock, direction, 0, rocks);
    }

    private static bool TryFall(Rock rock, Queue<Rock> rocks)
    {
        if (rock.Position.Y == 0)
        {
            return false;
        }
        return TryMove(rock, 0, -1, rocks);
    }

    private static bool TryMove(Rock rock, int x, int y, Queue<Rock> rocks)
    {
        Rock attempt = new(rock.Type, new Vector2Int(rock.Position.X + x, rock.Position.Y + y));
        foreach (Rock obstacle in rocks)
        {
            if (attempt.CollidesWith(obstacle))
            {
                return false;
            }
        }
        rock.Position = attempt.Position;
        return true;
    }

    private class JetPattern(string pattern)
    {
        private readonly string _pattern = pattern;
        private int current = 0;

        public char Next() => _pattern[current++ % _pattern.Length];
    }

    private class Rock
    {
        public static char[] Types = ['-', '+', 'J', '|', 'o'];

        public Vector2Int Position { get; set; }
        public char Type { get; }
        public Vector2Int BoundingBoxSize { get; }
        public Rectangle[] CollisionBoxes { get; }

        public Rock(char shape, Vector2Int position)
        {
            Position = position;
            Type = shape;
            switch (shape)
            {
                case '-':
                    BoundingBoxSize = new Vector2Int(4, 1);
                    CollisionBoxes = [new Rectangle(0, 0, 4, 1)];
                    break;
                case '+':
                    BoundingBoxSize = new Vector2Int(3, 3);
                    CollisionBoxes = [new Rectangle(0, 1, 3, 1), new Rectangle(1, 0, 1, 3)];
                    break;
                case 'J':
                    BoundingBoxSize = new Vector2Int(3, 3);
                    CollisionBoxes = [new Rectangle(0, 0, 3, 1), new Rectangle(2, 0, 1, 3)];
                    break;
                case '|':
                    BoundingBoxSize = new Vector2Int(1, 4);
                    CollisionBoxes = [new Rectangle(0, 0, 1, 4)];
                    break;
                case 'o':
                    BoundingBoxSize = new Vector2Int(2, 2);
                    CollisionBoxes = [new Rectangle(0, 0, 2, 2)];
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public Rectangle CurrentBoundingBox()
        {
            return new(Position.X, Position.Y, BoundingBoxSize.X, BoundingBoxSize.Y);
        }

        public bool CollidesWith(Rock obstacle)
        {
            if (CurrentBoundingBox().IntersectsWith(obstacle.CurrentBoundingBox()))
            {
                foreach (Rectangle box in CollisionBoxes)
                {
                    foreach (Rectangle obstacleBox in obstacle.CollisionBoxes)
                    {
                        Rectangle collider = new
                            (
                                Position.X + box.X,
                                Position.Y + box.Y,
                                box.Width,
                                box.Height
                            );
                        Rectangle obstacleCollider = new
                            (
                                obstacle.Position.X + obstacleBox.X,
                                obstacle.Position.Y + obstacleBox.Y,
                                obstacleBox.Width,
                                obstacleBox.Height
                            );
                        if (collider.IntersectsWith(obstacleCollider))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
