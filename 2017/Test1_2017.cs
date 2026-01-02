using System;
using System.Collections.Generic;

public class Test1_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 1;
    }

    public override void Execute()
    {
        List<int> digitList = new List<int>();
        foreach (char c in m_dataFileContents[0])
        {
            digitList.Add(int.Parse(""+c));
        }

        int lookAhead = IsPart1?1:(digitList.Count/2);
        
        int startIndex = 0;
        int currentIndex = startIndex;
        
        int total = 0;

        do
        {
            int checkValue = digitList[(currentIndex + lookAhead) % digitList.Count];

            if (digitList[currentIndex] == checkValue)
            {
                total += digitList[currentIndex];
            }

            currentIndex++;
            currentIndex %= digitList.Count;

        } while (currentIndex != startIndex);
        
        DebugOutput($"The total is {total}");

    }
    
    
    
    
}