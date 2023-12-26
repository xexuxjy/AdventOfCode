using System.Drawing;
using System.Numerics;
using CyrusBeckLineClipping;

public class Test24 : BaseTest
{
    public override void Initialise()
    {
        TestID = 24;
        IsTestInput = false;
        IsPart2 = false;
    }

    public override void Execute()
    {
        bool is2d = true;

        List<Hailstone> hailstones = new List<Hailstone>();

        int count = 0;
        foreach (string line in m_dataFileContents)
        {
            String[] data = line.Split('@', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            String[] pos = data[0].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            String[] vel = data[1].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            Hailstone hailstone = new Hailstone();
            hailstone.Id = count++;
            hailstone.Position = FromString(pos);
            hailstone.Velocity = FromString(vel);

            if (is2d)
            {
                hailstone.Position.Z = 0;
                hailstone.Velocity.Z = 0;
            }

            hailstones.Add(hailstone);
        }

        DoubleVector3 min = IsTestInput?new DoubleVector3(7, 7, 0):new DoubleVector3(200000000000000, 200000000000000, 0);
        DoubleVector3 max = IsTestInput?new DoubleVector3(27, 27, 0):new DoubleVector3(400000000000000, 400000000000000, 0);


        List<(Hailstone, Hailstone)> pairs = new List<(Hailstone, Hailstone)>();

        for (int i = 0; i < hailstones.Count; ++i)
        {
            for (int j = i + 1; j < hailstones.Count; ++j)
            {
                pairs.Add((hailstones[i], hailstones[j]));
            }
        }

        int numIntersects = 0;

        foreach (var pair in pairs)
        {
            IntersectionHelper.Ray result = IntersectionHelper.FindShortestDistance(
                new IntersectionHelper.Ray(pair.Item1.Position, pair.Item1.Direction),
                new IntersectionHelper.Ray(pair.Item2.Position, pair.Item2.Direction));

            if(result.Direction == DoubleVector3.Zero && result.Position >= min && result.Position <= max)
            {
                numIntersects++;
            }

            if (false)
            {
                DebugOutput($":A {pair.Item1.Position} {pair.Item1.Velocity}");
                DebugOutput($":B {pair.Item2.Position} {pair.Item2.Velocity}");


                if (result.Position == pair.Item1.Position)
                {
                    DebugOutput("Past for hailstone A");
                }
                else if (result.Position == pair.Item2.Position)
                {
                    DebugOutput("Past for hailstone B");
                }

                if (result.Position >= min && result.Position <= max)
                {
                    DebugOutput("In test window");
                }
                else
                {
                    DebugOutput("Outside test window");
                }

                DebugOutput($"result : {result.Position} {result.Direction} ");
                DebugOutput("");
            }
        }

        DebugOutput("Number of intersects is : " + numIntersects);
    }


    public DoubleVector3 FromString(string[] data)
    {
        return new DoubleVector3(Double.Parse(data[0]), Double.Parse(data[1]), Double.Parse((data[2])));
    }

    public class Hailstone
    {
        public int Id;
        public DoubleVector3 Position;
        public DoubleVector3 Velocity;
        public DoubleVector3 Direction => Velocity.Normalized;


        public void Step()
        {
            Position += Velocity;
        }

        public void Step2D()
        {
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
        }
    }
}