using System;
using System.Collections.Generic;

public class Test3_2025 : BaseTest
{
    public override void Initialise()
    {
        Year = 2025;
        TestID = 3;
    }

    public override void Execute()
    {
        long bestTotal = 0;
        foreach (string line in m_dataFileContents)
        {
            bestTotal += CalcHighest(line,IsPart1 ? 2 : 12);
        }
        DebugOutput($"Best total is {bestTotal}");
    }


    public long CalcPart1(string line)
    {
        char biggest = '0';
        int biggestIndex = -1;
        for(int i = 0; i < line.Length-1; i++)
        {
            if (line[i] > biggest)
            {
                biggest = line[i];
                biggestIndex = i;
            }
        }
        char nextBiggest = '0';
        for (int i = biggestIndex + 1; i < line.Length; i++)
        {
            if (line[i] > nextBiggest)
            {
                nextBiggest = line[i];
            }
        }
            
        string result = ""+biggest+nextBiggest;
        long value = long.Parse(result);
        return value;
    }

    public long CalcHighest(string line,int numValues)
    {
        int startPos = 0;
        string finalResult = "";
        
        for (int i = numValues - 1; i >= 0; i--)
        {
            char biggest = '0';
            startPos = GetNextBiggestFwd(line,startPos,i,ref biggest);
            finalResult += biggest;
        }

        //DebugOutput(finalResult);
        long value = long.Parse(finalResult);
        return value;

    }
    

    public int GetNextBiggestFwd(string line, int startPos,int pos,ref char biggest)
    {
        int biggestIndex = -1;
        for (int i = startPos; i<line.Length-(pos); i++)
        {
            if (line[i] > biggest)
            {
                biggest = line[i];
                biggestIndex = i;
            }
        }

        return biggestIndex+1;
    }

    
}