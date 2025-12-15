using System.Net;

public class Test21_2023 : BaseTest
{
    public override void Initialise()
    {
        Year = 2023;
        TestID = 21;
        IsTestInput = false;
        IsPart2 = true;
    }

    public override void Execute()
    {
        int width = 0;
        int height = 0;

        char[] dataGrid = Helper.GetCharGrid(m_dataFileContents, ref width, ref height);

        LongVector2 startPosition = new LongVector2();
        for (int i = 0; i < dataGrid.Length; i++)
        {
            if (dataGrid[i] == 'S')
            {
                startPosition = new LongVector2(i % width, i / width);
                break;
            }
        }

        int numSteps = IsTestInput ? 6 : 64;

        if (IsPart2)
        {

            var start = Enumerable.Range(0, width)
                .SelectMany(i => Enumerable.Range(0, width)
                    .Where(j => m_dataFileContents[i][j] == 'S')
                    .Select(j => new LongVector2(i, j)))
                .Single();

            var grids = 26501365 / width;
            var rem = 26501365 % width;

            // By inspection, the grid is square and there are no barriers on the direct horizontal / vertical path from S
            // So, we'd expect the result to be quadratic in (rem + n * gridSize) steps, i.e. (rem), (rem + gridSize), (rem + 2 * gridSize), ...
            // Use the code from Part 1 to calculate the first three values of this sequence, which is enough to solve for ax^2 + bx + c
            var sequence = new List<int>();
            var work = new HashSet<LongVector2> { start };
            var steps = 0;
            for (var n = 0; n < 3; n++)
            {
                int limit = n * width + rem;
                for (; steps < limit; steps++)
                {
                    // Funky modulo arithmetic bc modulo of a negative number is negative, which isn't what we want here
                    work = new HashSet<LongVector2>(work
                        .SelectMany(it => new[] { LongVector2.Up, LongVector2.Down, LongVector2.Left, LongVector2.Right }.Select(dir => it + dir))
                        .Where(dest => m_dataFileContents[(int)((dest.X % width) + width) % width][(int)((dest.Y % height) + height) % height] != '#'));
                }

                sequence.Add(work.Count);
            }

            double result = Helper.GetQuadratic(grids, sequence[0], sequence[1], sequence[2]);

            DebugOutput($"26501365 : {result}");


            int total = 0;
            int numSamples = 3;
            List<long> results = new List<long>();
            for (int i = 0; i < numSamples; ++i)
            {
                long numStepsQuad = (startPosition.X) + (i * width);
                HashSet<LongVector2> positions = new HashSet<LongVector2>();
                FollowRoute(startPosition, numStepsQuad, dataGrid, width, height, positions);
                results.Add(positions.Count);
            }

            DebugOutput("Part 2 terms : " + string.Join(",", results));
            double result2 = Helper.GetQuadratic(grids, results[0], results[1], results[2]);
            DebugOutput($"26501365 : {result2}");
        }
        else
        {
            HashSet<LongVector2> positions = new HashSet<LongVector2>();
            FollowRoute(startPosition, numSteps, dataGrid, width, height, positions);
            DebugOutput(Helper.DrawGrid(dataGrid, width, height));


            DebugOutput("Part 1 Possible positions = " + positions.Count);

        }


    }

    private Dictionary<(LongVector2, long), bool> m_exploredRoutes = new Dictionary<(LongVector2, long), bool>();

    public bool FollowRoute(LongVector2 start, long numSteps, char[] dataGrid, int width, int height, HashSet<LongVector2> positions)
    {
        LongVector2 position = start;

        bool existingRoute = false;



        var searchKey = (position, numSteps);
        if (m_exploredRoutes.TryGetValue(searchKey, out existingRoute))
        {
            return existingRoute;
        }


        bool hasRoute = false;
        foreach (LongVector2 option in LongVector2.Directions)
        {
            LongVector2 newPosition = position + option;

            //if (newPosition.X >= 0 && newPosition.X < width && newPosition.Y >= 0 && newPosition.Y < height)
            {
                char positionChar = CharAtPosition(newPosition, dataGrid, width, height);
                if (positionChar != '#')
                {
                    // final position
                    if (numSteps == 1)
                    {
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

    public char CharAtPosition(LongVector2 pos, char[] dataGrid, int width, int height)
    {
        if (IsPart2)
        {
            pos = new LongVector2(((pos.X % width) + width) % width, ((pos.Y % height) + height) % height);
        }

        return dataGrid[(pos.Y * width) + pos.X];
    }


}