using System;
using System.Text.RegularExpressions;

public class Test6_2024 : BaseTest
{
    static readonly char Obstacle = '#';
    static readonly char Empty = '.';

    static int ValidMove = 0;
    static int OutOfBounds = 1;
    static int GuardLoop = 2;

    public override void Initialise()
    {
        Year = 2024;
        TestID = 6;
    }

    public override void Execute()
    {

        int width = 0;
        int height = 0;
        char[] dataGrid = Helper.GetCharGrid(m_dataFileContents,ref width,ref height);

        IntVector2 guardPosition = new IntVector2();
        IntVector2 guardDirection= new IntVector2();


        for(int i=0;i<dataGrid.Length; i++)
        {
            if(dataGrid[i] != Obstacle && dataGrid[i] != Empty)
            {
                guardDirection = Helper.DirectionFromPointer(dataGrid[i]);
                guardPosition = new IntVector2(i % width, i / width);
                dataGrid[i] = Empty;
            }
        }

        if(IsPart2)
        {
            DoPart2(guardPosition,guardDirection,dataGrid,width,height);
        }
        else
        {
            DoPart1(guardPosition,guardDirection,dataGrid,width,height);
        }


    }


    public void DoPart1(IntVector2 guardPosition,IntVector2 guardDirection,char[] dataGrid,int width,int height)
    {
        HashSet<IntVector2> visitedLocations = new HashSet<IntVector2>();
        visitedLocations.Add(guardPosition);

        while(StepSimulation(ref guardPosition,ref guardDirection,dataGrid,width,height))
        {
            visitedLocations.Add(guardPosition);
            //DebugOutput(Helper.DrawGrid(dataGrid,width,height,guardPosition,guardDirection));
        }

        DebugOutput("Guard visited : "+visitedLocations.Count);
    }

    public void DoPart2(IntVector2 guardPosition, IntVector2 guardDirection, char[] dataGrid, int width, int height)
    {
        IntVector2 positionCopy = guardPosition;
        IntVector2 directionCopy = guardDirection;

        // didn't notice that obstacles only make sense if they would have been on the original path

        HashSet<IntVector2> visitedLocations = new HashSet<IntVector2>();
        //visitedLocations.Add(guardPosition);

        while(StepSimulation(ref positionCopy,ref directionCopy,dataGrid,width,height))
        {
            visitedLocations.Add(positionCopy);
        }

        int obstacleLoops = 0;

        foreach(IntVector2 location in visitedLocations)
        {
            int obstacleIndex  = (location.Y * width)+location.X;

            positionCopy = guardPosition;
            directionCopy = guardDirection;

            char[] dataGridCopy = new char[dataGrid.Length];
            Array.Copy(dataGrid, 0, dataGridCopy, 0, dataGrid.Length);

            dataGridCopy[obstacleIndex] = Obstacle;

            
            //HashSet<(IntVector2, IntVector2)> previousPaths = new HashSet<(IntVector2, IntVector2)>();
            HashSet<IntVector4> previousPaths = new HashSet<IntVector4>();


            int stepResult = ValidMove;
            while (stepResult == ValidMove)
            {
                stepResult = StepSimulationWithHistory(ref positionCopy, ref directionCopy, dataGridCopy, width, height, previousPaths);
                previousPaths.Add(new IntVector4(positionCopy.X,positionCopy.Y,directionCopy.X,directionCopy.Y));
                    
                //DebugOutput(Helper.DrawGrid(dataGridCopy,width,height,positionCopy,guardDirection));
            }

            if (stepResult == GuardLoop)
            {
                obstacleLoops++;

            } 
        }

        DebugOutput("Obstacle loops: " + obstacleLoops);


    }



    public bool StepSimulation(ref IntVector2 guardPosition, ref IntVector2 guardDirection, char[] dataGrid, int width, int height)
    {
        IntVector2 newPosition = guardPosition + guardDirection;
        if (Helper.InBounds(newPosition, width, height))
        {
            int newIndex = (newPosition.Y * width) + newPosition.X;
            if (dataGrid[newIndex] == Obstacle)
            {
                guardDirection = Helper.TurnRight(guardDirection);
            }
            else
            {
                guardPosition = newPosition;
            }
            return true;
        }

        return false;
    }


    public int StepSimulationWithHistory(ref IntVector2 guardPosition,ref IntVector2 guardDirection,char[] dataGrid,int width,int height,HashSet<IntVector4> previous)
    {
        IntVector2 newPosition = guardPosition + guardDirection;
        if(Helper.InBounds(newPosition,width,height))
        {
            int newIndex = (newPosition.Y * width) + newPosition.X;
            if(dataGrid[newIndex] == Obstacle)
            {
                guardDirection= Helper.TurnRight(guardDirection);
            }
            else
            {
                guardPosition = newPosition;
            }

            IntVector4 lookup = new IntVector4(guardPosition.X,guardPosition.Y,guardDirection.X,guardDirection.Y);
            // if we're in the same position and same direction we're in a loop
            if(previous.Contains(lookup))
            {
                return GuardLoop;
            }


            return ValidMove;
        }

        return OutOfBounds;
    }


}