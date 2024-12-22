using Spectre.Console;
using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics;
using System.Xml.Linq;


public class Test20_2024 : BaseTest, IMapDataInt
{
    int m_width;
    int m_height;
    char[] m_dataGrid;

    IntVector2 m_startPosition;
    IntVector2 m_endPosition;

    public const char WALL = '#';

    public override void Initialise()
    {
        Year = 2024;
        TestID = 20;
    }



    public override void Execute()
    {


        m_dataGrid = Helper.GetCharGrid(m_dataFileContents, ref m_width, ref m_height);

        IntVector2 startPos;
        IntVector2 endPos;

        for (int i = 0; i < m_dataGrid.Length; i++)
        {
            if (m_dataGrid[i] == 'S')
            {
                m_startPosition = Helper.GetPosition(i, m_width);
            }
            else if (m_dataGrid[i] == 'E')
            {
                m_endPosition = Helper.GetPosition(i, m_width);
            }
        }

        List<IntVector2> standardRoute = new List<IntVector2>();
        AStarInt astar = new AStarInt();

        astar.Initialize(this);

        astar.FindPath(m_startPosition, m_endPosition, standardRoute);
        //standardRoute.Remove(m_startPosition);

        DebugOutput($"Standard Path [{standardRoute.Count}] : " + string.Join(", ", standardRoute));


        //DebugOutput(Helper.DrawGrid(m_dataGrid, m_width, m_height));

        //foreach (IntVector2 iv2 in standardRoute)
        //{
        //    m_dataGrid[Helper.GetIndex(iv2, m_width)] = 'o';
        //}

        //DebugOutput(Helper.DrawGrid(m_dataGrid, m_width, m_height));

        List<List<IntVector2>> cheatPaths = new List<List<IntVector2>>();

        HashSet<IntVector4> cheatPositions = new HashSet<IntVector4>();


        int minimumSave = IsTestInput ? IsPart2?50 : 1 : 100;
        int cheatDistance = IsPart2 ? 20 : 2;


        for (int i = 0; i < standardRoute.Count; i++)
        {

            //if (standardRoute[i] != new IntVector2(9, 7))
            //{
            //    continue;
            //}

            for (int j = standardRoute.Count - 1; j > i; j--)
            {
                // furtherst point within cheatdistance.
                int mhd = standardRoute[i].ManhattanDistance(standardRoute[j]);
                if (mhd >= 1 && mhd <= cheatDistance)
                {
                    //IntVector2 diffv = standardRoute[j] - standardRoute[i];
                    //diffv = diffv / mhd;

                    //List<IntVector2> cheatPath = new List<IntVector2>();
                    //for (int a = 0; a <= i; a++)
                    //{
                    //    cheatPath.Add(standardRoute[a]);
                    //}

                    //for (int a = 1; a < mhd; a++)
                    //{
                    //    cheatPath.Add(standardRoute[i] + (diffv * a));
                    //}

                    //for (int a = j; a < standardRoute.Count; a++)
                    //{
                    //    cheatPath.Add(standardRoute[a]);
                    //}

                    int diff2 = standardRoute.Count - (j-i) + mhd;

                    int diff4 = standardRoute.Count - (standardRoute.Count - (j-i) + mhd);

                    if ( diff4 >= minimumSave)
                    {

                        IntVector4 pos = new IntVector4(standardRoute[i].X, standardRoute[i].Y, standardRoute[j].X, standardRoute[j].Y);
                        cheatPositions.Add(pos);


                        //cheatPaths.Add(cheatPath);
                        //DebugOutput($"Cheat Path [{diff}]  [{standardRoute[i]}->{standardRoute[j]}] [{cheatPath.Count}] : " + string.Join(", ", cheatPath));
                        //break;
                    }
                }

            }

        }


        long total = 0;
        Dictionary<int, int> cheatGroups = new Dictionary<int, int>();

        foreach (IntVector4 iv4 in cheatPositions)
        {
            int distance = new IntVector2(iv4.X, iv4.Y).ManhattanDistance(new IntVector2(iv4.Z, iv4.W));
            if (!cheatGroups.ContainsKey(distance))
            {
                cheatGroups[distance] = 0;
            }
            cheatGroups[distance] += 1;

        }


        foreach (int i in cheatGroups.Keys.OrderBy(k => k))
        {
            DebugOutput($"There are {cheatGroups[i]} cheats that save {i} picoseconds.");
            total += cheatGroups[i];
        }

        DebugOutput($"In total there are {total} cheats that save {minimumSave} or more");

        int ibreak = 0;

    }


    public bool CanMove(IntVector2 from, IntVector2 to)
    {
        return Helper.InBounds(to, m_width, m_height) && m_dataGrid[Helper.GetIndex(to, m_width)] != WALL;
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

