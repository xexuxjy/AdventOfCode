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

public class Test19_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 19;
    }


    public override void Execute()
    {
        Dictionary<string, List<string>> combinationDictionary = new Dictionary<string, List<string>>();
        List<(string,string)> replacements = new List<(string, string)>();

        for (int i = 0; i < m_dataFileContents.Count - 1; ++i)
        {
            string line = m_dataFileContents[i];

            if (line == "")
            {
                continue;
            }

            string[] strTokens = line.Split("=>", StringSplitOptions.TrimEntries);
            if (!combinationDictionary.TryGetValue(strTokens[0], out List<string> results))
            {
                results = new List<string>();
                combinationDictionary[strTokens[0]] = results;
            }

            results.Add(strTokens[1]);

            replacements.Add((strTokens[0], strTokens[1]));

        }

        string startPoint = m_dataFileContents.Last();


        HashSet<string> possibleResults = new HashSet<string>();



        List<List<string>> conversions = new List<List<string>>();

        List<string> tokens = new List<string>();


        int tokenIndex = 0;
        string token = "";
        while (tokenIndex < startPoint.Length - 1)
        {
            if (char.IsUpper(startPoint[tokenIndex]) && char.IsLower(startPoint[tokenIndex + 1]))
            {
                token = "" + startPoint[tokenIndex] + startPoint[tokenIndex + 1];
                tokenIndex += 2;
            }
            else
            {
                token = "" + startPoint[tokenIndex];
                tokenIndex += 1;
            }
            tokens.Add(token);
        }

        if (tokenIndex < startPoint.Length)
        {
            tokens.Add("" + startPoint.Last());
        }


        // 


        HashSet<string> uniqueTokens = new HashSet<string>();
        foreach (string s in tokens)
        {
            uniqueTokens.Add(s);
        }

        DebugOutput("Unique Tokens");
        foreach (string s in uniqueTokens)
        {
            string info = s+"  ";
            if(!combinationDictionary.ContainsKey(s))
            {
                info +=" no values";
            }
            else
            {
                info+=combinationDictionary[s].Count+" values";
            }
            DebugOutput(info);
        }



        //if (IsTestInput)
        {

            DebugOutput("");
            for (int i = 0; i < tokens.Count; ++i)
            {
                if (combinationDictionary.ContainsKey(tokens[i]))
                {
                    conversions.Add(combinationDictionary[tokens[i]]);
                }
                else
                {
                    List<string> results = new List<string>();
                    results.Add(tokens[i]);
                    conversions.Add(results);
                }
            }

            // merge together contiguous 1 length strings?
            List<List<string>> mergedConversions = new List<List<string>>();
            string mergedString = "";
            foreach(var option in conversions)
            {
                if(option.Count == 1)
                {
                    mergedString += option[0];
                }
                else
                {
                    if(mergedString != "")
                    {
                        List<string> mergedlist  = new List<string>();
                        mergedlist.Add(mergedString);
                        mergedConversions.Add(mergedlist);
                        mergedString = "";

                    }
                    mergedConversions.Add(option);
                }
            }

            if(mergedString != "")
            {
                List<string> mergedlist  = new List<string>();
                mergedlist.Add(mergedString);
                mergedConversions.Add(mergedlist);
                mergedString = "";
            }



            conversions = mergedConversions;


            int[] indices = new int[conversions.Count];


            foreach(var replacement in replacements)
            {
                int searchIndex = startPoint.IndexOf(replacement.Item1);
                while(searchIndex != -1) 
                {
                    string value = startPoint.Substring(0,searchIndex);
                    value += replacement.Item2;
                    value+= startPoint.Substring(searchIndex+replacement.Item1.Length);
                    searchIndex = startPoint.IndexOf(replacement.Item1,searchIndex+replacement.Item1.Length);
                    possibleResults.Add(value);
                }

            }


            if(IsPart2)
            {
                int numReplacements = 0;
                foreach(var replacement in replacements)
                {
                    if(replacement.Item1 == "e")
                    {
                        int searchIndex = startPoint.IndexOf(replacement.Item2);
                        while(searchIndex != -1) 
                        {
                            numReplacements++;
                            searchIndex = startPoint.IndexOf(replacement.Item2,searchIndex+replacement.Item2.Length);
                        }
                    }

                }
            }


            //BuildOptions(conversions, 0, indices, possibleResults);

            DebugOutput("");
            DebugOutput("");
            
            foreach (string option in possibleResults)
            {
                DebugOutput(option);
            }

            DebugOutput("" + possibleResults.Count);

        }

    }
    public void BuildOptions(List<List<string>> strings, int depth, int[] indices, HashSet<string> results)
    {
        if (depth >= strings.Count)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < strings.Count; i++)
            {
                result.Append(strings[i][indices[i]]);
            }
            DebugOutput(result.ToString());
            results.Add(result.ToString());
        }
        else
        {
            for (int j = 0; j < strings[depth].Count; j++)
            {
                indices[depth] = j;
                BuildOptions(strings, depth + 1, indices, results);
            }
        }
    }


    public void BuildOptionsTrie(List<List<string>> strings, int depth, int[] indices, Trie trie)
    {
        if (depth >= strings.Count)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < strings.Count; i++)
            {
                result.Append(strings[i][indices[i]]);
            }
            trie.Insert(result.ToString());
        }
        else
        {
            for (int j = 0; j < strings[depth].Count; j++)
            {
                indices[depth] = j;
                BuildOptionsTrie(strings, depth + 1, indices, trie);
            }
        }
    }

}