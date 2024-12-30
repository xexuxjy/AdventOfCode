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
        TestRoute(m_startPoint, m_startPoint, m_endPoint,IntVector2.Right, 0,0, moveList);

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
        DebugOutput("Part2 Points : " + (uniquePoints.Count + 1));

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
        for(int i=0;i<points.Count-1;++i)
        {
            IntVector2 diff = points[i+1]-points[i];

            cost+=1;
            if(diff == -lastDirection)
            {
                // expensive to turn
                cost+=2000;
                lastDirection = diff;
            }
            else if(diff != lastDirection)
            {
                // expensive to turn
                cost+=1000;
                lastDirection = diff;
            }


        }
        return cost;
    }



    public const int MAX_DEPTH = 1000;
    Dictionary<Tuple<int,int,int,int,int>, bool> m_exploredRoutes = new Dictionary<Tuple<int,int,int,int,int>, bool>();
    Dictionary<Tuple<int,int,int,int,int>, long> m_routeCosts = new Dictionary<Tuple<int,int,int,int,int>, long>();
    Dictionary<IntVector3, long> m_cheapestPositionCostMap = new Dictionary<IntVector3, long>();


    List<IntVector2> m_lowestCostRoute = new List<IntVector2>();
    List<List<IntVector2>> m_allRoutes = new List<List<IntVector2>>();
    long m_lowestCost = long.MaxValue;

    private static Object lockObject = new object();

    public bool TestRoute(IntVector2 position, IntVector2 start, IntVector2 end, IntVector2 lastDirection, int depth, long cost, List<IntVector2> moveList)
    {
        Tuple<int,int,int,int,int> searchKey = new Tuple<int, int, int, int, int>(position.X,position.Y,lastDirection.X,lastDirection.Y,depth);

        //if(m_exploredRoutes.ContainsKey(searchKey))
        //{
        //    if(!m_exploredRoutes[searchKey])
        //    {
        //        return false;
        //    }
        //}

        if (!IsEmpty(position))
        {
            m_exploredRoutes[searchKey] = false;
            return false;
        }

        // stop overflow of continually staying in one place
        if (depth > MAX_DEPTH)
        {
            m_exploredRoutes[searchKey] = false;
            return false;
        }


        if (cost > m_lowestCost)
        {
            m_exploredRoutes[searchKey] = false;
            return false;
        }


        if (position == end)
        {
            if (cost < m_lowestCost)
            {
                m_lowestCost = cost;
                m_lowestCostRoute.Clear();
                m_lowestCostRoute.AddRange(moveList);

                // clear out old routes
                //m_allRoutes.RemoveAll(x => CalculateRouteCost(x) > m_lowestCost);
                m_allRoutes.Clear();
            }
            List<IntVector2> listCopy = new List<IntVector2>();
            listCopy.AddRange(moveList);
            m_allRoutes.Add(listCopy);
        
            m_exploredRoutes[searchKey] = true;
            return true;
        }
        




        List<IntVector2> moveChoices = new List<IntVector2>();
        foreach (IntVector2 option in IntVector2.Directions)
        {
            // stop going back on outselves
            if (moveList.Count == 0 || (!moveList.Contains((position + option))))
            {
                moveChoices.Add(position + option);
            }

        }

        bool hasRoute = false;
        //foreach (IntVector2 option in moveChoices.OrderBy(x => x.ManhattanDistance(m_endPoint)))
        foreach (IntVector2 option in moveChoices)
        {
            List<IntVector2> listCopy = new List<IntVector2>();
            listCopy.AddRange(moveList);

            IntVector2 newDirection = option - position;
            long newCost = cost + CalculateRouteCost(lastDirection, newDirection);

            listCopy.Add(position);
            hasRoute |= TestRoute(option, start, end, newDirection, depth + 1, newCost, listCopy);
        }

        //if(m_exploredRoutes.ContainsKey(searchKey))
        //{
        //    int ibreak = 0;
        //    if(m_exploredRoutes[searchKey] != hasRoute)
        //    {
        //        int ibreak2=0;
        //    }
        //}

        m_exploredRoutes[searchKey] = hasRoute;

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

}