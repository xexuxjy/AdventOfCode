using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

public class Test9_2024 : BaseTest
{
    public const int Empty = -1;
    public override void Initialise()
    {
        Year = 2024;
        TestID = 9;
    }

    public override void Execute()
    {
        List<(int, int, int)> diskBlocks = new List<(int, int, int)>();
        List<(int, int, int)> freeBlocks = new List<(int, int, int)>();

        List<int> outputList = new List<int>();

        int runningCount = 0;
        string line = m_dataFileContents[0];

        for (int i = 0; i < line.Length; i++)
        {
            int length = CharUnicodeInfo.GetDecimalDigitValue(line[i]);
            if (i % 2 == 0)
            {
                diskBlocks.Add((i / 2, runningCount,length));
            }
            else
            {
                freeBlocks.Add((i / 2, runningCount,length));
            }

            runningCount += length;

        }


        for (int i = 0; i < diskBlocks.Count; ++i)
        {
            var diskBlock = diskBlocks[i];
            for (int j = 0; j < diskBlock.Item3; ++j)
            {
                outputList.Add(diskBlock.Item1);
            }
            if (i < diskBlocks.Count - 1)
            {
                var freeBlock = freeBlocks[i];
                for (int j = 0; j < freeBlock.Item3; ++j)
                { 
                    outputList.Add(Empty);
                }
            }

        }


        //List<int> compacted = CompactData1(outputList);

        List<int> compacted = IsPart2?CompactData2(outputList,diskBlocks.ToArray(),freeBlocks.ToArray()): CompactData1(outputList);

        

        long checksum = 0;
        for (int i = 0; i < compacted.Count; ++i)
        {
            if (compacted[i] != Empty)
            {
                checksum += compacted[i] * i;
            }
        }

        DebugOutput("Checksum is : " + checksum);



        int ibreak = 0;
    }

    public List<int> CompactData1(List<int> input)
    {
        List<int> output = new List<int>();

        int nextFreeIndex = 0;
        int nextBlockIndex = input.Count - 1;

        bool sorted = false;
        while (nextFreeIndex < nextBlockIndex)
        {
            while (input[nextFreeIndex] != Empty)
            {
                //output[nextFreeIndex] = input[nextFreeIndex];
                output.Add(input[nextFreeIndex]);
                nextFreeIndex++;
            }

            while (input[nextBlockIndex] == Empty)
            {
                nextBlockIndex--;
            }

            // swap chars?
            int t = input[nextFreeIndex];
            input[nextFreeIndex] = input[nextBlockIndex];
            input[nextBlockIndex] = t;

        }

        while (nextFreeIndex < input.Count)
        {
            //output[nextFreeIndex] = Empty;
            output.Add(Empty);
            nextFreeIndex++;
        }

        return output;
    }

    public List<int> CompactData2(List<int> input,(int,int,int)[] diskBlocks,(int,int,int)[] freeBlocks)
    {

        int diskBlocksSize = diskBlocks.Length;
        int freeBlocksSize = freeBlocks.Length;

        for(int i=diskBlocksSize-1; i>=0; i--)
        {

            for(int j =0;j<freeBlocksSize;j++)
            {
                // not to the left.
                if(diskBlocks[i].Item2 < freeBlocks[j].Item2)
                {
                    continue;
                }

                int remain = freeBlocks[j].Item3 - diskBlocks[i].Item3;
                if(remain >= 0)
                {
                    // space for file in free block. use it and reduce space.
                    int freePosition = freeBlocks[j].Item2;
                    freeBlocks[j] = (freeBlocks[j].Item1,  freePosition+diskBlocks[i].Item3 ,remain);

                    diskBlocks[i] = (diskBlocks[i].Item1,  freePosition,diskBlocks[i].Item3);

                }
            }
        }

        List<int> output = new List<int>();
        for(int i=0;i<input.Count;++i)
        {
            output.Add(Empty);
        }

        foreach(var block in diskBlocks)
        {
            for(int i=0;i<block.Item3;++i)
            {
                output[block.Item2+i] = block.Item1;
            }
        }

        foreach(var block in freeBlocks)
        {
            for(int i=0;i<block.Item3;++i)
            {
                if(output[block.Item2+i] != Empty)
                {
                        int ibreak = 0;
                }

                output[block.Item2+i] = Empty;
            }
        }

        //DebugOutput(string.Join("",input));
        //DebugOutput(string.Join("",output));

        return output;

    }


}