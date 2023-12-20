

using System.Diagnostics;
using System.Drawing;
using System.Numerics;

public class Test18 : BaseTest
{
    public override void Initialise()
    {
        TestID = 18;
        IsTestInput = true ;
        IsPart2 = false;
    }

    public override void Execute()
    {
        List<Tuple<LongVector2, int, string>> dataList = new List<Tuple<LongVector2, int, string>>();
        foreach (string data in m_dataFileContents)
        {
            string[] tokens = data.Split(" ",StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries);

            string dirString = tokens[0];
            int distance = int.Parse(tokens[1]);
            
            if (IsPart2)
            {
                dirString = "" + tokens[2].Substring(tokens[2].Length - 2, 1);
                string hexVal = "0x" + tokens[2].Substring(2, 6);
                distance = Convert.ToInt32(hexVal,16);
            }

            LongVector2 direction = new LongVector2();
            if (dirString == "U" || dirString == "0") direction = LongVector2.Down;
            else if (dirString == "D" || dirString == "1") direction = LongVector2.Up;
            else if (dirString == "L" || dirString == "2") direction = LongVector2.Left;
            else if (dirString == "R" || dirString == "3") direction = LongVector2.Right;

            
            
            dataList.Add(new (direction, distance, tokens[2]));
        }



        LongVector2 minBounds = new LongVector2(int.MaxValue, int.MaxValue);
        LongVector2 maxBounds = new LongVector2(int.MinValue, int.MinValue);
        

        
        LongVector2 currentPos = new LongVector2();
        foreach (var data in dataList)
        {
            for (int i = 0; i < data.Item2; ++i)
            {
                currentPos += data.Item1;
                minBounds.Min(currentPos);
                maxBounds.Max(currentPos);
            }
        }

        long width = (maxBounds.X - minBounds.X) + 1;
        long height = (maxBounds.Y - minBounds.Y) + 1;

        long val = GetIndex(minBounds.X, minBounds.Y, width, height, minBounds);
        long val2 = GetIndex(maxBounds.X, maxBounds.Y, width, height, minBounds);
        
        char[] sourceGrid = new char[width * height];
        Array.Fill(sourceGrid, '.');
        char[] destGrid = new char[width * height];
        
        foreach (var data in dataList)
        {
            for (int i = 0; i < data.Item2; ++i)
            {
                currentPos += data.Item1;

                long index = GetIndex(currentPos.X, currentPos.Y, width, height, minBounds);
                if(index > sourceGrid.Length)
                {
                    int ibreak = 0;
                }
                sourceGrid[index] = '#';
            }
        }

        Array.Copy(sourceGrid, destGrid, sourceGrid.Length);
        
        DebugOutput(Helper.DrawGrid(sourceGrid,width,height));

        // cheating by eyeballing
        LongVector2 start = new LongVector2(37,140);
        if (IsTestInput)
        {
            start = new LongVector2(1,1);
        }
        
        FloodFill(start, sourceGrid, destGrid, width, height);
        
        DebugOutput(Helper.DrawGrid(destGrid,width,height));
        int filledArea = destGrid.Where(x => x == '#').Count();
        DebugOutput("Filled Area = " + filledArea);

        long stripTotal = 0;
        for (int i = 0; i < height; ++i)
        { 
            long lineTotal = ParseStrips(i, dataList, width, height, minBounds);
            long floodLineTotal = CountLine(i, destGrid, width, height);

            if (lineTotal == floodLineTotal)
            {
                //DebugOutput($"Line  {i} matches with {lineTotal}");
            }
            else
            {
                //DebugOutput($"Line  {i} FAILS matches with lineTotal : {lineTotal}  and floodLineTotal : {floodLineTotal}");
            }
            
            stripTotal += lineTotal;

        }
        
        //ParseStrips(26, dataList, width, height, minBounds);
        
        DebugOutput("Filled Strips = " + stripTotal);
        
    }


    long CountLine(long line, char[] data,long width, long height)
    {
        long startIndex = (line * width);
        long endIndex = startIndex + width;
        long count = 0;
        for (long i = startIndex; i < endIndex; ++i)
        {
            if (data[i] == '#')
            {
                count++;
            }
        }

        return count;
    }
    
    

    public long ParseStrips(long line,List<Tuple<LongVector2, int, string>> dataList,long width,long height,LongVector2 minBounds)
    {
        LongVector2 currentPos = new LongVector2();

        bool[] src = new bool[width]; 
        bool[] dst = new bool[width];
        
        foreach (var data in dataList)
        {
            for (int i = 0; i < data.Item2; ++i)
            {
                currentPos += data.Item1;
                if (currentPos.Y + Math.Abs(minBounds.Y) == line)
                {
                    src[currentPos.X + Math.Abs(minBounds.X)] = true;
                }
            }
        }

        FillSpan(src, dst);
        
        return dst.Where(x=>x==true).Count();

    }
    
    
    
    
    public long 
        GetIndex(long x, long y, long width,long height,LongVector2 min)
    {
        return ((y+Math.Abs(min.Y)) * width) + x + Math.Abs(min.X);
    }


    public void FillSpan(bool[] src,bool[] dst)
    {
        bool open = false;
        bool nextCloses = false;
        for (int i = 0; i < src.Length-1; ++i)
        {
            if (src[i] && src[i + 1])
            {
                DebugOutput("Open at " + i);
                open = true;
            }
            else if(open && src[i] && !src[i + 1])
            {
                DebugOutput("Close at " + i);
                open = false;
            }
            else if (src[i] && !src[i + 1])
            {
                DebugOutput("Open at " + i);
                open = true;
            }

            dst[i] = open;
        }

        dst[dst.Length - 1] = src[src.Length - 1];
        if (open)
        {
            DebugOutput("Close at " + (src.Length - 1));
        }
    }
    
    
    
    
    public void FloodFill(LongVector2 start,char[] sourceGrid,char[] destGrid, long width,long height)
    {
        Stack<LongVector2> workingQueue = new Stack<LongVector2>();
        bool[] filled = new bool[sourceGrid.Length]; 
        
        workingQueue.Push(start);
        while (workingQueue.Count > 0)
        {
            LongVector2 current = workingQueue.Pop();
            filled[(current.Y * width) + current.X] = true;
            
            destGrid[(current.Y*width)+current.X] = '#';

            foreach (LongVector2 offset in LongVector2.Directions)
            {
                LongVector2 adjusted = current + offset;
                if(adjusted.X < 0 || adjusted.Y < 0 || adjusted.X >= width || adjusted.Y >= height || filled[(adjusted.Y*width)+adjusted.X])
                {
                    continue;
                }
                
                if(sourceGrid[(adjusted.Y*width)+adjusted.X] == '.')
                {
                    workingQueue.Push(adjusted);
                }
            }
        }
    }

    
}