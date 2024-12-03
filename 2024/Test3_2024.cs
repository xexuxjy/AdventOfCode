using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class Test3_2024 : BaseTest
{
    public override void Initialise()
    {
        Year = 2024;
        TestID = 3;
    }

    public override void Execute()
    {
        Regex regExp = new Regex(@"mul\([0-9]{1,3},[0-9]{1,3}\)");
        if (IsPart2)
        {
            regExp = new Regex(@"mul\([0-9]{1,3},[0-9]{1,3}\)|do\(\)|don't\(\)");
        }

        int total = 0;

        bool shouldMultiply = true;

        foreach (string line in m_dataFileContents)
        {
            MatchCollection matchCollection = regExp.Matches(line);
            foreach (Match match in matchCollection)
            {
                //DebugOutput(match.Value);
                if (match.Value == @"do()")
                {
                    shouldMultiply = true;
                }
                else if (match.Value == @"don't()")
                {
                    shouldMultiply = false;
                }
                else if (match.Value.StartsWith("mul"))
                {
                    if (shouldMultiply)
                    {
                        string temp = match.Value.Replace("mul(", "");
                        temp = temp.Replace(")", "");
                        string[] tokens = temp.Split(new char[] { ',' });
                        int val1 = int.Parse(tokens[0]);
                        int val2 = int.Parse(tokens[1]);
                        int result = val1 * val2;
                        total += result;
                    }
                }

            }
        }

        //DebugOutput($"Have found : {matchCollection.Count} matches");
        DebugOutput($"Total is : {total} ");

    }
}