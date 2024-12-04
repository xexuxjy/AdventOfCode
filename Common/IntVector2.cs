using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

public struct IntVector2
{
    public IntVector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public IntVector2(int x)
    {
        X = x;
        Y = x;
    }

    public IntVector2(IntVector2 v)
    {
        X = v.X;
        Y = v.Y;
    }

    public IntVector2(ref IntVector2 v)
    {
        X = v.X;
        Y = v.Y;
    }

    public static IntVector2 operator +(IntVector2 value1, IntVector2 value2)
    {
        IntVector2 vector;
        vector.X = value1.X + value2.X;
        vector.Y = value1.Y + value2.Y;
        return vector;
    }


    public static IntVector2 operator -(IntVector2 value1)
    {
        return new IntVector2(-value1.X, -value1.Y);
    }

    public static IntVector2 operator -(IntVector2 value1, IntVector2 value2)
    {
        IntVector2 vector;
        vector.X = value1.X - value2.X;
        vector.Y = value1.Y - value2.Y;
        return vector;
    }

    public static IntVector2 operator /(IntVector2 value1, int value)
    {
        IntVector2 vector;
        vector.X = value1.X / value;
        vector.Y = value1.Y / value;
        return vector;
    }

    public static IntVector2 operator *(IntVector2 value1, int value)
    {
        IntVector2 vector;
        vector.X = value1.X * value;
        vector.Y = value1.Y * value;
        return vector;
    }

    public static bool operator ==(IntVector2 value1, IntVector2 value2)
    {
        return value1.X == value2.X && value1.Y == value2.Y;
    }

    public static bool operator !=(IntVector2 value1, IntVector2 value2)
    {
        return value1.X != value2.X || value1.Y != value2.Y;
    }


    public void Min(IntVector2 v)
    {
        if (v.X < X)
        {
            X = v.X;
        }

        if (v.Y < Y)
        {
            Y = v.Y;
        }
    }

    public void Max(IntVector2 v)
    {
        if (v.X > X)
        {
            X = v.X;
        }

        if (v.Y > Y)
        {
            Y = v.Y;
        }
    }


    public int ManhattanDistance(IntVector2 v)
    {
        int distanceX = Math.Abs(X - v.X);
        int distanceY = Math.Abs(Y - v.Y);

        return distanceX + distanceY;

    }

    public override string ToString()
    {
        return "" + X + "," + Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object other)
    {
        if (!(other is IntVector2)) return false;

        return Equals((IntVector2)other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(IntVector2 other)
    {
        return X == other.X && Y == other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return X.GetHashCode() ^ (Y.GetHashCode() << 2);
    }

    public int X;
    public int Y;

    public static readonly IntVector2 Left = new IntVector2(-1, 0);
    public static readonly IntVector2 Right = new IntVector2(1, 0);
    public static readonly IntVector2 Up = new IntVector2(0, 1);
    public static readonly IntVector2 Down = new IntVector2(0, -1);

    public static readonly IntVector2[] Directions = new IntVector2[] { Left, Right, Up, Down };


    public static readonly IntVector2[] DiagonalDirections = new IntVector2[] {new IntVector2(-1,-1),new IntVector2(1,1),new IntVector2(-1,1),new IntVector2(1,-1) };

    public static readonly IntVector2[] AllDirections = new IntVector2[] { Left, Right,Up,Down,new IntVector2(-1,-1),new IntVector2(1,1),new IntVector2(-1,1),new IntVector2(1,-1) };







}