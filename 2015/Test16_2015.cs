using AdventOfCode.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Test8_2023;

public class Test16_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 16;
    }


    public override void Execute()
    {
        //Sue 1: children: 1, cars: 8, vizslas: 7

        List<Dictionary<string, int>> auntsList = new List<Dictionary<string, int>>();

        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(new char[] { ' ', ':', ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            string name = tokens[0];
            int id = int.Parse(tokens[1]);

            Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();

            for (int i = 2; i < tokens.Length; i += 2)
            {
                keyValuePairs[tokens[i]] = int.Parse(tokens[i + 1]);
            }

            auntsList.Add(keyValuePairs);
        }


        Dictionary<string, int> targetValues = new Dictionary<string, int>();
        targetValues["children"] = 3;
        targetValues["cats"] = 7;
        targetValues["samoyeds"] = 2;
        targetValues["pomeranians"] = 3;
        targetValues["akitas"] = 0;
        targetValues["vizslas"] = 0;
        targetValues["goldfish"] = 5;
        targetValues["trees"] = 3;
        targetValues["cars"] = 2;
        targetValues["perfumes"] = 1;

        for (int i = 0; i < auntsList.Count; i++)
        {
            if (i != 20)
            {
                //continue;
            }

            if (PotentialMatch(targetValues, auntsList[i]))
            {
                string debugInfo = "";
                foreach (string key in auntsList[i].Keys)
                {
                    debugInfo += key + " : " + auntsList[i][key] + ",";
                }


                DebugOutput($"Aunt {i + 1} is a potential match {debugInfo}");
            }
        }

    }

    // Things missing from your list aren't zero - you simply don't remember the value.

    public bool PotentialMatch(Dictionary<string, int> targetValues, Dictionary<string, int> testValues)
    {
        foreach (string key in targetValues.Keys)
        {
            if (testValues.ContainsKey(key))
            {

                if (IsPart2)
                {
                    if (key == "cats" || key == "trees")
                    {
                        //if (targetValues[key] <= testValues[key])
                        if (targetValues[key] >= testValues[key])
                        {
                            return false;
                        }
                    }
                    else if (key == "pomeranians" || key == "goldfish")
                    {
                        //if (targetValues[key] >= testValues[key])
                        if (targetValues[key] <= testValues[key])
                        {
                            return false;
                        }
                    }
                    else if (targetValues[key] != testValues[key])
                    {
                        return false;
                    }
                }
                else
                {

                    if (targetValues[key] != testValues[key])
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }



}

