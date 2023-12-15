using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

public class Test12 : BaseTest
{
    public override void Initialise()
    {
        TestID = 12;
        IsTestInput = false;
        IsPart2 = true;
    }

    public override void Execute()
    {
        long total = 0;
        foreach (string data in m_dataFileContents)
        {
            PuzzleLine puzzleLine = new PuzzleLine(data,IsPart2,this);
            long score = puzzleLine.Score();
            total += score;
            //DebugOutput("Score for line is : " + score);
        }

        DebugOutput("Total is : " + total);
    }


    public class PuzzleLine
    {
        // public const int MaxSlots = 12;
        // public const int MaxSize = 2 ^ MaxSlots;
        
        private string Line="";
        public List<int> GroupSizes = new List<int>();
        private BaseTest m_baseTest;
        

        public PuzzleLine(string data,bool part2,BaseTest baseTest)
        {
            m_baseTest = baseTest;
            int numCopies = part2 ? 5 : 1;

            string[] tokens = data.Split(' ',StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < numCopies; ++i)
            {
                Line += tokens[0];
                if (i < numCopies - 1)
                {
                    Line += "?";
                }
            }
            
            List<int> temp = new List<int>();
            Helper.ReadInts(tokens[1], temp);
            for (int i = 0; i < numCopies; ++i)
            {
                GroupSizes.AddRange(temp);
            }
            
        }

        public long Score()
        {
            //return CountOptions(Line.ToCharArray(), GroupSizes);
            return GetCount(Line, GroupSizes);
        }
        
        public int CountOptions(char[] startLine ,List<int> groupings)
        {
            int total = 0;
            ;
            char[] slots = new char[startLine.Length];

            long maxSize = (long)Math.Pow(2,startLine.Length);
            for (long i = 0; i < maxSize; ++i)
            {
                FillArray(slots, i);
                // check groupings
                if (CheckGroupings(slots, startLine,groupings))
                {
                    total++;
                }
            }

            return total;
        }

        private static List<int> tempList = new List<int>();

        // guidance from https://pastebin.com/djb8RJ85  - thanks

        private Dictionary<string, long> cache = new Dictionary<string, long>();

        public long Calculate(string data, List<int> groups)
        {
            string cacheKey = data + string.Join(',', groups);
            if (cache.TryGetValue(cacheKey, out long result))
            {
                return result;
            }

            long calculatedResult = GetCount(data, groups);
            //long calculatedResult = GetCountCopy(data, groups);

            cache[cacheKey] = calculatedResult;
            return calculatedResult;
        }


        public long GetCountCopy(string springs, List<int> groups)
        {
            while (true)
            {
                if (groups.Count == 0)
                {
                    return
                        springs.Contains('#')
                            ? 0
                            : 1; // No more groups to match: if there are no springs left, we have a match
                }

                if (string.IsNullOrEmpty(springs))
                {
                    return 0; // No more springs to match, although we still have groups to match
                }

                if (springs.StartsWith('.'))
                {
                    springs = springs.Trim('.'); // Remove all dots from the beginning
                    continue;
                }

                if (springs.StartsWith('?'))
                {
                    return Calculate("." + springs[1..], groups) +
                           Calculate("#" + springs[1..], groups); // Try both options recursively
                }

                if (springs.StartsWith('#')) // Start of a group
                {
                    if (groups.Count == 0)
                    {
                        return 0; // No more groups to match, although we still have a spring in the input
                    }

                    if (springs.Length < groups[0])
                    {
                        return 0; // Not enough characters to match the group
                    }

                    if (springs[..groups[0]].Contains('.'))
                    {
                        return 0; // Group cannot contain dots for the given length
                    }

                    if (groups.Count > 1)
                    {
                        if (springs.Length < groups[0] + 1 || springs[groups[0]] == '#')
                        {
                            return 0; // Group cannot be followed by a spring, and there must be enough characters left
                        }

                        springs = springs[
                            (groups[0] +
                             1)..]; // Skip the character after the group - it's either a dot or a question mark
                        //groups = groups[1..];
                        List<int> temp = new List<int>();
                        temp.AddRange(groups);
                        temp.RemoveAt(0);
                        groups = temp;
                        
                        m_baseTest.DebugOutput("copy data now : " + springs);
                        m_baseTest.DebugOutput("copy groups now :" + string.Join(',', groups));
                        continue;
                    }

                    springs = springs[groups[0]..]; // Last group, no need to check the character after the group
                    //groups = groups[1..];
                    List<int> temp2 = new List<int>();
                    temp2.AddRange(groups);
                    temp2.RemoveAt(0);
                    groups = temp2;
                    m_baseTest.DebugOutput("copy data now : " + springs);
                    m_baseTest.DebugOutput("copy groups now :" + string.Join(',', groups));

                    
                    continue;
                }
            }
        }
        

