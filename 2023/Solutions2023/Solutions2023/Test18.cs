

using System.Drawing;
using System.Numerics;

public class Test18 : BaseTest
{
    public override void Initialise()
    {
        TestID = 18;
        IsTestInput = false;
        IsPart2 = false;
    }

    public override void Execute()
    {
        List<Tuple<IntVector2, int, string>> dataList = new List<Tuple<IntVector2, int, string>>();
        foreach (string data in m_dataFileContents)
        {
            string[] tokens = data.Split(" ",StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries);
            IntVector2 direction = new IntVector2();
            if (tokens[0] == "U") direction = IntVector2.Down;
            else if (tokens[0] == "D") direction = IntVector2.Up;
            else if (tokens[0] == "L") direction = IntVector2.Left;
            else if (tokens[0] == "R") direction = IntVector2.Right;
            
            dataList.Add(new (direction, int.Parse(tokens[1]), tokens[2]));
        }



        IntVector2 minBounds = new IntVector2(int.MaxValue, int.MaxValue);
        IntVector2 maxBounds = new IntVector2(int.MinValue, int.MinValue);
        

        
        IntVector2 currentPos = new IntVector2();
        foreach (var data in dataList)
        {
            for (int i = 0; i < data.Item2; ++i)
            {
                currentPos += data.Item1;
                minBounds.Min(currentPos);
                maxBounds.Max(currentPos);
            }
        }

        int width = (maxBounds.X - minBounds.X) + 1;
        int height = (maxBounds.Y - minBounds.Y) + 1;

        int val = GetIndex(minBounds.X, minBounds.Y, width, height, minBounds);
        int val2 = GetIndex(maxBounds.X, maxBounds.Y, width, height, minBounds);
        
        char[] sourceGrid = new char[width * height];
        Array.Fill(sourceGrid, '.');
        char[] destGrid = new char[width * height];
        
        foreach (var data in dataList)
        {
            for (int i = 0; i < data.Item2; ++i)
            {
                currentPos += data.Item1;

                int index = GetIndex(currentPos.X, currentPos.Y, width, height, minBounds);
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
        IntVector2 start = new IntVector2(37,140);

        FloodFill(start, sourceGrid, destGrid, width, height);
        
        DebugOutput(Helper.DrawGrid(destGrid,width,height));
        int filledArea = destGrid.Where(x => x == '#').Count();
        DebugOutput("Filled Area = " + filledArea);
    }

    public int 
        GetIndex(int x, int y, int width,int height,IntVector2 min)
    {
        return ((y+Math.Abs(min.Y)) * width) + x + Math.Abs(min.X);
    }
    
    
    
    public void FloodFill(IntVector2 start,char[] sourceGrid,char[] destGrid, int width,int height)
    {
        Stack<IntVector2> workingQueue = new Stack<IntVector2>();
        bool[] filled = new bool[sourceGrid.Length]; 
        
        workingQueue.Push(start);
        while (workingQueue.Count > 0)
        {
            IntVector2 current = workingQueue.Pop();
            filled[(current.Y * width) + current.X] = true;
            
            destGrid[(current.Y*width)+current.X] = '#';

            foreach (IntVector2 offset in IntVector2.Directions)
            {
                IntVector2 adjusted = current + offset;
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