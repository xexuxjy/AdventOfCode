using System;
using System.Collections.Generic;

public class Test24_2016 : BaseTest,IMapDataInt
{
    Dictionary<(IntVector2,IntVector2), List<IntVector2>> m_shortestPathDictionary = new Dictionary<(IntVector2,IntVector2), List<IntVector2>>();
    List<IntVector2> m_checkPoints = new List<IntVector2>();

    public override void Initialise()
    {
        Year = 2016;
        TestID = 24;
    }

    public override void Execute()
    {
        IntVector2 startPoint = new IntVector2(0, 0);
        // find all pairs from x to y.
        for (int y = 0; y < m_dataFileContents.Count; ++y)
        {
            for (int x = 0; x < m_dataFileContents[y].Length; ++x)
            {
                if (char.IsDigit(m_dataFileContents[y][x]))
                {
                    if (m_dataFileContents[y][x] == '0')
                    {
                        startPoint = new IntVector2(x, y);
                    }
                    m_checkPoints.Add(new IntVector2(x, y));
                }
            }
        }
        
        var results = Combinations.CombinationsRosettaWoRecursion(m_checkPoints.ToArray(),2);

        foreach (var result in results)
        {
            AStarInt astar = new AStarInt();
            astar.SearchMethod = SearchMethod.AStar;
            astar.Initialize(this);
            List<IntVector2> results2 = new List<IntVector2>();
            astar.FindPath(result[0], result[1],results2);
            
            m_shortestPathDictionary[(result[0], result[1])] = results2;
            m_shortestPathDictionary[(result[1], result[0])] = results2;
        }


        m_checkPoints.Remove(startPoint);
        
        var allPermutations = Combinations.GetPermutations(m_checkPoints.ToArray());
        foreach (var permutation in allPermutations)
        {
            List<IntVector2> permutationsFromStart = new List<IntVector2>();
            permutationsFromStart.Add(startPoint);
            permutationsFromStart.AddRange(permutation);

            if (IsPart2)
            {
                permutationsFromStart.Add(startPoint);
            }
            
            int length = 0;
            for (int i = 0; i < permutationsFromStart.Count - 1;i++)
            {
                List<IntVector2> path = m_shortestPathDictionary[(permutationsFromStart[i], permutationsFromStart[i + 1])];
                length += path.Count;
                //if (i > 0 && i< permutation.Length - 2)
                //if (i > 0)
                {
                    length--;
                }
            }

            //DebugOutput(string.Join(' ',permutationsFromStart)+" == "+length);

            if (length < m_shortestPath)
            {
                m_shortestPath = length;
            }
        }
        
        
        
        //TestPossibility(startPoint,new HashSet<IntVector2>(),0,0);

        DebugOutput($"Shortest path is : {m_shortestPath}");
        
        int ibreak = 0;
    }

    public int m_shortestPath = int.MaxValue;
    
    public void TestPossibility(IntVector2 startPos,HashSet<IntVector2> visitedCheckPoints,int count,int depth)
    {
        if (depth > 150)
        {
            return;
        }
        if (visitedCheckPoints.Count == m_checkPoints.Count)
        {
            if (count < m_shortestPath)
            {
                m_shortestPath = count;
            }
            return;
        }
        
        foreach (var key in m_shortestPathDictionary.Keys)
        {
            if (key.Item1 == startPos)
            {
                int countCopy = count;
                HashSet<IntVector2> visitedCheckPointsCopy = new HashSet<IntVector2>();
                foreach (IntVector2 v in visitedCheckPoints)
                {
                    visitedCheckPointsCopy.Add(v);
                }

                //if(!visitedCheckPointsCopy.Contains(key.Item1) || !visitedCheckPointsCopy.Contains(key.Item2))
                {
                    List<IntVector2> path = m_shortestPathDictionary[key];
                    //countCopy += (path.Count);
                    bool addedPoint = false;
                    foreach (IntVector2 p in path)
                    {
                        countCopy++;
                        if (m_checkPoints.Contains(p) && !visitedCheckPointsCopy.Contains(p))
                        {
                            visitedCheckPointsCopy.Add(p);
                            addedPoint = true;
                            if (visitedCheckPointsCopy.Count == m_checkPoints.Count)
                            {
                                if (countCopy < m_shortestPath)
                                {
                                    m_shortestPath = countCopy;
                                }
                                return;
                            }
                        }
                    }

                    if (addedPoint)
                    {
                        TestPossibility(key.Item2, visitedCheckPointsCopy, countCopy, depth + 1);
                    }
                }
            }
        }

    }
    
    public bool CanMove(IntVector2 from, IntVector2 to)
    {
        return m_dataFileContents[to.Y][to.X] != '#';
    }

    public IntVector2[] GetDirections()
    {
        return IntVector2.Directions;
    }

    public float DistanceToTarget(IntVector2 v, IntVector2 t)
    {
        return v.ManhattanDistance(t);
    }
   
}