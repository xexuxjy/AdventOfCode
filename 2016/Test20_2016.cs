using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Test20_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 20;
    }

    public override void Execute()
    {
        List<(uint, uint)> ranges = new List<(uint, uint)>();
        List<(uint, uint)> mergedRanges = new List<(uint, uint)>();

        List<UIntRange> initialRanges = new List<UIntRange>();

        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split('-');
            initialRanges.Add(new UIntRange(uint.Parse(tokens[0]), uint.Parse(tokens[1])));
        }

        UIntRanges allRanges = new UIntRanges();

        foreach (UIntRange range in initialRanges)
        {
            allRanges.Merge(range);
        }

        // DebugOutput("Original");
        // foreach (UIntRange range in allRanges.ranges)
        // {
        //     DebugOutput($"{range.Start}-{range.End}");
        // }
        allRanges.Join();

        // DebugOutput("Joined");
        // foreach (UIntRange range in allRanges.joinedRanges)
        // {
        //     DebugOutput($"{range.Start}-{range.End}");
        // }


        // UIntRange a = new UIntRange(2122624, 19449261);
        // UIntRange b = new UIntRange(3306156, 16019348);
        //
        // bool overlap1 = a.Overlaps(b);
        // bool overlap2 = b.Overlaps(a);

        uint lowestAddress = 0;
        if (allRanges.joinedRanges[0].Start >= 0)
        {
            lowestAddress = allRanges.joinedRanges[0].End + 1;
        }

        DebugOutput($"Lowest address is : {lowestAddress}");


        long count = 0;
        uint maxVal = IsTestInput ? 9 : 4294967295;

        if (IsPart2)
        {
            for (int i = 0; i < allRanges.joinedRanges.Count - 1; i++)
            {
                count += (allRanges.joinedRanges[i + 1].Start - allRanges.joinedRanges[i].End-1);
            }

            count += (maxVal - allRanges.joinedRanges[allRanges.joinedRanges.Count - 1].End);
            DebugOutput($"Number of ips accessible is {count}");
        }

        int ibreak = 0;
    }
}


