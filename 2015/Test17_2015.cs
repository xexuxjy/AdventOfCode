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
using static Test8_2023;

public class Test17_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 17;
    }


    public override void Execute()
    {
        List<int> volumes = new List<int>();
        foreach (string line in m_dataFileContents)
        {
            volumes.Add(int.Parse(line));
        }

        int goalToReach = IsTestInput?25:150;
        int score = 0;
        Dictionary<int,int> lengthScores = new Dictionary<int,int>();

       
        foreach (var result in Combinations.GetAllCombosIter<int>(volumes))
        {
            //DebugOutput("" + result.Sum());
            if (result.Sum() == goalToReach)
            {
                int length = result.Count();
                if(!lengthScores.ContainsKey(length))
                {
                    lengthScores[length] = 1;
                }
                else
                {
                    lengthScores[length]++;
                }


                score++;
            }
        }

        DebugOutput($"Final score is {score}");
        if(IsPart2)
        {
            int lowestKey = lengthScores.Keys.Min();
            DebugOutput($"Score of lowest key ({lowestKey}) is {lengthScores[lowestKey]}");
        }


    }
}