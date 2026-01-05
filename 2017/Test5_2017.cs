using System;
using System.Collections.Generic;

public class Test5_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 5;
    }

    public override void Execute()
    {
        List<int> jumpList = new List<int>();
        foreach(string line in m_dataFileContents)
        {
            jumpList.Add(int.Parse(line));
        }

        int stepCount = 0;
        int programCounter = 0;
        while (programCounter >= 0 && programCounter < jumpList.Count)
        {
            int currentAddress = programCounter;
            int jumpValue = jumpList[currentAddress];
            programCounter += jumpValue;

            if (IsPart1)
            {
                jumpList[currentAddress]++;
            }
            else
            {
                jumpList[currentAddress] += (jumpValue >=3)?-1:1;
            }

            //DebugOutput($"{programCounter}   {jumpValue}");
            stepCount++;
        }

        DebugOutput($"Escaped after {stepCount} steps");
        
    }
}