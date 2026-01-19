using System;
using System.Collections.Generic;

public class Test10_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 10;
    }

    public override void Execute()
    {
        List<int> list = new List<int>();

        int size = IsTestInput ? 5 : 256;
        for (int i = 0; i < size; i++)
        {
            list.Add(i);
        }


        int currentPosition = 0;
        int skipSize = 0;

        string[] tokens = m_dataFileContents[0].Split(',');
        List<int> lengths = new List<int>();


        if (IsPart1)
        {
            foreach (string token in tokens)
            {
                lengths.Add(int.Parse(token));
            }
        }
        else
        {
            foreach (char c in m_dataFileContents[0])
            {
                lengths.Add((int)c);
            }

            lengths.AddRange(new int[] { 17, 31, 73, 47, 23 });
        }


        int numIterations = IsPart1 ? 1 : 64;

        for (int i = 0; i < numIterations; i++)
        {
            foreach (int length in lengths)
            {
                if (IsPart1)
                {
                    ProcessPart1(length, list, ref currentPosition, ref skipSize);
                }
                else
                {
                    ProcessPart2(length, list, ref currentPosition, ref skipSize);
                }
            }
        }

        if (IsPart1)
        {
            int finalResult = list[0] * list[1];
            DebugOutput($"The final result is {finalResult}");
        }
        else
        {
            List<int> denseHash = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                int hash = list[(i * 16)];
                for (int j = 1; j < 16; j++)
                {
                    hash = hash ^ list[(i * 16) + j];
                }

                denseHash.Add(hash);
            }

            string result = "";
            foreach (int i in denseHash)
            {
                result += ((byte)i).ToString("X2");
            }

            DebugOutput($"Final result is {result}");
        }
    }


    public void ProcessPart1(int length, List<int> list, ref int currentPosition, ref int skipSize)
    {
        for (int index = 0; index < (length / 2); index++)
        {
            int position1 = (currentPosition + index) % list.Count;
            int position2 = (currentPosition + length - 1 - index) % list.Count;

            list.Swap(position1, position2);
        }
        //DebugOutput(string.Join(',',list));

        currentPosition = (currentPosition + length + skipSize) % list.Count;
        skipSize++;
    }

    public void ProcessPart2(int length, List<int> list, ref int currentPosition, ref int skipSize)
    {
        for (int index = 0; index < (length / 2); index++)
        {
            int position1 = (currentPosition + index) % list.Count;
            int position2 = (currentPosition + length - 1 - index) % list.Count;

            list.Swap(position1, position2);
        }
        //DebugOutput(string.Join(',',list));

        currentPosition = (currentPosition + length + skipSize) % list.Count;
        skipSize++;
    }
}