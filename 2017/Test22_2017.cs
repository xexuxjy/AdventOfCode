using System;
using System.Collections.Generic;

public class Test22_2017 : BaseTest
{
    Dictionary<IntVector2,int> GridInformation = new Dictionary<IntVector2,int>();
    public IntVector2 CleanerPosition;
    public IntVector2 CleanerDirection;

    public long BurstInfectionCount = 0;

    public static int CLEAN = 0;
    public static int INFECTED = 1;
    public static int WEAKENED = 2;
    public static int FLAGGED = 3;
    
    public override void Initialise()
    {
        Year = 2017;
        TestID = 22;
    }

    public override void Execute()
    {
        CleanerPosition = new IntVector2(0,0);
        CleanerDirection = IntVector2.Down;

        int x = 0;
        int y = 0;
        
        IntVector2 offset = -new IntVector2(m_dataFileContents.Count/2, m_dataFileContents.Count/2);
        
        foreach (string line in m_dataFileContents)
        {
            x = 0;
            foreach (char c in line)
            {
                GridInformation[offset+new IntVector2(x, y)] = (c=='#')?1:0;
                x++;
            }

            y++;
        }

        int numIterations = IsPart1?10000:10000000;
        for (int i = 0; i < numIterations; i++)
        {
            if (IsPart1)
            {
                CleanerWorkBurstPart1();
            }
            else
            {
                CleanerWorkBurstPart2();
            }
        }
        DebugOutput($"There were {BurstInfectionCount} infections");
    }

    public bool Infected(IntVector2 position)
    {
        if (GridInformation.ContainsKey(position))
        {
            return GridInformation[position] == INFECTED;;
        }

        return false;
    }
    
    public void CleanerWorkBurstPart1()
    {
        if (Infected(CleanerPosition))
        {
            CleanerDirection = Helper.TurnRight(CleanerDirection);
            GridInformation[CleanerPosition] = CLEAN;
        }
        else
        {
            BurstInfectionCount++;
            GridInformation[CleanerPosition] = INFECTED;
            CleanerDirection = Helper.TurnLeft(CleanerDirection);
        }

        CleanerPosition += CleanerDirection;
    }

    /*
As you go to remove the virus from the infected nodes, it evolves to resist your attempt.

Now, before it infects a clean node, it will weaken it to disable your defenses. If it encounters an infected node, it will instead flag the node to be cleaned in the future. So:

    Clean nodes become weakened.
    Weakened nodes become infected.
    Infected nodes become flagged.
    Flagged nodes become clean.

Every node is always in exactly one of the above states.

The     
     * Decide which way to turn based on the current node:

    If it is clean, it turns left.
    If it is weakened, it does not turn, and will continue moving in the same direction.
    If it is infected, it turns right.
    If it is flagged, it reverses direction, and will go back the way it came.

     */
    
    public void CleanerWorkBurstPart2()
    {

        if (!GridInformation.ContainsKey(CleanerPosition)||  GridInformation[CleanerPosition] == CLEAN )
        {
            GridInformation[CleanerPosition] = WEAKENED;
            CleanerDirection = Helper.TurnLeft(CleanerDirection);
        }
        else if (GridInformation[CleanerPosition] == WEAKENED)
        {
            BurstInfectionCount++;
            GridInformation[CleanerPosition] = INFECTED;
        }
        else if (GridInformation[CleanerPosition] == INFECTED)
        {
            GridInformation[CleanerPosition] = FLAGGED;
            CleanerDirection = Helper.TurnRight(CleanerDirection);
        }
        else if (GridInformation[CleanerPosition] == FLAGGED)
        {
            GridInformation[CleanerPosition] = CLEAN;
            CleanerDirection *= -1;
        }

        CleanerPosition += CleanerDirection;
    }

    
}