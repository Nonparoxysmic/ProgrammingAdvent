// ProgrammingAdvent2018 by Nonparoxysmic
// https://github.com/Nonparoxysmic/ProgrammingAdvent
// for Advent of Code 2018
// https://adventofcode.com/2018

using System;

namespace ProgrammingAdvent2018.Program
{
    public class Vector2Int : IEquatable<Vector2Int>
    {
        static public Vector2Int UnitX { get => new Vector2Int(1, 0); }
        static public Vector2Int UnitY { get => new Vector2Int(0, 1); }
        static public Vector2Int Zero { get => new Vector2Int(0); }

        public int X { get; set; }
        public int Y { get; set; }

        public Vector2Int(int all)
        {
            X = all;
            Y = all;
        }

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2Int(Vector2Int vectorToCopy)
        {
            X = vectorToCopy.X;
            Y = vectorToCopy.Y;
        }

        public override bool Equals(object obj)
        {
            return (obj is Vector2Int vector) && Equals(vector);
        }

        public bool Equals(Vector2Int other)
        {
            return !(other is null) && X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X ^ Y;
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
            return new Vector2Int(multi * vector.X, multi * vector.Y);
        }

        public static bool operator ==(Vector2Int a, Vector2Int b)
        {
            if (a is null) { return b is null; }
            return a.Equals(b);
        }

        public static bool operator !=(Vector2Int a, Vector2Int b)
        {
            if (a is null) { return !(b is null); }
            return !a.Equals(b);
        }

        public static explicit operator Vector2Int(Vector3Int vectorToCast)
            => new Vector2Int(vectorToCast.X, vectorToCast.Y);

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public int TaxicabMagnitude()
        {
            return Math.Abs(X) + Math.Abs(Y);
        }
    }

    public class Vector3Int : IEquatable<Vector3Int>
    {
        static public Vector3Int UnitX { get => new Vector3Int(1, 0, 0); }
        static public Vector3Int UnitY { get => new Vector3Int(0, 1, 0); }
        static public Vector3Int UnitZ { get => new Vector3Int(0, 0, 1); }
        static public Vector3Int Zero { get => new Vector3Int(0); }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Vector3Int(int all)
        {
            X = all;
            Y = all;
            Z = all;
        }

        public Vector3Int(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3Int(Vector3Int vectorToCopy)
        {
            X = vectorToCopy.X;
            Y = vectorToCopy.Y;
            Z = vectorToCopy.Z;
        }

        public Vector3Int(Vector2Int vectorToCopy)
        {
            X = vectorToCopy.X;
            Y = vectorToCopy.Y;
            Z = 0;
        }

        public override bool Equals(object obj)
        {
            return (obj is Vector3Int vector) && Equals(vector);
        }

        public bool Equals(Vector3Int other)
        {
            return !(other is null) && X == other.X && Y == other.Y && Z == other.Z;
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
            return new Vector3Int(multi * vector.X, multi * vector.Y, multi * vector.Z);
        }

        public static bool operator ==(Vector3Int a, Vector3Int b)
        {
            if (a is null) { return b is null; }
            return a.Equals(b);
        }

        public static bool operator !=(Vector3Int a, Vector3Int b)
        {
            if (a is null) { return !(b is null); }
            return !a.Equals(b);
        }

        public static implicit operator Vector3Int(Vector2Int vectorToCast)
            => new Vector3Int(vectorToCast.X, vectorToCast.Y, 0);

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        public int TaxicabMagnitude()
        {
            return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
        }
    }
}
