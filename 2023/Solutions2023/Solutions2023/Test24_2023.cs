using CyrusBeckLineClipping;
using System.Drawing;
using System.Numerics;

public class Test24_2023 : BaseTest
{
    public override void Initialise()
    {
        Year = 2023;
        TestID = 24;
        IsTestInput = false;
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
            hailstone.Id = "" + count++;
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

        DoubleVector3 min = IsTestInput ? new DoubleVector3(7, 7, 0) : new DoubleVector3(200000000000000, 200000000000000, 0);
        DoubleVector3 max = IsTestInput ? new DoubleVector3(27, 27, 0) : new DoubleVector3(400000000000000, 400000000000000, 0);


        if (IsPart2)
        {
            Part2(hailstones, min, max);
        }
        else
        {
            Part1(hailstones, min, max);
        }




    }

    public void Part1(List<Hailstone> hailstones, DoubleVector3 min, DoubleVector3 max)
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

            if (result.Direction == DoubleVector3.Zero && result.Position >= min && result.Position <= max)
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

    public void Part2(List<Hailstone> hailstonesOriginal, DoubleVector3 min, DoubleVector3 max)
    {
        List<Hailstone> hailstones = new List<Hailstone>();
        // if it workss for the first 30, will it work for all? 
        // for (int i = 0; i < 5; ++i)
        // {
        //     hailstones.Add(hailstonesOriginal[i]);
        // }
        hailstones.AddRange(hailstonesOriginal);




        DoubleVector3 minVelocity = new DoubleVector3(int.MaxValue, int.MaxValue, int.MaxValue);
        DoubleVector3 maxVelocity = new DoubleVector3(int.MinValue, int.MinValue, int.MinValue);

        foreach (Hailstone h in hailstones)
        {
            minVelocity.Min(h.Velocity);
            maxVelocity.Max(h.Velocity);
        }

        int adjust = 0;
        minVelocity -= new DoubleVector3(adjust, adjust, adjust);
        maxVelocity += new DoubleVector3(adjust, adjust, adjust);


        long result = PossiblePositionsXY(hailstones, minVelocity, maxVelocity, 0);
        DebugOutput("final result is : " + result);


        // Task<HashSet<IntVector2>>[] taskArray =
        // {
        //     Task<HashSet<IntVector2>>.Factory.StartNew(() =>
        //         PossiblePositions(hailstones, minVelocity, maxVelocity, 0)),
        //     Task<HashSet<IntVector2>>.Factory.StartNew(() =>
        //         PossiblePositions(hailstones, minVelocity, maxVelocity, 1)),
        //     Task<HashSet<IntVector2>>.Factory.StartNew(() => PossiblePositions(hailstones, minVelocity, maxVelocity, 2))
        // };

        // Task<HashSet<IntVector2>>[] taskArray =
        // {
        //     Task<HashSet<IntVector2>>.Factory.StartNew(() =>
        //         PossiblePositionsRay(hailstones, minVelocity, maxVelocity, 0)),
        //     Task<HashSet<IntVector2>>.Factory.StartNew(() =>
        //         PossiblePositionsRay(hailstones, minVelocity, maxVelocity, 1)),
        //     Task<HashSet<IntVector2>>.Factory.StartNew(() => PossiblePositionsRay(hailstones, minVelocity, maxVelocity, 2))
        // };


        // HashSet<IntVector2> validXPV = taskArray[0].Result;
        // DebugOutput("XOptions : "+string.Join("  ",validXPV));
        // HashSet<IntVector2> validYPV = taskArray[1].Result;
        // DebugOutput("YOptions : "+string.Join("  ",validYPV));
        // HashSet<IntVector2> validZPV = taskArray[2].Result;
        // DebugOutput("ZOptions : "+string.Join("  ",validZPV));
        //
        //
        //
        // int maxTime = 1000;
        //
        // Hailstone rock = new Hailstone();
        // rock.Id = "Rock";
        //
        // DoubleVector3 foundPosition = new DoubleVector3();
        // DoubleVector3 foundVelocity = new DoubleVector3();
        //
        // foreach(IntVector2 x in validXPV)
        // {
        //     foreach(IntVector2 y in validYPV)
        //     {
        //         foreach (IntVector2 z in validZPV)
        //         {
        //             Task<bool>.Factory.StartNew(() =>
        //             {
        //                 rock.Position = new DoubleVector3(x.X, y.X, z.X);
        //                 rock.Velocity = new DoubleVector3(x.Y, y.Y, z.Y);
        //
        //                 HashSet<Hailstone> hitHailstones = new HashSet<Hailstone>();
        //
        //                 for (int time = 1; time <= maxTime; ++time)
        //                 {
        //                     foreach (Hailstone h2 in hailstones)
        //                     {
        //                         // DebugOutput(
        //                         //     $"Time {time}  Testing {rock.Id} {rock.Position + (rock.Velocity * time)}  against   {h2.Id}  {h2.Position + (h2.Velocity * time)}   {rock.Position + (rock.Velocity * time) == h2.Position + (h2.Velocity * time)}");
        //
        //                         if (rock.Position + (rock.Velocity * time) == h2.Position + (h2.Velocity * time))
        //                         {
        //                             hitHailstones.Add(h2);
        //                             if (hitHailstones.Count == hailstones.Count)
        //                             {
        //                                 foundPosition = rock.Position;
        //                                 foundVelocity = rock.Velocity;
        //                                 DebugOutput($"Found solution with {foundPosition} {foundVelocity}");
        //                                 return true;
        //                             }
        //                         }
        //
        //                     }
        //                 }
        //
        //                 return false;
        //             });
        //         }
        //     }
        // }

        //haveSolution:
        //DebugOutput($"Found solution with {foundPosition} {foundVelocity}");
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
                rock.Position = (h.Position + h.Velocity) - rock.Velocity;

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


    public HashSet<IntVector2> PossiblePositionsRay(List<Hailstone> hailstones, DoubleVector3 minVelocity, DoubleVector3 maxVelocity, int axis)
    {
        // DoubleVector3 minWindow = new DoubleVector3();
        // minWindow[axis] = IsTestInput ? 7 : 200000000000000;
        // DoubleVector3 maxWindow = new DoubleVector3();
        // maxWindow[axis] = IsTestInput ? 27 : 400000000000000;

        DoubleVector3 minWindow = new DoubleVector3();
        minWindow[axis] = IsTestInput ? 7 : 200000000000000;
        DoubleVector3 maxWindow = new DoubleVector3();
        maxWindow[axis] = IsTestInput ? 30 : 400000000000000;


        int maxTime = 1000;

        HashSet<IntVector2> validXPV = new HashSet<IntVector2>();

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
                rock.Position = (hCopy.Position + hCopy.Velocity) - rock.Velocity;

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

                    // if (result.Direction.FuzzyEquals(DoubleVector3.Zero) && result.Position[axis] >= minWindow[axis] &&
                    //     result.Position[axis] <= maxWindow[axis])
                    if (result.Direction.FuzzyEquals(DoubleVector3.Zero))
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



    // finally with complete help from : https://pastebin.com/fkpZWn8X - thankyou.
    // need to look and see  why my original plan of picking a random set from the list and finding a point on that didn't work.

    public long PossiblePositionsXY(List<Hailstone> hailstones, DoubleVector3 minVelocity,
        DoubleVector3 maxVelocity, int axis)
    {

        int range = 300;
        for (int x = -range; x < range; x++)
        {
            for (int y = -range; y < range; y++)
            {
                var result1 = TryIntersectPos2(hailstones[1], hailstones[0], (x, y));
                var result2 = TryIntersectPos2(hailstones[2], hailstones[0], (x, y));
                var result3 = TryIntersectPos2(hailstones[3], hailstones[0], (x, y));

                if (!result1.intersects || result1.pos != result2.pos || result1.pos != result3.pos) continue;

                for (int z = -range; z < range; z++)
                {
                    // We know at what timestamp we would intersect the rock its initial position, so we can just check where the Z would end up at
                    // Check them for the first four hailstones as well
                    var intersectZ = hailstones[1].BIPosition.Z + result1.time * (hailstones[1].IntVel.Z + z);
                    var intersectZ2 = hailstones[2].BIPosition.Z + result2.time * (hailstones[2].IntVel.Z + z);
                    var intersectZ3 = hailstones[3].BIPosition.Z + result3.time * (hailstones[3].IntVel.Z + z);

                    // If they don't align, keep searching
                    if (intersectZ != intersectZ2 || intersectZ != intersectZ3) continue;

                    //  final = new DoubleVector3(result1.pos.X, result2.pos.Y, intersectZ);
                    // DebugOutput($"Found result {final}");

                    // If four hailstones happen to align, just assume we found the answer and exit.
                    return (long)(result1.pos.X + result2.pos.Y + intersectZ);
                }


            }
        }

        return 0;
    }

    public (bool intersects, (long X, long Y) pos, long time) TryIntersectPos(Hailstone one, Hailstone two, DoubleVector3 offset)
    {

        Hailstone adjustedOne = new Hailstone() { Position = one.Position, Velocity = one.Velocity + offset };
        Hailstone adjustedTwo = new Hailstone() { Position = two.Position, Velocity = two.Velocity + offset };

        // var a = (Pos: (one.Position.X, one.Position.Y), Vel: (X: one.Velocity.X + offset.x, Y: one.Vel.Y + offset.y));
        // var c = (Pos: (two.Position.X, two.Position.Y), Vel: (X: two.Velocity.X + offset.x, Y: two.Vel.Y + offset.y));

        //Determinant
        long D = (long)((adjustedOne.Velocity.X * -1 * adjustedTwo.Velocity.Y) - (adjustedOne.Velocity.Y * -1 * adjustedTwo.Velocity.X));

        if (D == 0) return (false, (-1, -1), -1);

        var Qx = (-1 * adjustedTwo.Velocity.Y * (adjustedTwo.Position.X - adjustedOne.Position.X)) - (-1 * adjustedTwo.Velocity.X * (adjustedTwo.Position.Y - adjustedOne.Position.Y));
        var Qy = (adjustedOne.Velocity.X * (adjustedTwo.Position.Y - adjustedOne.Position.Y)) - (adjustedOne.Velocity.Y * (adjustedTwo.Position.X - adjustedOne.Position.X));

        var t = (long)(Qx / D);
        var s = (long)(Qy / D);

        var Px = (long)(adjustedOne.Position.X + t * adjustedOne.Velocity.X);
        var Py = (long)(adjustedOne.Position.Y + t * adjustedOne.Velocity.Y);

        // Returns the intersection point, as well as the timestamp at which "one" will reach it with the given velocity.
        return (true, (Px, Py), t);
    }

    public (bool intersects, (BigInteger X, BigInteger Y) pos, BigInteger time) TryIntersectPos2(Hailstone one, Hailstone two, (int x, int y) offset)
    {
        var a = (Pos: (one.BIPosition.X, one.BIPosition.Y), Vel: (X: one.IntVel.X + offset.x, Y: one.IntVel.Y + offset.y));
        var c = (Pos: (two.BIPosition.X, two.BIPosition.Y), Vel: (X: two.IntVel.X + offset.x, Y: two.IntVel.Y + offset.y));

        //Determinant
        BigInteger D = (a.Vel.X * -1 * c.Vel.Y) - (a.Vel.Y * -1 * c.Vel.X);

        if (D == 0) return (false, (-1, -1), -1);

        var Qx = (-1 * c.Vel.Y * (c.Pos.X - a.Pos.X)) - (-1 * c.Vel.X * (c.Pos.Y - a.Pos.Y));
        var Qy = (a.Vel.X * (c.Pos.Y - a.Pos.Y)) - (a.Vel.Y * (c.Pos.X - a.Pos.X));

        var t = Qx / D;
        var s = Qy / D;

        var Px = (a.Pos.X + t * a.Vel.X);
        var Py = (a.Pos.Y + t * a.Vel.Y);

        // Returns the intersection point, as well as the timestamp at which "one" will reach it with the given velocity.
        return (true, (Px, Py), t);
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

        public (BigInteger X, BigInteger Y, BigInteger Z) BIPosition
        {
            get { return ((BigInteger)Position.X, (BigInteger)Position.Y, (BigInteger)Position.Z); }
        }

        public (int X, int Y, int Z) IntVel
        {
            get { return ((int)Velocity.X, (int)Velocity.Y, (int)Velocity.Z); }
        }

        public DoubleVector3 Velocity;
        public DoubleVector3 Direction => new DoubleVector3(Velocity.X, Velocity.Y, Velocity.Z).Normalized;
    }
}