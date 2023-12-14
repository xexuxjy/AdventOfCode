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
        IsTestInput = true;
        IsPart2 = true;
    }

    public override void Execute()
    {
        int total = 0;
        foreach (string data in m_dataFileContents)
        {
            PuzzleLine puzzleLine = new PuzzleLine(data,IsPart2);
            int score = puzzleLine.Score();
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

        public PuzzleLine(string data,bool part2)
        {
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

        public int Score()
        {
            return CountOptions(Line.ToCharArray(), GroupSizes);
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