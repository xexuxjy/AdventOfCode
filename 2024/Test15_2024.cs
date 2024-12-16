using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;


public class Test15_2024 : BaseTest
{
    public const char WALL = '#';
    public const char EMPTY = '.';
    public const char ROBOT = '@';
    public const char BOX = 'O';
    public const char BOXO = '{';
    public const char BOXC = '}';

    public override void Initialise()
    {
        Year = 2024;
        TestID = 15;
    }

    public override void Execute()
    {
        List<string> warehouse = new List<string>();
        List<WarehouseBox> allBoxes = new List<WarehouseBox>();


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

        int width = warehouse[0].Length; ;
        int height = warehouse.Count;
        int partMultiplier = IsPart2 ? 2 : 1;
        char[] dataGrid = null;

        dataGrid = new char[width * height * partMultiplier];
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                int index = (y * width * partMultiplier) + (x * partMultiplier);
                if (warehouse[y][x] == EMPTY)
                {
                    dataGrid[index] = EMPTY;
                    if (IsPart2)
                    {
                        dataGrid[index + 1] = EMPTY;
                    }

                }
                else if (warehouse[y][x] == WALL)
                {
                    dataGrid[index] = WALL;
                    if (IsPart2)
                    {
                        dataGrid[index + 1] = WALL;
                    }
                }
                else if (warehouse[y][x] == ROBOT)
                {
                    dataGrid[index] = ROBOT;
                    if (IsPart2)
                    {
                        dataGrid[index + 1] = EMPTY;
                    }
                }
                else if (warehouse[y][x] == BOX)
                {
                    if (IsPart2)
                    {
                        dataGrid[index] = '[';
                        dataGrid[index + 1] = ']';
                    }
                    else
                    {
                        dataGrid[index] = BOX;
                    }
                    WarehouseBox b = new WarehouseBox();
                    if(IsPart2)
                    {
                        b.Bounds = new IntVector2(1, 0);
                    }

                    b.Move(Helper.GetPosition(index, width * partMultiplier));
                    allBoxes.Add(b);
                }

            }
        }

        width *= partMultiplier;



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

        Dictionary<char, Spectre.Console.Color> colourMap = new Dictionary<char, Spectre.Console.Color>();
        colourMap[WALL] = Spectre.Console.Color.White;
        colourMap[BOX] = Spectre.Console.Color.Yellow;
        colourMap[BOXO] = Spectre.Console.Color.Yellow;
        colourMap[BOXC] = Spectre.Console.Color.Yellow;
        colourMap[ROBOT] = Spectre.Console.Color.Green;
        colourMap[EMPTY] = Spectre.Console.Color.Grey;

        Dictionary<char, IntVector2> commandTranslation = new Dictionary<char, IntVector2>();
        commandTranslation['^'] = IntVector2.Down;
        commandTranslation['v'] = IntVector2.Up;
        commandTranslation['<'] = IntVector2.Left;
        commandTranslation['>'] = IntVector2.Right;

        bool animate = false;
        if (animate)
        {

            Table table = new Table().Centered();

            AnsiConsole.Live(table).Start(ctx =>
            {
                if (ctx != null)
                {
                    table.AddColumn("");
                    table.AddRow("");

                    for (int i = 0; i < commandString.Length; ++i)
                    {
                        IntVector2 direction = commandTranslation[commandString[i]];
                        Simulate(dataGrid, ref robotPosition, direction, width, allBoxes);
                        UpdateDataGrid(dataGrid, allBoxes, robotPosition, width);

                        string text = Helper.DrawGrid(dataGrid, width, height, colourMap);
                        table.Columns[0].Header($"Step [[{i}]]  [[{commandString[i]}]]");
                        table.UpdateCell(0, 0, text);
                        //DebugOutput($"Step [[{i}]]  [[{commandString[i]}]]");
                        //DebugOutput(Helper.DrawGrid(dataGrid,width,height));
                        //Helper.DrawGridToConsole(dataGrid,width,height, colourMap,10);
                        ctx.Refresh();
                        Thread.Sleep(10);
                    }
                }
            });
        }
        else
        {
            for (int i = 0; i < commandString.Length; ++i)
            {
                IntVector2 direction = commandTranslation[commandString[i]];
                Simulate(dataGrid, ref robotPosition, direction, width, allBoxes);
                UpdateDataGrid(dataGrid, allBoxes, robotPosition, width);

                //DebugOutput($"Step [[{i}]]  [[{commandString[i]}]]");
                //DebugOutput(Helper.DrawGrid(dataGrid,width,height));
                //Helper.DrawGridToConsole(dataGrid,width,height, colourMap,10);
            }
        }





        long totalSum = 0;
        foreach (WarehouseBox wb in allBoxes)
        {
            totalSum += ((100 * wb.Position.Y) + wb.Position.X);
        }

        DebugOutput("GPS Total is : " + totalSum);
    }

    public void UpdateDataGrid(char[] dataGrid, List<WarehouseBox> boxes, IntVector2 robot, int width)
    {
        for (int i = 0; i < dataGrid.Length; i++)
        {
            if (dataGrid[i] != WALL)
            {
                dataGrid[i] = EMPTY;
            }
        }

        dataGrid[Helper.GetIndex(robot, width)] = ROBOT;
        foreach (WarehouseBox box in boxes)
        {
            box.DrawGrid(dataGrid, width, IsPart2);
        }

    }


    public char[] Simulate(char[] dataGrid, ref IntVector2 robotPosition, IntVector2 direction, int width, List<WarehouseBox> allBoxes)
    {
        IntVector2 originalPosition = robotPosition;
        IntVector2 updatedPosition = originalPosition + direction;

        int originalIndex = Helper.GetIndex(originalPosition, width);
        int newIndex = Helper.GetIndex(updatedPosition, width);

        if (dataGrid[newIndex] == EMPTY)
        {
            dataGrid[originalIndex] = EMPTY;
            dataGrid[newIndex] = ROBOT;
            robotPosition = updatedPosition;
        }
        else if (dataGrid[newIndex] != WALL)
        {
            // must be a box somewhere
            List<WarehouseBox> collided = allBoxes.FindAll(x => x.ContainsPoint(updatedPosition));
            Debug.Assert(collided.Count == 1);
            if (collided.Count > 0)
            {

                if (MoveBox(collided[0], direction, dataGrid, width, allBoxes))
                {
                    robotPosition = updatedPosition;
                }
            }
        }

        return dataGrid;
    }

    public bool MoveBox(WarehouseBox box, IntVector2 direction, char[] dataGrid, int gridWidth, List<WarehouseBox> allBoxes)
    {

        List<WarehouseBox> effectedBoxes = box.GetTouchingBoxesInDirection(direction, dataGrid, gridWidth, allBoxes);

        // if any of the boxes moving in direction touch a wall then it can't move.
        foreach (WarehouseBox b in effectedBoxes)
        {
            if (b.WallInDirection(direction, dataGrid, gridWidth))
            {
                return false;
            }
        }

        foreach (WarehouseBox b in effectedBoxes)
        {
            b.Move(b.Position + direction);
        }
        return true;
    }


    public class WarehouseBox
    {
        public IntVector2 Position;
        public IntVector2 Bounds;

        public IntVector2 Min;
        public IntVector2 Max;

        public List<IntVector2> AllPoints = new List<IntVector2>();

        public void Move(IntVector2 p)
        {
            Position = p;
            Min = Position;
            Max = Min + Bounds;
            AllPoints.Clear();
            for (int x = Min.X; x <= Max.X; x++)
            {
                for (int y = Min.Y; y <= Max.Y; y++)
                {
                    AllPoints.Add(new IntVector2(x, y));
                }
            }

        }

        public bool WallInDirection(IntVector2 direction, char[] dataGrid, int gridWidth)
        {
            IntVector2 possibleMove = Position + direction;

            IntVector2 updatedMin = Min + direction;
            IntVector2 updatedMax = Max + direction;

            for (int x = updatedMin.X; x <= updatedMax.X; x++)
            {
                for (int y = updatedMin.Y; y <= updatedMax.Y; y++)
                {
                    int index = Helper.GetIndex(new IntVector2(x, y), gridWidth);
                    if (dataGrid[index] == WALL)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public bool ContainsPoint(IntVector2 point)
        {
            return AllPoints.Contains(point);
        }

        public bool TestOverlap(WarehouseBox b, IntVector2 offset)
        {
            IntVector2 min = Position + offset;
            IntVector2 max = min + Bounds;

            foreach (IntVector2 iv2 in AllPoints)
            {
                if (b.AllPoints.Contains(iv2 + offset))
                {
                    return true;
                }
            }
            return false;

        }

        public List<WarehouseBox> GetTouchingBoxesInDirection(IntVector2 direction, char[] dataGrid, int gridWidth, List<WarehouseBox> allBoxes)
        {
            HashSet<WarehouseBox> boxesToCheck = new HashSet<WarehouseBox>();
            HashSet<WarehouseBox> checkedBoxes = new HashSet<WarehouseBox>();
            HashSet<WarehouseBox> newBoxes = new HashSet<WarehouseBox>();
            bool boxAdded = true;

            boxesToCheck.Add(this);

            while (boxAdded == true)
            {
                boxAdded = false;

                foreach (WarehouseBox b in boxesToCheck)
                {
                    checkedBoxes.Add(b);

                    foreach (WarehouseBox b2 in allBoxes)
                    {
                        if (!checkedBoxes.Contains(b2) && b.TestOverlap(b2, direction))
                        {
                            newBoxes.Add(b2);
                            boxAdded = true;
                        }
                    }
                }

                boxesToCheck.RemoveWhere(x => checkedBoxes.Contains(x));

                foreach (WarehouseBox b3 in newBoxes)
                {
                    boxesToCheck.Add(b3);
                }
                newBoxes.Clear();

            }

            List<WarehouseBox> resultList = new List<WarehouseBox>();
            resultList.AddRange(checkedBoxes);
            return resultList;
        }

        public void DrawGrid(char[] dataGrid, int width, bool IsPart2)
        {
            int index = Helper.GetIndex(Position, width);
            if (IsPart2)
            {
                if (dataGrid[index] == WALL || dataGrid[index + 1] == WALL)
                {
                    int ibreak = 0;
                }
                dataGrid[index] = '{';
                dataGrid[index + 1] = '}';
            }
            else
            {
                dataGrid[index] = BOX;
            }
        }
    }


}