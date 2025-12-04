using System;
using System.Collections.Generic;
using System.Security.AccessControl;

public class Test4_2025 : BaseTest
{
    public override void Initialise()
    {
        Year = 2025;
        TestID = 4;
    }

    public char PAPER = '@';
    public char REMOVABLE = '$';
    public char EMPTY = '.';
    
    private int Width;
    private int Height;

    private char[] DataGrid;
    private char[] RemovableGrid;
    
    private int RemovableCount = 0;
    
    public override void Execute()
    {
        DataGrid = Helper.GetCharGrid(m_dataFileContents,ref Width,ref Height);
        RemovableGrid = new char[Width * Height];
        Array.Copy(DataGrid, RemovableGrid, DataGrid.Length);
        
        int totalMovable=1;

        if (IsPart2)
        {
            while (totalMovable > 0)
            {
                totalMovable = CountMoveable();
                //DebugOutput($"TM : {totalMovable}   {RemovableCount}");
                //DebugOutput(Helper.DrawGrid(RemovableGrid, Width,Height));
                //DebugOutput(Helper.DrawGrid(DataGrid, Width,Height));
                
                for (int i = 0; i < RemovableGrid.Length; i++)
                {
                    if (RemovableGrid[i] == REMOVABLE)
                    {
                        DataGrid[i] = EMPTY;
                    }
                }
                Array.Copy(DataGrid, RemovableGrid, DataGrid.Length);
            }
        }

        if (IsPart1)
        {
            DebugOutput($"Total movable is {totalMovable}");
        }
        else
        {
            DebugOutput($"Total removable is {RemovableCount}");
        }
    }

    public int CountMoveable()
    {
        int totalMoveable=0;
        
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                IntVector2 pos = new IntVector2(x, y);
                int index = (pos.Y * Width) + pos.X;
                if (DataGrid[index] == PAPER)
                {
                    int count = 0;
                    foreach (IntVector2 direction in IntVector2.AllDirections)
                    {
                        IntVector2 adjustedPos = new IntVector2(x, y) + direction;;
                        if (Helper.InBounds(adjustedPos, Width, Height))
                        {
                            int newIndex = (adjustedPos.Y * Width) + adjustedPos.X;
                            if (DataGrid[newIndex] == PAPER)
                            {
                                count++;
                            }
                        }
                    }

                    if (count < 4)
                    {
                        totalMoveable++;
                        if (IsPart2)
                        {
                            RemovableGrid[index] = REMOVABLE;
                            RemovableCount++;
                        }
                    }
                    
                }
                
            }
        }
        return totalMoveable;

    }
    
}