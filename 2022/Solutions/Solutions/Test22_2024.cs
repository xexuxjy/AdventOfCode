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
        long startNumber = long.Parse(m_dataFileContents[0]);

        DebugOutput(""+startNumber);
        for(int i=0;i<10;i++)
        {
            startNumber = ApplyRules(startNumber);
            DebugOutput(""+startNumber);
        }
    }

    public long ApplyRules(long secretValue)
    {
        // these will probably be shifts
        long step1 = secretValue * 64;
        // mix result into secret number (value)

        secretValue = Mix(secretValue,step1);
        secretValue = Prune(secretValue);

        long step2 = secretValue / 32; // round to neearest integer 
        // mix result into secret number (value)
        secretValue = Mix(secretValue,step2);
        //prune this result
        secretValue = Prune(secretValue);

        long step3 = secretValue * 2048;
        // mix result into secret number (value)
        secretValue = Mix(secretValue,step3);

        //prune this result
        secretValue = Prune(secretValue);

        return secretValue;
    }

    public long Mix(long secret,long value)
    {
        return (long)(secret^value);
    }

    public long Prune(long value)
    {
        return value % 16777216;
    }


}


