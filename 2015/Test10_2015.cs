using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test10_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 10;
    }


    public override void Execute()
    {
        foreach (string line in m_dataFileContents)
        {
            if (line.StartsWith("#"))
            { continue; }


            int numIterations = IsPart2?50:40;
            string output = line;
            for(int i=0;i<numIterations; i++)
            {
                output  = ConvertLine(output);
            }

            //DebugOutput($"{line} becomes {output} length {output.Length}");
            DebugOutput($"{line} becomes length {output.Length}");
        }


    }

    public string ConvertLine(string line)
    {
        List<StringBuilder> tokens = new List<StringBuilder>();

        StringBuilder output = new StringBuilder();;
        StringBuilder currentToken = new StringBuilder();
        int lineLength = line.Length;
        char[] characters = line.ToCharArray();

        for (int i = 0; i < lineLength; i++)
        {
            if (currentToken.Length != 0 && currentToken[0] != characters[i])
            {
                tokens.Add(currentToken);
                currentToken = new StringBuilder();;
            }

            if (currentToken.Length == 0 || characters[i] == currentToken[0])
            {
                currentToken.Append(characters[i]);
            }
        }
        tokens.Add(currentToken);

        foreach(StringBuilder token in tokens)
        {
            output.Append($"{token.Length}{token[0]}");
        }

        return output.ToString();
    }

}