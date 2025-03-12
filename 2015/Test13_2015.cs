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

public class Test13_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 13;
    }


    public override void Execute()
    {
        List<Tuple<string,string,int>> seatingScores = new List<Tuple<string, string, int>>();
        HashSet<string> people = new HashSet<string>();
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(' ');
            string person = tokens[0];
            int amount = int.Parse(tokens[3]);
            if(tokens[2] == "lose")
            {
                amount *=-1;
            }
            string neighbour = tokens[10].Replace(".","");
            //DebugOutput($"{person} {amount} {neighbour}");
            seatingScores.Add(new Tuple<string, string, int>(person,neighbour, amount));
            people.Add(person);
        }

        if(IsPart2)
        {
            foreach(string person in people)
            {
                seatingScores.Add(new Tuple<string, string, int>("Self",person,0));
                seatingScores.Add(new Tuple<string, string, int>(person,"Self",0));
            }
            people.Add("Self");
        }


        List<string> test = new List<string>();
        test.Add("David");
        test.Add("Alice");
        test.Add("Bob");
        test.Add("Carol");

        int testScore = CalcScore(test,seatingScores);

        int highestScore = int.MinValue;
        foreach (var permutation in Combinations.Permute(people))
        {
            List<string> permAsList = permutation.ToList();
            int score = CalcScore(permAsList, seatingScores);
            if (score > highestScore)
            {
                highestScore = score;
            }
            //DebugOutput($"Permutation {string.Join(", ", permAsList)} is {score}");
        }
        DebugOutput($"Highest score was {highestScore}");

    }

    int CalcScore(List<string> order,List<Tuple<string,string,int>> scores)
    {
        int score = 0;
        for(int i=0;i<order.Count;i++)
        {
            int prevNeighbourIndex = i-1;
            if(prevNeighbourIndex <0)
            {
                prevNeighbourIndex+=order.Count;
            }

            int nextNeighbourIndex = i+1;
            if(nextNeighbourIndex >= order.Count)
            {
                nextNeighbourIndex-=order.Count;
            }

            string person = order[i];
            string prevNeighbour = order[prevNeighbourIndex];
            string nextNeighbour = order[nextNeighbourIndex];
            int pScore = scores.Find(x=>x.Item1==person && x.Item2 == prevNeighbour).Item3;
            int nScore = scores.Find(x=>x.Item1==person && x.Item2 == nextNeighbour).Item3;
            //DebugOutput($"{person} {prevNeighbour} {nextNeighbour} {pScore} {nScore}");

            score += pScore;
            score += nScore;

        }
        return score;
    }


}