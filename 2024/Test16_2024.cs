using System.Numerics;

public class Test16_2024 : BaseTest, IMapData
{
    public const char EMPTY = '.';
    public const char WALL= '#';

    private Vector2 m_startPoint;
    private Vector2 m_endPoint;
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
        m_dataGrid = Helper.GetCharGrid(m_dataFileContents,ref m_width,ref m_height);

        IntVector2 startPosition = Helper.GetPosition(Array.IndexOf(m_dataGrid,'S'),m_width);
        IntVector2 endPosition = Helper.GetPosition(Array.IndexOf(m_dataGrid,'E'),m_width);

        m_startPoint = new Vector2(startPosition.X,startPosition.Y);
        m_endPoint = new Vector2(endPosition.X,endPosition.Y);

        AStar aStar = new AStar(SearchMethod.BreadthFirst);
        
        aStar.Initialize(this);
        List<Vector2> results = new List<Vector2>();
        if (aStar.FindPath(m_startPoint, m_endPoint, results))
        {
            int ibreak = 0;
            DebugOutput("Found a path of length : "+results.Count);
        }

        foreach(Vector2 point in results)
        {
            int index = Helper.GetIndex((int)point.X,(int)point.Y,m_width);
            m_dataGrid[index] = '*';
        }

        DebugOutput(Helper.DrawGrid(m_dataGrid,m_width,m_height));
    }

    public bool CanMove(Vector2 from, Vector2 to)
    {
        IntVector2 iv2 = new IntVector2((int)to.X,(int)to.Y);
        int index = Helper.GetIndex(iv2,m_width);
        return Helper.InBounds(iv2,m_width,m_height) && m_dataGrid[index] != WALL;

    }

    public Vector2[] GetDirections()
    {
        return AStar.BasicDirections;
    }

    public Vector2 GetTargetPosition()
    {
        return m_endPoint;
    }

    public float DistanceToTarget(Vector2 v)
    {
        return Math.Abs(m_endPoint.X - v.X) + Math.Abs(m_endPoint.Y - v.Y);
    }
}