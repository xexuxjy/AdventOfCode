using System;
using System.Collections.Generic;

public class Test7_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 7;
    }

    public override void Execute()
    {
        int supportTLSCount = 0;
        int supportSLSCount = 0;
        
        foreach (string line in m_dataFileContents)
        {
            int startIndex = 0;
            List<string> normalSequences = new List<string>();
            List<string> hyperNetSequences = new List<string>();

            List<string> ABASequences = new List<string>();
            

            string currentToken = "";
            bool inHyperNet = false;
            
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '[')
                {
                    if (currentToken.Length > 0)
                    {
                        normalSequences.Add(currentToken);
                    }
                    currentToken = "";
                    inHyperNet = true;
                }
                else if (line[i] == ']')
                {
                    inHyperNet = false;
                    hyperNetSequences.Add(currentToken);
                    currentToken = "";
                }
                else
                {
                    currentToken += line[i];
                }
            }

            if (currentToken.Length > 0)
            {
                normalSequences.Add(currentToken);
            }

            bool abbaOut = false;
            bool abbaIn = false;
            bool babMatches = false;
            
            foreach (string s in normalSequences)
            {
                if (ContainsABBA(s))
                {
                    abbaOut = true;
                    break;
                }
            }

            if (IsPart2)
            {
                foreach (string s in normalSequences)
                {
                    GetABAList(s,ABASequences);
                }
            }
            
            foreach(string s in hyperNetSequences)
            {
                if (ContainsABBA(s))
                {
                    abbaIn = true;
                    break;
                }
            }

            if (IsPart2)
            {
                foreach (string aba in ABASequences)
                {
                    foreach (string s in hyperNetSequences)
                    {
                        string bab = "" + aba[1] + aba[0] + aba[1];
                        if (s.Contains(bab))
                        {
                            babMatches = true;
                            break;
                        }
                    }
                }
            }


            bool supportsTLS = abbaOut && !abbaIn;
            if (supportsTLS)
            {
                supportTLSCount++;
            }
            
            
            bool supportsSLS = babMatches;
            if (supportsSLS)
            {
                supportSLSCount++;
            }
            
            DebugOutput($"{line}  supports TLS : {supportsTLS}  supports SLS : {supportsSLS}");

        }

        if (IsPart1)
        {
            DebugOutput($"There are {supportTLSCount} TLS valid lines");
        }
        else
        {
            DebugOutput($"There are {supportSLSCount} SLS valid lines");
        }

    }
    
    
    public bool ContainsABBA(string line)
    {
        for (int i = 0; i <= line.Length - 4; i++)
        {
            if (line[i] != line[i+1] && line[i] == line[i + 3] && line[i + 1] == line[i + 2])
            {
                return true;
            }
        }

        return false;
    }

    
    public bool ContainsABA(string line)
    {
        for (int i = 0; i <= line.Length - 3; i++)
        {
            if (line[i] != line[i+1] && line[i] == line[i + 3] && line[i + 1] == line[i + 2])
            {
                return true;
            }
        }

        return false;
    }
    
    public void GetABAList(string line,List<string> abaList)
    {
        for (int i = 0; i <= line.Length - 3; i++)
        {
            if (line[i] != line[i+1] && line[i] == line[i + 2])
            {
                abaList.Add(line.Substring(i,3));
            }
        }
    }

}