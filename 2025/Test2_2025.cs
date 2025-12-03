using System;
using System.Collections.Generic;

public class Test2_2025 : BaseTest
{
    public override void Initialise()
    {
        Year = 2025;
        TestID = 2;
    }

    public override void Execute()
    {
        String dataLine = m_dataFileContents[0];
        string[] ranges = dataLine.Split(',',StringSplitOptions.RemoveEmptyEntries);
        long total = 0;
        foreach (var rangeString in ranges)
        {
            string[] splitRange =  rangeString.Split('-');
            string lhs = splitRange[0];
            string rhs = splitRange[1];
            (long,long) range =  (long.Parse(lhs), long.Parse(rhs));
            List<long> result = CheckDuplicate(range);
            foreach (long val in result)
            {
                total += val;
            }
        }
        DebugOutput($"Total of invalid ID's is : {total}");
    }

    public List<long> CheckDuplicate((long,long) range)
    {
        List<long> result = new List<long>();
        for (long i = range.Item1; i <= range.Item2; i++)
        {
            long invalidId = IsPart2?GetInvalidIDPart2(i):GetInvalidID(i);
            
            if(invalidId != 0)
            {
                result.Add(invalidId);
            }
        }
        return result;
    }

    public long GetInvalidID(long value)
    {
        string valueAsString = value.ToString();
        // round up
        int halfLength = (valueAsString.Length+1) / 2;
        string testVal = valueAsString.Substring(0,halfLength);
        if (valueAsString.Substring(halfLength).Contains(testVal))
        {
            //DebugOutput($"Original Value {value} testVal {testVal} matches");
            return long.Parse(valueAsString);
        }

        return 0;
    }
    
    public long GetInvalidIDPart2(long value)
    {
        string valueAsString = value.ToString();
        // round up
        int splitLength = (valueAsString.Length+1) / 2;
        int numSplits = 1;

        if (value == 44444)
        {
            int ibreak = 0;
        }
        
        while (splitLength > 0)
        {
            numSplits++;
            int startPoint = 0; 
            string testVal = valueAsString.Substring(0, splitLength);
            bool allMatches = true;
            while (startPoint < valueAsString.Length)
            {
                if (startPoint + splitLength > valueAsString.Length)
                {
                    allMatches = false;
                    break;
                }
                if (!valueAsString.Substring(startPoint, splitLength).Equals(testVal))
                {
                    allMatches = false;
                    break;
                }

                startPoint+= splitLength;
            }
            
            if (allMatches)
            {
                bool sanity = (valueAsString.Length % testVal.Length) == 0;
                bool sanity2 = (valueAsString.Length / testVal.Length) > 1;

                if (sanity && sanity2)
                {
                    //DebugOutput($"Original Value {value} splitLength {splitLength} testVal {testVal}  matches");
                    return long.Parse(valueAsString);
                }
                else
                {   
                    //DebugOutput($"***** FAILED SANITY {splitLength}  {numSplits}  {numSplits*testVal.Length}  {valueAsString.Length}");
                }
            }
            splitLength--;
        }

        return 0;
    }
}