using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test4_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 4;
    }

    public override void Execute()
    {

        string secretKey = m_dataFileContents[0];

        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            int maxNumbers = 100000000;
            int lowest = -1;
            for (int i = 0; i < maxNumbers; i++)
            {
                string code = (secretKey + "" + i);
                byte[] hashBytes = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(code));
                string converted = Convert.ToHexString(hashBytes);

                bool found = true;

                int numZeros = IsPart2?6:5;

                for (int b = 0; b < numZeros; ++b)
                {
                    if (converted[b] != '0')
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    lowest = i;
                    break;
                }
            }

            if (lowest != -1)
            {
                DebugOutput("Found a value at : " + lowest);
            }
        }

    }
}

