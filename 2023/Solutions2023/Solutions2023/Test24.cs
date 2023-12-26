using System.Drawing;
using System.Numerics;
using CyrusBeckLineClipping;

public class Test24 : BaseTest
{
    public override void Initialise()
    {
        TestID = 24;
        IsTestInput = true;
        IsPart2 = true;
    }

    public override void Execute()
    {
        bool is2d = !IsPart2;

        List<Hailstone> hailstones = new List<Hailstone>();

        int count = 0;
        foreach (string line in m_dataFileContents)
        {
            String[] data = line.Split('@', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            String[] pos = data[0].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            String[] vel = data[1].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            Hailstone hailstone = new Hailstone();
            hailstone.Id = ""+count++;
            hailstone.Position = DV3FromString(pos);
            hailstone.Velocity = DV3FromString(vel);

            if (is2d)
            {
                hailstone.Position.Z = 0;
                hailstone.Velocity.Z = 0;
            }

            hailstones.Add(hailstone);
        }

        // List<Hailstone> sortedFailstones = new List<Hailstone>();
        // hailstones.AddRange(hailstones.OrderBy(x => x.Position.X).ThenBy(y => y.Position.Y).ThenBy(z => z.Position.Z));

        DoubleVector3 min = IsTestInput?new DoubleVector3(7, 7, 0):new DoubleVector3(200000000000000, 200000000000000, 0);
        DoubleVector3 max = IsTestInput?new DoubleVector3(27, 27, 0):new DoubleVector3(400000000000000, 400000000000000, 0);


        if (IsPart2)
        {
            Part2(hailstones, min, max);
        }
        else
        {
            Part1(hailstones, min, max);
        }



        
    }

    public void Part1(List<Hailstone> hailstones,DoubleVector3 min,DoubleVector3 max)
    {
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

    public void Part2(List<Hailstone> hailstones, DoubleVector3 min, DoubleVector3 max)
    {
        DoubleVector3 minVelocity = new DoubleVector3(int.MaxValue, int.MaxValue, int.MaxValue);
        DoubleVector3 maxVelocity = new DoubleVector3(int.MinValue, int.MinValue, int.MinValue);

        foreach (Hailstone h in hailstones)
        {
            minVelocity.Min(h.Velocity);
            maxVelocity.Max(h.Velocity);
        }

        minVelocity -= new DoubleVector3(1, 1, 1);
        maxVelocity += new DoubleVector3(1, 1, 1);
        
        HashSet<IntVector2> validXPV = PossiblePositions(hailstones, minVelocity, maxVelocity, 0);
        DebugOutput("XOptions : "+string.Join("  ",validXPV));
        HashSet<IntVector2> validYPV = PossiblePositions(hailstones, minVelocity, maxVelocity, 1);
        DebugOutput("YOptions : "+string.Join("  ",validYPV));
        HashSet<IntVector2> validZPV = PossiblePositions(hailstones, minVelocity, maxVelocity, 2);
        DebugOutput("ZOptions : "+string.Join("  ",validZPV));

        // HashSet<IntVector2> validXPV = PossiblePositionsRay(hailstones, minVelocity, maxVelocity, 0);
        // DebugOutput("XOptions : "+string.Join("  ",validXPV));
        // HashSet<IntVector2> validYPV = PossiblePositionsRay(hailstones, minVelocity, maxVelocity, 1);
        // DebugOutput("YOptions : "+string.Join("  ",validYPV));
        // HashSet<IntVector2> validZPV = PossiblePositionsRay(hailstones, minVelocity, maxVelocity, 2);
        // DebugOutput("ZOptions : "+string.Join("  ",validZPV));

        
        
        
        int maxTime = 1000;
        
        Hailstone rock = new Hailstone();
        rock.Id = "Rock";

        DoubleVector3 foundPosition = new DoubleVector3();
        DoubleVector3 foundVelocity = new DoubleVector3();
        
        foreach(IntVector2 x in validXPV)
        {
            foreach(IntVector2 y in validYPV)
            {
                foreach (IntVector2 z in validZPV)
                {
                    rock.Position = new DoubleVector3(x.X, y.X, z.X);
                    rock.Velocity = new DoubleVector3(x.Y, y.Y, z.Y);

                    HashSet<Hailstone> hitHailstones = new HashSet<Hailstone>();

                    for (int time = 1; time <= maxTime; ++time)
                    {
                        foreach (Hailstone h2 in hailstones)
                        {
                        
                            if (rock.Position + (rock.Velocity * time) == h2.Position + (h2.Velocity * time))
                            {
                                hitHailstones.Add(h2);
                                // DebugOutput(
                                //     $"Time {time}  Testing {rock.Id} {rock.Position.X + (rock.Velocity * time).X}  against   {h2.Id}  {h2.Position.X + (h2.Velocity * time).X}   {rock.Position.X + (rock.Velocity * time).X == h2.Position.X + (h2.Velocity * time).X}");
                            }

                            if (hitHailstones.Count == hailstones.Count)
                            {
                                foundPosition = rock.Position;
                                foundVelocity = rock.Velocity;
                                goto haveSolution;
                            }
                        }
                    }
                    
                    
                }
            }
        }
        
        haveSolution:
        DebugOutput($"Found solution with {foundPosition} {foundVelocity}");
    }
    
    
    

    public HashSet<IntVector2> PossiblePositions(List<Hailstone> hailstones, DoubleVector3 minVelocity, DoubleVector3 maxVelocity, int axis)
    {

        int maxTime = 1000;

        HashSet<IntVector2> validXPV = new HashSet<IntVector2>();

        foreach (Hailstone h in hailstones)
        {
            //DebugOutput($"** Hailstone {h.Id} **** [{h.Position}]  [{h.Velocity}]");
            
            Hailstone rock = new Hailstone();
            rock.Id = "Rock";

            // minVelocity[axis] = minVelocity[axis];
            // maxVelocity[axis] = maxVelocity[axis];
            
            
            DoubleVector3 diff = maxVelocity - minVelocity;
            
            for (int x = 0; x < diff[axis]; x++)
            {
                DoubleVector3 t = new DoubleVector3();
                t[axis] = x;
                rock.Velocity = minVelocity + t;
                rock.Position = (h.Position+h.Velocity)-rock.Velocity;
                
                //DebugOutput($"Rock [{rock.Position}]  [{rock.Velocity}]");
                
                int numIntersects = 0;

                HashSet<Hailstone> hitHailstones = new HashSet<Hailstone>();

                for (int time = 1; time <= maxTime; ++time)
                {
                    foreach (Hailstone h2 in hailstones)
                    {
                        
                        if (rock.Position[axis] + (rock.Velocity * time)[axis] == h2.Position[axis] + (h2.Velocity * time)[axis])
                        {
                            hitHailstones.Add(h2);
                            // DebugOutput(
                            //     $"Time {time}  Testing {rock.Id} {rock.Position.X + (rock.Velocity * time).X}  against   {h2.Id}  {h2.Position.X + (h2.Velocity * time).X}   {rock.Position.X + (rock.Velocity * time).X == h2.Position.X + (h2.Velocity * time).X}");
                        }

                        if (hitHailstones.Count == hailstones.Count)
                        {
                            goto testvalid;
                        }
                    }
                }
                
                testvalid:
                if (hitHailstones.Count == hailstones.Count)
                {
                    validXPV.Add(new IntVector2((int)rock.Position[axis], (int)rock.Velocity[axis]));
                }

            }
        }

        return validXPV;
    }
 
    
    public List<IntVector2> PossiblePositionsRay(List<Hailstone> hailstones, DoubleVector3 minVelocity, DoubleVector3 maxVelocity, int axis)
    {
        DoubleVector3 minWindow = new DoubleVector3();
        minWindow[axis] = IsTestInput ? 7 : 200000000000000;
        DoubleVector3 maxWindow = new DoubleVector3();
        maxWindow[axis] = IsTestInput ? 27 : 400000000000000;
        
        int maxTime = 1000;

        List<IntVector2> validXPV = new List<IntVector2>();

        foreach (Hailstone h in hailstones)
        {
            Hailstone rock = new Hailstone();
            rock.Id = "Rock";

            Hailstone hCopy = new Hailstone();
            hCopy.Position[axis] = h.Position[axis];
            hCopy.Velocity[axis] = h.Velocity[axis];
            
            DoubleVector3 diff = maxVelocity - minVelocity;
            
            for (int x = 0; x < diff[axis]; x++)
            {
                rock.Velocity[axis] = minVelocity[axis] + x;
                rock.Position = (hCopy.Position+hCopy.Velocity)-rock.Velocity;
                
                //DebugOutput($"Rock [{rock.Position}]  [{rock.Velocity}]");
                
                int numIntersects = 0;

                HashSet<Hailstone> hitHailstones = new HashSet<Hailstone>();

                foreach (Hailstone h2 in hailstones)
                {
                    Hailstone h2Copy = new Hailstone();
                    h2Copy.Position[axis] = h2.Position[axis];
                    h2Copy.Velocity[axis] = h2.Velocity[axis];
                    
                    IntersectionHelper.Ray result = IntersectionHelper.FindShortestDistance(
                        new IntersectionHelper.Ray(rock.Position, rock.Direction),
                        new IntersectionHelper.Ray(h2Copy.Position, h2Copy.Direction));

                    if (result.Direction.FuzzyEquals(DoubleVector3.Zero) && result.Position[axis] >= minWindow[axis] &&
                        result.Position[axis] <= maxWindow[axis])
                    {
                        hitHailstones.Add(h2);
                        if (hitHailstones.Count == hailstones.Count)
                        {
                            goto testvalid;
                        }
                    }
                }

                testvalid:
                if (hitHailstones.Count == hailstones.Count)
                {
                    validXPV.Add(new IntVector2((int)rock.Position[axis], (int)rock.Velocity[axis]));
                }

            }
        }

        return validXPV;
    }
 
    

    
    
    
    
    
    

    public DoubleVector3 DV3FromString(string[] data)
    {
        return new DoubleVector3(Double.Parse(data[0]), Double.Parse(data[1]), Double.Parse((data[2])));
    }
    
    
    public DoubleVector3 IV3FromString(string[] data)
    {
        return new DoubleVector3(int.Parse(data[0]), int.Parse(data[1]), int.Parse((data[2])));
    }

    public class Hailstone
    {
        public string Id;
        public DoubleVector3 Position;
        public DoubleVector3 Velocity;
        public DoubleVector3 Direction => new DoubleVector3(Velocity.X,Velocity.Y,Velocity.Z).Normalized;
    }
}