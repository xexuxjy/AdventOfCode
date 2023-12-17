using System.Text;

public class Test15 : BaseTest
{
    public override void Initialise()
    {
        TestID = 15;
        IsTestInput = false;
        IsPart2 = false;
    }

    public int GenerateHASH(string val)
    {
        int total = 0;
        foreach (char c in val)
        {
            int asciiVal = (int)c;
            total += c;
            total *= 17;
            total = total % 256;
        }

        return total;
    }

    public override void Execute()
    {
        long finalTotal = 0;
        List<string> parseResuls = new List<string>();
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            foreach (string token in tokens)
            {
                finalTotal += GenerateHASH(token);
            }
        }

        DebugOutput("Final total is " + finalTotal);
    }
    
    
    
    
}