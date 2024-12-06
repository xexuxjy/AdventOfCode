using System.Collections;
using System.Runtime.CompilerServices;

public struct IntVector4
{
    public static readonly IntVector4 Zero = new IntVector4();

    public IntVector4(int x, int y, int z,int w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public IntVector4(int x)
    {
        X = x;
        Y = x;
        Z = x;
        W = x;
    }

    public IntVector4(IntVector4 v)
    {
        X = v.X;
        Y = v.Y;
        Z = v.Z;
        W = v.W;
    }

    public IntVector4(ref IntVector4 v)
    {
        X = v.X;
        Y = v.Y;
        Z = v.Z;
        W = v.W;
    }

    public static IntVector4 operator +(IntVector4 value1, IntVector4 value2)
    {
        IntVector4 vector;
        vector.X = value1.X + value2.X;
        vector.Y = value1.Y + value2.Y;
        vector.Z = value1.Z + value2.Z;
        vector.W = value1.W + value2.W;
        return vector;
    }



    public static IntVector4 operator -(IntVector4 value1, IntVector4 value2)
    {
        IntVector4 vector;
        vector.X = value1.X - value2.X;
        vector.Y = value1.Y - value2.Y;
        vector.Z = value1.Z - value2.Z;
        vector.W= value1.W - value2.W;
        return vector;
    }

    public static IntVector4 operator /(IntVector4 value1, int value)
    {
        IntVector4 vector;
        vector.X = value1.X / value;
        vector.Y = value1.Y / value;
        vector.Z = value1.Z / value;
        vector.W = value1.W / value;

        return vector;
    }

    public static IntVector4 operator *(IntVector4 value1, int value)
    {
        IntVector4 vector;
        vector.X = value1.X * value;
        vector.Y = value1.Y * value;
        vector.Z = value1.Z * value;
        vector.W = value1.W * value;
        return vector;
    }

    public static bool operator <(IntVector4 value1, IntVector4 value2)
    {
        return value1.X < value2.X || value1.Y < value2.Y || value1.Z < value2.Z || value1.W < value2.W;
    }

    public static bool operator <=(IntVector4 value1, IntVector4 value2)
    {
        return value1.X <= value2.X || value1.Y <= value2.Y || value1.Z <= value2.Z || value1.W <= value2.W;
    }


    public static bool operator >(IntVector4 value1, IntVector4 value2)
    {
        return value1.X > value2.X || value1.Y > value2.Y || value1.Z > value2.Z || value1.W > value2.W;
    }

    public static bool operator >=(IntVector4 value1, IntVector4 value2)
    {
        return value1.X >= value2.X || value1.Y >= value2.Y || value1.Z >= value2.Z || value1.W >= value2.W;
    }

    public static bool operator ==(IntVector4 value1, IntVector4 value2)
    {
        return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z && value1.W == value2.W;
    }

    public static bool operator !=(IntVector4 value1, IntVector4 value2)
    {
        return value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z || value1.W != value2.W;
    }


    public void Min(IntVector4 v)
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

        if(v.W < W)
        {
            W = v.W;
        }

    }

    public void Max(IntVector4 v)
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
        if(v.W > W) 
        {
            W = v.W;
        }

    }


    public int ManhattanDistance(IntVector4 v)
    {
        int distanceX = Math.Abs(X - v.X);
        int distanceY = Math.Abs(Y - v.Y);
        int distanceZ = Math.Abs(Z - v.Z);
        int distanceW = Math.Abs(W - v.W);

        return distanceX + distanceY + distanceZ + distanceW;

    }

    public double Magnitude
    {
        get { return Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z+this.W*this.W); }
    }

    public double SqrMagnitude
    {
        get { return this.X * this.X + this.Y * this.Y + this.Z * this.Z+this.W*this.W; }
    }


    public override string ToString()
    {
        return "(" + X + "," + Y + "," + Z + ","+W+")";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object other)
    {
        if (!(other is IntVector4)) return false;

        return Equals((IntVector4)other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(IntVector4 other)
    {
        return X == other.X && Y == other.Y && Z == other.Z && W==other.W;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return X ^ (Y << 8) ^ (Y >> 24) ^ (Z << 16) ^ (Z >> 16) ^ (W << 24) ^ (W >> 8);

    }

    public int this[int i]
    {
        get
        {
            switch (i)
            {
                case (0): return X;
                case (1): return Y;
                case (2): return Z;
                case (3): return W;
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
                case (3): W = value; break;
                default:
                    {
                        break;
                    }
            }
        }
    }

    public int X;
    public int Y;
    public int Z;
    public int W;
}