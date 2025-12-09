using System.Collections;
using System.Runtime.CompilerServices;

public struct LongVector3 : IComparable<LongVector3>, IEquatable<LongVector3>
{
    public static readonly LongVector3 Zero = new LongVector3();

    public LongVector3(long x, long y, long z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public LongVector3(long x)
    {
        X = x;
        Y = x;
        Z = x;
    }

    public LongVector3(LongVector3 v)
    {
        X = v.X;
        Y = v.Y;
        Z = v.Z;
    }

    public LongVector3(ref LongVector3 v)
    {
        X = v.X;
        Y = v.Y;
        Z = v.Z;
    }

    public static LongVector3 operator +(LongVector3 value1, LongVector3 value2)
    {
        LongVector3 vector;
        vector.X = value1.X + value2.X;
        vector.Y = value1.Y + value2.Y;
        vector.Z = value1.Z + value2.Z;
        return vector;
    }



    public static LongVector3 operator -(LongVector3 value1, LongVector3 value2)
    {
        LongVector3 vector;
        vector.X = value1.X - value2.X;
        vector.Y = value1.Y - value2.Y;
        vector.Z = value1.Z - value2.Z;
        return vector;
    }

    public static LongVector3 operator /(LongVector3 value1, long value)
    {
        LongVector3 vector;
        vector.X = value1.X / value;
        vector.Y = value1.Y / value;
        vector.Z = value1.Z / value;
        return vector;
    }

    public static LongVector3 operator *(LongVector3 value1, long value)
    {
        LongVector3 vector;
        vector.X = value1.X * value;
        vector.Y = value1.Y * value;
        vector.Z = value1.Z * value;
        return vector;
    }

    public static bool operator <(LongVector3 value1, LongVector3 value2)
    {
        return value1.X < value2.X || value1.Y < value2.Y || value1.Z < value2.Z;
    }

    public static bool operator <=(LongVector3 value1, LongVector3 value2)
    {
        return value1.X <= value2.X || value1.Y <= value2.Y || value1.Z <= value2.Z;
    }


    public static bool operator >(LongVector3 value1, LongVector3 value2)
    {
        return value1.X > value2.X || value1.Y > value2.Y || value1.Z > value2.Z;
    }

    public static bool operator >=(LongVector3 value1, LongVector3 value2)
    {
        return value1.X >= value2.X || value1.Y >= value2.Y || value1.Z >= value2.Z;
    }

    public static bool operator ==(LongVector3 value1, LongVector3 value2)
    {
        return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z;
    }

    public static bool operator !=(LongVector3 value1, LongVector3 value2)
    {
        return value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z;
    }


    public void Min(LongVector3 v)
    {
        if (v.X < X)
        {
            X = v.X;
        }

        if (v.Y < Y)
        {
            Y = v.Y;
        }

        if (v.Z < Z)
        {
            Z = v.Z;
        }

    }

    public void Max(LongVector3 v)
    {
        if (v.X > X)
        {
            X = v.X;
        }

        if (v.Y > Y)
        {
            Y = v.Y;
        }
        if (v.Z > Z)
        {
            Z = v.Z;
        }

    }


    public long ManhattanDistance(LongVector3 v)
    {
        long distanceX = Math.Abs(X - v.X);
        long distanceY = Math.Abs(Y - v.Y);
        long distanceZ = Math.Abs(Z - v.Z);
        return distanceX + distanceY + distanceZ;

    }

    public double Magnitude
    {
        get { return Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z); }
    }

    public double Magnitude2
    {
        get
        {
            long x2 = this.X * this.X;
            long y2 = this.Y * this.Y;
            long z2 = this.Z * this.Z;
            long total = x2+y2+z2;
            double sqrt = Math.Sqrt(total);
            return sqrt;
        }
    }

    
    public double SqrMagnitude
    {
        get { return this.X * this.X + this.Y * this.Y + this.Z * this.Z; }
    }


    public override string ToString()
    {
        return "(" + X + "," + Y + "," + Z + ")";
    }

    public int CompareTo(LongVector3 other)
    {
        if (this == other)
        {
            return 0;
        }

        if (this < other)
        {
            return -1;
        }

        return 1;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(LongVector3 other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public long this[long i]
    {
        get
        {
            switch (i)
            {
                case (0): return X;
                case (1): return Y;
                case (2): return Z;
                default:
                    {
                        return 0;
                    }
            }
        }
        set
        {
            switch (i)
            {
                case (0): X = value; break;
                case (1): Y = value; break;
                case (2): Z = value; break;
                default:
                    {
                        break;
                    }
            }
        }
    }

    public long X;
    public long Y;
    public long Z;
}