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
        IsPart2 = true;
    }

    void PrintGrid(List<char[]> grid)
    {
        foreach (char[] line in grid)
        {
            DebugOutput(new string(line));
        }

        DebugOutput("");
    }
    
    public override void Execute()
    {
        List<List<char[]>> patterns = new List<List<char[]>>();
        List<char[]> patternData = new List<char[]>();
        patterns.Add(patternData);

        for (int i = 0; i < m_dataFileContents.Count; ++i)
        {
            if (m_dataFileContents[i] == "")
            {
                patternData = new List<char[]>();
                patterns.Add(patternData);
            }
            else
            {
                patternData.Add(m_dataFileContents[i].ToCharArray());
            }
        }

        // List<char[]> test = new List<char[]>();
        // test.Add("#....".ToCharArray());
        // test.Add("#....".ToCharArray());
        // test.Add(".....".ToCharArray());
        // test.Add("#....".ToCharArray());
        // test.Add("#.....".ToCharArray());
        // test.Add(".....".ToCharArray());
        //
        // PrintGrid(test);
        // List<char[]> rotatedTest = RotatePattern(test);
        // PrintGrid(rotatedTest);

        // int originalSccore = 
        //
        // for (int i = 0; i < 15; i++)
        // {
        //     List<char[]> a = CreateCopyWithChange(patterns[1],i);
        //     List<char[]> r = RotatePattern(a);
        //
        //     int score = TryFindReflection(a, r, 0, 0, 0);
        //     DebugOutput("Option " + i + " is " + score);
        // }

        if (IsPart2)
        {
            Part2(patterns);
        }
        else
        {
            Part1(patterns);
        }
    }

    public void Part1(List<List<char[]>> patterns)
    {
        int pos = 0;
        int totalScore = 0;
        int totalScoreCheck = 0;
        
        foreach (List<char[]> pattern in patterns)
        {
            List<char[]> rotated = RotatePattern(pattern);

            List<string> test = new List<string>();
            foreach (char[] ca in pattern)
            {
                test.Add(new string(ca));
            }

            int testScore = CheckVerticalReflectionsString(test, pos);
            
            int verticalScore = CheckVerticalReflections(pattern,pos);
            int horizontalScore = CheckVerticalReflections(rotated,pos) ;

            int newScore = TryFindReflection(pattern, rotated, pos,0);
            totalScoreCheck += newScore;
            

            if (verticalScore != 0)
            {
                horizontalScore = 0;
            }
            
            totalScore += verticalScore;
            totalScore += (100 * horizontalScore);
            
            
            pos++;
        }

        DebugOutput("Total Score for part 1 is : " + totalScore+"  check score is : "+totalScoreCheck);
        
    }

    public List<char[]> CreateCopyWithChange(List<char[]> pattern, int option)
    {
        // create a copy.
        List<char[]> patternCopy = new List<char[]>();
        foreach (char[] cp in pattern)
        {
            patternCopy.Add(new string(cp).ToCharArray());
        }

        int y = option / pattern[0].Length;
        int x = option % pattern[0].Length;

        if (patternCopy[y][x] == '.')
        {
            patternCopy[y][x] = '#';
        }
        else
        {
            patternCopy[y][x] = '.';
        }

        return patternCopy;

    }
    
    public void Part2(List<List<char[]>> patterns)
    {
        int pos = 0;
        int totalScore = 0;
        foreach (List<char[]> pattern in patterns)
        {
            int numOptions = pattern.Count * pattern[0].Length;
            
            int originalScore = TryFindReflection(pattern, RotatePattern(pattern), pos,0);             
            
            
            for (int option = 0; option < numOptions; ++option)
            {
                // create a copy.
                List<char[]> patternCopy = CreateCopyWithChange(pattern, option);
                List<char[]> rotated = RotatePattern(patternCopy);

                int verticalScore = CheckVerticalReflections(patternCopy, pos);
                int horizontalScore = CheckVerticalReflections(rotated, pos);
                //
                // if (verticalScore != 0 || horizontalScore != 0)
                // {
                //     totalScore += verticalScore;
                //     totalScore += (100 * horizontalScore);
                //     break;
                // }
                int newScore = TryFindReflection(patternCopy, rotated, pos,originalScore);
                totalScore += newScore;
                
                if (newScore != 0)
                {
                    // found a match
                    break;
                }
            }
            pos++;
        }

        DebugOutput("Total Score for part 2 is : " + totalScore);
    }


    public List<char[]> RotatePattern(List<char[]> original)
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
       
        return results;
    }

    public int CheckVerticalReflections(List<char[]> originalPattern,int id)
    {
        int minPoints = 1;
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
                matches = true;
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

    
    public int CheckVerticalReflectionsString(List<string> originalPattern,int id)
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
                matches = true;
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
    private int TryFindReflection(List<char[]> asRows,List <char[]> asColumns, int Id,int originalScore)
    {
        //var asRows = block.SplitByNewline();
        //var asColumns = block.SplitIntoColumns().ToList();
        //Check rows
        for (int i = 1; i < asRows.Count; i++)
        {
            if (asRows.Take(i).Reverse().Zip(asRows.Skip(i)).All(x => x.First.SequenceEqual(x.Second)))
            {
                if(initialMirrors.TryGetValue(Id, out (int num, bool isRow) x))
                {
                    if (x.isRow && x.num == i) continue;
                }
                initialMirrors[Id] = (i, true);


                
                int result = i * 100;
                // just want the new one
                if (result == originalScore)
                {
                    continue;
                }
                
                return result;
            }
        }

         for (int i = 1; i < asColumns.Count; i++)
         {
             if (asColumns.Take(i).Reverse().Zip(asColumns.Skip(i)).All(x => x.First.SequenceEqual(x.Second)))
             {
                 if (initialMirrors.TryGetValue(Id, out (int num, bool isRow) x))
                 {
                     if (!x.isRow && x.num == i) continue;
                         
                 }
                 initialMirrors[Id] = (i, false);
                 // just want the new one
                 if (i == originalScore)
                 {
                     continue;
                 }
                 return i;
             }
        }

        return 0;
    }    
    
}