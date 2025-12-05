using System;
using System.Collections.Generic;

public class Test5_2025 : BaseTest
{
    public override void Initialise()
    {
        Year = 2025;
        TestID = 5;
    }

    public override void Execute()
    {
        bool rangesDone = false;
        Ranges<ulong> ranges = new Ranges<ulong>();
        List<ulong> ingredients = new List<ulong>();
        
        foreach (string line in m_dataFileContents)
        {
            if (line == "")
            {
                rangesDone = true;
                continue;
            }

            if (!rangesDone)
            {
                string[] tokens = line.Split('-');
                Range<ulong> range = new Range<ulong>(ulong.Parse(tokens[0]), ulong.Parse(tokens[1]));
                ranges.Merge(range);
            }
            else
            {
                ingredients.Add(ulong.Parse(line));
            }
        }

        ulong freshCount = 0;
        foreach (ulong ingredient in ingredients)
        {
            if (ranges.Contains(ingredient))
            {
                freshCount++;
            }
        }

        ulong allFreshCount=0;
        if (IsPart2)
        {
            foreach (Range<ulong> range in ranges.ranges)
            {
                allFreshCount += ((range.End-range.Start)+1);
            }
        }

        if (IsPart1)
        {
            DebugOutput($"Fresh count is : {freshCount}");
        }
        else
        {
            DebugOutput($"Part2 AllFresh count is : {allFreshCount}");
        }
    }
}