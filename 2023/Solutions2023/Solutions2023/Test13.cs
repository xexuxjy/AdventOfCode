using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

public class Test13 : BaseTest
{
    public override void Initialise()
    {
        TestID = 13;
        IsTestInput = false;
        IsPart2 = false;
    }

    void PrintGrid(List<string> grid)
    {
        foreach (string line in grid)
        {
            DebugOutput(line);
        }

        DebugOutput("");
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

        // List<string> test = new List<string>();
        // test.Add("#....");
        // test.Add("#....");
        // test.Add(".....");
        // test.Add("#....");
        // test.Add("#.....");
        // test.Add(".....");
        //
        // PrintGrid(test);
        // List<string> rotatedTest = RotatePattern(test);
        // PrintGrid(rotatedTest);

        int pos = 0;
        int totalScore = 0;
        foreach (List<string> pattern in patterns)
        {
            List<string> rotated = RotatePattern(pattern);

            int verticalScore = CheckVerticalReflections(pattern,pos);
            int horizontalScore = CheckVerticalReflections(rotated,pos) ;

            int newScore = TryFindReflection(pattern, rotated, pos,horizontalScore,verticalScore);
            totalScore += newScore;
            

            // if (verticalScore != 0)
            // {
            //     horizontalScore = 0;
            // }
            //
            // if (verticalScore != 0 && horizontalScore != 0)
            // {
            //     DebugOutput("Got multiple lines of reflection for position " + pos+" V : "+verticalScore+"  H: "+horizontalScore);
            //     foreach (string line in pattern)
            //     {
            //         DebugOutput(line);
            //     }
            //
            //     DebugOutput("");
            //     int ibreak = 0;
            // }
            
            // totalScore += verticalScore;
            // totalScore += (100 * horizontalScore);
            
            
            pos++;
        }

        DebugOutput("Total Score is : " + totalScore);
    }

    public List<string> RotatePattern(List<string> original)
    {
        List<char[]> results = new List<char[]>();

        int rows = original.Count;
        int columns = original[0].Length;
        
        for (int i = 0; i< columns;++i)
        {
            results.Add(new char[original.Count]);
            for (int j = 0; j < rows; ++j)
            {
                results[i][j] = original[j][i];
            }
            
        }
       
        List<string> finalResults = new List<string>();
        foreach (char[] ca in results)
        {
            finalResults.Add(new String(ca));
        }

        return finalResults;
    }

    public int CheckVerticalReflections(List<string> originalPattern,int id)
    {
        int minPoints = 3;
        int endPoint = originalPattern[0].Length - minPoints;
        int midPoint = (originalPattern[0].Length) / 2;
        int points = midPoint;

        int foundPoint = 0;

        for (int reflectionPoint = 0; reflectionPoint < endPoint; ++reflectionPoint)
        {
            bool matches = true;

            int numPoints = 8;
            while (numPoints >= minPoints)
            {
                for (int y = 0; y < originalPattern.Count; ++y)
                {
                    for (int x = 0; x < numPoints; x++)
                    {
                        int pos1 = reflectionPoint- x;
                        int pos2 = reflectionPoint + x + 1;

                        if (pos1 < 0 || pos1 >= originalPattern[y].Length || pos2 < 0 ||
                            pos2 >= originalPattern[y].Length || originalPattern[y][pos1] != originalPattern[y][pos2])
                        {
                            matches = false;
                            break;
                        }
                        //DebugOutput($"Pos1 : {pos1+1}  Pos2 : {pos2+1}");
                    }
                }
                if (matches)
                {
                    foundPoint = reflectionPoint+1;
                    break;
                }

                numPoints--;
            }
            if (matches)
            {
                foundPoint = reflectionPoint+1;
                break;
            }
        }

        //DebugOutput("Final vertical result was : " + foundPoint);

        return foundPoint;
    }
    
    
    Dictionary<int, (int num, bool isRow)> initialMirrors= new(); 
    private int TryFindReflection(List<string> asRows,List <string> asColumns, int Id,int hscore,int vscore)
    {
        //var asRows = block.SplitByNewline();
        //var asColumns = block.SplitIntoColumns().ToList();
        //Check rows
        for (int i = 1; i < asRows.Count; i++)
        {
            if (asRows.Take(i).Reverse().Zip(asRows.Skip(i)).All(x => x.First == x.Second))
            {
                if(initialMirrors.TryGetValue(Id, out (int num, bool isRow) x))
                {
                    if (x.isRow && x.num == i) continue;
                }
                initialMirrors[Id] = (i, true);

                if (vscore != 0 && i != vscore)
                {
                    int ibreak = 0;
                    DebugOutput($"Id {Id} Theirs {i}  Mine {vscore}  100 mult");
                    PrintGrid(asRows);
                }
                
                int result = i * 100;
                return result;
            }
        }

         for (int i = 1; i < asColumns.Count; i++)
         {
             if (asColumns.Take(i).Reverse().Zip(asColumns.Skip(i)).All(x => x.First == x.Second))
             {
                 if (initialMirrors.TryGetValue(Id, out (int num, bool isRow) x))
                 {
                     if (!x.isRow && x.num == i) continue;
                         
                 }
                 initialMirrors[Id] = (i, false);
                 if (hscore !=0 && i != hscore)
                 {
                     int ibreak = 0;
                     DebugOutput($"Id {Id} Theirs {i}  Mine {hscore}");
                     PrintGrid(asColumns);
                 }
                 return i;
             }
        }

        return 0;
    }    
    
}