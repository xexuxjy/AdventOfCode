using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

public class Test10 : BaseTest
{
    public override void Initialise()
    {
        TestID = 10;
        IsTestInput = false;
        IsPart2 = false;
    }

    public override void Execute()
    {
        IntVector2 startPosition = new IntVector2();
        for (int y = 0; y < m_dataFileContents.Count; ++y)
        {
            for (int x = 0; x < m_dataFileContents[y].Length; ++x)
            {
                if (m_dataFileContents[y][x] == 'S')
                {
                    startPosition = new IntVector2(x, y);
                    break;
                }
            }
        }

        DebugOutput("StartPosition is : " + startPosition);
        // 

        List<IntVector2>startOptions = new List<IntVector2>();
        // need to decide a start direction...
        foreach (IntVector2 pos in IntVector2.Directions)
        {
            IntVector2 p = startPosition + pos;
            if (IsPipe(p))
            {
                startOptions.Add(p);
            }
        }

        List<IntVector2> longestRoute = null;
        foreach (IntVector2 firstPipe in startOptions)
        {
            List<IntVector2> route = new List<IntVector2>();
            if (FollowPipe(firstPipe, startPosition, route))
            {
                if (longestRoute == null || longestRoute.Count < route.Count)
                {
                    longestRoute = route;
                }
                
                //DebugOutput("Found loop : " + string.Join(" ",route));
            }
        }
        
        //DebugOutput("Longest route : "+((longestRoute.Count)/2)+" : " + string.Join(" ",longestRoute)); 
        DebugOutput("Longest route : "+((longestRoute.Count)/2));
        
    }

    // follow paths, seems a bit A* / Path findery


    public bool FollowPipe(IntVector2 current, IntVector2 goal, List<IntVector2> route)
    {
        
        IntVector2 pos1 = new IntVector2();
        IntVector2 pos2 = new IntVector2();
        IntVector2 last = new IntVector2();
        
        
        route.Add(goal);
        route.Add(current);
        
        last = goal;
        
        while (current != goal)
        {
            GetConnectionPositions(current, ref pos1, ref pos2);

            if (!IsPipe(pos1) || !IsPipe(pos2))
            {
                break;
            }
            
            // which is new?
           
            if (pos1 == last)
            {
                last = current;
                current = pos2;
            }
            else
            {
                last = current;
                current = pos1;
            }


            route.Add(current);

            if (current == goal)
            {
                break;
            }
            
        }

        return route.FindAll(x=>x==goal).Count == 2;
    }
    

    public void GetConnectionPositions(IntVector2 pos, ref IntVector2 position1, ref IntVector2 position2)
    {
        switch (m_dataFileContents[pos.Y][pos.X])
        {
            case '-':
                position1 = pos + new IntVector2(-1, 0);
                position2 = pos + new IntVector2(+1, 0);
                break;
            case '|' : 
                position1 = pos + new IntVector2(-0, -1);
                position2 = pos + new IntVector2(+0, 1);
                break;
            case '7' : 
                position1 = pos + new IntVector2(-1, 0);
                position2 = pos + new IntVector2(+0, 1);
                break;
            case 'J' : 
                position1 = pos + new IntVector2(-1, 0);
                position2 = pos + new IntVector2(+0, -1);
                break;
            case 'L' : 
                position1 = pos + new IntVector2(+1, 0);
                position2 = pos + new IntVector2(+0, -1);
                break;
            case 'F' : 
                position1 = pos + new IntVector2(1, 0);
                position2 = pos + new IntVector2(+0, 1);
                break;
        }
    }

    public bool IsPipe(IntVector2 pos)
    {
        if (pos.Y >= 0 && pos.X >= 0 && pos.Y < m_dataFileContents.Count && pos.X < m_dataFileContents[0].Length)
        {
            return m_dataFileContents[pos.Y][pos.X] != '.';
        }

        return false;
    }
    public bool IsPipe(char c)
    {
        return c == 'S' || c != '.';
    }
    
}