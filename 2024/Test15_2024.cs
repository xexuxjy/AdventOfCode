using Spectre.Console;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;


public class Test15_2024 : BaseTest
{
    public const char WALL = '#';
    public const char EMPTY = '.';
    public const char ROBOT = '@';
    public const char BOX = 'O';

    public override void Initialise()
    {
        Year = 2024;
        TestID = 15;
    }

    public override void Execute()
    {
        List<string> warehouse = new List<string>();
        string commandString = "";
        foreach (string line in m_dataFileContents)
        {
            if (line.StartsWith("#"))
            {
                warehouse.Add(line);
            }
            else if (line != "")
            {
                commandString += line;
            }
        }

        int width = 0;
        int height = 0;
        char[] dataGrid = Helper.GetCharGrid(warehouse, ref width, ref height);
        int robotIndex = 0;

        for (int i = 0; i < dataGrid.Length; i++)
        {
            if (dataGrid[i] == ROBOT)
            {
                robotIndex = i;
                break;
            }
        }

        IntVector2 robotPosition = Helper.GetPosition(robotIndex, width);

        Dictionary<char,Spectre.Console.Color> colourMap = new Dictionary<char, Color>();
        colourMap[WALL] = Color.White;
        colourMap[BOX] = Color.Yellow;
        colourMap[ROBOT] = Color.Green;
        colourMap[EMPTY] = Color.Grey;


        Table table = new Table().Centered();


        AnsiConsole.Live(table).Start(ctx=>
        {
            if (ctx != null) {
                
                
                table.AddColumn("");
                table.AddRow("");

                for(int i=0;i<commandString.Length;++i)
                {
                    char move = commandString[i];
                    UpdateGrid(dataGrid, ref robotPosition, move, width);
                    string text = Helper.DrawGrid(dataGrid,width,height,colourMap);
                    table.UpdateCell(0,0,text);
                    //DebugOutput(Helper.DrawGrid(dataGrid,width,height));
                    //Helper.DrawGridToConsole(dataGrid,width,height, colourMap,10);
                    ctx.Refresh();
                    Thread.Sleep(10);
                }
            } });


        long totalSum=0;
        for(int i=0;i<dataGrid.Length;++i)
        {
            if(dataGrid[i] == BOX)
            {
                IntVector2 pos = Helper.GetPosition(i,width);
                totalSum += ((100 * pos.Y) + pos.X);
            }
        }
        
        DebugOutput("GPS Total is : "+totalSum);
    }

    public char[] UpdateGrid(char[] dataGrid, ref IntVector2 robotPosition, char command, int width)
    {
        IntVector2 updatedPosition = robotPosition;
        IntVector2 direction = IntVector2.Down;
        switch (command)
        {
            case '^':
                direction = IntVector2.Down;
                break;
            case 'v':
                direction = IntVector2.Up;
                break;
            case '<':
                direction = IntVector2.Left;
                break;
            case '>':
                direction = IntVector2.Right;
                break;
        }

        int oldIndex = Helper.GetIndex(robotPosition, width);
        int newIndex = Helper.GetIndex(robotPosition + direction, width);
        
        if (dataGrid[newIndex] == EMPTY)
        {
            updatedPosition = robotPosition + direction;
            dataGrid[oldIndex] = EMPTY;
            dataGrid[newIndex] = ROBOT;
        }
        else if (dataGrid[newIndex] == BOX)
        {
            //need to look along that line and move things if possible
            int count = 0;
            bool keepGoing = true;
            bool foundMove = false;
            while (keepGoing)
            {
                IntVector2 testPosition = robotPosition + (direction * count);
                int testIndex = Helper.GetIndex(testPosition, width);
                if (dataGrid[testIndex] == EMPTY)
                {
                    foundMove = true;
                    keepGoing = false;
                }
                else if (dataGrid[testIndex] == WALL)
                {
                    foundMove = false;
                    keepGoing = false;
                }
                else
                {
                    count++;
                }
            }

            // if we found we can shift things then do so.
            if (foundMove)
            {
                for (int i = count; i > 0; i--)
                {
                    IntVector2 pnext = robotPosition + (direction * i);
                    IntVector2 pcurrent = robotPosition + (direction * (i - 1));

                    int pNextIndex = Helper.GetIndex(pnext, width);
                    int pCurrentIndex = Helper.GetIndex(pcurrent, width);
                    dataGrid[pNextIndex] = BOX;
                    dataGrid[pCurrentIndex] = EMPTY;
                }

                updatedPosition = robotPosition + direction;
                dataGrid[oldIndex] = EMPTY;
                dataGrid[newIndex] = ROBOT;
            }
        }
        else // should be a wall
        {
            // do nothing
        }

        robotPosition = updatedPosition;

        return dataGrid;
    }

}