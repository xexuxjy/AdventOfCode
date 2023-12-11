using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

public class Test11 : BaseTest
{
    public override void Initialise()
    {
        TestID = 11;
        IsTestInput = false;
        IsPart2 = false;
    }

    public override void Execute()
    {
        Galaxy galaxy = new Galaxy(m_dataFileContents);

        // for (int i = 0; i < galaxy.OriginalStarList.Count;++i)
        // {
        //     DebugOutput($"Star original {galaxy.OriginalStarList[i]}   inflated to {galaxy.InflatedStarList[i]}");
        // }

        // int start = 0;
        // int end = 7;
        //DebugOutput($"Distance between Star {start+1} {galaxy.InflatedStarList[start]} and Star {end+1} {galaxy.InflatedStarList[end]} is {cd.Count}
        // int distance = galaxy.InflatedStarList[start].ManhattanDistance(galaxy.InflatedStarList[end]);
        // DebugOutput($"Distance between Star {start+1} {galaxy.InflatedStarList[start]} and Star {end+1} {galaxy.InflatedStarList[end]} is {distance}");

        List<int> indices = new List<int>();
        for (int i = 0; i < galaxy.InflatedStarList.Count; ++i)
        {
            indices.Add(i);
        }

        int total = 0;
        var pairList = indices.SelectMany((fst, i) => indices.Skip(i + 1).Select(snd => (fst, snd)));
        foreach (var pair in pairList)
        {
            int d = galaxy.InflatedStarList[pair.fst].ManhattanDistance(galaxy.InflatedStarList[pair.snd]);
            total += d;
        }

        DebugOutput($"Total distance between all inflated stars is : "+total);
        
    }

    public class CounterDelegate
    {
        public int Count;

        public bool PlotPoint(int x, int y)
        {
            Count++;
            return true;
        }
    }

    public class Galaxy
    {
        public List<string> OriginalPositions = new List<string>();
        public List<string> InflatedPositions = new List<string>();
        public List<int> EmptyColumns = new List<int>();
        public List<int> EmptyRows = new List<int>();

        public List<IntVector2> OriginalStarList = new List<IntVector2>();
        public List<IntVector2> InflatedStarList = new List<IntVector2>();
        
        public Galaxy(List<string> data)
        {
            OriginalPositions.AddRange(data);

            for (int i = 0; i < OriginalPositions.Count; ++i)
            {
                if (!OriginalPositions[i].Contains('#'))
                {
                    EmptyRows.Add(i);
                }
            }

            for (int i = 0; i < OriginalPositions[0].Length; ++i)
            {
                bool empty = true;
                for (int j = 0; j < OriginalPositions.Count; ++j)
                {
                    if(OriginalPositions[j][i] == '#')
                    {
                        empty = false;
                        break;
                    }
                }

                if (empty)
                {
                    EmptyColumns.Add(i);
                }
            }

            
            for (int i = 0; i < OriginalPositions.Count; ++i)
            {
                for (int j = 0; j < OriginalPositions[i].Length; ++j)
                {
                    if(OriginalPositions[i][j] == '#')
                    {
                        OriginalStarList.Add(new IntVector2(j,i));
                    }
                }
            }

            
            foreach (IntVector2 star in OriginalStarList)
            {
                InflatedStarList.Add(GetInflatedPosition(star));
            }
            
            
            
        }


        public IntVector2 GetInflatedPosition(IntVector2 original)
        {
            int downShift = EmptyRows.FindAll(x => x < original.Y).Count;
            int rightShift = EmptyColumns.FindAll(x => x < original.X).Count;

            return original + new IntVector2(rightShift, downShift);

        }
        
        
        
        
        
    }
    
}