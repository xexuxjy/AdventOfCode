using System;
using System.Collections.Generic;

public class Test13_2016 : BaseTest, IMapDataInt
{
    public int Magic;

    public override void Initialise()
    {
        Year = 2016;
        TestID = 13;
    }


    public override void Execute()
    {
        Magic = int.Parse(m_dataFileContents[0]);
        IntVector2 startPosition = new IntVector2(1, 1);
        IntVector2 targetPosition = IsTestInput?new IntVector2(7, 4):new  IntVector2(31, 39);

        // int size = 50;
        // for (int y = 0; y < size; y++)
        // {
        //     string output = "";
        //
        //     for (int x = 0; x < size; x++)
        //     {
        //         output += IsWall(new IntVector2(x, y)) ? "#" : ".";
        //     }
        //
        //     DebugOutput(output);
        // }

        if (IsPart1)
        {
            ExecutePart1(startPosition, targetPosition);
        }
        else
        {
            Dictionary<IntVector2,int> visited = ExecutePart2(startPosition, 50);
            // for (int y = 0; y < size; y++)
            // {
            //     string output = "";
            //
            //     for (int x = 0; x < size; x++)
            //     {
            //         IntVector2 val = new IntVector2(x, y);
            //         if (visited.ContainsKey(val))
            //         {
            //             output += "V";
            //         }
            //         else if (IsWall(val))
            //         {
            //             output += "#";
            //         }
            //         else
            //         {
            //             output += ".";
            //         }
            //     }
            //
            //     DebugOutput(output);
            // }
            
            
        }


        
    }

    public void ExecutePart1(IntVector2 startPosition,IntVector2 targetPosition)
    {
        AStarInt astar = new AStarInt();
        astar.Initialize(this);
        List<IntVector2> standardRoute = new List<IntVector2>();
        astar.FindPath(startPosition, targetPosition, standardRoute);

        DebugOutput("Route length is "+(standardRoute.Count-1));

    }

    public Dictionary<IntVector2,int> ExecutePart2(IntVector2 startPosition,int  maxDistance)
    {
        Dictionary<IntVector2,int> visited = new Dictionary<IntVector2, int>();
        visited[startPosition] = 0;
        //Test(visited, startPosition, maxDistance);
        Test(visited, startPosition, 0);
        DebugOutput($"Visited {visited.Count} unique squares");
        return visited;
    }

    public void Test(Dictionary<IntVector2,int> visited, IntVector2 position, int depth)
    {
        if (depth < 50)
        {
            foreach (var direction in IntVector2.Directions)
            {
                IntVector2 targetPosition = position + direction;
                
                if(CanMove(targetPosition,targetPosition))
                {
                    int newDepth = depth + 1;
                    if (!visited.ContainsKey(targetPosition) || visited[targetPosition] > newDepth)
                    {
                        visited[targetPosition] = newDepth;
                        Test(visited, targetPosition, newDepth);
                    }
                }
            }
        }
        else
        {
            int ibreak = 0;
        }
    }

    
    
    public bool IsWall(IntVector2 location)
    {
        //Find x*x + 3*x + 2*x*y + y + y*y.
        long val = (location.X * location.X) + (location.X * 3) + (2 * location.X * location.Y) + location.Y +
                   (location.Y * location.Y);
        //Add the office designer's favorite number (your puzzle input).
        val += Magic;

        // Find the binary representation of that sum; count the number of bits that are 1.
        // If the number of bits that are 1 is even, it's an open space.
        // If the number of bits that are 1 is odd, it's a wall.

        long result = CountSetBits(val);

        return result % 2 == 1;
    }

    static long CountSetBits(long n)
    {
        long count = 0;
        while (n > 0)
        {
            count += n & 1;
            n >>= 1;
        }

        return count;
    }

    public bool CanMove(IntVector2 from, IntVector2 to)
    {
        return to.X >= 0 && to.Y >= 0 && !IsWall(to);
    }

    public IntVector2[] GetDirections()
    {
        return IntVector2.Directions;
    }

    public float DistanceToTarget(IntVector2 v, IntVector2 t)
    {
        return v.ManhattanDistance(t);
    }
}

