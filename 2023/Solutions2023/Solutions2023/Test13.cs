using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

public class Test13 : BaseTest
{
    public override void Initialise()
    {
        TestID = 13;
        IsTestInput = true;
        IsPart2 = false;
    }

    public override void Execute()
    {
        List<List<string>> patterns = new List<List<string>>();
        List<string> patternData = new List<string>();
        patterns.Add(patternData);

        for (int i = 0; i < m_dataFileContents.Count; ++i)
        {
            if (m_dataFileContents[i] == "")
            {
                patternData = new List<string>();
                patterns.Add(patternData);
            }
            else
            {
                patternData.Add(m_dataFileContents[i]);
            }
        }

        int totalScore = 0;
        foreach (List<string> pattern in patterns)
        {
            int verticalScore = CheckReflection(pattern,true);
            int horizontalScore = CheckReflection(pattern,false);
            totalScore += verticalScore;
            totalScore += (100 * horizontalScore);
        }

        DebugOutput("Total Score is : " + totalScore);
    }


    public int CheckReflection(List<string> originalPattern,bool vertical)
    {
        int midPoint = originalPattern.Count / 2;
        midPoint++;
        
        int foundPoint = 0;

        for (int reflectionPoint = 2; reflectionPoint <= midPoint; ++reflectionPoint)
        {
            bool matches = true;
            int points = reflectionPoint;

            for (int y = 0; y < originalPattern.Count; ++y)
            {
                
                for (int x = 0; x < points; x++)
                {
                    int pos1 = reflectionPoint - (vertical ?  x : y);
                    int pos2 = reflectionPoint + (vertical ? x : y) + 1;

                    int yIndex1 = vertical ? y : pos1;
                    int yIndex2 = vertical ? y : pos2;

                    int xIndex1 = vertical ? pos1 : x;
                    int xIndex2 = vertical ? pos2 : x;
                    
                    //DebugOutput($"Pos1 : {pos1+1}  Pos2 : {pos2+1}");
                    if (yIndex1 >= 0 && yIndex1 < originalPattern.Count && yIndex2 >= 0 &&
                        yIndex2 < originalPattern.Count && xIndex1 >= 0 && xIndex1 < originalPattern[0].Length &&
                        xIndex2 >= 0 && xIndex2 < originalPattern[0].Length)
                    {
                        if (originalPattern[yIndex1][xIndex1] != originalPattern[yIndex2][xIndex2])
                        {
                            matches = false;
                            break;
                        }
                    }
                }

                if (!matches)
                {
                    break;
                }
            }

            if (matches)
            {
                foundPoint = reflectionPoint+1;
                break;
            }
        }

        DebugOutput("Final "+(vertical?"Vertical":"Horizontal")+ " result was : " + foundPoint);

        return foundPoint;
    }
}