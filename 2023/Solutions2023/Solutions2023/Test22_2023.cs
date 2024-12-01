using System.Numerics;

public class Test22_2023 : BaseTest
{
    public override void Initialise()
    {
        Year = 2023;
        TestID = 22;
        IsTestInput = false;
        IsPart2 = true;
    }

    public override void Execute()
    {
        List<Brick> brickList = new List<Brick>();
        int id = 0;
        foreach (string line in m_dataFileContents)
        {
            string[] bricks = line.Split('~');
            string[] brick1 = bricks[0].Split(',');
            string[] brick2 = bricks[1].Split(',');

            IntVector3 v1 = new IntVector3(int.Parse(brick1[0]), int.Parse(brick1[1]), int.Parse(brick1[2]));
            IntVector3 v2 = new IntVector3(int.Parse(brick2[0]), int.Parse(brick2[1]), int.Parse(brick2[2]));

            brickList.Add(new Brick((id++), v1, v2));
        }


        SimulateForces(brickList, 0, "INIT");


        if (IsPart2)
        {
            int totalFall = 0;
            foreach (Brick stableBrick in brickList)
            {
                int fall = 0;
                List<Brick> listCopy = DeepCopyList(brickList);
                listCopy.RemoveAll(x => x.Id == stableBrick.Id);
                SimulateForces(listCopy, stableBrick.Position.Z);
                foreach (Brick b in listCopy)
                {
                    if (brickList.Find(x => x.Id == b.Id).Position != b.Position)
                    {
                        fall++;
                    }
                }

                totalFall += fall;
            }


            DebugOutput("Total number of bricks to fall is : " + totalFall);

        }
        else
        {


            int removeableBricks = 0;

            // reach rest state.

            Dictionary<string, Brick> brickMap = new Dictionary<string, Brick>();
            foreach (Brick b in brickList)
            {
                brickMap[b.Id] = b;
            }


            Dictionary<Brick, (List<Brick>, List<Brick>)> supportedByMap =
                new Dictionary<Brick, (List<Brick>, List<Brick>)>();
            foreach (Brick b in brickList)
            {
                List<Brick> supportedBy = new List<Brick>();
                SupportedBy(b, brickList, supportedBy);

                List<Brick> supports = new List<Brick>();
                Supports(b, brickList, supports);


                string supportsInfo = String.Join(", ", supports.Select(x => x.Id));
                string supportedByInfo = String.Join(", ", supportedBy.Select(x => x.Id));

                DebugOutput($"{b.Id} supports [{supportsInfo}]  supported by [{supportedByInfo}]");

                supportedByMap[b] = (supportedBy, supports);
            }

            foreach (Brick b in brickList)
            {
                bool removeable = true;
                foreach (Brick b2 in brickList)
                {
                    if (b != b2 && supportedByMap[b2].Item1.Count == 1 && supportedByMap[b2].Item1[0] == b)
                    {
                        removeable = false;
                        break;
                    }
                }

                if (removeable)
                {
                    removeableBricks++;
                }
            }



            DebugOutput($"Can remove {removeableBricks} bricks");
        }

    }


    public void Supports(Brick b, List<Brick> bricks, List<Brick> supports)
    {
        foreach (Brick checkBrick in bricks)
        {
            if (checkBrick != b)
            {
                if (Overlaps(b.Position + new IntVector3(0, 0, 1), b.EndPoint + new IntVector3(0, 0, 1), checkBrick.Position, checkBrick.EndPoint))
                {
                    supports.Add(checkBrick);
                }
            }
        }
    }

    public void SupportedBy(Brick b, List<Brick> bricks, List<Brick> supports)
    {
        foreach (Brick checkBrick in bricks)
        {
            if (checkBrick != b)
            {
                if (Overlaps(b.Position + new IntVector3(0, 0, -1), b.EndPoint + new IntVector3(0, 0, -1), checkBrick.Position, checkBrick.EndPoint))
                {
                    supports.Add(checkBrick);
                }
            }
        }
    }

