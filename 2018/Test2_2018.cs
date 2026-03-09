using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;

public class Test2_2018 : BaseTest
{
    public override void Initialise()
    {
        Year = 2018;
        TestID = 2;
    }

    public override void Execute()
    {
        if (IsPart1)
        {
            ExecutePart1();
        }
        else
        {
            ExecutePart2();
        }
    }

    public void ExecutePart1()
    {
        int twoCount = 0;
        int threeCount = 0;

        foreach (string line in m_dataFileContents)
        {
            HashSet<char> letters = new HashSet<char> ();

            foreach (char c in line)
            {
                letters.Add(c);
            }

            bool doneTwo = false;
            bool doneThree = false;
            
            foreach (char c in letters)
            {
                int count1 = line.Count(d => d == c); 
                if (!doneTwo && count1 == 2)
                {
                    doneTwo = true;
                    twoCount++;
                }
                if (!doneThree && count1 == 3)
                {
                    doneThree = true;
                    threeCount++;
                }
            }

            int ibreak = 0;
        }
        
        int total = twoCount * threeCount;
        DebugOutput($"Checksum is {total}");

    }

    public void ExecutePart2()
    {

        HashSet<string> examinedValues = new HashSet<string>();
        bool found = false;
        foreach (string line in m_dataFileContents)
        {
            foreach (string examined in examinedValues)
            {
                if(DifferentByOne(line,examined,out int diffIndex))
                {
                    found = true;
                    string result = "";
                    for (int i = 0; i < diffIndex; i++)
                    {
                        result += line[i];
                    }

                    for (int i = diffIndex+1; i < line.Length; i++)
                    {
                        result += line[i];
                    }
                    DebugOutput($"Result with character removed {result}");
                }
            }
            
            if(!found)
            {
                examinedValues.Add(line);                
            }
            else
            {
                break;
            }
        }
    }

    public bool DifferentByOne(string line1, string line2,out int diffIndex)
    {
        diffIndex = -1;
        Debug.Assert(line1.Length == line2.Length);
        int diff = 0;
        for (int i = 0; i < line1.Length; i++)
        {
            if (line1[i] != line2[i])
            {
                diff++;
                if (diff > 1)
                {
                    return false;
                }
                diffIndex = i;
            }
        }
        return true;
    }
    
}