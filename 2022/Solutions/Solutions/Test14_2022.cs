﻿using System.Numerics;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Test14_2022 : BaseTest
{

    public char ROCK = '#';
    public char AIR = '.';
    public char SAND = 'o';
    public char SOURCE = '+';

    public Vector2 SandOrigin = new Vector2(500, 0);

    public int MaxDepth = 0;
    public int LastFall = 0;

    public bool SecondHalf = true;
    public bool ReachedGoal = false;

        public override void Initialise()
    {
        Year = 2022;
        TestID = 14;
        IsTestInput = true;
        IsPart2 = false;
    }


    public override void Execute()
    {
        TestID = 14;
        IsTestInput = false;

        ReadDataFile();
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(" -> ");
            List<Vector2> vectors = new List<Vector2>();
            foreach (string token in tokens)
            {
                string[] t = token.Split(",");
                Vector2 v = new Vector2(int.Parse(t[0]), int.Parse(t[1]));
                vectors.Add(v);
            }
            for (int i = 0; i < vectors.Count - 1; ++i)
            {
                AddRockPath(vectors[i], vectors[i + 1]);
            }

        }

        AddPoint(SandOrigin, SOURCE);

        int stepCount = 0;
        while (!ReachedGoal)
        {
            SimulateSand();
            stepCount++;
        }

        stepCount--;
        m_debugInfo.Add("Finished sim with " + stepCount + " steps.");

        DrawDebug();
        WriteDebugInfo();
    }




    public void SimulateSand()
    {
        // from origin point straight down.
        Vector2 currentPoint = SandOrigin;
        Vector2 drop = new Vector2(0, 1);
        bool falling = true;

        if (SecondHalf)
        {
            if (IsBlocked(currentPoint + new Vector2(0, 1)) && IsBlocked(currentPoint + new Vector2(-1, 1)) && IsBlocked(currentPoint + new Vector2(1, 1)))
            {
                ReachedGoal = true;
                return;
            }
        }

        while (falling)
        {
            Vector2 newPoint = currentPoint + drop;
            // re-enable straight down.
            drop = new Vector2(0, 1);

            if (!SecondHalf)
            {
                // escape.
                if (newPoint.Y > MaxDepth)
                {
                    falling = false;
                    ReachedGoal = true;
                }
            }

            if (IsClear(newPoint))
            {
                currentPoint = newPoint;
            }
            //else if(CharAtLocation(newPoint) == ROCK)
            //{
            //    falling = false;
            //}
            else if (IsBlocked(newPoint))
            {
                // try and see if left is empty.
                if (IsClear(currentPoint + new Vector2(-1, 1)))
                {
                    drop = new Vector2(-1, 1);
                }
                else if (IsClear(currentPoint + new Vector2(1, 1)))
                {
                    drop = new Vector2(1, 1);
                }
                else
                {
                    falling = false;
                }
            }
        }

        AddPoint(currentPoint, SAND);
        LastFall = (int)currentPoint.Y;

    }


    public char CharAtLocation(Vector2 v)
    {
        if (m_sparseMap.Keys.Contains(v))
        {
            return m_sparseMap[v];
        }
        if (SecondHalf)
        {
            if ((int)v.Y == MaxDepth + 2)
            {
                return ROCK;
            }
        }
        return AIR;
    }

    List<Tuple<Vector2, Vector2>> m_vectorPairList = new List<Tuple<Vector2, Vector2>>();

    public Dictionary<Vector2, char> m_sparseMap = new Dictionary<Vector2, char>();




    public void AddRockPath(Vector2 from, Vector2 to)
    {
        m_vectorPairList.Add(new Tuple<Vector2, Vector2>(from, to));

        Vector2 diff = to - from;
        Vector2 step;
        if (diff.X < 0)
        {
            step = new Vector2(-1, 0);
        }
        else if (diff.X > 0)
        {
            step = new Vector2(1, 0);
        }
        else if (diff.Y < 0)
        {
            step = new Vector2(0, -1);
        }
        else
        {
            step = new Vector2(0, 1);
        }




        Vector2 current = from;
        while (current != to)
        {
            AddPoint(current, ROCK);
            current += step;
        }
        AddPoint(to, ROCK);
    }

    public void AddPoint(Vector2 point, char c)
    {
        m_sparseMap[point] = c;

        if (c == ROCK)
        {
            if (point.Y > MaxDepth)
            {
                MaxDepth = (int)point.Y;
            }
        }
    }


    public bool IsBlocked(Vector2 pos)
    {
        return CharAtLocation(pos) == SAND || CharAtLocation(pos) == ROCK;
    }
    public bool IsClear(Vector2 pos)
    {
        return CharAtLocation(pos) == AIR;
    }

    public void DrawDebug()
    {
        int minx = int.MaxValue;
        int miny = int.MaxValue;
        int maxx = int.MinValue;
        int maxy = int.MinValue;

        foreach (var v in m_vectorPairList)
        {
            minx = (int)Math.Min(minx, v.Item1.X);
            minx = (int)Math.Min(minx, v.Item2.X);

            miny = (int)Math.Min(miny, v.Item1.Y);
            miny = (int)Math.Min(miny, v.Item1.Y);

            maxx = (int)Math.Max(maxx, v.Item1.X);
            maxx = (int)Math.Max(maxx, v.Item2.X);

            maxy = (int)Math.Max(maxy, v.Item1.Y);
            maxy = (int)Math.Max(maxy, v.Item2.Y);
        }

        miny = 0;


        int xrange = maxx - minx;
        int yrange = maxy - miny;

        xrange += 1;
        yrange += 1;

        string[] columns = new string[xrange];
        for (int i = 0; i < xrange; i++)
        {
            columns[i] = "" + (minx + i);
        }

        int xInset = 2;
        int digits = ("" + maxx).Length;
        for (int y = 0; y < digits; y++)
        {
            StringBuilder header = new StringBuilder();
            for (int i = 0; i < xInset; ++i)
            {
                header.Append(" ");
            }

            for (int x = 0; x < xrange; x++)
            {
                if (y < columns[x].Length)
                {
                    header.Append(columns[x][y]);
                }
                else
                {
                    header.Append(" ");
                }
            }
            m_debugInfo.Add(header.ToString());
        }

        for (int y = 0; y < yrange; y++)
        {
            StringBuilder data = new StringBuilder();
            for (int x = 0; x < xrange; x++)
            {
                if (x == 0)
                {
                    data.Append((miny + y) + " ");
                }
                Vector2 v = new Vector2(minx + x, miny + y);
                char c = CharAtLocation(v);
                data.Append(c);

            }
            m_debugInfo.Add(data.ToString());
        }


    }
}