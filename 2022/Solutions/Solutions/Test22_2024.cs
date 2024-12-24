using System.Net.Sockets;

public class Test22_2024 : BaseTest
{

    public override void Initialise()
    {
        Year = 2024;
        TestID = 22;
    }

    public override void Execute()
    {
        long total = 0;
        foreach (string line in m_dataFileContents)
        {
            long startNumber = long.Parse(line);
            long startNumberOriginal = startNumber;

            int numIterations = 2000;


            for (int i = 0; i < numIterations; i++)
            {
                startNumber = ApplyRules(startNumber);

            }

            DebugOutput($"{startNumberOriginal}: {startNumber}");
            total += startNumber;
        }

        DebugOutput("Total is : "+total);
    }

    public long ApplyRules(long secretValue)
    {
        // these will probably be shifts
        long step1 = secretValue * 64;
        // mix result into secret number (value)

        secretValue = Mix(secretValue, step1);
        secretValue = Prune(secretValue);

        long step2 = secretValue / 32; // round to neearest integer 
        // mix result into secret number (value)
        secretValue = Mix(secretValue, step2);
        //prune this result
        secretValue = Prune(secretValue);

        long step3 = secretValue * 2048;
        // mix result into secret number (value)
        secretValue = Mix(secretValue, step3);

        //prune this result
        secretValue = Prune(secretValue);

        return secretValue;
    }

    public long Mix(long secret, long value)
    {
        return (long)(secret ^ value);
    }

    public long Prune(long value)
    {
        return value % 16777216;
    }


}


