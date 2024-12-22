using System.Collections.Generic;
using System.Numerics;

public class Test18_2024 : BaseTest,IMapDataInt
{
    int m_width = 7;
    int m_height = 7;

    char[] m_dataGrid;

    char BYTE = '#';
    char EMPTY = '.';

    IntVector2 m_startPosition;
    IntVector2 m_endPosition;


    public override void Initialise()
    {
        Year = 2024;
        TestID = 18;
    }


    public override void Execute()
    {

        List<IntVector2> FallingBytes = new List<IntVector2>();

        IntVector2 max = new IntVector2();

        foreach (string line in m_dataFileContents)
        {

            string[] tokens = line.Split(',');
            IntVector2 iv2 = new IntVector2(int.Parse(tokens[0]), int.Parse(tokens[1]));
            FallingBytes.Add(iv2);
            max.Max(iv2);
        }

        m_width = max.X+1;
        m_height = max.Y+1;

        m_dataGrid = new char[m_width * m_height];
        Array.Fill(m_dataGrid,EMPTY);

    List<IntVector2> results = new List<IntVector2>();
        
        if(IsPart2)
        {
            m_startPosition = new IntVector2();
            m_endPosition = max;

            AStarInt astar = new AStarInt(SearchMethod.AStar);
            astar.Initialize(this);
            
            int count  = 0;
            foreach(IntVector2 iv in FallingBytes )
            {
                results.Clear();
                m_dataGrid[Helper.GetIndex(iv,m_width)] = BYTE;
                //DebugOutput("Testing at block "+count);
                if(!astar.FindPath(m_startPosition,m_endPosition,results))
                {
                    break;
                }
                count++;
            }

            DebugOutput($"Adding block at {FallingBytes[count]} stopped us");

        }
        else
        {
            int limit = 1024;
            int count  = 0;

            foreach(IntVector2 iv in FallingBytes )
            {
                m_dataGrid[Helper.GetIndex(iv,m_width)] = BYTE;
                count++;
                if( count >= limit )
                {
                    break;
                }
            }

            DebugOutput(Helper.DrawGrid(m_dataGrid,m_width,m_height));

            m_startPosition = new IntVector2();
            m_endPosition = max;

            AStarInt astar = new AStarInt(SearchMethod.AStar);
            astar.Initialize(this);

            if(astar.FindPath(m_startPosition,m_endPosition,results))
            {
                foreach(IntVector2 iv in results )
                {
                    m_dataGrid[Helper.GetIndex(iv,m_width)] = 'O';
                }
            }
        }
        




        DebugOutput(Helper.DrawGrid(m_dataGrid,m_width,m_height));

        DebugOutput("Minimum steps is : "+(results.Count-1));

    }

    public bool CanMove(IntVector2 from, IntVector2 to)
    {
        return Helper.InBounds(to,m_width,m_height) && m_dataGrid[Helper.GetIndex(to,m_width)] == EMPTY;
    }

    public IntVector2[] GetDirections()
    {
        return IntVector2.Directions;
    }

    public IntVector2 GetTargetPosition()
    {
        return m_endPosition;
    }

    public float DistanceToTarget(IntVector2 v,IntVector2 t)
    {
        return v.ManhattanDistance(t);
    }
}