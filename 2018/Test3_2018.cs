using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Test3_2018 : BaseTest
{
    public override void Initialise()
    {
        Year = 2018;
        TestID = 3;
    }

    public override void Execute()
    {
        Dictionary<IntVector2, HashSet<int>> claimedMap = new Dictionary<IntVector2, HashSet<int>>();
        foreach (string line in m_dataFileContents)
        {
            //#1 @ 1,3: 4x4
            string[] tokens = line.Split(' ');
            int id = int.Parse(tokens[0].Replace("#", ""));
            int x = int.Parse(tokens[2].Split(',')[0]);
            int y = int.Parse(tokens[2].Split(',')[1].Replace(":", ""));

            int width = int.Parse(tokens[3].Split('x')[0]);
            int height = int.Parse(tokens[3].Split('x')[1]);

            int ibreak = 0;

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    IntVector2 coord = new IntVector2(x+i, y+j);
                    if (!claimedMap.TryGetValue(coord, out HashSet<int> claimedSet))
                    {
                        claimedSet = new HashSet<int>();
                        claimedMap.Add(coord, claimedSet);
                    }

                    claimedSet.Add(id);
                }
            }

        }
        
        int total = 0;
        foreach (IntVector2 coord in claimedMap.Keys)
        {
            if (claimedMap[coord].Count >= 2)
            {
                total++;
            }
        }

        DebugOutput($"There are {total} pieces with more than 2 claims");

        if (IsPart2)
        {
            List<int> possibilities = new List<int>();
            for (int i = 0; i < m_dataFileContents.Count; i++)
            {
                possibilities.Add(i + 1);
            }
            
            foreach (var set in claimedMap.Values)
            {
                if (set.Count >= 2)
                {
                    foreach (int num in set)
                    {
                        possibilities.Remove(num);
                    }
                }
            }
            Debug.Assert(possibilities.Count == 1);
            DebugOutput($"The only non overlapping one is {possibilities[0]}");
        }
        
    }
}