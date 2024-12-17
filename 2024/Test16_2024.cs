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
        TestRoute(m_startPoint, m_startPoint,m_endPoint,0, moveList);

        foreach(IntVector2 move in m_lowestCostRoute)
        {
            m_dataGrid[Helper.GetIndex(move,m_width)] = '*';
        }


        m_lowestCost+=1;
        DebugOutput(Helper.DrawGrid(m_dataGrid, m_width, m_height));
        DebugOutput("Found lowest cost as : " + m_lowestCost);
    }

    public long CalculateRouteCost(List<IntVector2> points)
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
    Dictionary<IntVector2,bool> m_exploredRoutes = new Dictionary<IntVector2, bool>();
    Dictionary<IntVector2,long> m_cheapestPositionCostMap = new Dictionary<IntVector2, long>();


    List<IntVector2> m_lowestCostRoute = new List<IntVector2>();
    long m_lowestCost = long.MaxValue;

    public bool TestRoute(IntVector2 position, IntVector2 start, IntVector2 end, int depth, List<IntVector2> moveList)
    {
        bool existingRoute;

        if (!IsEmpty(position))
        {
            return false;
        }

        // stop overflow of continually staying in one place
        if (depth > MAX_DEPTH)
        {
            return false;
        }


        long cost = CalculateRouteCost(moveList);
        IntVector2 searchKey = new IntVector2(position.X, position.Y);

        if (!m_cheapestPositionCostMap.ContainsKey(searchKey))
        {
            m_cheapestPositionCostMap[searchKey] = cost;
        }

        if(cost > m_cheapestPositionCostMap[searchKey])
        {
            return false;
        }

        m_cheapestPositionCostMap[searchKey] = cost;


        if (position == end)
        {
            if(cost < m_lowestCost)
            {
                m_lowestCost = cost;
                m_lowestCostRoute.Clear();
                m_lowestCostRoute.AddRange(moveList);
            }
            //DebugOutput("End : "+string.Join("   ", moveList));
            return true;
        }


        List<IntVector2> moveChoices = new List<IntVector2>();
        foreach (IntVector2 option in IntVector2.Directions)
        {
            moveChoices.Add(position + option);
        }

        bool hasRoute = false;
        
        foreach (IntVector2 option in moveChoices.OrderBy(x => x.ManhattanDistance(m_endPoint)))
        {
            moveList.Add(position);
            hasRoute |= TestRoute(option, start, end, depth + 1, moveList);
            moveList.RemoveAt(moveList.Count - 1);
        }

        m_exploredRoutes[searchKey] = hasRoute;

        return hasRoute;
    }

    public bool IsEmpty(IntVector2 pos)
    {
        if (!Helper.InBounds(pos, m_width, m_height))
        {
            return false;
        }
        return m_dataGrid[Helper.GetIndex(pos,m_width)] != WALL;
    }

}