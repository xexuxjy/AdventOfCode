using System.Diagnostics;
using System.Text;

public class Test16 : BaseTest
{
    public override void Initialise()
    {
        TestID = 16;
        IsTestInput = false;
        IsPart2 = true;
    }

    public int m_width;
    public int m_height;

    public override void Execute()
    {
        int highestScore = 0;
        int width = m_dataFileContents[0].Length;
        int height = m_dataFileContents.Count;
        m_width = width;
        m_height = height;

        List<Tuple<IntVector2, IntVector2>> startPositions = new List<Tuple<IntVector2, IntVector2>>();
        if (IsPart2)
        {
            for (int i = 0; i < width; ++i)
            {
                startPositions.Add(new Tuple<IntVector2, IntVector2>(new IntVector2(i, 0), IntVector2.Up));
                startPositions.Add(
                    new Tuple<IntVector2, IntVector2>(new IntVector2(i, height - 1), IntVector2.Down));
            }

            for (int i = 0; i < height; ++i)
            {
                startPositions.Add(new Tuple<IntVector2, IntVector2>(new IntVector2(0, i), IntVector2.Right));
                startPositions.Add(
                    new Tuple<IntVector2, IntVector2>(new IntVector2(width - 1, i), IntVector2.Left));
            }
        }
        else
        {
            startPositions.Add(new Tuple<IntVector2, IntVector2>(new IntVector2(0, 0), IntVector2.Right));
        }

        foreach (var pair in startPositions)
        {
            int score = GetScore(pair.Item1, pair.Item2);
            if (score > highestScore)
            {
                highestScore = score;
            }
        }


        DebugOutput("Number of cells energized is : " + highestScore);
    }

    public int GetScore(IntVector2 initialPosition, IntVector2 initialDirection)
    {
        List<Beam> beams = new List<Beam>();
        List<Beam> newBeams = new List<Beam>();

        Beam startBeam = new Beam(initialPosition, initialDirection);
        beams.Add(startBeam);

        int width = m_dataFileContents[0].Length;
        int height = m_dataFileContents.Count;

        Dictionary<Tuple<IntVector2, IntVector2>, bool> visited = new Dictionary<Tuple<IntVector2, IntVector2>, bool>();
        bool[] energised = new bool[width * height];

        int lastCount = 0;


        while (true)
        {
            bool noBeamsActive = true;
            foreach (Beam b in beams)
            {
                if (!b.StillActive)
                {
                    continue;
                }

                noBeamsActive = false;

                energised[(b.CurrentPosition.Y * width) + b.CurrentPosition.X] = true;
                Tuple<IntVector2, IntVector2> t =
                    new Tuple<IntVector2, IntVector2>(b.CurrentPosition, b.CurrentDirection);

                if (visited.TryGetValue(t, out bool a))
                {
                    b.Stop();
                }
                else
                {
                    visited[t] = true;

                    char currentChar = GetContents(b.CurrentPosition);
                    if (currentChar == '/' || currentChar == '\\')
                    {
                        b.CurrentDirection = Rotate(b.CurrentDirection, currentChar);
                    }
                    else if (currentChar == '|')
                    {
                        if (b.CurrentDirection == IntVector2.Left || b.CurrentDirection == IntVector2.Right)
                        {
                            b.CurrentDirection = IntVector2.Up;
                            // need to split.

                            // add beams below but only if it makes sense
                            if (InBounds(b.CurrentPosition + IntVector2.Down))
                            {
                                newBeams.Add(new Beam(b.CurrentPosition, IntVector2.Down));
                            }
                        }
                    }
                    else if (currentChar == '-')
                    {
                        if (b.CurrentDirection == IntVector2.Up || b.CurrentDirection == IntVector2.Down)
                        {
                            b.CurrentDirection = IntVector2.Right;
                            // need to split.
                            // add beams left but only if it makes sense
                            if (InBounds(b.CurrentPosition + IntVector2.Left))
                            {
                                newBeams.Add(new Beam(b.CurrentPosition, IntVector2.Left));
                            }
                        }
                    }

                    IntVector2 nextPos = b.CurrentPosition + b.CurrentDirection;
                    if (InBounds(nextPos))
                    {
                        b.Move();
                    }
                    else
                    {
                        b.Stop();
                    }

                    energised[(b.CurrentPosition.Y * width) + b.CurrentPosition.X] = true;
                }
            }


            beams.AddRange(newBeams);
            newBeams.Clear();

            if (noBeamsActive)
            {
                break;
            }
        }

        //DebugOutput(DebugGrid(energised, m_width, m_height));
        lastCount = energised.Count(x => x == true);

        return lastCount;
    }


    public string DebugGrid(HashSet<IntVector2> positions)
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < m_dataFileContents.Count; ++y)
        {
            for (int x = 0; x < m_dataFileContents[0].Length; ++x)
            {
                sb.Append(positions.Contains(new IntVector2(x, y)) ? '#' : '.');
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public string DebugGrid(bool[] grid, int width, int height)
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                sb.Append(grid[(y * width) + x] ? '#' : '.');
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }


    public IntVector2 Rotate(IntVector2 direction, char symbol)
    {
        if (direction == IntVector2.Right && symbol == '/') return IntVector2.Down;
        if (direction == IntVector2.Right && symbol == '\\') return IntVector2.Up;
        if (direction == IntVector2.Left && symbol == '\\') return IntVector2.Down;
        if (direction == IntVector2.Left && symbol == '/') return IntVector2.Up;
        if (direction == IntVector2.Up && symbol == '/') return IntVector2.Left;
        if (direction == IntVector2.Up && symbol == '\\') return IntVector2.Right;
        if (direction == IntVector2.Down && symbol == '/') return IntVector2.Right;
        if (direction == IntVector2.Down && symbol == '\\') return IntVector2.Left;

        Debug.Assert(false);
        return new IntVector2();

        // if (symbol == '/')
        // {
        //     return new IntVector2(direction.Y, -direction.X);
        // }
        // else
        // {
        //     return new IntVector2(-direction.Y, direction.X);
        // }
    }

    public bool InBounds(IntVector2 v)
    {
        return v.X >= 0 && v.Y >= 0 && v.X < m_dataFileContents[0].Length && v.Y < m_dataFileContents.Count;
    }

    public char GetContents(IntVector2 v)
    {
        return m_dataFileContents[v.Y][v.X];
    }


    public class Beam
    {
        public static int count = 0;
        public IntVector2 CurrentDirection = new IntVector2();
        public IntVector2 CurrentPosition = new IntVector2();

        private bool m_stopped = false;

        private int id = count++;

        public Beam(IntVector2 initialPosition, IntVector2 initialDirection)
        {
            CurrentDirection = initialDirection;
            CurrentPosition = initialPosition;
        }

        public void Move()
        {
            CurrentPosition += CurrentDirection;
        }

        public void Stop()
        {
            m_stopped = true;
        }

        public bool StillActive
        {
            get { return !m_stopped; }
        }
    }
}