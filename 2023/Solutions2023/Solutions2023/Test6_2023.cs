using System.Diagnostics;

public class Test6_2023 : BaseTest
{
    public override void Initialise()
    {
        Year = 2023;
        TestID = 6;
        IsTestInput = false;
        IsPart2 = true;
    }

    public override void Execute()
    {
        List<long> raceTimes = new List<long>();
        List<long> raceRecords = new List<long>();

        if (IsPart2)
        {
            Helper.ReadLongs(m_dataFileContents[0].Replace("Time:", "").Replace(" ", ""), raceTimes);
            Helper.ReadLongs(m_dataFileContents[1].Replace("Distance:", "").Replace(" ", ""), raceRecords);
        }
        else
        {
            Helper.ReadLongs(m_dataFileContents[0].Replace("Time:", ""), raceTimes);
            Helper.ReadLongs(m_dataFileContents[1].Replace("Distance:", ""), raceRecords);
        }

        long totalWays = 1;

        for (int i = 0; i < raceTimes.Count; ++i)
        {
            long numOptions = 0;
            long bestDistance = CalculateOptimal(raceTimes[i], ref numOptions);
            long numWinners = CalculateNumWinners(raceTimes[i], raceRecords[i]);
            long numWinnersQuad = CalculateNumWinnersQuadratic(raceTimes[i], raceRecords[i]);

            totalWays *= numWinners;
            if (bestDistance > raceRecords[i])
            {
                DebugOutput(
                    $"Race number [{i}] duration [{raceTimes[i]}], new record [{bestDistance}]  old record [{raceRecords[i]}]  improvement of [{bestDistance - raceRecords[i]}]  numWinners[{numWinners}]");
            }
        }

        DebugOutput("Total possibilities : " + totalWays);

    }


    public long CalculateNumWinners(long limit, long record)
    {
        int numOptions = 0;
        for (int hold = 0; hold < limit; ++hold)
        {
            long distance = (limit - hold) * hold;
            if (distance > record)
            {
                numOptions++;
            }
        }
        return numOptions;
    }

    public long CalculateNumWinnersQuadratic(long limit, long record)
    {
        //https://www.reddit.com/r/adventofcode/comments/18bwuzt/2023_day_06_im_just_glad_this_worked_after_the/
        /*
        x(T - x) = L
        Tx - x^2 = L
        0 = x^2 - Tx + L
        */

        // quad formulaa
        // (-b±√(b²-4ac))/(2a) .

        long part = (long)Math.Sqrt((limit * limit) - 4 * record);

        long highBound = (-limit + part) / 2;
        long lowBound = (-limit - part) / 2;

        int numOptions = 0;
        for (int hold = 0; hold < limit; ++hold)
        {
            long distance = (limit - hold) * hold;
            if (distance > record)
            {
                numOptions++;
            }
        }

        Debug.Assert((highBound - lowBound) == numOptions);
        long diff = highBound - lowBound;
        return numOptions;
    }


    public long CalculateOptimal(long limit, ref long numOptions)
    {
        long best = 0;
        numOptions = 0;
        for (int hold = 0; hold < limit; ++hold)
        {
            long distance = (limit - hold) * hold;
            if (distance > best)
            {
                numOptions = 1;
                best = distance;
            }
            else if (distance == best)
            {
                numOptions++;
            }
        }

        return best;
    }
}