using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Test6_2025 : BaseTest
{
    public override void Initialise()
    {
        Year = 2025;
        TestID = 6;
    }


    public override void Execute()
    {
        List<CephalopodMath> maths = new List<CephalopodMath>();

        List<int> separators = new List<int>();

        for (int i = 0; i < m_dataFileContents[0].Length; i++)
        {
            bool allEmpty = true;
            foreach (string line in m_dataFileContents)
            {
                if (line[i] != ' ')
                {
                    allEmpty = false;
                    break;
                }
            }

            if (allEmpty)
            {
                separators.Add(i);
            }
        }

        separators.Add(m_dataFileContents[0].Length);


        List<char[]> charBlocks = new List<char[]>();
        int startPos = 0;
        for (int sepIndex = 0; sepIndex < separators.Count; sepIndex++)
        {
            int width = separators[sepIndex] - startPos;
            int count = 0;
            char[] data = new char[width * m_dataFileContents.Count];
            foreach (string line in m_dataFileContents)
            {
                for (int i = startPos; i < startPos + width; i++)
                {
                    data[count++] = line[i];
                }
            }

            startPos = separators[sepIndex] + 1;
            charBlocks.Add(data);
            CephalopodMath math = new CephalopodMath();
            math.NumberData = data;
            math.Width = width;
            math.Height = m_dataFileContents.Count;
            maths.Add(math);
        }


        long total = 0;
        foreach (CephalopodMath math in maths)
        {
            math.BuildNumbers(IsPart1);
            DebugOutput("" + math.GetResult());
            total += math.GetResult();
        }

        DebugOutput($"Total result is {total}");
    }
}

public class CephalopodMath
{
    public string Operation;
    public List<long> Numbers = new List<long>();


    public char[] NumberData;
    public int Width;
    public int Height;


    public void BuildNumbers(bool isPart1)
    {
        if (isPart1)
        {
            for (int y = 0; y < Height; y++)
            {
                string lineData = "";
                for (int x = 0; x < Width; x++)
                {
                    lineData += NumberData[(y * Width) + x];
                }

                if (y < Height - 1)
                {
                    Numbers.Add(long.Parse(lineData));
                }
                else
                {
                    Operation = new string(lineData).Replace(" ", "");
                }
            }
        }
        else
        {
            for (int x = 0; x < Width; x++)
            {
                string lineData = "";
                for (int y = 0; y < Height-1; y++)
                {
                    lineData += NumberData[(y * Width) + x];
                }
                Numbers.Add(long.Parse(lineData));
            }

            for (int y = Height - 1; y < Height; y++)
            {
                string lineData = "";
                for (int x = 0; x < Width; x++)
                {
                    lineData += NumberData[(y * Width) + x];
                }
                Operation = new string(lineData).Replace(" ", "");
            }
        }
    }


    public long GetResult()
    {
        switch (Operation)
        {
            case "+":
                return Numbers.Aggregate(0L, (acc, val) => acc + val);
            case "*":
                return Numbers.Aggregate(1L, (acc, val) => acc * val);
            case "/":
                return Numbers.Aggregate(Numbers[0], (acc, val) => acc / val);
            case "-":
                return Numbers.Aggregate(0L, (acc, val) => acc - val);
        }

        Debug.Assert(false, "Shouldn't get here");
        return long.MinValue;
    }
}