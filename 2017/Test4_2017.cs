using System;
using System.Collections.Generic;

public class Test4_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 4;
    }

    public override void Execute()
    {
        int valid = 0;
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(' ');
            HashSet<string> set = new HashSet<string>();
            foreach (string token in tokens)
            {
                if (IsPart1)
                {
                    set.Add(token);
                }
                else
                {
                    char[] arr = token.ToCharArray();

                    // Sort the character array
                    Array.Sort(arr);

                    // Convert sorted character array back to string
                    set.Add(new string(arr));
                }
            }

            if (set.Count == tokens.Length)
            {
                valid++;
            }
        }
        
        DebugOutput($"There are {valid} / {m_dataFileContents.Count} valid number of lines in the file.");
        
    }
}