// ProgrammingAdvent2023 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2023
// https://adventofcode.com/2023

namespace ProgrammingAdvent2023.Common;

internal readonly struct Vector2Int(int x, int y)
{
    public static Vector2Int UnitX { get => _unitX; }
    private static readonly Vector2Int _unitX = new(1, 0);

    public static Vector2Int UnitY { get => _unitY; }
    private static readonly Vector2Int _unitY = new(0, 1);

    public static Vector2Int Zero { get => _zero; }
    private static readonly Vector2Int _zero = new(0);

    public int X { get; } = x;
    public int Y { get; } = y;

    public Vector2Int(int all) : this(all, all) { }

    public Vector2Int(Vector2Int toCopy) : this(toCopy.X, toCopy.Y) { }

    public override bool Equals(object? obj)
    {
        return (obj is Vector2Int vector) && Equals(vector);
    }

    public bool Equals(Vector2Int other)
    {
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2Int operator -(Vector2Int vectorToNegate)
    {
        return new Vector2Int(-vectorToNegate.X, -vectorToNegate.Y);
    }

    public static Vector2Int operator *(int multi, Vector2Int vector)
    {
        return new Vector2Int(multi * vector.X, multi * vector.Y);
    }

    public static Vector2Int operator *(Vector2Int vector, int multi)
    {
        return multi * vector;
    }

    public static Vector2Int operator /(Vector2Int vector, int divisor)
    {
        return new Vector2Int(vector.X / divisor, vector.Y / divisor);
    }

    public static bool operator ==(Vector2Int a, Vector2Int b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Vector2Int a, Vector2Int b)
    {
        return !a.Equals(b);
    }

    public static int DotProduct(Vector2Int a, Vector2Int b)
    {
        return a.X * b.X + a.Y * b.Y;
    }

    public static explicit operator Vector2Int(Vector3Int vectorToCast)
        => new(vectorToCast.X, vectorToCast.Y);

    public static implicit operator Vector2Int((int, int) tupleToCast)
        => new(tupleToCast.Item1, tupleToCast.Item2);

    public static explicit operator Vector2Int((int, int, int) tupleToCast)
        => new(tupleToCast.Item1, tupleToCast.Item2);

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public int TaxicabMagnitude()
    {
        return Math.Abs(X) + Math.Abs(Y);
    }
}
