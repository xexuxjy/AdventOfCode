using Microsoft.VisualBasic;
using Spectre.Console;
using System.Numerics;
using System.Xml.Linq;

public class Test16_2024 : BaseTest
{
    public const char EMPTY = '.';
    public const char WALL = '#';

    private IntVector2 m_startPoint;
    private IntVector2 m_endPoint;
    private char[] m_dataGrid;
    int m_width;
    int m_height;

    public override void Initialise()
    {
        Year = 2024;
        TestID = 16;
    }



    public override void Execute()
    {
        m_width = 0;
        m_height = 0;
        m_dataGrid = Helper.GetCharGrid(m_dataFileContents, ref m_width, ref m_height);

        m_startPoint = Helper.GetPosition(Array.IndexOf(m_dataGrid, 'S'), m_width);
        m_endPoint = Helper.GetPosition(Array.IndexOf(m_dataGrid, 'E'), m_width);

        List<IntVector2> moveList = new List<IntVector2>();
        //TestRoute(m_startPoint, m_startPoint, m_endPoint, IntVector2.Right, 0, 0, moveList);

        TestRoute3(m_startPoint, m_startPoint, m_endPoint, IntVector2.Right, 0, moveList);

        foreach (IntVector2 move in m_lowestCostRoute)
        {
            m_dataGrid[Helper.GetIndex(move, m_width)] = '*';
        }




        //m_lowestCost += 1;

        HashSet<IntVector2> uniquePoints = new HashSet<IntVector2>();
        foreach (List<IntVector2> route in m_allRoutes)
        {
            foreach (IntVector2 move in route)
            {
                uniquePoints.Add(move);

            }
        }
        DebugOutput(Helper.DrawGrid(m_dataGrid, m_width, m_height));
        DebugOutput("Found lowest cost as : " + m_lowestCost);
        DebugOutput("Part2 Points : " + uniquePoints.Count);

    }

    public long CalculateRouteCost(IntVector2 lastDirection, IntVector2 newDirection)
    {
        long cost = 0;

        cost += 1;
        if (newDirection == -lastDirection)
        {
            // expensive to turn
            cost += 2000;
        }
        else if (newDirection != lastDirection)
        {
            // expensive to turn
            cost += 1000;
        }

        return cost;
    }

    public long CalculateRouteCostList(List<IntVector2> points)
    {
        long cost = 0;


        // fix amount for first square, initially starting east.
        IntVector2 lastDirection = IntVector2.Right;
        for (int i = 0; i < points.Count - 1; ++i)
        {
            IntVector2 diff = points[i + 1] - points[i];

            cost += 1;
            if (diff == -lastDirection)
            {
                // expensive to turn
                cost += 2000;
                lastDirection = diff;
            }
            else if (diff != lastDirection)
            {
                // expensive to turn
                cost += 1000;
                lastDirection = diff;
            }


        }
        return cost;
    }

    public const int MAX_DEPTH = 1000;
    Dictionary<IntVector4, bool> m_exploredRoutes = new Dictionary<IntVector4, bool>();
    Dictionary<IntVector2, long> m_cheapestPositionCostMap = new Dictionary<IntVector2, long>();


    List<IntVector2> m_lowestCostRoute = new List<IntVector2>();
    List<List<IntVector2>> m_allRoutes = new List<List<IntVector2>>();
    long m_lowestCost = long.MaxValue;

    public bool TestRoute(IntVector2 position, IntVector2 start, IntVector2 end, IntVector2 lastDirection, int depth, long cost, List<IntVector2> moveList)
    {
        bool existingRoute;

        IntVector2 searchKey = new IntVector2(position.X, position.Y);
        IntVector4 searchKey2 = new IntVector4(position.X, position.Y, lastDirection.X, lastDirection.Y);


        if (!IsEmpty(position))
        {
            return false;
        }

        // stop overflow of continually staying in one place
        if (depth > MAX_DEPTH)
        {
            return false;
        }

        if (cost > m_lowestCost)
        {
            return false;
        }

        if (m_cheapestPositionCostMap.TryGetValue(searchKey, out long currentCost))
        {
            //if (currentCost < cost)
            if (cost > currentCost)
            {
                return false;
            }
        }

        m_cheapestPositionCostMap[searchKey] = cost;

        if (m_exploredRoutes.TryGetValue(searchKey2, out existingRoute))
        {
            //return existingRoute; 
        }



        if (position == end)
        {
            if (cost < m_lowestCost)
            {
                m_lowestCost = cost;
                m_lowestCostRoute.Clear();
                m_lowestCostRoute.AddRange(moveList);

                // clear out old routes
                m_allRoutes.Clear();
            }
            if (cost == m_lowestCost)
            {
                List<IntVector2> listCopy = new List<IntVector2>();
                listCopy.AddRange(moveList);
                m_allRoutes.Add(listCopy);
            }

            //DebugOutput("End : "+string.Join("   ", moveList));
            return true;
        }


        // try keep going ahead, no backtracking.
        List<IntVector2> moveChoices = new List<IntVector2>();
        moveChoices.Add(lastDirection);
        moveChoices.Add(Helper.TurnLeft(lastDirection));
        moveChoices.Add(Helper.TurnRight(lastDirection));

        //foreach (IntVector2 option in IntVector2.Directions)
        //{
        //    // stop going back on outselves
        //    //if (moveList.Count == 0 || (!moveList.Contains((position + option))))
        //    {
        //        moveChoices.Add(position + option);
        //    }

        //}

        bool hasRoute = false;
        foreach (IntVector2 option in moveChoices)
        {
            //List<IntVector2> listCopy = new List<IntVector2>();
            //listCopy.AddRange(moveList);

            long newCost = cost + CalculateRouteCost(lastDirection, option);

            //listCopy.Add(position);
            moveList.Add(position);
            hasRoute |= TestRoute(position + option, start, end, option, depth + 1, newCost, moveList);
            moveList.RemoveAt(moveList.Count - 1);
            if (hasRoute)
            {
                //break;
            }
        }


        m_exploredRoutes[searchKey2] = hasRoute;

        return hasRoute;
    }

