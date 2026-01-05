using System;
using System.Collections.Generic;

public class Test3_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 3;
    }

    Dictionary<int,IntVector2> NumberToPosition = new Dictionary<int,IntVector2>();
    Dictionary<IntVector2,int> PositionToNumber = new Dictionary<IntVector2,int>();

    private int KeyNumber = 0;
    private int Part2Number = 0;
    
    public void AddValue(IntVector2 position, int number)
    {
        NumberToPosition[number] = position;
        PositionToNumber[position] = number;
    }
    
    public override void Execute()
    {
        KeyNumber = IsTestInput?125:289326;

        int count = 1;
        IntVector2 currentPoint = IntVector2.Zero;

        
        AddValue(currentPoint, count);
       
        currentPoint = IntVector2.Right;
        

        int stepSize = 1;

        while ((IsPart1 && count <= KeyNumber) || (IsPart2 && Part2Number == 0))
        {
            for (int i = 0; i < stepSize; i++)
            {
                int val = CalculateValue(currentPoint, ref count);
                AddValue(currentPoint, val);
                currentPoint += IntVector2.Down;
            }

            stepSize++;

            for (int i = 0; i < stepSize; i++)
            {
                int val = CalculateValue(currentPoint, ref count);
                AddValue(currentPoint, val);
                currentPoint += IntVector2.Left;
            }
            

            for (int i = 0; i < stepSize; i++)
            {
                int val = CalculateValue(currentPoint, ref count);
                AddValue(currentPoint, val);
                currentPoint += IntVector2.Up;
            }

            stepSize++;

            for (int i = 0; i < stepSize; i++)
            {
                int val = CalculateValue(currentPoint, ref count);
                AddValue(currentPoint, val);
                currentPoint += IntVector2.Right;
            }

        }

        IntVector2 min = new IntVector2(int.MaxValue, int.MaxValue);
        IntVector2 max = new IntVector2(int.MinValue, int.MinValue);
        foreach (var key in NumberToPosition.Values)
        {
            min.Min(key);
            max.Max(key);
        }

        if (IsPart1)
        {
            IntVector2 position = NumberToPosition[KeyNumber];
            DebugOutput($"The distance to {KeyNumber} {position}  is {position.ManhattanDistance(IntVector2.Zero)}");
        }
        else
        {
            DebugOutput($"The first number written is {Part2Number}");
        }
        

        // for (int y = min.Y; y <= max.Y; y++)
        // {
        //     string line = "";
        //     for (int x = min.X; x <= max.X; x++)
        //     {
        //         if (grid.ContainsKey(new IntVector2(x, y)))
        //         {
        //             line += grid[new IntVector2(x, y)];
        //             line += "\t";
        //         }
        //     }
        //     DebugOutput(line);
        // }

        
        
        
    }

    public int CalculateValue(IntVector2 currentPosition,ref int count)
    {
        if (IsPart1)
        {
            return ++count;
        }
        else
        {
            // check all ordinals
            int returnValue = 0;
            foreach (var modifier in IntVector2.AllDirections)
            {
                IntVector2 updatedPosition = currentPosition + modifier;
                if (PositionToNumber.TryGetValue(updatedPosition, out int result))
                {
                    returnValue += result;
                }
            }

            if (Part2Number == 0 && returnValue > KeyNumber)
            {
                Part2Number = returnValue;
            }
            
            return returnValue;
        }
    }
    
}