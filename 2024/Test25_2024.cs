using static Test19_2024;

public class Test25_2024 : BaseTest
{

    public override void Initialise()
    {
        Year = 2024;
        TestID = 25;
    }


    public override void Execute()
    {
        List<string> lines = new List<string>();
        List<LockItem> lockItems = new List<LockItem>();

        // make parsing easies
        m_dataFileContents.Add("");
        for (int i = 0; i < m_dataFileContents.Count; i++)
        {
            string line = m_dataFileContents[i];
            if (line == "")
            {
                int width = 0;
                int height = 0;
                char[] grid = Helper.GetCharGrid(lines, ref width, ref height);
                bool[] boolGrid = new bool[grid.Length];
                for (int j = 0; j < grid.Length; ++j)
                {
                    boolGrid[j] = grid[j] == '#';
                }

                LockItem lockItem = new LockItem() { DataGrid = boolGrid, Width = width, Height = height, IsKey = grid[0] == '#' };
                lockItem.BuildHeights();
                lockItems.Add(lockItem);
                lines = new List<string>();
            }
            else
            {
                lines.Add(line);
            }

        }

        int match = 0;
        foreach (var e in lockItems.SelectMany((fst, i) => lockItems.Skip(i + 1).Select(snd => (fst, snd))))
        {
            //if (e.fst.Test(e.snd))
            if (e.fst.Test2(e.snd))
            {
                match++;
            }
        }


        DebugOutput($"There is a total of {match} matches");

        int ibreak = 0;
    }

}


public class LockItem
{
    public bool[] DataGrid;
    public int Width;
    public int Height;
    public List<int> ColumnHeights = new List<int>();
    public bool IsKey = false;

    public void BuildHeights()
    {
        for (int i = 0; i < Width;++i)
        {
            int columnHeight = 0;
            for (int j = 0; j < Height; ++j)
            {
                if (DataGrid[(j * Width) + i])
                {
                    columnHeight++;
                }
            }
            ColumnHeights.Add(columnHeight);
        }
    }

    public bool Test(LockItem lockItem)
    {

        if (IsKey == lockItem.IsKey)
        {
            return false;
        }
        for (int i = 0; i < DataGrid.Length; i++)
        {
            bool opResult = (DataGrid[i] ^ lockItem.DataGrid[i]);
            if (!opResult)
            {
                return false;
            }
        }
        return true;
    }


    public bool Test2(LockItem lockItem)
    {
        if (IsKey == lockItem.IsKey)
        {
            return false;
        }
        for(int i = 0;i<Width;++i)
        {

            if(ColumnHeights[i] + lockItem.ColumnHeights[i] > Height)
            {
                return false;
            }
        }
        return true;
    }
}

