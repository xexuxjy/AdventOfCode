using System;
using System.Collections.Generic;
public class Test2_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 2;
    }
    public override void Execute()
    {
        string code = IsPart1?Part1():Part2();
        
        DebugOutput($"The code is : {code}");
    }

    public string Part1()
    {
        IntVector2 startPosition = new IntVector2(1,1);
        IntVector2 currentPosition = startPosition;

        List<IntVector2> finalResults = new List<IntVector2>();


        int gridSize  = 2;
        string[] grid = new string[3];
        grid[0] = "123";
        grid[1] = "456";
        grid[2] = "789";


        foreach(string line in m_dataFileContents)
        {
            foreach(char c in line)
            {
                IntVector2 result = currentPosition;
                switch (c)
                {
                    case 'U':
                        result += IntVector2.Down;
                        break;
                    case 'D':
                        result += IntVector2.Up;
                        break;
                    case 'L':
                        result += IntVector2.Left;
                        break;
                    case 'R':
                        result += IntVector2.Right;
                        break;

                }
                if(result.X >= 0 && result.X <= gridSize && result.Y >=0 && result.Y <= gridSize)
                {
                    currentPosition = result;
                }
            }
            finalResults.Add(currentPosition);
        }


        string code = "";
        foreach(IntVector2 iv2 in finalResults)
        {
            code += grid[iv2.Y][iv2.X];
        }
        return code;
    }


    public string Part2()
    {
        IntVector2 startPosition = new IntVector2(0,2);
        IntVector2 currentPosition = startPosition;

        List<IntVector2> finalResults = new List<IntVector2>();

        int gridSize = 4;

        string[] grid = new string[5];
        grid[0] = "  1  ";
        grid[1] = " 234 ";
        grid[2] = "56789";
        grid[3] = " ABC ";
        grid[4] = "  D  ";
        


        foreach(string line in m_dataFileContents)
        {
            foreach(char c in line)
            {
                IntVector2 result = currentPosition;
                switch (c)
                {
                    case 'U':
                        result += IntVector2.Down;
                        break;
                    case 'D':
                        result += IntVector2.Up;
                        break;
                    case 'L':
                        result += IntVector2.Left;
                        break;
                    case 'R':
                        result += IntVector2.Right;
                        break;

                }
                if(result.X >= 0 && result.X <= gridSize && result.Y >=0 && result.Y <= gridSize && grid[result.Y][result.X] != ' ')
                {
                    currentPosition = result;
                }
            }
            finalResults.Add(currentPosition);
        }


        string code = "";
        foreach(IntVector2 iv2 in finalResults)
        {
            string t  = grid[iv2.Y];
            code += grid[iv2.Y][iv2.X];
        }
        return code;
    }
}
