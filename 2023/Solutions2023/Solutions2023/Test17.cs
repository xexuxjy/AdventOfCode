using System.Diagnostics;
using System.Text;

public class Test17 : BaseTest
{
    static IntVector2[] PossibleMoves = new IntVector2[]
    {
        new IntVector2(1, 0), new IntVector2(2, 0), new IntVector2(3, 0), new IntVector2(1, 1),
        new IntVector2(2, 1), new IntVector2(3, 1)
    };

    // static IntVector2[] PossibleMoves = new IntVector2[]
    // {
    //     new IntVector2(1, 0), new IntVector2(1, 1)
    // };



    public int[] NumGrid;

    public int Width;
    public int Height;

    public override void Initialise()
    {
        TestID = 17;
        IsTestInput = true;
        IsPart2 = false;
    }

    public override void Execute()
    {
        NumGrid = Helper.GetNumGrid(m_dataFileContents, ref Width, ref Height);

        IntVector2 startPos = new IntVector2();
        IntVector2 endPos = new IntVector2(Width - 1, Height - 1);

        List<IntVector2> moveList = new List<IntVector2>();
        List<IntVector2> optionList = new List<IntVector2>();

        //optionList.Add(new IntVector2(0, 0));
        TestRoute(startPos, IntVector2.Right, 0, startPos, endPos, 0, moveList,optionList);
        //TestRoute(startPos, IntVector2.Up, 0, startPos, endPos, 0, moveList, optionList);

        DebugOutput("Smallest Heatloss is :" + m_smallestHeatLoss);

        DebugOutput("OptionList : " + string.Join(" , ", m_shortestInstructions));

        List<IntVector2> testList = new List<IntVector2>();
        testList.Add(new IntVector2(2, 0));
        testList.Add(new IntVector2(1, 1));
        testList.Add(new IntVector2(3, 1));
        testList.Add(new IntVector2(1, 0));
        testList.Add(new IntVector2(3, 0));
        testList.Add(new IntVector2(2, 1));
        testList.Add(new IntVector2(2, 0));
        testList.Add(new IntVector2(2, 1));
        testList.Add(new IntVector2(1, 0));
        testList.Add(new IntVector2(3, 1));
        testList.Add(new IntVector2(1, 0));
        testList.Add(new IntVector2(3, 0));
        testList.Add(new IntVector2(1, 1));
        testList.Add(new IntVector2(2, 1));
        testList.Add(new IntVector2(1, 0));


        List<IntVector2> trace = new List<IntVector2>();
        // DebugOutput("Calced route is : " + FollowRoute(startPos, IntVector2.Right, testList, trace));
        // DebugOutput(DrawGrid(trace));
        DebugOutput(DrawGrid(m_shortestRoute));
        // x part of vector is forward move, y part is turn 0 left,1 right
    }

    public int FollowRoute(IntVector2 start, IntVector2 direction, List<IntVector2> options, List<IntVector2> trace)
    {
        IntVector2 position = start;
        int heatLoss = 0;
        int stepCount = 0;
        foreach (IntVector2 option in options)
        {
            IntVector2 newPosition = position + (direction * option.X);
            IntVector2 newDirection = Rotate(direction, option.Y);
            for (int i = 0; i < option.X; ++i)
            {
                heatLoss += GetHeatLoss(position + (direction * (i + 1)));
                trace.Add(position + (direction * (i + 1)));
            }

            direction = newDirection;
            position = newPosition;
            DebugOutput("FollowRoute : " + position + " step count " + (stepCount++));
        }

        return heatLoss;
    }


    public string DrawGrid(List<IntVector2> moveList)
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                sb.Append(moveList.Contains(new IntVector2(x, y)) ? "*" : ".");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public bool InBounds(IntVector2 v)
    {
        return v.X >= 0 && v.Y >= 0 && v.X < m_dataFileContents[0].Length && v.Y < m_dataFileContents.Count;
    }

    public int GetHeatLoss(IntVector2 v)
    {
        return NumGrid[(v.Y * Width) + v.X];
    }


