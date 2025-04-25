using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
public class Test1_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 1;
    }
    public override void Execute()
    {
        IntVector2 startPosition = new IntVector2();
        IntVector2 currentPosition = startPosition;

        IntVector2 heading = IntVector2.Down;

        List<IntVector2> visitedLocations = new List<IntVector2>();

        IntVector2? firstDuplicate = null;

        string[] tokens = m_dataFileContents[0].Split(',',StringSplitOptions.TrimEntries);
        foreach(string token in tokens)
        {
            char direction = token[0];
            int distance = int.Parse(token.Substring(1));

            if(direction == 'L')
            {
                heading = Helper.TurnLeft(heading);
            }
            else
            {
                heading = Helper.TurnRight(heading);
            }

            if(IsPart1)
            {
                currentPosition += (heading * distance);
            }
            else
            {
                for(int i=0;i<distance;i++)
                {
                    currentPosition += heading;
                    if(visitedLocations.Contains(currentPosition))
                    {
                        firstDuplicate = currentPosition;
                        break;
                    }
                    else
                    {
                        visitedLocations.Add(currentPosition);
                    }
                }
            }
            if(firstDuplicate.HasValue)
            {
                break;
            }

        }

        if(IsPart1)
        {
            DebugOutput($"Distance to headquarters is { currentPosition.ManhattanDistance(startPosition)}");
        }
        else
        {
            DebugOutput($"Distance to first duplicate is { firstDuplicate.Value.ManhattanDistance(startPosition)}");
        }
    }
}
