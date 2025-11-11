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


public class UIntRange
{
    public uint Start;
    public uint End;

    public UIntRange(uint s, uint e)
    {
        Start = s;
        End = e;
    }

    public void Merge(UIntRange r)
    {
        Start = Math.Min(Start, r.Start);
        End = Math.Max(End, r.End);
    }

    public bool Overlaps(UIntRange r)
    {
        return (r.Start <= Start && r.End >= Start) || (r.Start <= End && r.End >= End);
    }

    public bool Contains(uint value)
    {
        return Start <= value && End <= value;
    }
}

public class UIntRanges
{
    public List<UIntRange> ranges = new List<UIntRange>();
    public List<UIntRange> joinedRanges = new List<UIntRange>();

    public void Join()
    {
        UIntRange activeRange = null;
        for (int i = 0; i < ranges.Count; i++)
        {
            if (activeRange == null)
            {
                activeRange = new UIntRange(ranges[i].Start, ranges[i].End);
            }

            if (i < ranges.Count - 1)
            {
                if (ranges[i + 1].Start == (activeRange.End + 1))
                {
                    activeRange.End = ranges[i + 1].End;
                }
                else
                {
                    joinedRanges.Add(activeRange);
                    activeRange = null;
                }
            }
        }

        if (activeRange != null)
        {
            joinedRanges.Add(activeRange);
        }
    }

    public void Merge(UIntRange range)
    {
        bool found = false;

        foreach (UIntRange r in ranges)
        {
            if (r.Overlaps(range) || range.Overlaps(r))
            {
                range.Merge(r);
            }
        }

        ranges.RemoveAll(x => x.Overlaps(range));
        ranges.Add(range);

        // keep them in order
        ranges.Sort((UIntRange left, UIntRange right) => left.Start.CompareTo(right.Start));
    }

    public bool HasSingleHole(int min, int max, ref long result)
    {
        if (ranges.Count == 0)
        {
            return false;
        }

        // if there are more than 2 ranges then there must be more then one hole.
        if (ranges.Count != 2)
        {
            return false;
        }

        // make sure the gap between end of range 1 and start of range 2 is 1...
        if (ranges[0].End + 1 == ranges[1].Start - 1)
        {
            result = ranges[0].End + 1;
            return true;
        }

        return false;
    }

    public bool Contains(uint value)
    {
        if (ranges.Count == 0)
        {
            return false;
        }

        if (ranges.Count == 1)
        {
            return ranges[0].Contains(value);
        }

        int start = 0;
        for (int i = 0; i < ranges.Count; ++i)
        {
            if (ranges[i].Contains(value))
            {
                return true;
            }

            if (ranges[i].Start > value)
            {
                return false;
            }
        }

        return false;
    }
}