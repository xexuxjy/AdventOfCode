using System;
using System.Collections.Generic;

public class Test12_2017 : BaseTest
{
    public static Dictionary<int,ProgramNode> ProgramNodes = new Dictionary<int,ProgramNode>();

    
    public override void Initialise()
    {
        Year = 2017;
        TestID = 12;
    }

    public override void Execute()
    {
        foreach (string line in m_dataFileContents)
        {
            string[] split = line.Split("<->", StringSplitOptions.RemoveEmptyEntries);
            int id =  int.Parse(split[0]);
            string[] targets = split[1].Split(',');
            ProgramNode node = new ProgramNode();
            node.Id = id;
            foreach (string target in targets)
            {
                node.Connections.Add(int.Parse(target));
            }
            ProgramNodes[id] = node;
        }
     
        // build two way connections.
        foreach (ProgramNode node in ProgramNodes.Values)
        {
            foreach (int connection in node.Connections)
            {
                ProgramNodes[connection].Connections.Add(node.Id);
            }
        }

        if (IsPart1)
        {
            int targetNode = 0;
            List<int> results = PerformSearch(targetNode);
            DebugOutput($"We can reach a total of {results.Count} / {ProgramNodes.Count}  values");            
        }
        else
        {
            List<int> availableValues = new List<int>();
            availableValues.AddRange(ProgramNodes.Keys);
            int groups = 0;
            while (availableValues.Count > 0)
            {
                List<int> programs = PerformSearch(availableValues.First());
                if (programs.Count > 0)
                {
                    groups++;
                    //DebugOutput(string.Join(", ", programs));
                    availableValues.RemoveAll(programs.Contains);
                }
            }
            DebugOutput($"There are {groups} different groups");
        }
        


    }

    public List<int> PerformSearch(int targetNode)
    {
        List<int> results = new List<int>();
        foreach (ProgramNode node in ProgramNodes.Values)
        {
            List<int> moveList = new List<int>();
            if (CanReach(node.Id, targetNode, new HashSet<IntVector2>(),moveList))
            {
                results.Add(node.Id);
                //DebugOutput($"{node.Id} -> {string.Join(',',moveList)}");
            }
        }
        return results;
    }
    
    
    public bool CanReach(int nodeId,int targetNode,HashSet<IntVector2> visited,List<int> path)
    {
        path.Add(nodeId);
        bool reachable = false;

        ProgramNode node = ProgramNodes[nodeId];
        if (node.Connections.Contains(targetNode))
        {
            reachable = true;
        }
        else
        {
            foreach (int connection in node.Connections)
            {
                if (!visited.Contains(new IntVector2(nodeId, connection)))
                {
                    visited.Add(new IntVector2(nodeId, connection));
                    visited.Add(new IntVector2( connection,nodeId));

                    if (CanReach(connection, targetNode, visited, path))
                    {
                        reachable = true;
                        break;
                    }
                }
            }
        }

        if (!reachable)
        {
            path.Remove(path.Last());
        }
        return reachable;
    }
    
}

public class ProgramNode
{
    public int Id;
    public HashSet<int> Connections = new HashSet<int>();
}