using System.Runtime.CompilerServices;

public struct DoubleVector2
{
    public DoubleVector2(double x, double y)
    {
        X = x;
        Y = y;
    }

    public DoubleVector2(double x)
    {
        X = x;
        Y = x;
    }

    public DoubleVector2(DoubleVector2 v)
    {
        X = v.X;
        Y = v.Y;
    }

    public DoubleVector2(ref DoubleVector2 v)
    {
        X = v.X;
        Y = v.Y;
    }

    public DoubleVector2(LongVector2 v)
    {
        X = v.X;
        Y = v.Y;
    }

    public static DoubleVector2 operator +(DoubleVector2 value1, DoubleVector2 value2)
    {
        DoubleVector2 vector;
        vector.X = value1.X + value2.X;
        vector.Y = value1.Y + value2.Y;
        return vector;
    }


    public static DoubleVector2 operator -(DoubleVector2 value1)
    {
        return new DoubleVector2(-value1.X, -value1.Y);
    }

    public static DoubleVector2 operator -(DoubleVector2 value1, DoubleVector2 value2)
    {
        DoubleVector2 vector;
        vector.X = value1.X - value2.X;
        vector.Y = value1.Y - value2.Y;
        return vector;
    }

    public static DoubleVector2 operator /(DoubleVector2 value1, double value)
    {
        DoubleVector2 vector;
        vector.X = value1.X / value;
        vector.Y = value1.Y / value;
        return vector;
    }

    public static DoubleVector2 operator *(DoubleVector2 value1, double value)
    {
        DoubleVector2 vector;
        vector.X = value1.X * value;
        vector.Y = value1.Y * value;
        return vector;
    }

    public static bool operator ==(DoubleVector2 value1, DoubleVector2 value2)
    {
        return value1.X == value2.X && value1.Y == value2.Y;
    }

    public static bool operator !=(DoubleVector2 value1, DoubleVector2 value2)
    {
        return value1.X != value2.X || value1.Y != value2.Y;
    }


    public void Min(DoubleVector2 v)
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

    public void Max(DoubleVector2 v)
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



    public double ManhattanDistance(DoubleVector2 v)
    {
        double distanceX = Math.Abs(X - v.X);
        double distanceY = Math.Abs(Y - v.Y);

        return distanceX + distanceY;
    }

    public override string ToString()
    {
        return "" + X + "," + Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object other)
    {
        if (!(other is DoubleVector2)) return false;

        return Equals((DoubleVector2)other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(DoubleVector2 other)
    {
        return X == other.X && Y == other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return X.GetHashCode() ^ (Y.GetHashCode() << 2);
    }

    const double EPSILON_MAGNITUDE = 9.99999974737875E-06; // ~= 1e-5
    const double EPSILON_MAGNITUDE_SQR = EPSILON_MAGNITUDE * EPSILON_MAGNITUDE;

    public void Normalize()
    {
        double magnitude = this.Magnitude;
        if (magnitude > EPSILON_MAGNITUDE)
            this = this / magnitude;
        else
            this = DoubleVector2.Zero;
    }

    public DoubleVector2 Normalized
    {
        get
        {
            DoubleVector2 vector2d = new DoubleVector2(this.X, this.Y);
            vector2d.Normalize();
            return vector2d;
        }
    }

    public double Magnitude
    {
        get { return Math.Sqrt(this.X * this.X + this.Y * this.Y); }
    }

    public double SqrMagnitude
    {
        get { return this.X * this.X + this.Y * this.Y; }
    }


    public double X;
    public double Y;

    public static readonly DoubleVector2 Zero = new DoubleVector2(0, 0);
    public static readonly DoubleVector2 Left = new DoubleVector2(-1, 0);
    public static readonly DoubleVector2 Right = new DoubleVector2(1, 0);
    public static readonly DoubleVector2 Up = new DoubleVector2(0, 1);
    public static readonly DoubleVector2 Down = new DoubleVector2(0, -1);

    public static readonly DoubleVector2[] Directions = new DoubleVector2[] { Left, Right, Up, Down };
}