    public bool IsEmpty(IntVector2 pos)
    {
        if (!Helper.InBounds(pos, m_width, m_height))
        {
            return false;
        }
        return m_dataGrid[Helper.GetIndex(pos, m_width)] != WALL;
    }

    // Thanks to help from https://github.com/GoldenQubicle/AdventOfCode/blob/master/AoC2024/Day16.cs  though I shouldn't have been so dim...

    Dictionary<IntVector2,long> CostDictionary = new Dictionary<IntVector2,long>();

    public void TestRoute3(IntVector2 position, IntVector2 start, IntVector2 end, IntVector2 lastDirection, int depth, List<IntVector2> moveList)
    {
        var queue = new PriorityQueue<GridState, long>();
        var atExit = false;
        var maxDistance = start.ManhattanDistance(end);
        var seats = new HashSet<(int, int)>();
        var minPathLength = int.MaxValue;

        queue.Enqueue(new(start, lastDirection, 0, new()), maxDistance);

        while (queue.TryDequeue(out var state, out _))
        {
            var (current, dir, cost, seen) = state;
            seen.Add(current);

            List<(IntVector2, IntVector2, long)> moveChoices = new List<(IntVector2, IntVector2, long)>();
            foreach (IntVector2 choice in IntVector2.Directions)
            {
                IntVector2 newPos = current + choice;
                if (IsEmpty(newPos) && !seen.Contains(newPos))
                {
                    moveChoices.Add((newPos, choice, cost));
                }
            }

            if (moveChoices.Count == 0)
            {
                continue; // dead end
            }

            //as long as there's only 1 option in the same direction just keep moving
            //could optimize this more by also taking direction switches into account
            //however for some reason it doesn't work as expected and cannot bother to find out why
            while (moveChoices.Count == 1 && moveChoices[0].Item2 == dir)
            {
                moveChoices[0] = (moveChoices[0].Item1, moveChoices[0].Item2, cost + 1);

                current = moveChoices[0].Item1;
                cost = moveChoices[0].Item3;
                seen.Add(current);

                if (current == end) // we made it out
                {
                    atExit = true;
                    break;
                }

                moveChoices.Clear();
                foreach (IntVector2 choice in IntVector2.Directions)
                {
                    IntVector2 newPos = current + choice;
                    if (IsEmpty(newPos) && !seen.Contains(newPos))
                    {
                        moveChoices.Add((newPos, choice, cost));
                    }            
                }
            }


            if (atExit)
			{
				//for part 2 we're only interested in the BEST paths
				//annoyingly there are paths with the same cost but MORE cells visited than the best paths
				//consequently we check on cost & path length, and clear the seats if a better minimum is found for either
				if (cost < m_lowestCost)
				{
					m_lowestCost = cost;
					seats.Clear( );
				}

				if (cost == m_lowestCost && seen.Count < minPathLength)
				{
					minPathLength = seen.Count;
					seats.Clear( );
				}

				if (cost == m_lowestCost && seen.Count == minPathLength)
                {
                    m_allRoutes.Add(seen.ToList());
					//seats.AddRange(seen.Select(c => c.Position));
                }

				atExit = false;
				continue;
			}

            moveChoices.ForEach(o =>
			{
				var newDir = o.Item2;
				var newCost = newDir == dir ? cost + 1 : cost + 1001;

                if(CostDictionary.TryGetValue(o.Item1,out long existingCost))
                {
                    if(newCost > existingCost)
                    {
                        return;
                    }
                }

                CostDictionary[o.Item1] = newCost;

				queue.Enqueue(new(o.Item1, newDir, newCost,[.. seen]),newCost - maxDistance - o.Item1.ManhattanDistance(end)); //prioritize cost and moving towards the end. 
			});

        }
    }


}

public record GridState(IntVector2 Current, IntVector2 Direction, long cost, HashSet<IntVector2> Seen);