    private List<IntVector2> m_shortestRoute = new List<IntVector2>();
    private List<IntVector2> m_shortestInstructions = new List<IntVector2>();
    private int m_smallestHeatLoss = 500; //Int32.MaxValue;

    private int MAX_DEPTH = 20;//1000;

    Dictionary<(IntVector2, IntVector2, int), bool> m_exploredRoutes =
        new Dictionary<(IntVector2, IntVector2, int), bool>();

    
    public bool TestRoute(IntVector2 position, IntVector2 direction, int heatLoss, IntVector2 start, IntVector2 end,
        int depth,
        List<IntVector2> moveList, List<IntVector2> optionList)
    {
        bool existingRoute;
        var searchKey = (position, direction, heatLoss);
        if (m_exploredRoutes.TryGetValue(searchKey, out existingRoute))
        {
            return existingRoute;
        }

        if (position == end)
        {
            if (heatLoss < m_smallestHeatLoss)
            {
                m_smallestHeatLoss = heatLoss;
                m_shortestRoute.Clear();
                m_shortestRoute.AddRange(moveList);
                m_shortestInstructions.AddRange(optionList);
            }

            return true;
        }

        if (heatLoss > 150)
        {
            return false;
        }
        
        // stop overflow of continually staying in one place
        if (depth > MAX_DEPTH)
        {
            return false;
        }

        if (heatLoss >= m_smallestHeatLoss)
        {
            return false;
        }


        List<(IntVector2, IntVector2, IntVector2, int)> moveChoices =
            new List<(IntVector2, IntVector2, IntVector2, int)>();

        foreach (IntVector2 option in PossibleMoves)
        {
            IntVector2 newPosition = position + (direction * option.X);
            IntVector2 newDirection = Rotate(direction, option.Y);
            int newHeatLoss = heatLoss;
            if (InBounds(newPosition))
            {
                for (int i = 0; i < option.X; ++i)
                {
                    newHeatLoss += GetHeatLoss(position + (direction * (i + 1)));
                }

                moveChoices.Add((option, newPosition, newDirection, newHeatLoss));
            }
        }


        int count = 0;
        bool hasRoute = false;
        
        
        // try and move and turn in a direction that will get us closer.
        foreach (var option in moveChoices.OrderBy(x => x.Item2.ManhattanDistance(end)).ThenBy(x=>(x.Item2+x.Item3).ManhattanDistance(end)))
        {

            optionList.Add(option.Item1);

            IntVector2 diff = option.Item2 - position;
            int val = Math.Max(Math.Abs(diff.X), Math.Abs(diff.Y));
            for (int i = 0; i < val; i++)
            {
                moveList.Add(position + (direction * i));
            }

            if (optionList.Count == 2 && optionList.Last() == new IntVector2(2, 0))
            {
                int ibreak = 0;
            }
            
            //DebugOutput($"Position : {option.Item2} Direction {option.Item3} Choice {option.Item1}  depth {depth} heatloss {option.Item4} available options {moveChoices.Count} all choice {string.Join("  ",optionList)}");
            hasRoute |= TestRoute(option.Item2, option.Item3, option.Item4, start, end, depth + 1, moveList,
                optionList);


            for (int i = 0; i < val; ++i)
            {
                moveList.RemoveAt(moveList.Count - 1);
            }

            optionList.RemoveAt(optionList.Count - 1);

            count++;
        }


         m_exploredRoutes[searchKey] = hasRoute;


        return hasRoute;
    }

    public IntVector2 Rotate(IntVector2 dir, int left)
    {
        if (dir == IntVector2.Left && left == 0) return IntVector2.Down;
        if (dir == IntVector2.Right && left == 0) return IntVector2.Up;

        if (dir == IntVector2.Left && left == 1) return IntVector2.Up;
        if (dir == IntVector2.Right && left == 1) return IntVector2.Down;

        if (dir == IntVector2.Down && left == 0) return IntVector2.Right;
        if (dir == IntVector2.Up && left == 0) return IntVector2.Left;

        if (dir == IntVector2.Down && left == 1) return IntVector2.Left;
        if (dir == IntVector2.Up && left == 1) return IntVector2.Right;

        Debug.Assert(false);
        return new IntVector2();
    }
}