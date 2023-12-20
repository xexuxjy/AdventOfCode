using System.Collections;
using System.Runtime.CompilerServices;

public struct LongVector2
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public LongVector2(long x, long y)
    {
        X = x;
        Y = y;
    }

    public LongVector2(long x)
    {
        X = x;
        Y = x;
    }

    public LongVector2(LongVector2 v)
    {
        X = v.X;
        Y = v.Y;
    }

    public LongVector2(ref LongVector2 v)
    {
        X = v.X;
        Y = v.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LongVector2 operator +(LongVector2 value1, LongVector2 value2)
    {
        LongVector2 vector;
        vector.X = value1.X + value2.X;
        vector.Y = value1.Y + value2.Y;
        return vector;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LongVector2 operator -(LongVector2 value1, LongVector2 value2)
    {
        LongVector2 vector;
        vector.X = value1.X - value2.X;
        vector.Y = value1.Y - value2.Y;
        return vector;
    }

    public static LongVector2 operator /(LongVector2 value1, int value)
    {
        LongVector2 vector;
        vector.X = value1.X / value;
        vector.Y = value1.Y / value;
        return vector;
    }

    public static LongVector2 operator *(LongVector2 value1, int value)
    {
        LongVector2 vector;
        vector.X = value1.X * value;
        vector.Y = value1.Y * value;
        return vector;
    }

    public void Min(LongVector2 v)
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

    public void Max(LongVector2 v)
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


    public long ManhattanDistance(LongVector2 v)
    {
        long distanceX = Math.Abs(X - v.X);
        long distanceY = Math.Abs(Y - v.Y);

        return distanceX + distanceY;
    }

    public void ManhattanDistance(LongVector2 v, out long distanceX, out long distanceY)
    {
        distanceX = Math.Abs(X - v.X);
        distanceY = Math.Abs(Y - v.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object other)
    {
        if (!(other is LongVector2)) return false;

        return Equals((LongVector2)other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(LongVector2 other)
    {
        return X == other.X && Y == other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return X.GetHashCode() ^ (Y.GetHashCode() << 2);
    }


    public long X;
    public long Y;

    public static readonly LongVector2 Left = new LongVector2(-1, 0);
    public static readonly LongVector2 Right = new LongVector2(1, 0);
    public static readonly LongVector2 Up = new LongVector2(0, 1);
    public static readonly LongVector2 Down = new LongVector2(0, -1);


    public static readonly LongVector2[] Directions = new LongVector2[] { Left, Right, Up, Down };
}