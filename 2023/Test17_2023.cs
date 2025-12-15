using System.Diagnostics;
using System.Text;

public class Test17_2023 : BaseTest
{

    public int[] NumGrid;

    public int Width;
    public int Height;

    public override void Initialise()
    {
        Year = 2023;
        TestID = 17;
        IsTestInput = false;
        IsPart2 = true;
    }

    public override void Execute()
    {
        NumGrid = Helper.GetNumGrid(m_dataFileContents, ref Width, ref Height);

        IntVector2 startPos = new IntVector2();
        IntVector2 endPos = new IntVector2(Width - 1, Height - 1);

        List<IntVector2> moveList = new List<IntVector2>();


        int min = 1;
        int max = 4;

        if (IsPart2)
        {
            min = 4;
            max = 11;

        }

        List<IntVector2> possibleMoves = new List<IntVector2>();

        for (int i = 0; i < (max - min); i++)
        {
            possibleMoves.Add(new IntVector2(min + i, 0));
            possibleMoves.Add(new IntVector2(min + i, 1));
        }

        TestRoute(startPos, IntVector2.Right, 0, startPos, endPos, 0, moveList, possibleMoves);
        TestRoute(startPos, IntVector2.Up, 0, startPos, endPos, 0, moveList, possibleMoves);


        //DebugOutput("OptionList : " + string.Join(" , ", m_shortestInstructions));

        DebugOutput(DrawGrid(m_shortestRoute));

        DebugOutput("Smallest Heatloss is :" + m_smallestHeatLoss);
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
    private int m_smallestHeatLoss = Int32.MaxValue;

    private int MAX_DEPTH = 1000;

    Dictionary<(IntVector2, IntVector2, int), bool> m_exploredRoutes =
        new Dictionary<(IntVector2, IntVector2, int), bool>();


    public bool TestRoute(IntVector2 position, IntVector2 direction, int heatLoss, IntVector2 start, IntVector2 end,
        int depth,
        List<IntVector2> moveList, List<IntVector2> possibleMoves)
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
            }

            return true;
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

        foreach (IntVector2 option in possibleMoves)
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
        foreach (var option in moveChoices.OrderBy(x => x.Item2.ManhattanDistance(end)).ThenBy(x => (x.Item2 + x.Item3).ManhattanDistance(end)))
        {
            IntVector2 diff = option.Item2 - position;
            int val = Math.Max(Math.Abs(diff.X), Math.Abs(diff.Y));
            for (int i = 0; i < val; i++)
            {
                moveList.Add(position + (direction * i));
            }

            //DebugOutput($"Position : {option.Item2} Direction {option.Item3} Choice {option.Item1}  depth {depth} heatloss {option.Item4} available options {moveChoices.Count} all choice {string.Join("  ",optionList)}");
            hasRoute |= TestRoute(option.Item2, option.Item3, option.Item4, start, end, depth + 1, moveList,
                possibleMoves);


            for (int i = 0; i < val; ++i)
            {
                moveList.RemoveAt(moveList.Count - 1);
            }


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