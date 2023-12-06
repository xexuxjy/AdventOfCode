using System.Diagnostics;

public class Test5 : BaseTest
{
    public override void Initialise()
    {
        TestID = 5;
        IsTestInput = false;
        IsPart2 = true;
    }

    public override void Execute()
    {

        List<long> seeds = new List<long>();
        List<RangeGroup> rangeGroups = new List<RangeGroup>();

        RangeGroup rangeGroup = null;

        string seedLine = m_dataFileContents[0];
        seedLine = seedLine.Replace("seeds:", "");

        long lowestLocation = long.MaxValue;
       

        for (int i = 0; i < m_dataFileContents.Count; ++i)
        {
            string line = m_dataFileContents[i];
            if (line.Contains("map:"))
            {
                rangeGroup = new RangeGroup();
                rangeGroups.Add(rangeGroup);
            }

            if (line.Length > 0 && Char.IsDigit(line[0]))
            {
                Range range = Range.FromString(line);
                rangeGroup.Ranges.Add(range);
            }
        }

        foreach (RangeGroup rg in rangeGroups)
        {
            if (IsPart2)
            {
                rg.Ranges.Sort((x, y) => x.TargetStart.CompareTo(y.TargetStart));
            }
            else
            {
                rg.Ranges.Sort((x, y) => x.SourceStart.CompareTo(y.SourceStart));
            }
            
            int ibreak = 0;
        }
                 
        
        Debug.Assert(rangeGroups.Count == 7);

        string[] tokens = seedLine.Split(' ',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (IsPart2)
        {
            List<LongVector2> seedRanges = new List<LongVector2>();
            for (int i = 0; i < tokens.Length; i+=2)
            {
                seedRanges.Add(new LongVector2(long.Parse(tokens[i]), long.Parse(tokens[i + 1])));
            }
            
            seedRanges.Sort((x,y) => x.X.CompareTo(y.X));

            
            Tuple<long,long> testResult = TranslateLocation(46, rangeGroups);



            long location = 0;
            bool keepRunning = true;
            while (keepRunning)
            {
                Tuple<long, long> seedResult = TranslateLocation(location++, rangeGroups);
                foreach (LongVector2 seedRange in seedRanges)
                {
                    if (seedResult.Item2 >= seedRange.X && seedResult.Item2 < (seedRange.X + seedRange.Y))
                    {
                        DebugOutput("Found seed : " + seedResult.Item2);
                        keepRunning = false;
                        break;
                    }
                }
            }
        }
        else
        {
            foreach (string seed in tokens)
            {
                seeds.Add(long.Parse(seed));
            }

            foreach (long seed in seeds)
            {
                TranslateSeed(seed, rangeGroups, ref lowestLocation);
            }
        }
        DebugOutput("Lowest location : " + lowestLocation);
    }

    public long TranslateSeed(long seed, List<RangeGroup> rangeGroups, ref long lowestLocation)
    {
        int groupCount = 0;
        List<long> results = new List<long>();
        results.Add(seed);
        for (int i = 1; i < 8; ++i)
        {
            results.Add(rangeGroups[groupCount++].Translate(results[i - 1]));
        }

        long location = results[7];

        if (location < lowestLocation)
        {
            lowestLocation = location;
        }

        // DebugOutput(
        //     $"Seed {results[0]}, soil {results[1]}, fertilizer {results[2]}, water {results[3]}, light {results[4]}, temperature {results[5]}, humidity {results[6]}, location {results[7]}.");
        return location;
    }

    public Tuple<long,long> TranslateLocation(long location, List<RangeGroup> rangeGroups)
    {
        int groupCount = 1;
        List<long> results = new List<long>();
        results.Add(location);

        for (int i = 0; i < rangeGroups.Count; i++)
        {
            results.Add(rangeGroups[rangeGroups.Count-(groupCount++)].TranslateBack(results[i]));
        }

        return new Tuple<long, long>(location,results.Last());
    }
    
    
    

    public class RangeGroup
    {
        public List<Range> Ranges = new List<Range>();

        public long Translate(long sourceId)
        {
            foreach (Range r in Ranges)
            {
                long result = r.GetTargetForSource(sourceId);
                if (result != -1)
                {
                    if (result < 0)
                    {
                        int ibreak = 0;
                    }

                    return result;
                }
            }

            return sourceId;
        }

        public long TranslateBack(long targetId)
        {
            foreach (Range r in Ranges)
            {
                long result = r.GetSourceForTarget(targetId);
                if (result != -1)
                {
                    if (result < 0)
                    {
                        int ibreak = 0;
                    }

                    return result;
                }
            }
            return targetId;
        }
            
        
    }

    public class Range
    {
        public static Range FromString(string data)
        {
            string[] tokens = data.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            Range r = new Range();
            r.TargetStart = long.Parse(tokens[0]);
            r.SourceStart = long.Parse(tokens[1]);
            r.Length = long.Parse(tokens[2]);
            return r;
        }

        public bool InSourceRange(long id)
        {
            return id >= SourceStart && id < (SourceStart + Length);
        }

        public bool InTargetRange(long id)
        {
            return id >= TargetStart && id < (TargetStart + Length);
        }
        
        public long GetTargetForSource(long id)
        {
            if (InSourceRange(id))
            {
                return TargetStart + (id - SourceStart);
            }

            return -1;
        }

        public long GetSourceForTarget(long id)
        {
            if (InTargetRange(id))
            {
                return SourceStart + (id - TargetStart);
            }

            return -1;
        }

        
        
        public long TargetStart;
        public long SourceStart;
        public long Length;
    }
}