using System.Diagnostics;
using System.Text;

public class Test16 : BaseTest
{
    public override void Initialise()
    {
        TestID = 16;
        IsTestInput = true;
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
        while (true)
        {
            HashSet<IntVector2> energisedSet = GetEnergisedSet(beams);

            foreach (Beam b in beams)
            {
                if (!b.StillActive)
                {
                    continue;
                }
                
                IntVector2 nextPos = b.CurrentPosition + b.CurrentDirection;
                if (InBounds(nextPos))
                {
                    b.Move();
                    char nextChar = GetContents(nextPos);
                    if (nextChar == '/' || nextChar == '\\')
                    {
                        b.CurrentDirection = Rotate(b.CurrentDirection, nextChar);
                    }
                    else if (nextChar == '|')
                    {
                        if (b.CurrentDirection == IntVector2.Left || b.CurrentDirection == IntVector2.Right)
                        {
                            b.CurrentDirection = IntVector2.Up;
                            // need to split.
                            newBeams.Add(new Beam(nextPos,IntVector2.Down));
                        }
                    }
                    else if (nextChar == '-')
                    {
                        if (b.CurrentDirection == IntVector2.Up || b.CurrentDirection == IntVector2.Down)
                        {
                            b.CurrentDirection = IntVector2.Right;
                            // need to split.
                            newBeams.Add(new Beam(nextPos,IntVector2.Left));
                        }
                    }
                }
                else
                {
                    b.Stop();
                }
            }

            beams.AddRange(newBeams);
            newBeams.Clear();


            HashSet<IntVector2> newEnergisedSet = GetEnergisedSet(beams);

            if (newEnergisedSet.SequenceEqual(energisedSet))
            {
                sameSetCount++;
                if (sameSetCount > 3)
                {
                    break;
                }
            }
            else
            {
                sameSetCount = 0;
            }


            // DebugOutput(DebugGrid(beams));
            //
            // bool beamsActive = false;            
            //
            // foreach (Beam b in beams)
            // {
            //     if (b.StillActive)
            //     {
            //         beamsActive = true;
            //         break;
            //     }
            // }
            //
            // if (!beamsActive)
            // {
            //     break;
            // }
            //
            // count++;
            // if (count > escape)
            // {
            //     break;
            // }

        }

        HashSet<IntVector2> finalResults = GetEnergisedSet(beams);
        
        
        
        
        DebugOutput("Number of cells energized is : " + finalResults.Count);
        DebugOutput(DebugGrid(finalResults));
    }

    public HashSet<IntVector2> GetEnergisedSet(List<Beam> beams)
    {
        HashSet<IntVector2> finalResults = new HashSet<IntVector2>();
        foreach (Beam b in beams)
        {
            foreach (IntVector2 pos in b.PreviousPositions)
            {
                finalResults.Add(pos);
            }
        }

        return finalResults;
    }

    public String DebugGrid(List<Beam> beams)
    {
        return DebugGrid(GetEnergisedSet(beams));
    }
    
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
        public List<IntVector2> PreviousPositions = new List<IntVector2>();

        private bool m_stopped = false;
        
        private int id = count++;
        public Beam(IntVector2 initialPosition, IntVector2 initialDirection)
        {
            CurrentDirection = initialDirection;
            CurrentPosition = initialPosition;
        }

        public void Move()
        {
            PreviousPositions.Add(CurrentPosition);
            CurrentPosition += CurrentDirection;
        }

        public void Stop()
        {
            PreviousPositions.Add(CurrentPosition);
            m_stopped = true;
        }
        
        public bool StillActive
        {
            get { return !m_stopped; }
        }
        
    }
    
}