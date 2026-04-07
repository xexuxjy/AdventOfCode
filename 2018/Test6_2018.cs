using System;
using System.Collections.Generic;

public class Test6_2018 : BaseTest
{
    public override void Initialise()
    {
        Year = 2018;
        TestID = 6;
    }

    public override void Execute()
    {
        List<IntVector2> positions = new List<IntVector2>();
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            IntVector2 position = new IntVector2(int.Parse(tokens[0]), int.Parse(tokens[1]));
            positions.Add(position);
        }

        var sortedList = positions.OrderBy(x => x.X).ThenBy(x => x.Y);


        // figure out which points are 'infinite' (i.e. on the outside)

        IntVector2 min = new IntVector2(int.MaxValue, int.MaxValue);
        IntVector2 max = new IntVector2(int.MinValue, int.MinValue);

        foreach (IntVector2 position in sortedList)
        {
            min = new IntVector2(Math.Min(position.X, min.X), Math.Min(position.Y, min.Y));
            max = new IntVector2(Math.Max(position.X, max.X), Math.Max(position.Y, max.Y));
        }

        List<IntVector2> prunedResult = new List<IntVector2>();
        foreach (IntVector2 position in positions)
        {
            if (position.X < max.X && position.X > min.X && position.Y < max.Y && position.Y > min.Y)
            {
                prunedResult.Add(position);
            }
        }

        Dictionary<IntVector2, List<(IntVector2, int)>> distanceMap =
            new Dictionary<IntVector2, List<(IntVector2, int)>>();

        // shrink the grid?
        //min = min - new IntVector2(1, 1);
        //max = max + new IntVector2(1, 1);

        //min = IntVector2.Zero;
        //max *= 2;

        for (int x = min.X; x <= max.X; x++)
        {
            for (int y = min.Y; y <= max.Y; y++)
            {
                IntVector2 testPosition = new IntVector2(x, y);

                List<(IntVector2, int)> distanceList = new List<(IntVector2, int)>();
                distanceMap.Add(testPosition, distanceList);

                foreach (IntVector2 position in positions)
                {
                    int distance = testPosition.ManhattanDistance(position);
                    distanceList.Add((position, distance));
                }
            }
        }

        foreach (List<(IntVector2, int)> distanceList in distanceMap.Values)
        {
            distanceList.Sort((a, b) => a.Item2.CompareTo(b.Item2));
        }

        Dictionary<IntVector2, int> finalScore = new Dictionary<IntVector2, int>();
        HashSet<IntVector2> possibleInfinites = new HashSet<IntVector2>();
        
        // now score the points .
        foreach (IntVector2 position in prunedResult)
        {
            finalScore[position] = 0;
            foreach(IntVector2 key in distanceMap.Keys)
            {
                var distanceList = distanceMap[key];
                if (distanceList.Count > 0 && distanceList[0].Item1 == position)
                {
                    if (key.X == min.X || key.X == max.X || key.Y == min.Y || key.Y == max.Y)
                    {
                        possibleInfinites.Add(distanceList[0].Item1);
                    }
                    
                    if (distanceList.Count == 1 || distanceList[0].Item2 < distanceList[1].Item2)
                    {
                        finalScore[position]++;
                    }
                }
            }
        }

        foreach (IntVector2 position in possibleInfinites)
        {
            if (finalScore.ContainsKey(position))
            {
                finalScore.Remove(position);
            }
        }
        
        
        int largestArea = finalScore.Values.Max();

        DebugOutput($"The largest area is {largestArea}");



        if (IsPart2)
        {
            long regionSize = 0;
            int limit = 10000;
            foreach (IntVector2 position in distanceMap.Keys)
            {
                int sum = 0;
                foreach (var distanceInfo in distanceMap[position])
                {
                    sum +=  distanceInfo.Item2;
                }

                if (sum < limit)
                {
                    regionSize++;
                }
            }
            DebugOutput($"Part 2 region size is {regionSize}");
        }
        
        if (false)
        {

            List<char> output = new List<char>();

            for (int y = 0; y <= max.Y; y++)
            {
                for (int x = 0; x <= max.X; x++)
                {
                    IntVector2 testPosition = new IntVector2(x, y);
                    char c = '.';

                    if (positions.Contains(testPosition))
                    {
                        int index = positions.IndexOf(testPosition);
                        c = (char)((int)'A' + index);
                    }

                    // else if (distanceMap.ContainsKey(testPosition))
                    // {
                    //     if (distanceMap[testPosition].Count == 1 ||
                    //         distanceMap[testPosition][0].Item2 < distanceMap[testPosition][1].Item2)
                    //     {
                    //         int index = positions.IndexOf(distanceMap[testPosition][0].Item1);
                    //         c = (char)((int)'a' + index);
                    //     }
                    // }
                    output.Add(c);
                }
            }

            int width = max.X - min.X + 1;
            int height = max.Y - min.Y + 1;

            DebugOutput(Helper.DrawGrid(output.ToArray(), width, height));
        }
    }
}