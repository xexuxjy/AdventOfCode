using System;
using System.Collections.Generic;
public class Test6_2017 : BaseTest
{
public override void Initialise()
{
Year = 2017;
TestID = 6;
}
public override void Execute()
{
    
    List<int> blockAllocations = new List<int>();
    
    foreach (string token in m_dataFileContents[0].Split('\t'))
    {
        blockAllocations.Add(int.Parse(token));
    }

    
    
    int numCycles = 0;
    bool keepGoing = true;

    List<List<int>> seenStates = new List<List<int>>(); 
    List<int> initialAllocation =  new List<int>();
    
    List<(int,List<int>)> allocationsPerStep = new List<(int,List<int>)>();
    
    initialAllocation.AddRange(blockAllocations);
    seenStates.Add(initialAllocation);

    int part2Count = 0;
    
    while (keepGoing)
    {
        int largest = int.MinValue;
        int largestIndex = 0;
        for (int i = 0; i < blockAllocations.Count; i++)
        {
            if (blockAllocations[i] > largest)
            {
                largest = blockAllocations[i];
                largestIndex = i;
            }
        }

        int toAllocate = largest;
        int startIndex = largestIndex;
        blockAllocations[startIndex] = 0;
        
        while (toAllocate > 0)
        {
            startIndex = (startIndex+1)%blockAllocations.Count;
            blockAllocations[startIndex]++;
            toAllocate--;
        }

        List<int> seenState = new List<int>();
        seenState.AddRange(blockAllocations);

        if (IsPart1)
        {
            foreach (var state in seenStates)
            {
                if (Enumerable.SequenceEqual(seenState, state))
                {
                    keepGoing = false;
                    DebugOutput($"Completes after {numCycles} cycles");
                    break;
                }
            }
        }
        else
        {
            foreach (var stepAllocation in allocationsPerStep)
            {
                if (Enumerable.SequenceEqual(seenState, stepAllocation.Item2))
                {
                    keepGoing = false;
                    DebugOutput($"Found loop length of {numCycles-stepAllocation.Item1}");
                    break;
                }
            }
        }
            

        seenStates.Add(seenState);
        allocationsPerStep.Add((numCycles, seenState));
        
        numCycles++;

    }
    
}
}
