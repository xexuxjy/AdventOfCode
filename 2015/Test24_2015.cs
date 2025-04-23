using AdventOfCode.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Test19_2022;
using static Test8_2023;

public class Test24_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 24;
    }


    public override void Execute()
    {
        List<int> weights = new List<int>();
        foreach(string line in m_dataFileContents)
        {
            weights.Add(int.Parse(line));
        }

        int total = weights.Sum();
        int numGroups = IsPart1?3:4;
        int average = total / numGroups;

        List<int> bestList = null;

        long bestEntanglement = 1;

        weights.Reverse();

        //foreach(var result in Combinations.GetAllCombosIterYield(weights))
        
        foreach(var result in GetAllCombosIterYieldTest(weights,average,bestList))
        {
            if(result.Sum() == average)
            {
                if(bestList == null || result.Count < bestList.Count)
                {
                    bestList = new List<int>();
                    bestList.AddRange(result);
                    bestEntanglement = 1;
                    foreach(int i in bestList)
                    {
                        bestEntanglement *= i;
                    }
                }
                else if(result.Count == bestList.Count)
                {
                    long newEntanglement =1;
                    foreach(int i in result)
                    {
                        newEntanglement *= i;
                    }

                    if(newEntanglement < bestEntanglement)
                    {
                        bestList = new List<int>();
                        bestList.AddRange(result);
                        bestEntanglement = 1;
                        foreach(int i in bestList)
                        {
                            bestEntanglement *= i;
                        }
                    }
                }
            }
        }


        DebugOutput($"Total is {total} average is {average} cabinList has {bestList.Count} items and entanglement {bestEntanglement}");
    }

    public static IEnumerable<List<int>> GetAllCombosIterYieldTest(List<int> list,int average,List<int> bestList)
    {
        int comboCount = (int)Math.Pow(2, list.Count) - 1;
        if (comboCount == 1)
        {
            yield return list;
        }
        else
        {
            for (int i = 1; i < comboCount + 1; i++)
            {
                List<int> tempList = new List<int>();
                int tempTotal = 0;
                for (int j = 0; j < list.Count; j++)
                {
                    if ((i >> j) % 2 != 0)
                    {
                        tempList.Add(list[j]);
                        tempTotal += list[j];
                        if(tempTotal > average || (bestList != null && tempList.Count > bestList.Count))
                        {
                            break;
                        }
                    }
                }
                yield return tempList;
            }
        }
    }


}