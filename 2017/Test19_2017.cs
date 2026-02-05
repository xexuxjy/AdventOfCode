using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using Spectre.Console;

public class Test19_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 19;
    }

    public override void Execute()
    {
        int width = 0;
        int height = 0;

        char[] characterGrid = Helper.GetCharGrid(m_dataFileContents, ref width, ref height);

        IntVector2 startPosition = new IntVector2();
        IntVector2 direction = IntVector2.Up;


        List<char> visitedLocations = new List<char>();


        for (int i = 0; i < width; i++)
        {
            if (characterGrid[i] == '|')
            {
                startPosition.X = i;
                break;
            }
        }


        int steps = CalculateResults(startPosition, direction,visitedLocations,characterGrid,width,height);
        //int steps = AnimateResults(startPosition, direction,visitedLocations,characterGrid,width,height);
    
        DebugOutput($"Visited {string.Join("", visitedLocations)}  and took {steps} steps");
    }


    public int CalculateResults(IntVector2 startPosition, IntVector2 direction, List<char> visitedLocations,
        char[] characterGrid, int width, int height)
    {
        IntVector2 currentPosition = startPosition;

        bool reachedEnd = false;

        HashSet<IntVector4> visited = new HashSet<IntVector4>();
        
        int count = 0;
        while (!reachedEnd)
        {
            currentPosition = StepPosition(currentPosition, ref direction, characterGrid, width, height,
                visitedLocations);
            
            if (!Helper.InBounds(currentPosition, width, height) || characterGrid[Helper.GetIndex(currentPosition, width)] == ' ' )
            {
                reachedEnd = true;
            }

            count++;
        }

        return count;
    }

    public int AnimateResults(IntVector2 startPosition, IntVector2 direction,List<char> visitedLocations,char[] characterGrid,int width,int height)
    {
        IntVector2 currentPosition = startPosition;

        bool reachedEnd = false;

        int count = 0;

        Table table = new Table().Centered();

        AnsiConsole.Live(table).Start(ctx =>
        {
            if (ctx != null)
            {
                table.AddColumn("");
                table.AddRow("");

                while (!reachedEnd)
                {
                    currentPosition = StepPosition(currentPosition, ref direction, characterGrid, width, height,
                        visitedLocations);
                    if (!Helper.InBounds(currentPosition, width, height))
                    {
                        reachedEnd = true;
                    }

                    int span = 10;
                    
                    
                    StringBuilder sb = new StringBuilder();

                    int min = Math.Max(0,currentPosition.Y - span);
                    int max = Math.Min(height,currentPosition.Y + span);
                    
                    for (int y = min; y < max; ++y)
                    {
                        for (int x = 0; x < width; ++x)
                        {
                            char toDraw = characterGrid[y * width + x];
                            if (new IntVector2(x, y) == currentPosition)
                            {
                                toDraw = '*';
                            }
                
                            sb.Append(toDraw);
                        }

                        sb.AppendLine();
                    }



                    string text = sb.ToString();//DebugGridPosition(characterGrid, width, height, currentPosition);
                    char currentChar = characterGrid[Helper.GetIndex(currentPosition, width)];

                    if (currentChar == ' ')
                    {
                        int ibreak = 0;
                    }
                    
                    table.Columns[0]
                        .Header(
                            $"Step [[{count}]]  [[{currentPosition}]]  [[{direction}]] [[{currentChar}]] [[{string.Join("", visitedLocations)}]] ");
                    table.UpdateCell(0, 0, text);
                    ctx.Refresh();
                    Thread.Sleep(10);
                    count++;
                }
            }
        });
        return count;
    }
    

    public IntVector2 StepPosition(IntVector2 currentPosition, ref IntVector2 direction,char[] characterGrid,int width,int height,List<char> visitedLocations)
    {
        int currentIndex = Helper.GetIndex(currentPosition, width);
        char currentChar = characterGrid[currentIndex];

        if (currentChar >= 'A' && currentChar <= 'Z')
        {
            visitedLocations.Add(currentChar);
        }
        else if (currentChar == '+')
        {
            IntVector2 currentDirection = direction;
            if (direction == IntVector2.Up || direction == IntVector2.Down)
            {
                if (AdjustDirection(currentPosition, IntVector2.Left, characterGrid, width, height))
                {
                    direction = IntVector2.Left;
                }
                else
                {
                    direction = IntVector2.Right;
                }
            }
            else
            {
                if (AdjustDirection(currentPosition, IntVector2.Up, characterGrid, width, height))
                {
                    direction = IntVector2.Up;
                }
                else
                {
                    direction = IntVector2.Down;
                }
            }
        }
        
        return currentPosition+direction;
    }
    
    public string DebugGridPosition(char[] data,int width,int height,IntVector2 position)
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                char toDraw = data[y * width + x];
                if (new IntVector2(x, y) == position)
                {
                    toDraw = '*';
                }
                
                sb.Append(toDraw);
            }

            sb.AppendLine();
        }

        return sb.ToString();

    }
    
    public bool AdjustDirection(IntVector2 currentPosition, IntVector2 direction, char[] characterGrid, int width,
        int height)
    {
        IntVector2 newPosition = currentPosition + direction;
        if (Helper.InBounds(newPosition, width, height))
        {
            int nextIndex = Helper.GetIndex(newPosition, width);
            char nextChar = characterGrid[nextIndex];

            if (nextChar >= 'A' && nextChar <= 'Z')
            {
                return true;
            }
            
            if ((direction == IntVector2.Left || direction == IntVector2.Right) && nextChar == '-')
            {
                return true;
            }
            
            if ((direction == IntVector2.Up || direction == IntVector2.Down) && nextChar == '|')
            {
                return true;
            }
        }

        return false;
    }
}