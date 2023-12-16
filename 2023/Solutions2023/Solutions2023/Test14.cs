using System.Text;

public class Test14 : BaseTest
{
    public override void Initialise()
    {
        TestID = 14;
        IsTestInput = false;
        IsPart2 = true;
    }


    public override void Execute()
    {
        if (IsPart2)
        {
            Part2();
        }
        else
        {
            Part1();
        }
    }

    public void Part1()
    {
        Platform platform = new Platform(m_dataFileContents, this);
        while (platform.MoveUp()) ;
        DebugOutput("Platform load is : " + platform.CalculateLoad());
    }

    public void Part2()
    {
        long numIterations = 1000000000;

        Platform platform = new Platform(m_dataFileContents, this);
        string initialPositions = platform.GetKey();
        //DebugOutput("Initial positions : " + initialPositions);
        //platform.Print();


        Dictionary<string, long> cache = new Dictionary<string, long>();

        long periodCycle = 0;
        long periodOffset = 0;

        for (long l = 0; l < numIterations; ++l)
        {
            while (platform.MoveUp()) ;
            while (platform.MoveLeft()) ;
            while (platform.MoveDown()) ;
            while (platform.MoveRight()) ;

            //platform.Print();

            //DebugOutput($"iteration {l} load {platform.CalculateLoad()}");

            string positions = platform.GetKey();
            if (cache.ContainsKey(positions))
            {
                //DebugOutput($"Found a repeat of {positions}  old value was {cache[positions]} new value is {l} load value is {platform.CalculateLoad()}");
                if (periodCycle == 0)
                {
                    periodOffset = cache[positions];
                    periodCycle = l + 1 - cache[positions];
                }

                //platform.Print();
                break;
            }
            else
            {
                cache[positions] = l + 1;
            }
        }

        platform.Reset();
        DebugOutput("PeriodCycle is : " + periodCycle + " period offset is " + periodOffset);

        long iterDiff = (numIterations - periodOffset) % periodCycle;

        long remainingIterations = periodOffset + iterDiff;
        
        for (long l = 0; l < remainingIterations; ++l)
        {
            while (platform.MoveUp()) ;
            while (platform.MoveLeft()) ;
            while (platform.MoveDown()) ;
            while (platform.MoveRight()) ;
        }

        int cheatLoad = platform.CalculateLoad();
        DebugOutput("Part 2 Platform load is : " + cheatLoad);
    }

    public class Platform
    {
        public char[,] Grid;
        public char[,] OriginalGrid;
        public int Width;
        public int Height;

        public BaseTest m_baseTest;

        public Platform(List<string> data, BaseTest baseTest)
        {
            m_baseTest = baseTest;

            Height = data.Count;
            Width = data[0].Length;

            Grid = new char[Height, Width];
            OriginalGrid = new char[Height, Width];

            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    Grid[x, y] = data[y][x];
                    OriginalGrid[x, y] = data[y][x];
                }
            }

            int ibreak = 0;
        }

        public void Reset()
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    Grid[x, y] = OriginalGrid[x, y];
                }
            }
        }

        public bool IsEmpty(int x, int y)
        {
            return Grid[x, y] == '.';
        }

        public bool IsRoundRock(int x, int y)
        {
            return Grid[x, y] == 'O';
        }

        public bool IsFixedRock(int x, int y)
        {
            return Grid[x, y] == '#';
        }

        public void MoveRock(int fromX, int fromY, int toX, int toY)
        {
            Grid[toX, toY] = 'O';
            Grid[fromX, fromY] = '.';
        }

        public bool MoveUp()
        {
            bool moved = false;
            for (int y = 1; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    if (IsRoundRock(x, y) && IsEmpty(x, y - 1))
                    {
                        MoveRock(x, y, x, y - 1);
                        moved = true;
                    }
                }
            }

            return moved;
        }


        public bool MoveDown()
        {
            bool moved = false;

            for (int y = Height - 2; y >= 0; --y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    if (IsRoundRock(x, y) && IsEmpty(x, y + 1))
                    {
                        MoveRock(x, y, x, y + 1);
                        moved = true;
                    }
                }
            }

            return moved;
        }

        public bool MoveLeft()
        {
            bool moved = false;

            for (int y = 0; y < Height; ++y)
            {
                for (int x = 1; x < Width; ++x)
                {
                    if (IsRoundRock(x, y) && IsEmpty(x - 1, y))
                    {
                        MoveRock(x, y, x - 1, y);
                        moved = true;
                    }
                }
            }

            return moved;
        }

        public bool MoveRight()
        {
            bool moved = false;

            for (int y = 0; y < Height; ++y)
            {
                for (int x = Width - 2; x >= 0; --x)
                {
                    if (IsRoundRock(x, y) && IsEmpty(x + 1, y))
                    {
                        MoveRock(x, y, x + 1, y);
                        moved = true;
                    }
                }
            }

            return moved;
        }

        public int CalculateLoad()
        {
            int load = 0;
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    if (IsRoundRock(x, y))
                    {
                        load += Height - y;
                    }
                }
            }

            return load;
        }

        public string GetKey()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    sb.Append(Grid[x, y]);
                }
            }

            return sb.ToString();
        }

        public void Print()
        {
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < Grid.GetLength(0); ++y)
            {
                for (int x = 0; x < Grid.GetLength(1); ++x)
                {
                    sb.Append(Grid[x, y]);
                }

                sb.AppendLine();
            }

            m_baseTest.DebugOutput(sb.ToString());
        }
    }
}