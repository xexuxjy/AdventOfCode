public class Test21 : BaseTest
{
    public override void Initialise()
    {
        TestID = 21;
        IsTestInput = false;
        IsPart2 = false;
    }

    public override void Execute()
    {
        int width = 0;
        int height = 0;
        
        char[] dataGrid = Helper.GetCharGrid(m_dataFileContents, ref width, ref height);

        IntVector2 startPosition = new IntVector2();
        for (int i = 0; i < dataGrid.Length;i++)
        {
            if (dataGrid[i] == 'S')
            {
                startPosition = new IntVector2(i % width, i / width);
                break;
            }
        }

        int numSteps = IsTestInput ? 6 : 64;
        HashSet<IntVector2> positions = new HashSet<IntVector2>();
        FollowRoute(startPosition,numSteps, dataGrid, width, height, positions);
        DebugOutput(Helper.DrawGrid(dataGrid,width,height));
            
        
        DebugOutput("Possible positions = " + positions.Count);

    }

    private Dictionary<(IntVector2, int), bool> m_exploredRoutes = new Dictionary<(IntVector2, int), bool>();

    public bool FollowRoute(IntVector2 start, int numSteps,char[] dataGrid,int width,int height,HashSet<IntVector2> positions)
    {
        IntVector2 position = start;

        bool existingRoute = false;
            
            
        
        var searchKey = (position,  numSteps);
        if (m_exploredRoutes.TryGetValue(searchKey, out existingRoute))
        {
            return existingRoute;
        }


        bool hasRoute = false;
        foreach (IntVector2 option in IntVector2.Directions)
        {
            IntVector2 newPosition = position + option;

            if (newPosition.X >= 0 && newPosition.X < width && newPosition.Y >= 0 && newPosition.Y < height)
            {

                if (dataGrid[(newPosition.Y * width) + newPosition.X] != '#')
                {
                    // final position
                    if (numSteps == 1)
                    {
                        dataGrid[(newPosition.Y * width) + newPosition.X] = 'O';
                        positions.Add(newPosition);
                    }

                    if ((numSteps - 1) > 0)
                    {
                        hasRoute |= FollowRoute(newPosition, (numSteps - 1), dataGrid, width, height, positions);
                    }
                }
            }

        }

        m_exploredRoutes[searchKey] = hasRoute;
        return hasRoute;
    }

    
}