    public void SimulateForces(List<Brick> brickList, int minHeight = 0, string debug = "")
    {
        IntVector3 gravity = new IntVector3(0, 0, -1);

        // foreach (Brick b in brickList)
        // {
        //     DebugOutput($"{debug}  Simulate Forces brick {b.Id} starts at  {b.Position} ends {b.EndPoint}");
        // }


        bool bricksStable = false;
        while (!bricksStable)
        {
            bool didMove = false;
            foreach (Brick b in brickList)
            {
                if (b.Position.Z < minHeight)
                {
                    continue;
                }

                if ((b.Position + gravity).Z > 0)
                {
                    // create a ray / box going from ground up.
                    IntVector3 rayStart = b.Position;
                    rayStart.Z += gravity.Z;
                    IntVector3 rayEnd = rayStart + b.Dimensions;
                    //rayEnd.Z = rayStart.Z; // only care about bottom slice

                    bool canMove = true;
                    foreach (Brick b2 in brickList)
                    {
                        if (b != b2)
                        {
                            if (Overlaps(rayStart, rayEnd, b2.Position, b2.EndPoint))
                            {
                                // can't move.
                                canMove = false;
                                break;
                            }
                        }
                    }

                    if (canMove)
                    {
                        b.Position += gravity;
                        didMove = true;
                    }
                }
            }

            if (!didMove)
            {
                bricksStable = true;
            }
        }
        // foreach (Brick b in brickList)
        // {
        //     DebugOutput($"{debug}  Simulate Forces brick {b.Id} ends at  {b.Position}");
        // }

    }

    public List<Brick> DeepCopyList(List<Brick> list)
    {
        List<Brick> results = new List<Brick>();

        foreach (Brick b in list)
        {
            results.Add(Brick.Copy(b));
        }

        return results;
    }


    // AABB overlap test.
    public static bool Overlaps(IntVector3 positionA, IntVector3 endPointA, IntVector3 positionB, IntVector3 endPointB)
    {
        // cheat and shrink boxes slightly to avoid touching edges and corners
        Vector3 posAv3 = new Vector3(positionA.X, positionA.Y, positionA.Z);
        Vector3 epAv3 = new Vector3(endPointA.X, endPointA.Y, endPointA.Z);

        Vector3 posBv3 = new Vector3(positionB.X, positionB.Y, positionB.Z);
        Vector3 epBv3 = new Vector3(endPointB.X, endPointB.Y, endPointB.Z);

        // posAv3 *= 0.95f;
        // epAv3 *= 0.95f;
        // posBv3 *= 0.95f;
        // epBv3 *= 0.95f;


        // return (posAv3.X <= epBv3.X && epAv3.X >= posBv3.X) &&
        //        (posAv3.Y <= epBv3.Y && epAv3.Y >= posBv3.Y) &&
        //        (posAv3.Z <= epBv3.Z && epAv3.Z >= posBv3.Z);

        return (posAv3.X < epBv3.X && epAv3.X > posBv3.X) &&
               (posAv3.Y < epBv3.Y && epAv3.Y > posBv3.Y) &&
               (posAv3.Z < epBv3.Z && epAv3.Z > posBv3.Z);

    }


    public class Brick
    {
        public string Id;

        private IntVector3 m_position;
        public IntVector3 Position
        {
            get { return m_position; }
            set
            {
                m_position = value;
            }
        }


        public IntVector3 EndPoint
        {
            get { return m_position + Dimensions; }
        }


        public IntVector3 Dimensions;

        private int m_numBricks = 0;
        public Brick(int id, IntVector3 pos1, IntVector3 pos2)
        {
            char c = (char)((int)'A' + id);
            Id = "" + c;
            Dimensions = pos2 - pos1;
            Dimensions += new IntVector3(1, 1, 1);

            Position = pos1;
            m_numBricks = (int)Dimensions.Magnitude;
            m_numBricks += 1;
        }

        private Brick()
        { }

        public static Brick Copy(Brick src)
        {
            Brick b = new Brick();
            b.Id = src.Id;
            b.m_position = src.m_position;
            b.m_numBricks = src.m_numBricks;
            b.Dimensions = src.Dimensions;
            return b;
        }



    }

}