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
        List<(int, int)> pageOrderingRules = new List<(int, int)>();
        List<List<int>> pageOrderList = new List<List<int>>();
        foreach (string line in m_dataFileContents)
        {
            if (line == "")
            {
                continue;
            }

            if (line.Contains("|"))
            {
                string[] split = line.Split('|');
                pageOrderingRules.Add((int.Parse(split[0]), int.Parse(split[1])));
            }
            else
            {
                List<int> pageOrder = new List<int>();
                pageOrderList.Add(pageOrder);
                string[] tokens = line.Split(",");
                foreach (string token in tokens)
                {
                    pageOrder.Add(int.Parse(token));
                }
            }

        }

        //List<int> condensedList = CreateCondensedRules(pageOrderingRules);

        foreach (List<int> pageOrder in pageOrderList)
        {
            bool validPage = TestPageOrder(pageOrder, pageOrderingRules);
            if (IsPart2)
            {
                if (!validPage)
                {
                    FixPageOrder(pageOrder, pageOrderingRules);
                    total += pageOrder[pageOrder.Count / 2];
                }
            }
            else
            {
                if (validPage)
                {
                    total += pageOrder[pageOrder.Count / 2];
                }
            }
        }


        DebugOutput("Total of mid points is : " + total);
    }

    public bool TestPageOrder(List<int> pageOrder, List<int> pageOrderingRules)
    {
        bool matches = true;
        for (int i = 0; i < pageOrder.Count - 1; ++i)
        {
            int index1 = pageOrderingRules.IndexOf(pageOrder[i]);
            int index2 = pageOrderingRules.IndexOf(pageOrder[i + 1]);

            if (index1 != -1 && index2 != -1)
            {
                if (index2 < index1)
                {
                    matches = false;
                    break;
                }
            }
        }

        return matches;
    }

    public int FindLowestIndexWithCache(int index, List<int> resultList, List<(int, int)> pageOrderingRules, Dictionary<int, List<(int, int)>> pairCache)
    {
        List<(int, int)> temp = null;
        // find all places where index1 if first
        if (pairCache.ContainsKey(index))
        {
            temp = pairCache[index];
        }
        else
        {
            temp = pageOrderingRules.FindAll(x => x.Item1 == index);
            pairCache[index] = temp;
        }

        // go through all the rules that reference Item1 and find it's lowest point?
        int lowestIndex2 = resultList.Count;
        foreach (var temp2 in temp)
        {
            int found = resultList.IndexOf(temp2.Item2);
            if (found != -1 && found < lowestIndex2)
            {
                lowestIndex2 = found;
            }
        }
        return lowestIndex2;
    }

    public List<int> CreateCondensedRules(List<(int, int)> pageOrderingRules)
    {

        int count = 0;
        List<int> resultList = new List<int>();
        bool ordered = false;

        Dictionary<int, List<(int, int)>> pairCache = new Dictionary<int, List<(int, int)>>();

        while (!ordered)
        {
            DebugOutput(string.Join(",", resultList));
            count++;
            ordered = true;
            foreach ((int, int) pair in pageOrderingRules)
            {
                int index1 = resultList.IndexOf(pair.Item1);
                int index2 = resultList.IndexOf(pair.Item2);

                if (index1 == -1 && index2 == -1)
                {
                    resultList.Add(pair.Item1);
                    resultList.Add(pair.Item2);
                    ordered = false;
                    DebugOutput(string.Join(",", resultList));
                }
                else if (index1 != -1 && index2 == -1)
                {
                    resultList.Add(pair.Item2);
                    ordered = false;
                }
                else if (index1 == -1 && index2 != -1)
                {
                    int lowestIndex = FindLowestIndexWithCache(pair.Item1, resultList, pageOrderingRules, pairCache);
                    int insertIndex = lowestIndex > 0 ? lowestIndex - 1 : 0;

                    resultList.Insert(insertIndex, pair.Item1);
                    ordered = false;
                    DebugOutput(string.Join(",", resultList));
                }
                else
                {
                    if (index2 < index1)
                    {
                        resultList.Remove(pair.Item1);
                        int lowestIndex = FindLowestIndexWithCache(pair.Item1, resultList, pageOrderingRules, pairCache);

                        int insertIndex = lowestIndex > 0 ? lowestIndex - 1 : 0;
                        resultList.Insert(insertIndex, pair.Item1);
                        ordered = false;
                        DebugOutput(string.Join(",", resultList));
                    }
                }

            }
            if (count > 1000)
            {
                break;
            }
        }
        DebugOutput("Ordered List");
        return resultList;
    }


    public void FixPageOrder(List<int> pageOrder, List<(int, int)> pageOrderingRules)
    {
        while (!TestPageOrder(pageOrder, pageOrderingRules))
        {
            foreach ((int, int) pair in pageOrderingRules)
            {
                int index1 = pageOrder.IndexOf(pair.Item1);
                int index2 = pageOrder.IndexOf(pair.Item2);

                if (index1 != -1 && index2 != -1)
                {

                    if (index2 < index1)
                    {
                        int t = pageOrder[index1];
                        pageOrder[index1] = pageOrder[index2];
                        pageOrder[index2] = t;
                    }
                }
            }
        }

    }


    public bool TestPageOrder(List<int> pageOrder, List<(int, int)> pageOrderingRules)
    {
        bool matches = true;
        foreach ((int, int) pair in pageOrderingRules)
        {
            int index1 = pageOrder.IndexOf(pair.Item1);
            int index2 = pageOrder.IndexOf(pair.Item2);

            if (index1 != -1 && index2 != -1)
            {
                if (index2 < index1)
                {
                    matches = false;
                    break;
                }
            }
        }

        return matches;
    }
}