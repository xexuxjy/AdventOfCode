using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test5_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 5;
    }

    public bool IsNicePart1(string line)
    {
        string[] invalidStrings = new string[] { "ab", "cd", "pq", "xy" };
        char[] vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };

        bool valid = true;
        foreach (string invalid in invalidStrings)
        {
            if (line.Contains(invalid))
            {
                valid = false;
                break;
            }
        }
        if (valid)
        {
            int vowelCount = 0;
            foreach (char vowel in vowels)
            {
                foreach (char c in line)
                {
                    if (c == vowel)
                    {
                        vowelCount++;
                    }
                }
            }

            if (!(vowelCount >= 3))
            {
                valid = false;
            }

        }

        if (valid)
        {
            bool containsDouble = false;
            for (char c = 'a'; c <= 'z'; c++)
            {
                string doubleString = "" + c + c;
                if (line.Contains(doubleString))
                {
                    containsDouble = true;
                    break;
                }
            }
            if (!containsDouble)
            {
                valid = false;
            }


        }
        return valid;
    }

    public bool IsNicePart2(string line)
    {
        bool valid = true;
        bool containsDouble = false;

        bool passesTest1=true;
        for (char c = 'a'; c <= 'z'; c++)
        {
            for (char c2 = 'a'; c2 <= 'z'; c2++)
            {
                string doubleString = "" + c + c2 + c;
                if (line.Contains(doubleString))
                {
                    containsDouble = true;
                    break;
                }
                if (containsDouble)
                {
                    break;
                }
            }
        }

        if (!containsDouble)
        {
            passesTest1 = false;
        }

        bool containsPattern = false;
        bool passesTest2 = true;
        for (char c = 'a'; c <= 'z'; c++)
        {
            for (char c2 = 'a'; c2 <= 'z'; c2++)
            {
                string doubleString = "" + c + c2;
                int index1 = line.IndexOf(doubleString);
                if (index1 != -1)
                {
                    int index2 = index1+1;
                    do
                    {
                    index2 = line.IndexOf(doubleString, index2);
                    if (index2 != -1)
                    {
                        if (index2 - index1 >= 2)
                        {
                            containsPattern = true;
                            break;
                        }
                        index2++;
                    }

                    }while(index2 != -1);

                }
            }
            if (containsPattern)
            {
                break;
            }
        }
        if (!containsPattern)
        {
            passesTest2 = false;
        }

        if(!passesTest1)
        {
            //DebugOutput(line+" : fails test1");
        }

        if(!passesTest2)
        {
            //DebugOutput(line+" : fails test2");
        }


        return passesTest1 && passesTest2;
    }
    public override void Execute()
    {
        int nice = 0;
        foreach (string line in m_dataFileContents)
        {
            bool valid = IsPart2 ? IsNicePart2(line) : IsNicePart1(line);

            if (valid)
            {
                nice++;
                DebugOutput(line + " is nice");
            }

        }

        DebugOutput($"There are {nice} nice strings");
    }



}

