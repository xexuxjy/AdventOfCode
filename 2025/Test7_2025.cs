using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Test7_2025 : BaseTest
{
    public char START = 'S';
    public char SPLITTER = '^';
    public char BEAM = '|';

    int Width = 0;
    int Height = 0;

    private int SplitCount = 0;
    
    public override void Initialise()
    {
        Year = 2025;
        TestID = 7;
    }

    public List<Beam> Beams = new List<Beam>();
    
    public override void Execute()
    {
        char[] dataGrid = Helper.GetCharGrid(m_dataFileContents, ref Width, ref Height);
        IntVector2 startPoint = new IntVector2();
        
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (dataGrid[(y * Width) + x] == START)
                {
                    startPoint = new IntVector2(x, y);
                    break;
                }
            }
        }

        if (IsPart1)
        {
            ExecutePart1(dataGrid, startPoint);
        }
        else
        {
            ExecutePart2(dataGrid, startPoint);
        }
    }

    public void ExecutePart1(char[] dataGrid,IntVector2 startPoint)
    {
        Beam startBeam = new Beam(startPoint,null);
        Beams.Add(startBeam);
        bool keepGoing = true;
        List<Beam> newBeams = new List<Beam>();
        while (keepGoing)
        {
            foreach (Beam beam in Beams)
            {
                if (beam.Active)
                {
                    if (beam.Move(Width,Height))
                    {
                        int index = (beam.CurrentPoint.Y * Width) + beam.CurrentPoint.X;
                        if (dataGrid[index] == SPLITTER)
                        {
                            beam.Active = false;
                            SplitCount++;

                            IntVector2 left = beam.CurrentPoint+IntVector2.Left;
                            if (NewBeamValid(left, Beams, newBeams))
                            {
                                newBeams.Add(new Beam(left,beam));
                            }
                            
                            IntVector2 right = beam.CurrentPoint+IntVector2.Right;
                            if (NewBeamValid(right, Beams, newBeams))
                            {
                                newBeams.Add(new Beam(right,beam));
                            }
                        }
                    }
                    else
                    {
                        beam.Active = false;
                    }
                }
            }

            List<Beam> parents = new List<Beam>();
            foreach (Beam newBeam in newBeams)
            {
                bool exists = false;
                foreach (Beam beam in Beams)
                {
                    if (beam.Moves.Contains(newBeam.StartPoint))
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    
                    Beams.Add(newBeam);
                    if (!parents.Exists(x => x == newBeam.Parent))
                    {
                        parents.Add(newBeam.Parent);
                    }
                }
            }
            
            
            parents.Clear();
            newBeams.Clear();

            bool allInactive = true;
            foreach (Beam beam in Beams)
            {
                if (beam.Active)
                {
                    allInactive = false;
                    break;
                }
            }
            keepGoing = !allInactive;
            //DrawBeams(Beams,dataGrid,Width,Height);
            //DebugOutput($"SplitCount is {SplitCount}");
        }

        
        //DrawBeams(Beams,dataGrid,Width,Height);

        foreach (Beam beam in Beams)
        {
            foreach (Beam beam2 in Beams)
            {
                if (beam != beam2)
                {
                    foreach (IntVector2 point in beam.Moves)
                    {
                        if (beam2.Moves.Contains(point))
                        {
                            int ibreak = 0;
                        }
                    }
                }
            }
        }
        
        
        DebugOutput($"Completed with {SplitCount} splits");
    }

    public void ExecutePart2(char[] dataGrid, IntVector2 startPoint)
    {
        long timeLines = CountTimelines(startPoint, dataGrid, 0,new Dictionary<IntVector2, long>());
        DebugOutput($"Completed with {timeLines} timelines");
    }
    
    public long CountTimelines(IntVector2 startPoint, char[] dataGrid,int depth,Dictionary<IntVector2,long> cache)
    {
        if (cache.ContainsKey(startPoint))
        {
            return cache[startPoint];
        }

        if (!Helper.InBounds(startPoint, Width, Height))
        {
            cache[startPoint] = 1;
            return 1;
        }

        
        if (depth > 1000)
        {
            return 0;
        }

        long score = 0;

        int index = (startPoint.Y * Width) + startPoint.X;
        if (dataGrid[index] == SPLITTER)
        {
            IntVector2 left = startPoint + IntVector2.Left+IntVector2.Up; 
            score+= CountTimelines(left, dataGrid, depth + 1,cache);

            IntVector2 right = startPoint + IntVector2.Right+IntVector2.Up;
            score+= CountTimelines(right, dataGrid, depth + 1,cache);
        }
        else
        {
            score += CountTimelines(startPoint+IntVector2.Up, dataGrid, depth + 1,cache);
        }
         
        cache[startPoint] = score;
        return score;
    }
    
    public void DrawBeams(List<Beam> beams, char[] grid, int width, int height)
    {
        foreach (Beam beam in beams)
        {
            foreach (IntVector2 move in beam.Moves)
            {
                int index = (move.Y * width) + move.X;
                if (grid[index] != SPLITTER)
                {
                    grid[index] = BEAM;
                }
            }
        }
        DebugOutput(Helper.DrawGrid(grid,width,height));
    }
    
    public bool NewBeamValid(IntVector2 newStartPoint, List<Beam> activeBeams, List<Beam> newBeams)
    {
        if (Helper.InBounds(newStartPoint, Width, Height))
        {
            if (activeBeams.Exists(x => x.Moves.Contains(newStartPoint)))
            {
                return false;
            }
            if (newBeams.Exists(x => x.Moves.Contains(newStartPoint)))
            {
                return false;
            }

            return true;
        }
        return false;
    }
    
    
    public class Beam
    {
        public Beam(IntVector2 startPoint,Beam parent)
        {
            StartPoint = startPoint;
            CurrentPoint = startPoint;
            Moves.Add(startPoint);
            Parent = parent;
        }
        
        public IntVector2 StartPoint;
        public IntVector2 CurrentPoint;
        public Beam Parent;
        
        
        public List<IntVector2> Moves = new List<IntVector2>();
        public bool Active = true;
        public bool Move(int width, int height)
        {
            if (Active)
            {
                IntVector2 newPoint = CurrentPoint + IntVector2.Up;
                if (Helper.InBounds(newPoint, width, height))
                {
                    CurrentPoint = newPoint;
                    Moves.Add(CurrentPoint);
                    return true;
                }
            }

            return false;
        }

        
    }
    
}