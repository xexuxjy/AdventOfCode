using Spectre.Console;
using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
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


        DebugOutput(Helper.DrawGrid(m_dataGrid,m_width,m_height));

        foreach(IntVector2 iv2 in standardRoute)
        {
            m_dataGrid[Helper.GetIndex(iv2,m_width)] = 'o';
        }

        DebugOutput(Helper.DrawGrid(m_dataGrid,m_width,m_height));

        List<List<IntVector2>> cheatPaths = new List<List<IntVector2>>();

        List<(IntVector4, int)> cheatPositions = new List<(IntVector4, int)>();


        int minimumSave = IsTestInput?1: 100;

        for (int i = 0; i < standardRoute.Count; i++)
        {

            //if (standardRoute[i] != new IntVector2(9, 7))
            //{
            //    continue;
            //}

            for (int j = standardRoute.Count - 1; j > i; j--)
            {
                // furtherst point within 2.
                int mhd = standardRoute[i].ManhattanDistance(standardRoute[j]);
                if (mhd >= 1 && mhd <= 2)
                {
                    IntVector2 diffv = standardRoute[j] - standardRoute[i];
                    diffv = diffv / mhd;

                    if(diffv.X != 0 && diffv.Y !=0)
                    {
                        int ibreak3 = 0;
                    }


                    List<IntVector2> cheatPath = new List<IntVector2>();
                    for (int a = 0; a <= i; a++)
                    {
                        cheatPath.Add(standardRoute[a]);
                    }

                    for(int a=1;a<mhd; a++)
                    {
                        cheatPath.Add(standardRoute[i]+(diffv*a));
                    }

                    for (int a = j; a < standardRoute.Count; a++)
                    {
                        cheatPath.Add(standardRoute[a]);
                    }


                    int diff = standardRoute.Count - cheatPath.Count;

                    //int ad = j-i;
                    //if(ad == 6)
                    //{
                    //    DebugOutput($"J-I = {ad}  diff {diff}   [{standardRoute[i]}->{standardRoute[j]}]");
                    //}



                    if (diff >= minimumSave)
                    {

                        IntVector4 pos = new IntVector4(standardRoute[i].X, standardRoute[i].Y, standardRoute[j].X, standardRoute[j].Y);
                        cheatPositions.Add((pos, diff));


                        cheatPaths.Add(cheatPath);
                        //DebugOutput($"Cheat Path [{diff}]  [{standardRoute[i]}->{standardRoute[j]}] [{cheatPath.Count}] : " + string.Join(", ", cheatPath));
                        //break;
                    }
                }

            }

        }
        long total = 0;
        Dictionary<int, int> cheatGroups = new Dictionary<int, int>();
        for (int i = 0; i < cheatPaths.Count; i++)
        {
            int diff = standardRoute.Count - cheatPaths[i].Count;
            if (!cheatGroups.ContainsKey(diff))
            {
                cheatGroups[diff] = 0;
            }
            cheatGroups[diff] += 1;
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

    public float DistanceToTarget(IntVector2 v)
    {
        return v.ManhattanDistance(m_endPosition);
    }
}

