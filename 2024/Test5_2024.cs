using System;
using System.Text.RegularExpressions;

public class Test5_2024 : BaseTest
{
    public override void Initialise()
    {
        Year = 2024;
        TestID = 5;
    }

    public override void Execute()
    {
        int total = 0;
        List<(int,int)> pageOrderingRules = new List<(int,int)>();
        List<List<int>> pageOrderList = new List<List<int>>();
        foreach(string line in m_dataFileContents)
        {
            if(line == "")
            {
                continue;
            }

            if(line.Contains("|"))
            {
                string[] split = line.Split('|');
                pageOrderingRules.Add((int.Parse(split[0]),int.Parse(split[1])));
            }
            else
            {
                List<int> pageOrder = new List<int>();
                pageOrderList.Add(pageOrder);
                string[] tokens= line.Split(",");
                foreach(string token in tokens)
                {
                    pageOrder.Add(int.Parse(token));
                }
            }

        }

        List<int> condensedList = CreateCondensedRules(pageOrderingRules);

        foreach(List<int> pageOrder in pageOrderList)
        {
            if(TestPageOrder(pageOrder,pageOrderingRules))
            {
                total += pageOrder[pageOrder.Count/2];
            }
            else if(IsPart2)
            {
                FixPageOrder(pageOrder, pageOrderingRules);
                total += pageOrder[pageOrder.Count/2];
            }
        }


        DebugOutput("Total of mid points is : "+total);
    }

    public bool TestPageOrder(List<int> pageOrder,List<(int,int)> pageOrderingRules)
    {
        bool matches = true;
        foreach((int,int) pair in pageOrderingRules)
        {
            int index1 = pageOrder.IndexOf(pair.Item1);
            int index2 = pageOrder.IndexOf(pair.Item2);

            if(index1 != -1 && index2 != -1)
            {
                if(index2 < index1)
                {
                    matches = false;
                    break;
                }
            }
        }

        return matches;
    }

    public List<int> CreateCondensedRules(List<(int,int)> pageOrderingRules)
    {
        List<int> resultList = new List<int>();

        foreach((int,int) pair in pageOrderingRules)
        {
            int index1 = resultList.IndexOf(pair.Item1);
            int index2 = resultList.IndexOf(pair.Item2);

            if( index1 == -1 && index2 == -1)
            {
                resultList.Add(pair.Item1);
                resultList.Add(pair.Item2);
            }
            else if(index1 != -1 && index2 == -1)
            {
                resultList.Add(pair.Item2);
            }
            else if(index1 == -1 && index2 != -1)
            {
                resultList.Insert(index2, pair.Item1);
            }
        }
        return resultList;;
    }


    public void FixPageOrder(List<int> pageOrder,List<(int,int)> pageOrderingRules)
    {
        while(!TestPageOrder(pageOrder,pageOrderingRules))
        {
            foreach((int,int) pair in pageOrderingRules)
            {
            int index1 = pageOrder.IndexOf(pair.Item1);
            int index2 = pageOrder.IndexOf(pair.Item2);

                if(index1 != -1 && index2 != -1)
                {
                    int t = pageOrder[index1];
                    pageOrder[index1] = pageOrder[index2];
                    pageOrder[index2] = t;
                }
            }
        }

    }

}