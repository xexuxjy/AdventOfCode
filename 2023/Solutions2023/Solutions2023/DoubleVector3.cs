using System.Runtime.CompilerServices;

public struct DoubleVector3
{
    public DoubleVector3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public DoubleVector3(double x)
    {
        X = x;
        Y = x;
        Z = x;
    }

    public DoubleVector3(DoubleVector3 v)
    {
        X = v.X;
        Y = v.Y;
        Z = v.Z;
    }

    public DoubleVector3(ref DoubleVector3 v)
    {
        X = v.X;
        Y = v.Y;
        Z = v.Z;
    }

    public DoubleVector3(LongVector2 v)
    {
        X = v.X;
        Y = v.Y;
    }

    public static DoubleVector3 operator +(DoubleVector3 value1, DoubleVector3 value2)
    {
        DoubleVector3 vector;
        vector.X = value1.X + value2.X;
        vector.Y = value1.Y + value2.Y;
        vector.Z = value1.Z + value2.Z;
        
        return vector;
    }


    public static DoubleVector3 operator -(DoubleVector3 value1)
    {
        return new DoubleVector3(-value1.X, -value1.Y,-value1.Z);
    }

    public static DoubleVector3 operator -(DoubleVector3 value1, DoubleVector3 value2)
    {
        DoubleVector3 vector;
        vector.X = value1.X - value2.X;
        vector.Y = value1.Y - value2.Y;
        vector.Z = value1.Z - value2.Z;
        return vector;
    }

    public static DoubleVector3 operator /(DoubleVector3 value1, double value)
    {
        DoubleVector3 vector;
        vector.X = value1.X / value;
        vector.Y = value1.Y / value;
        vector.Z = value1.Z / value;
        return vector;
    }

    public static DoubleVector3 operator *(DoubleVector3 value1, double value)
    {
        DoubleVector3 vector;
        vector.X = value1.X * value;
        vector.Y = value1.Y * value;
        vector.Z = value1.Z * value;
        return vector;
    }

    public bool FuzzyEquals(DoubleVector3 v)
    {
        return Math.Abs(X-v.X) < EPSILON_MAGNITUDE &&  Math.Abs(Y-v.Y) < EPSILON_MAGNITUDE && Math.Abs(Z-v.Z) < EPSILON_MAGNITUDE;
    }
    
    public static bool operator ==(DoubleVector3 value1, DoubleVector3 value2)
    {
        return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z;
    }

    public static bool operator !=(DoubleVector3 value1, DoubleVector3 value2)
    {
        return value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z;
    }

    public static bool operator >(DoubleVector3 value1, DoubleVector3 value2)
    {
        return value1.X > value2.X && value1.Y > value2.Y && value1.Z > value2.Z;
    }

    public static bool operator <(DoubleVector3 value1, DoubleVector3 value2)
    {
        return value1.X < value2.X && value1.Y < value2.Y && value1.Z < value2.Z;
    }
    
    public static bool operator >=(DoubleVector3 value1, DoubleVector3 value2)
    {
        return value1.X >= value2.X && value1.Y >= value2.Y && value1.Z >= value2.Z;
    }

    public static bool operator <=(DoubleVector3 value1, DoubleVector3 value2)
    {
        return value1.X <= value2.X && value1.Y <= value2.Y && value1.Z <= value2.Z;
    }
    

    public void Min(DoubleVector3 v)
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

    public void Max(DoubleVector3 v)
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

    public static double Dot(DoubleVector3 value1, DoubleVector3 value2)
    {
        return value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z;
    }

    
    public static DoubleVector3 Cross(DoubleVector3 value1, DoubleVector3 value2)
    {
        double x, y, z;
        x = value1.Y * value2.Z - value2.Y * value1.Z;
        y = (value1.X * value2.Z - value2.X * value1.Z) * -1;
        z = value1.X * value2.Y - value2.X * value1.Y;
        return new DoubleVector3(x, y, z);
    }


    public double ManhattanDistance(DoubleVector3 value)
    {
        double distanceX = Math.Abs(X - value.X);
        double distanceY = Math.Abs(Y - value.Y);
        double distanceZ = Math.Abs(Z - value.Z);

        return distanceX + distanceY+distanceZ;
    }

    public override string ToString()
    {
        return "" + X + "," + Y + ","+Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object other)
    {
        if (!(other is DoubleVector3)) return false;

        return Equals((DoubleVector3)other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(DoubleVector3 other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        var yHash = Y.GetHashCode();
        var zHash = Z.GetHashCode();
        return X.GetHashCode() ^ (yHash << 4) ^ (yHash >> 28) ^ (zHash >> 4) ^ (zHash << 28);
    }

    const double EPSILON_MAGNITUDE = 9.99999974737875E-06; // ~= 1e-5
    const double EPSILON_MAGNITUDE_SQR = EPSILON_MAGNITUDE * EPSILON_MAGNITUDE;

    public void Normalize()
    {
        double magnitude = this.Magnitude;
        if (magnitude > EPSILON_MAGNITUDE)
            this = this / magnitude;
        else
            this = DoubleVector3.Zero;
    }

    public DoubleVector3 Normalized
    {
        get
        {
            DoubleVector3 vector3d = new DoubleVector3(this.X, this.Y,this.Z);
            vector3d.Normalize();
            return vector3d;
        }
    }

    public double Magnitude
    {
        get { return Math.Sqrt(this.X * this.X + this.Y * this.Y+this.Z * this.Z); }
    }

    public double SqrMagnitude
    {
        get { return this.X * this.X + this.Y * this.Y+this.Z * this.Z; }
    }

    public double this[int i]
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
                    return 0.0;
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
    
    public double X;
    public double Y;
    public double Z;

    public static readonly DoubleVector3 Zero = new DoubleVector3(0, 0,0);
    // public static readonly DoubleVector3 Left = new DoubleVector3(-1, 0);
    // public static readonly DoubleVector3 Right = new DoubleVector3(1, 0);
    // public static readonly DoubleVector3 Up = new DoubleVector3(0, 1);
    // public static readonly DoubleVector3 Down = new DoubleVector3(0, -1);
    //
    // public static readonly DoubleVector3[] Directions = new DoubleVector3[] { Left, Right, Up, Down };
}