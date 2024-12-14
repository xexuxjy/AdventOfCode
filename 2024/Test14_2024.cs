public class Test14_2024 : BaseTest
{
    public override void Initialise()
    {
        Year = 2024;
        TestID = 14;
    }

    public override void Execute()
    {
        List<IntVector2> positionList = new List<IntVector2>();
        List<IntVector2> velocityList = new List<IntVector2>();

        IntVector2 roomSize = new IntVector2();

        foreach(string line in m_dataFileContents)
        {
            string editedLine= line.Replace("p=","").Replace("v=","");
            string[] tokens = editedLine.Split(new char[] {',',' ' },StringSplitOptions.RemoveEmptyEntries);
            IntVector2 position = new IntVector2(int.Parse(tokens[0]),int.Parse( tokens[1]));
            IntVector2 velocity = new IntVector2(int.Parse(tokens[2]),int.Parse( tokens[3]));
            positionList.Add(position);
            velocityList.Add(velocity);

            roomSize.Max(position);
        }

        roomSize.X+=1;
        roomSize.Y+=1;

        char[] debugGrid = new char[(roomSize.X) * (roomSize.Y)];
        int numberOfSteps = 100;
        for(int i=0;i<numberOfSteps;++i)
        {
            for(int j=0;j<positionList.Count;++j)
            {
                IntVector2 newPosition = positionList[j]+ velocityList[j];
                newPosition = Helper.WrapMove(newPosition,roomSize);
                positionList[j] = newPosition;
            }
            //DebugGrid(debugGrid,positionList,roomSize);
        }

        int total = 0;
        IntVector2 max = new IntVector2(roomSize.X,roomSize.Y);
        IntVector2 mid = new IntVector2(max)/2;
        
        int q1 = CountAreas(positionList,new IntVector2(0,0),new IntVector2(mid.X-1,mid.Y-1));
        int q2 = CountAreas(positionList,new IntVector2(mid.X+1,0),new IntVector2(max.X-1,mid.Y-1));
        int q3 = CountAreas(positionList,new IntVector2(0,mid.Y+1),new IntVector2(mid.X-1,max.Y-1));
        int q4 = CountAreas(positionList,new IntVector2(mid.X+1,mid.Y+1),max);
        
        total = q1 * q2 * q3 * q4;

        DebugOutput("Total safety factor is : "+total);

        int ibreak = 0;

    }

    void DebugGrid(char[] debugGrid,List<IntVector2> positions,IntVector2 bounds)
    {
        for(int i=0;i<debugGrid.Length;i++)
        {
            debugGrid[i] = '.';
        }

        foreach(IntVector2 p in positions)
        {
            int index = Helper.GetIndex(p,bounds.X);
            int count = positions.FindAll(x=>x==p).Count;

            debugGrid[index] = (char)(((int)'0')+count);
        }

        DebugOutput(Helper.DrawGrid(debugGrid,bounds.X,bounds.Y));
    }


    public int CountAreas(List<IntVector2> positions,IntVector2 min,IntVector2 max)
    {
        DebugOutput($"Min {min}  Max {max}");
        int result = 0;
        foreach(IntVector2 p in positions)
        {
            if(Helper.InBounds(p,min,max))
            {
                result++; 
            }
        }
        return result;
    }
}