        public long GetCount(string data, List<int> groups)
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                if (groups.Count == 0)
                {
                    if (data.Contains('#'))
                    {
                        return 0;
                    }
                    return 1;
                }

                if (string.IsNullOrEmpty(data))
                {
                    return 0;
                }

                // remove any dots at the begining
                if (data.StartsWith('.'))
                {
                    data = data.TrimStart('.');
                    continue;
                }


                if (data.StartsWith('?'))
                {
                    // look at both options
                    return Calculate("." + data[1..], groups) + Calculate("#" + data[1..], groups);
                }


                if (data.StartsWith('#'))
                {
                    if (groups.Count == 0)
                    {
                        return 0;
                    }

                    // not enough data to match a group
                    if (data.Length < groups[0])
                    {
                        return 0;
                    }

                    // got a dot before next group.
                    int nextDot = data.IndexOf('.');
                    if (nextDot != -1 && nextDot < groups[0])
                    {
                        return 0;
                    }

                    // group longer than expected.
                    if (data.Length > groups[0] && data[groups[0]] == '#')
                    {
                        return 0;
                    }

                    // to get here we must have matched the group size correctly to the block
                    
                    // more groups to match
                    if (groups.Count > 1)
                    {
                        if (data.Length < groups[0] + 1 || data[groups[0]] == '#')
                        {
                            return 0; // Group cannot be followed by a spring, and there must be enough characters left
                        }

                        
                        data = data[(groups[0]+1)..];
                        //m_baseTest.DebugOutput("data now : " + data);
                        
                        //groups = groups[1..groups.Count];
                        List<int> temp = new List<int>();
                        temp.AddRange(groups);
                        temp.RemoveAt(0);
                        groups = temp;
                        //m_baseTest.DebugOutput("groups now :" + string.Join(',', groups));
                        continue;
                    }
                    
                    data = data[groups[0]..]; // Last group, no need to check the character after the group
                    //groups = groups[1..];
                    List<int> temp2 = new List<int>();
                    temp2.AddRange(groups);
                    temp2.RemoveAt(0);
                    groups = temp2;
                    //m_baseTest.DebugOutput("data now : " + data);
                    //m_baseTest.DebugOutput("groups now :" + string.Join(',', groups));


                    
                }

            }
            return 0;
        }
        
        
        
        public bool CheckGroupings(char[] slots,char[] startLine, List<int> groupings)
        {
            for (int i = 0; i < slots.Length; ++i)
            {
                if (startLine[i] != '?' && slots[i] != startLine[i])
                {
                    return false;
                }
            }
            // ok it's a possible match.
            tempList.Clear();
            int start = 0;
            int count = 0;
            while (start < slots.Length)
            {
                if (slots[start] == '#')
                {
                    count++;
                }
                else
                {
                    if (count > 0)
                    {
                        tempList.Add(count);
                    }

                    count = 0;
                }

                start++;
            }

            if (count > 0)
            {
                tempList.Add(count);
            }

            return tempList.SequenceEqual(groupings);

        }
        
        public void FillArray(char[] slots, long value)
        {
            for (int i = 0; i < slots.Length; ++i)
            {
                slots[i] = ((value & (1 << i)) != 0) ? '#' : '.';
            }
        }
        

    }
    
    
}