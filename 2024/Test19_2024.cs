using System.Numerics;
using System.Xml.Linq;

public class Test19_2024 : BaseTest
{

    public override void Initialise()
    {
        Year = 2024;
        TestID = 19;
    }



    public override void Execute()
    {
        List<string> availablePatterns = new List<string>();
        List<string> targetPatterns = new List<string>();

        string[] tokens = m_dataFileContents[0].Split(',');
        foreach (string token in tokens)
        {
            availablePatterns.Add(token.Trim());
        }
        for (int i = 2; i < m_dataFileContents.Count; i++)
        {
            targetPatterns.Add(m_dataFileContents[i].Trim());
        }

        List<string> sortedPatterns = new List<string>();
        sortedPatterns.AddRange(availablePatterns.OrderBy(x => x.Length));



        List<string> reducedList = new List<string>();
        // remove the patterns that can be made from other patterns?
        foreach(string availablePattern in sortedPatterns)
        {
            List<string> patternCopy = new List<string>();
            patternCopy.AddRange(sortedPatterns);
            patternCopy.Remove(availablePattern);
            if(!TestPossibility(availablePattern,patternCopy))
            {
                reducedList.Add(availablePattern);
            }

        }



        int count = 0;
        int score = 0;
        foreach (string targetPattern in targetPatterns)
        {
            bool foundPossible = TestPossibility(targetPattern, reducedList);
            if (foundPossible)
            {
                score++;
            }
            DebugOutput($"Testing pattern {count}  valid {foundPossible} ");
            count++;
        }


        DebugOutput($"{score} out of {targetPatterns.Count}  are possible.");
        int ibreak = 0;

    }

    // to start with, don;t need to know what the pattern is, just that one exists
    public bool TestPossibility(string targetPattern, List<string> availablePatterns)
    {
        if (targetPattern == "")
        {
            return true;
        }

        bool hasMatch = false;
        foreach (string s in availablePatterns)
        {
            if(s.Length > targetPattern.Length)
            {
                return false;
            }

            //DebugOutput($"Testing {targetPattern} with {s} matches {targetPattern.StartsWith(s)}");
            if (targetPattern.StartsWith(s))
            {
                bool match =TestPossibility(targetPattern.Substring(s.Length), availablePatterns);; 
                if(match)
                {
                    return true;
                }
            }
        }
        return hasMatch;
    }


    // need to reduce the set of possible types.



}