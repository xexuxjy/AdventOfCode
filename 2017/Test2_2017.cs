using System;
using System.Collections.Generic;

public class Test2_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 2;
    }

    public override void Execute()
    {
        List<List<int>> spreadSheet = new List<List<int>>();
        foreach (string line in m_dataFileContents)
        {
            List<int> digitList = new List<int>();
            spreadSheet.Add(digitList);

            string[] tokens = line.Split(new char[]{' ','\t'});
            foreach (string token in tokens)
            {
                digitList.Add(int.Parse(token));
            }
            digitList.Sort();
        }


        int total = 0;

        if (IsPart1)
        {
            foreach (List<int> spread in spreadSheet)
            {
                total += (spread.Last() - spread.First());
            }
        }
        else
        {
            foreach (List<int> spread in spreadSheet)
            {
                for (int i = 0; i < spread.Count; i++)
                {
                    for (int j = 0; j < spread.Count; j++)
                    {
                        if (i != j && (spread[i] % spread[j] == 0))
                        {
                            total += spread[i] / spread[j];
                        }
                    }
                }
            }
        }

        DebugOutput($"The total is {total}");
        
    }
}