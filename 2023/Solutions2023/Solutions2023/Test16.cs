using System.Diagnostics;
using System.Text;

public class Test16 : BaseTest
{
    public override void Initialise()
    {
        TestID = 16;
        IsTestInput = false;
        IsPart2 = false;
    }

    public override void Execute()
    {
        List<Beam> beams = new List<Beam>();
        List<Beam> newBeams = new List<Beam>();

        Beam startBeam = new Beam(new IntVector2(), IntVector2.Right);
        beams.Add(startBeam);

        Debug.Assert(Rotate(IntVector2.Right, '/') == IntVector2.Down);
        Debug.Assert(Rotate(IntVector2.Right, '\\') == IntVector2.Up);
        Debug.Assert(Rotate(IntVector2.Left, '\\') == IntVector2.Down);
        Debug.Assert(Rotate(IntVector2.Left, '/') == IntVector2.Up);
        Debug.Assert(Rotate(IntVector2.Up, '/') == IntVector2.Left);
        Debug.Assert(Rotate(IntVector2.Up, '\\') == IntVector2.Right);
        Debug.Assert(Rotate(IntVector2.Down, '/') == IntVector2.Right);
        Debug.Assert(Rotate(IntVector2.Down, '\\') == IntVector2.Left);


        int escape = 100;
        int count = 0;
        int sameSetCount = 0;

        int width = m_dataFileContents[0].Length;
        int height = m_dataFileContents.Count;
        
        //HashSet<IntVector2> boardEnergisedSet = new HashSet<IntVector2>();
        bool[] energised = new bool[width*height];
    
        int lastCount = energised.Count(x=>x==true);
       
        while (true)
        {
            foreach (Beam b in beams)
            {
                if (!b.StillActive)
                {
                    continue;
                }

                energised[(b.CurrentPosition.Y * width) + b.CurrentPosition.X] = true;
                
                char currentChar = GetContents(b.CurrentPosition);
                if (currentChar == '/' || currentChar == '\\')
                {
                    b.CurrentDirection = Rotate(b.CurrentDirection, currentChar);
                }
                else if (currentChar == '|')
                {
                    if (b.CurrentDirection == IntVector2.Left || b.CurrentDirection == IntVector2.Right)
                    {
                        b.CurrentDirection = IntVector2.Up;
                        // need to split.
                        
                        // add beams below but only if it makes sense
                        if (InBounds(b.CurrentPosition + IntVector2.Down))
                        {
                            newBeams.Add(new Beam(b.CurrentPosition, IntVector2.Down));
                        }
                    }
                }
                else if (currentChar == '-')
                {
                    if (b.CurrentDirection == IntVector2.Up || b.CurrentDirection == IntVector2.Down)
                    {
                        b.CurrentDirection = IntVector2.Right;
                        // need to split.
                        // add beams left but only if it makes sense
                        if (InBounds(b.CurrentPosition + IntVector2.Left))
                        {
                            newBeams.Add(new Beam(b.CurrentPosition, IntVector2.Left));
                        }



                    }
                }

                IntVector2 nextPos = b.CurrentPosition + b.CurrentDirection;
                if (InBounds(nextPos))
                {
                    b.Move();
                }
                else
                {
                    b.Stop();
                }

                energised[(b.CurrentPosition.Y * width) + b.CurrentPosition.X] = true;

            }

            beams.AddRange(newBeams);
            newBeams.Clear();


            int curentCount = energised.Count(x=>x==true);
            
            if(curentCount == lastCount)
            {
                sameSetCount++;
                if (sameSetCount > 3)
                {
                    break;
                }
            }
            else
            {
                lastCount = curentCount;
                sameSetCount = 0;
            }
        }

    

        DebugOutput("Number of cells energized is : " + lastCount);
        DebugOutput(DebugGrid(energised,width,height));
    }

    // public HashSet<IntVector2> GetEnergisedSet(List<Beam> beams)
    // {
    //     HashSet<IntVector2> finalResults = new HashSet<IntVector2>();
    //     foreach (Beam b in beams)
    //     {
    //         foreach (IntVector2 pos in b.PreviousPositions)
    //         {
    //             finalResults.Add(pos);
    //         }
    //     }
    //
    //     return finalResults;
    // }

    public string DebugGrid(HashSet<IntVector2> positions)
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < m_dataFileContents.Count; ++y)
        {
            for (int x = 0; x < m_dataFileContents[0].Length; ++x)
            {
                sb.Append(positions.Contains(new IntVector2(x, y)) ? '#' : '.');
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public string DebugGrid(bool[] grid,int width,int height)
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                sb.Append(grid[(y*width)+x] ? '#' : '.');
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    
    public IntVector2 Rotate(IntVector2 direction, char symbol)
    {
        if (direction == IntVector2.Right && symbol == '/') return IntVector2.Down;
        if (direction == IntVector2.Right && symbol == '\\') return IntVector2.Up;
        if (direction == IntVector2.Left && symbol == '\\') return IntVector2.Down;
        if (direction == IntVector2.Left && symbol == '/') return IntVector2.Up;
        if (direction == IntVector2.Up && symbol == '/') return IntVector2.Left;
        if (direction == IntVector2.Up && symbol == '\\') return IntVector2.Right;
        if (direction == IntVector2.Down && symbol == '/') return IntVector2.Right;
        if (direction == IntVector2.Down && symbol == '\\') return IntVector2.Left;

        Debug.Assert(false);
        return new IntVector2();

        // if (symbol == '/')
        // {
        //     return new IntVector2(direction.Y, -direction.X);
        // }
        // else
        // {
        //     return new IntVector2(-direction.Y, direction.X);
        // }
    }

    public bool InBounds(IntVector2 v)
    {
        return v.X >= 0 && v.Y >= 0 && v.X < m_dataFileContents[0].Length && v.Y < m_dataFileContents.Count;
    }

    public char GetContents(IntVector2 v)
    {
        return m_dataFileContents[v.Y][v.X];
    }


    public class Beam
    {
        public static int count = 0;
        public IntVector2 CurrentDirection = new IntVector2();
        public IntVector2 CurrentPosition = new IntVector2();
 
        private bool m_stopped = false;

        private int id = count++;

        public Beam(IntVector2 initialPosition, IntVector2 initialDirection)
        {
            CurrentDirection = initialDirection;
            CurrentPosition = initialPosition;
        }

        public void Move()
        {
            CurrentPosition += CurrentDirection;
        }

        public void Stop()
        {
            m_stopped = true;
        }

        public bool StillActive
        {
            get { return !m_stopped; }
        }
    }
}