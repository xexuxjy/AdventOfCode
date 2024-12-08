using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

public class Test8_2024 : BaseTest
{

    public override void Initialise()
    {
        Year = 2024;
        TestID = 8;
    }

    public override void Execute()
    {

        int width = 0;
        int height = 0;
        char[] dataGrid = Helper.GetCharGrid(m_dataFileContents, ref width, ref height);

        Dictionary<char, List<IntVector2>> antennaLocations = new Dictionary<char, List<IntVector2>>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                char c = dataGrid[(y * width) + x];
                List<IntVector2> locations;
                if (c != '.')
                {
                    if (!antennaLocations.TryGetValue(c, out locations))
                    {
                        locations = new List<IntVector2>();
                        antennaLocations[c] = locations;
                    }
                    locations.Add(new IntVector2(x, y));
                }
            }
        }


        // got all the data, now to sort out pairs?

        HashSet<IntVector2> antiNodeMap = new HashSet<IntVector2>();

        foreach (char c in antennaLocations.Keys)
        {
            List<IntVector2> locations = antennaLocations[c];
            // need to find every pair?
            foreach (var pair in Combinations.CombinationsRosettaWoRecursion(locations.ToArray(), 2))
            {
                //DebugOutput($"For '{c}' there is : {pair[0]}  and {pair[1]}");

                IntVector2 diff = pair[1] - pair[0];

                if(IsPart2)
                {
                    antiNodeMap.Add(pair[0]);
                    antiNodeMap.Add(pair[1]);
                }

                //DebugOutput($"{diff}  : {v1}  and {v2}");
                IntVector2 v1 = pair[0]-diff;
                IntVector2 v2 = pair[1]+diff;

                while(Helper.InBounds(v1,width,height))
                {
                    antiNodeMap.Add(v1);
                    v1 = v1 - diff;
                    if(!IsPart2)
                    {
                        break;
                    }

                };

                while(Helper.InBounds(v2,width,height))
                {
                    antiNodeMap.Add(v2);
                    v2 = v2 + diff;
                    if(!IsPart2)
                    {
                        break;
                    }
                };

            }
        }

        foreach(IntVector2 iv2 in antiNodeMap)
        {
            int index = (iv2.Y * width)+iv2.X;
            dataGrid[index] = '#';
        }

        DebugOutput(Helper.DrawGrid(dataGrid, width, height));

        DebugOutput($"Total of {antiNodeMap.Count} antinodes");

    }
}