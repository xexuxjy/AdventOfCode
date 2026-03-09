using System;
using System.Collections.Generic;

public class Test1_2018 : BaseTest
{
    public override void Initialise()
    {
        Year = 2018;
        TestID = 1;
    }

    public override void Execute()
    {
        int total = 0;
        HashSet<int> totals = new HashSet<int>();

        bool keepGoing = true;
        while (keepGoing)
        {
            foreach (string line in m_dataFileContents)
            {
                total += int.Parse(line);
                if (!totals.Contains(total))
                {
                    totals.Add(total);
                }
                else
                {
                    if (IsPart2)
                    {
                        DebugOutput($"Found duplicate at {total}");
                        keepGoing = false;
                        break;
                    }
                }
            }

            if (IsPart1)
            {
                break;
            }
        }

        DebugOutput($"The total is {total}");
    }
}