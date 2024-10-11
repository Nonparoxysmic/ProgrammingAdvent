// ProgrammingAdvent2022 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2022
// https://adventofcode.com/2022

namespace ProgrammingAdvent2022.Common;

internal readonly struct Vector3Int(int x, int y, int z)
{
    public static Vector3Int UnitX { get => _unitX; }
    private static readonly Vector3Int _unitX = new(1, 0, 0);

    public static Vector3Int UnitY { get => _unitY; }
    private static readonly Vector3Int _unitY = new(0, 1, 0);

    public static Vector3Int UnitZ { get => _unitZ; }
    private static readonly Vector3Int _unitZ = new(0, 0, 1);

    public static Vector3Int Zero { get => _zero; }
    private static readonly Vector3Int _zero = new(0);

    public int X { get; } = x;
    public int Y { get; } = y;
    public int Z { get; } = z;

    public Vector3Int(int all) : this(all, all, all) { }

    public Vector3Int(Vector3Int toCopy) : this(toCopy.X, toCopy.Y, toCopy.Z) { }

    public override bool Equals(object? obj)
    {
        return (obj is Vector3Int vector) && Equals(vector);
    }

    public bool Equals(Vector3Int other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override int GetHashCode()
    {
        return X ^ Y ^ Z;
    }

    public static Vector3Int operator +(Vector3Int a, Vector3Int b)
    {
        return new Vector3Int(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public static Vector3Int operator -(Vector3Int a, Vector3Int b)
    {
        return new Vector3Int(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    public static Vector3Int operator -(Vector3Int vectorToNegate)
    {
        return new Vector3Int(-vectorToNegate.X, -vectorToNegate.Y, -vectorToNegate.Z);
    }

    public static Vector3Int operator *(int multi, Vector3Int vector)
    {
        return new Vector3Int(multi * vector.X, multi * vector.Y, multi * vector.Z);
    }

    public static Vector3Int operator *(Vector3Int vector, int multi)
    {
        return multi * vector;
    }

    public static Vector3Int operator /(Vector3Int vector, int divisor)
    {
        return new Vector3Int(vector.X / divisor, vector.Y / divisor, vector.Z / divisor);
    }

    public static bool operator ==(Vector3Int a, Vector3Int b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Vector3Int a, Vector3Int b)
    {
        return !a.Equals(b);
    }

    public static implicit operator Vector3Int(Vector2Int vectorToCast)
        => new(vectorToCast.X, vectorToCast.Y, 0);

    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }

    public int TaxicabMagnitude()
    {
        return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
    }
}
