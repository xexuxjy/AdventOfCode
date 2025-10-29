using System;
using System.Collections.Generic;
using System.Text;

public class Test16_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 16;
    }

    public void FillDisk(List<int> input, int diskSize)
    {
        while (input.Count < diskSize)
        {
            input =  Dragonify(input);
        }
        
        List<int> checksum = Checksum(input,diskSize);
        while (checksum.Count % 2 == 0)
        {
            checksum = Checksum(checksum,diskSize);
        }
        
        DebugOutput($"Final checksum is : {string.Join(',', checksum).Replace(",","")}");
    }
    
    public List<int> Dragonify(List<int> input)
    {
        List<int> reverseCopy = new List<int>(input);
        reverseCopy.Reverse();
        for (int i = 0; i < reverseCopy.Count; i++)
        {
            if (reverseCopy[i] == 0)
            {
                reverseCopy[i] = 1;
            }
            else
            {
                reverseCopy[i] = 0;
            }
        }

        List<int> finalResult = new List<int>(input);
        finalResult.Add(0);
        finalResult.AddRange(reverseCopy);
        return finalResult;
    }

    public List<int> Checksum(List<int> input, int diskSize)
    {
        List<int> result =  new List<int>();
        int bound = int.Min(input.Count, diskSize);
        for (int i = 0; i < bound; i+=2)
        {
            result.Add((input[i] == input[i + 1])?1:0);
            
        }
        return result;
    }
    
    
    public override void Execute()
    {
        // DebugOutput(string.Join(',', Dragonify(new List<int>() { 1 })).Replace(",",""));
        // DebugOutput(string.Join(',', Dragonify(new List<int>() { 0 })).Replace(",",""));
        // DebugOutput(string.Join(',', Dragonify(new List<int>() { 1,1,1,1,1 })).Replace(",",""));
        // DebugOutput(string.Join(',', Dragonify(new List<int>() { 1,1,1,1,0,0,0,0,1,0,1,0 })).Replace(",",""));

        if (IsTestInput)
        {
            FillDisk(new List<int>() { 1, 0, 0, 0, 0 }, 20);
        }
        else
        {
            FillDisk(new List<int>() { 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1 }, IsPart1?272:35651584);
        }

    }
}