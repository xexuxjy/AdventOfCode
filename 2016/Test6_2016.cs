using System;
using System.Collections.Generic;

public class Test6_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 6;
    }

    public override void Execute()
    {
        List<Dictionary<char,int>> message = new List<Dictionary<char,int>>();

        for (int i = 0; i < m_dataFileContents[0].Length; i++)
        {
            message.Add(new Dictionary<char,int>());
        }
        
        foreach (string line in m_dataFileContents)
        {
            for (int i = 0; i < line.Length; i++)
            {
                Dictionary<char,int> d = message[i];
                char c = line[i];

                if (!d.ContainsKey(c))
                {
                    d[c] = 0;
                }

                d[c] += 1;
            }
        }

        string result = "";
        foreach (Dictionary<char, int> d in message)
        {
            if (IsPart1)
            {
                result += d.MaxBy(kvp => kvp.Value).Key;
            }
            else
            {
                result += d.MinBy(kvp => kvp.Value).Key;
            }
        }
        
        DebugOutput($"The message is {result}");
    }
}