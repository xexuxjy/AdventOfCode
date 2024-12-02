using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test2_2024 : BaseTest
{
    public override void Initialise()
    {
        Year = 2024;
        TestID = 2;
    }

    public override void Execute()
    {
        int safeCount = 0;


        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(new char[] { ' ' });
            bool safe = true;
            int direction = 0;
                
            List<int> intList = new List<int>();     
            for (int i = 0; i < tokens.Length; i++)
            {
                intList.Add(int.Parse(tokens[i]));
            }

            if(IsPart2)
            {
                safe = TestList(intList);
                if(!safe)
                {
                    for(int j = 0; j < intList.Count;++j)
                    {
                        bool tempSafe = TestListWithRemove(intList,j);
                        if(tempSafe)
                        {
                            safe = true;
                            break; 
                         }
                    }
                }
            }
            else
            {
                safe = TestList(intList);
            }

            if (safe)
            {
                safeCount++;
            }

        }
        DebugOutput("Total safe reports is  : " + safeCount);

    }

    public bool TestListWithRemove(List<int> intList,int removeIndex)
    {
        List<int> listCopy = new List<int>();
        listCopy.AddRange(intList);
        listCopy.RemoveAt(removeIndex);

        return TestList(listCopy);

    }

    public bool TestList(List<int> intList)
    {
        bool safe = true;
        int direction = 0;

        for (int i = 0; i < intList.Count-1; i++)
        {
            int val1 = intList[i];
            int val2 = intList[i+1];

            int currentDirection = (val2 > val1) ? 1 : -1;

            if (direction == 0)
            {
                direction = currentDirection;
            }
            if (currentDirection != direction)
            {
                safe = false;
                break;
            }

            int absDiff = Math.Abs(val2 - val1);
            if (absDiff < 1 || absDiff > 3)
                {
                safe = false;
                break;
            }
        }
        return safe;
    }
}
