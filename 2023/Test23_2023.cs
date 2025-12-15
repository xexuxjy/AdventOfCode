using System.Numerics;

public class Test23_2023 : BaseTest
{
    public override void Initialise()
    {
        Year = 2023;
        TestID = 23;
        IsTestInput = false;
        IsPart2 = true;
    }

    public override void Execute()
    {
        int width = 0;
        int height = 0;
        char[] dataGrid = Helper.GetCharGrid(m_dataFileContents, ref width, ref height);

        int startX = m_dataFileContents[0].IndexOf('.');
        int endX = m_dataFileContents.Last().IndexOf('.');

        IntVector2 startPosition = new IntVector2(startX, 0);
        IntVector2 endtPosition = new IntVector2(endX, m_dataFileContents.Count - 1);

        int numSteps = 0;
        //HashSet<IntVector2> alreadyVisited = new HashSet<IntVector2>();
        HashSet<IntVector2> alreadyVisited = new HashSet<IntVector2>();

        if (IsPart2)
        {
            int stackSize = 1024 * 1024 * 100;
            Thread t = new Thread(new ThreadStart(() => FollowRoutePart2(startPosition, endtPosition, numSteps, dataGrid, width, height, alreadyVisited)), stackSize);
            t.Start();

            t.Join();
        }
        else
        {
            FollowRoute(startPosition, endtPosition, numSteps, dataGrid, width, height, alreadyVisited);
        }

        foreach (IntVector2 v in m_longestRoute)
        {
            dataGrid[(v.Y * width) + v.X] = 'O';
        }

        DebugOutput(Helper.DrawGrid(dataGrid, width, height));

        DebugOutput("Found longest route with : " + m_longestRoute.Count + "  elements ");

    }

    public HashSet<IntVector2> m_longestRoute = new HashSet<IntVector2>();

    private Dictionary<(IntVector2, int, int), bool> m_exploredRoutes = new Dictionary<(IntVector2, int, int), bool>();

    public bool FollowRoute(IntVector2 start, IntVector2 end, int numSteps, char[] dataGrid, int width, int height, HashSet<IntVector2> alreadyVisited)
    {
        IntVector2 position = start;

        bool existingRoute = false;

        var searchKey = (position, numSteps, m_longestRoute.Count);

        if (m_exploredRoutes.TryGetValue(searchKey, out existingRoute))
        {
            //return existingRoute;
        }

        if (start == end)
        {
            if (alreadyVisited.Count > m_longestRoute.Count)
            {
                m_longestRoute.Clear();
                foreach (IntVector2 v in alreadyVisited)
                {
                    m_longestRoute.Add(v);
                }
            }

            return true;
        }

        alreadyVisited.Add(start);


        char currentGridChar = dataGrid[(start.Y * width) + start.X];


        bool hasRoute = false;
        if (currentGridChar == '>')
        {
            hasRoute |= FollowRoute(start + IntVector2.Right, end, (numSteps + 1), dataGrid, width, height, alreadyVisited);
        }
        else if (currentGridChar == '<')
        {
            hasRoute |= FollowRoute(start + IntVector2.Left, end, (numSteps + 1), dataGrid, width, height, alreadyVisited);
        }
        else if (currentGridChar == '^')
        {
            hasRoute |= FollowRoute(start + IntVector2.Down, end, (numSteps + 1), dataGrid, width, height, alreadyVisited);
        }
        else if (currentGridChar == 'v')
        {
            hasRoute |= FollowRoute(start + IntVector2.Up, end, (numSteps + 1), dataGrid, width, height, alreadyVisited);
        }
        else if (currentGridChar == '.')
        {
            foreach (IntVector2 option in IntVector2.Directions)
            {
                IntVector2 newPosition = position + option;
                if (newPosition.X >= 0 && newPosition.X < width && newPosition.Y >= 0 && newPosition.Y < height)
                {

                    char newGridChar = dataGrid[(newPosition.Y * width) + newPosition.X];
                    if (!alreadyVisited.Contains(newPosition) &&
                        dataGrid[(newPosition.Y * width) + newPosition.X] != '#')
                    {
                        // can't walk up slope and be shifted down again.
                        if ((option == IntVector2.Down && newGridChar == 'v') ||
                            (option == IntVector2.Up && newGridChar == '^') ||
                            (option == IntVector2.Right && newGridChar == '<') ||
                            (option == IntVector2.Left && newGridChar == '>'))
                        {
                            continue;
                        }


                        hasRoute |= FollowRoute(newPosition, end, (numSteps + 1), dataGrid, width, height,
                            alreadyVisited);
                    }
                }
            }
        }

        alreadyVisited.Remove(start);

        m_exploredRoutes[searchKey] = hasRoute;
        return hasRoute;
    }


    public bool FollowRoutePart2(IntVector2 start, IntVector2 end, int numSteps, char[] dataGrid, int width, int height, HashSet<IntVector2> alreadyVisited)
    {
        IntVector2 position = start;

        // bool existingRoute = false;

        // var searchKey = (position,  numSteps,m_longestRoute.Count);
        //
        //
        // if (m_exploredRoutes.TryGetValue(searchKey, out existingRoute))
        // {
        //     //return existingRoute;
        // }

        if (start == end)
        {
            if (alreadyVisited.Count > m_longestRoute.Count)
            {
                m_longestRoute.Clear();
                foreach (IntVector2 v in alreadyVisited)
                {
                    m_longestRoute.Add(v);
                }
            }

            return true;
        }

        alreadyVisited.Add(start);


        bool hasRoute = false;

        foreach (IntVector2 option in IntVector2.Directions)
        {
            IntVector2 newPosition = position + option;
            if (newPosition.X >= 0 && newPosition.X < width && newPosition.Y >= 0 && newPosition.Y < height)
            {
                char newGridChar = dataGrid[(newPosition.Y * width) + newPosition.X];
                if (newGridChar != '#' && !alreadyVisited.Contains(newPosition))
                {
                    hasRoute |= FollowRoutePart2(newPosition, end, (numSteps + 1), dataGrid, width, height,
                        alreadyVisited);
                }
            }
        }

        alreadyVisited.Remove(start);

        // m_exploredRoutes[searchKey] = hasRoute;
        return hasRoute;
    }


}