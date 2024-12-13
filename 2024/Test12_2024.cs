using System.Linq;
using static Test12_2024;

public class Test12_2024 : BaseTest
{
    public override void Initialise()
    {
        Year = 2024;
        TestID = 12;
    }

    public override void Execute()
    {
        int width = 0;
        int height = 0;
        char[] originalGarden = Helper.GetCharGrid(m_dataFileContents, ref width, ref height);
        char[] garden = Helper.GetCharGrid(m_dataFileContents, ref width, ref height);

        List<List<IntVector2>> allPlots = new List<List<IntVector2>>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                IntVector2 startPos = new IntVector2(x, y);
                if (garden[Helper.GetIndex(startPos, width)] != ' ')
                {
                    List<IntVector2> currentPlot = new List<IntVector2>();
                    allPlots.Add(currentPlot);
                    BuildPlot(startPos, garden, width, height, currentPlot);
                    foreach (IntVector2 iv2 in currentPlot)
                    {
                        garden[Helper.GetIndex(iv2, width)] = ' ';
                    }
                }

            }
        }

        long totalPrice = 0;
        foreach (var plot in allPlots)
        {
            long perimeter = CalcPerimeter(plot);
            long numberOfSides = GetNumberOfSides(plot);
            DebugOutput($"Plot area {plot.Count}  perimeter {perimeter} side {numberOfSides}");


            totalPrice += (plot.Count * (IsPart2?numberOfSides:perimeter));

            int ibreak = 0;
        }

        DebugOutput("Total price is : " + totalPrice);

    }

    public int CalcPerimeter(List<IntVector2> points)
    {
        int total = 0;
        foreach (IntVector2 point in points)
        {
            int neighbours = CountNeighbours(point, points);
            total += neighbours;
        }
        return total;
    }

    public int CountNeighbours(IntVector2 point, List<IntVector2> points)
    {
        int neighbours = 0;
        foreach (IntVector2 direction in IntVector2.Directions)
        {
            if (points.Contains(point + direction))
            {
                neighbours++;
            }

        }
        return 4 - neighbours;
    }

    public enum Side
    {
        Up = 1,
        Left = 2,
        Right = 4,
        Down = 8
    };


    public Dictionary<IntVector2, int> GetNeighboutMaskMap(List<IntVector2> points)
    {
        Dictionary<IntVector2, int> result = new Dictionary<IntVector2, int>();

        foreach (IntVector2 point in points)
        {
            int neighbourMask = 0;
            if (!points.Contains(point + IntVector2.Left))
            {
                neighbourMask |= (int)Side.Left;
            }
            if (!points.Contains(point + IntVector2.Up))
            {
                neighbourMask |= (int)Side.Up;
            }
            if (!points.Contains(point + IntVector2.Right))
            {
                neighbourMask |= (int)Side.Right;
            }
            if (!points.Contains(point + IntVector2.Down))
            {
                neighbourMask |= (int)Side.Down;
            }

            result[point] = neighbourMask;
        }
        return result;
    }


    // remove inside squares.
    // start following a direction,until it ends then change, continue, every change in direction is a side?
    // e shapes though
    // if a point ranges from it's actual vale to +1 then you would 

    public int GetNumberOfSides(List<IntVector2> points)
    {
        Dictionary<IntVector2, int> neighbourMaskMap = GetNeighboutMaskMap(points);

        HashSet<(int, IntVector2, IntVector2)> minMaxPairs = new HashSet<(int, IntVector2, IntVector2)>();


        foreach (IntVector2 point in points)
        {
            IntVector2 min = point;
            IntVector2 max = point;

            foreach (Side side in Enum.GetValues(typeof(Side)))
            {
                min = point;
                max = point;
                if (TestPoint(point, neighbourMaskMap, side, ref min, ref max))
                {
                    minMaxPairs.Add(((int)side, min, max));
                }
            }
        }


        return minMaxPairs.Count;
    }

    public bool TestPoint(IntVector2 point, Dictionary<IntVector2, int> neighbourMaskMap, Side direction, ref IntVector2 min, ref IntVector2 max)
    {
        if ((neighbourMaskMap[point] & (int)direction) == 0)
        {
            return false;
        }


        IntVector2 current = point;
        bool keepGoing = true;
        IntVector2 offset = (direction == Side.Up || direction == Side.Down) ? IntVector2.Left : IntVector2.Down;

        while (keepGoing)
        {
            IntVector2 np = current + offset;
            if (neighbourMaskMap.Keys.Contains(np) && ((neighbourMaskMap[np] & (int)direction) != 0))
            {
                min = np;
                current = np;
            }
            else
            {
                keepGoing = false;
            }
        }

        keepGoing = true;
        offset *= -1;
        while (keepGoing)
        {
            IntVector2 np = current + offset;
            if (neighbourMaskMap.Keys.Contains(np) && ((neighbourMaskMap[np] & (int)direction) != 0))
            {
                max = np;
                current = np;
            }
            else
            {
                keepGoing = false;
            }
        }
        return true;
    }



    public void BuildPlot(IntVector2 position, char[] garden, int width, int height, List<IntVector2> currentPlot)
    {
        currentPlot.Add(position);
        foreach (IntVector2 direction in IntVector2.Directions)
        {
            IntVector2 newPosition = position + direction;
            if (Helper.InBounds(newPosition, width, height))
            {
                if (garden[Helper.GetIndex(newPosition, width)] == garden[Helper.GetIndex(position, width)] && !currentPlot.Contains(newPosition))
                {
                    BuildPlot(newPosition, garden, width, height, currentPlot);
                }
            }
        }

    }

}