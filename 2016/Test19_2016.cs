using System;
using System.Collections.Generic;

public class Test19_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 19;
    }

    public override void Execute()
    {
        List<int> presentCounts = new List<int>();
        List<int> activeElves = new List<int>();
        List<int> removeList = new List<int>();

        int numElves = IsTestInput?5:3005290;
        
        for (int i = 0; i < numElves; i++)
        {
            presentCounts.Add(1);
            activeElves.Add(i);
        }

        while (true)
        {
            for(int i=0;i<activeElves.Count;i++)
            {
                int elf = activeElves[i];
                if (!removeList.Contains(elf))
                {
                    for (int j = 1; j < activeElves.Count; j++)
                    {
                        int target = activeElves[(i + j) % activeElves.Count];
                        if (!removeList.Contains(target))
                        {
                            presentCounts[elf] += presentCounts[target];
                            presentCounts[target] = 0;
                            removeList.Add(target);
                            break;
                        }
                    }
                }
            }
            activeElves.RemoveAll(removeList.Contains);
            removeList.Clear();
            DebugOutput("Active Elves : "+activeElves.Count);
            if (activeElves.Count == 1)
            {
                goto WinnerFound;
            }

        }

        for (int i = 0; i < activeElves.Count; i += 2)
        {
            int target = i+1 % activeElves.Count;
        }
        
        WinnerFound:
            DebugOutput($"The winner is Elf {activeElves[0]+1} with {presentCounts[activeElves[0]]} presents.");
    }
}