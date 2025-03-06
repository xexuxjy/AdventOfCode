using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Test8_2023;

public class Test9_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 9;
    }


    public override void Execute()
    {

        //London to Dublin = 464

        List<Path<string>> pathList = new List<Path<string>>();
        HashSet<string> allLocations = new HashSet<string>();

        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(' ');
            string from = tokens[0];
            string to = tokens[2];
            int cost = int.Parse(tokens[4]);

            allLocations.Add(from);
            allLocations.Add(to);


            pathList.Add(new Path<string>() { Source = from, Destination = to, Cost = cost });
        }


        int noPathValue = 10000;

        int answer = IsPart2 ? int.MinValue : int.MaxValue;
        List<string> locationList = allLocations.ToList();
        int[,] graphMatrix = CalculateFloydMarshall(pathList, locationList);

        int permutationCount = 0;
        foreach (var permutation in Combinations.Permute(allLocations))
        {
            permutationCount++;
            List<string> permutationList = permutation.ToList();
            int cost = 0;

            string previous = permutationList[0];

            foreach (string destination in permutationList)
            {
                int srcIndex = locationList.IndexOf(previous);
                int dstIndex = locationList.IndexOf(destination);
                int distanceCost = graphMatrix[srcIndex, dstIndex];
                cost = cost + distanceCost;
                previous = destination;
            }

            //DebugOutput($"Result for  [{string.Join(',', permutationList)}] {cost}");
            if (IsPart2)
            {
                if (cost > answer)
                {
                    answer = cost;
                    DebugOutput($"Found high cost for [{string.Join(',', permutationList)}] {cost}");
                }
            }
            else
            {
                if (cost < answer)
                {
                    answer = cost;
                    DebugOutput($"Found low cost for [{string.Join(',', permutationList)}] {cost}");
                }
            }
        }

        DebugOutput($"Num Permutations is {permutationCount}");
        if (IsPart2)
        {
            DebugOutput($"Highest cost is {answer}");
        }
        else
        {
            DebugOutput($"Lowest cost is {answer}");
        }

        int ibreak = 0;

    }


    public int[,] CalculateFloydMarshall(List<Path<string>> pathList, List<string> locationList)
    {
        int cardinality = locationList.Count;

        int[,] graphMatrix = new int[cardinality, cardinality];
        int[,] graphMatrix2 = new int[cardinality, cardinality];
        for (int i = 0; i < cardinality; i++)
        {
            for (int j = 0; j < cardinality; j++)
            {
                graphMatrix[i, j] = -1;
                graphMatrix2[i, j] = -1;
            }
        }

        for (int i = 0; i < cardinality; i++)
        {
            for (int j = 0; j < cardinality; j++)
            {
                if (i == j)
                {
                    graphMatrix[i, j] = 0;
                    graphMatrix2[i, j] = 0;
                }
                else
                {
                    Path<string> p = pathList.Find(x => x.Source == locationList[i] && x.Destination == locationList[j]);

                    // try opposite direction
                    if (p == null)
                    {
                        p = pathList.Find(x => x.Source == locationList[j] && x.Destination == locationList[i]);
                    }

                    if (p != null)
                    {
                        graphMatrix[i, j] = p.Cost;
                        graphMatrix2[i, j] = p.Cost;
                    }

                }
            }
        }

        floydWarshall(graphMatrix);

        for (int i = 0; i < cardinality; i++)
        {
            for (int j = 0; j < cardinality; j++)
            {
                if (graphMatrix[i, j] != graphMatrix2[i, j])
                {
                    int ibreak = 0;
                }
            }
        }

        return graphMatrix2;
    }

    static void floydWarshall(int[,] graph)
    {
        int V = graph.GetLength(0);

        // Add all vertices one by one to
        // the set of intermediate vertices.
        for (int k = 0; k < V; k++)
        {

            // Pick all vertices as source one by one
            for (int i = 0; i < V; i++)
            {

                // Pick all vertices as destination
                // for the above picked source
                for (int j = 0; j < V; j++)
                {

                    // If vertex k is on the shortest path from
                    // i to j, then update the value of graph[i,j]

                    if ((graph[i, j] == -1 ||
                        graph[i, j] > (graph[i, k] + graph[k, j]))
                        && (graph[k, j] != -1 && graph[i, k] != -1))
                        graph[i, j] = graph[i, k] + graph[k, j];
                }
            }
        }
    }

}