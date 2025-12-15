using System;
using System.Collections.Generic;

public class Test11_2025 : BaseTest
{
    public Dictionary<string,Node> NodeLookup =  new Dictionary<string,Node>();
    public Dictionary<string,HashSet<string>> ReverseNodeLookup = new Dictionary<string,HashSet<string>>();
    
    
    public override void Initialise()
    {
        Year = 2025;
        TestID = 11;
    }

    public override void Execute()
    {
        foreach (string line in m_dataFileContents)
        {
            Node node = new Node();
            string[] tokens = line.Split(new char[] { ':',' '},StringSplitOptions.RemoveEmptyEntries);
            node.Id = tokens[0];
            NodeLookup[node.Id] = node;
            for (int i = 1; i < tokens.Length; i++)
            {
                node.Connections.Add(tokens[i]);
            }
        }
        Node outNode = new Node();
        outNode.Id = "out";
        NodeLookup[outNode.Id] = outNode;

        foreach (Node node in NodeLookup.Values)
        {
            HashSet<string> reverseSet;
            if (!ReverseNodeLookup.TryGetValue(node.Id, out reverseSet))
            {
                reverseSet = new HashSet<string>();
                ReverseNodeLookup[node.Id] = reverseSet;
            }
            
            foreach (Node node2 in NodeLookup.Values)
            {
                if (node2.Connections.Contains(node.Id))
                {
                    reverseSet.Add(node2.Id);
                }
            }
        }
        Node startNode =  IsPart1?NodeLookup["you"]:NodeLookup["svr"];
        Node outNodeLookup =  NodeLookup["out"];
        Node dacNodeLookup =  NodeLookup["dac"];
        Node fftNodeLookup =  NodeLookup["fft"];

        
        List<string> routes = new List<string>();

        
        

        if (IsPart1)
        {
            FindRoute(startNode, outNodeLookup, routes,0);
            DebugOutput($"There are {AllRoutes.Count} possible routes.");

        }
        else
        {
        
            TargetCache.Clear();
            long numStartToFFT = FindRouteCache(startNode, fftNodeLookup, 0);
            
            TargetCache.Clear();
            long numFFTToDAC = FindRouteCache(fftNodeLookup, dacNodeLookup, 0);

            TargetCache.Clear();
            long numDACToOut = FindRouteCache(dacNodeLookup, outNodeLookup, 0);

            long total = numStartToFFT * numFFTToDAC * numDACToOut;
            
            DebugOutput($"There are {total} possible routes.");
            
            // FindRouteReverse(outNodeLookup,startNode, routes,0);
            // AllRoutes.RemoveAll(x => !(x.Contains("fft") && x.Contains("dac")));
        }
        
        
        int ibreak = 0;
    }

    
    
    
    public List<List<string>> AllRoutes = new List<List<string>>();
    public bool FindRoute(Node currentNode, Node targetNode,List<string> route,int depth,bool stopAtFirst=false)
    {
        // depth check?
        if (depth > 100)
        {
            DebugOutput($"Failed depth test");
            return false;
        }
    
        if (currentNode == targetNode)
        {
            List<string> succesfulRoute = new List<string>();
            succesfulRoute.AddRange(route);
            AllRoutes.Add(succesfulRoute);
            
            return true;
        }

        // look for cyclic routes?

        if (route.Contains(currentNode.Id))
        {
            DebugOutput($"Hit cyclic route {currentNode.Id} : {string.Join(',', route)}");
        }
        
        
        bool found = false;
        route.Add(currentNode.Id);
        foreach (string connection in currentNode.Connections)
        {
            if (stopAtFirst && AllRoutes.Count > 0)
            {
                return true;
            }
            
            if (FindRoute(NodeLookup[connection], targetNode,route,depth+1,stopAtFirst))
            {
                found = true;
            }
        }
        if(found)
        
        route.Remove(route.Last());
        return found;
    }

    public Dictionary<Node,long> TargetCache = new Dictionary<Node,long>();
    
    public long FindRouteCache(Node currentNode, Node targetNode,int depth)
    {
        // depth check?
        if (depth > 100)
        {
            DebugOutput($"Failed depth test");
            return 0L;
        }

        if (TargetCache.ContainsKey(currentNode))
        {
            return TargetCache[currentNode];
        }
        
        if (currentNode == targetNode)
        {
            return 1;
        }

        // look for cyclic routes?

        
        long targetCount = 0L;
        foreach (string connection in currentNode.Connections)
        {
            targetCount += FindRouteCache(NodeLookup[connection], targetNode, depth + 1);
        }

        TargetCache[currentNode] = targetCount;
        
        return targetCount;
    }

    
    
    
    

    public bool FindRouteReverse(Node currentNode, Node targetNode,List<string> route,int depth)
    {
        // depth check?
        if (depth > 100)
        {
            DebugOutput($"Failed depth test");
            return false;
        }
        
        if (currentNode == targetNode)
        {
            List<string> succesfulRoute = new List<string>();
            succesfulRoute.AddRange(route);
            AllRoutes.Add(succesfulRoute);
            
            return true;
        }

        // look for cyclic routes?

        if (route.Contains(currentNode.Id))
        {
            DebugOutput($"Hit cyclic route {currentNode.Id} : {string.Join(',', route)}");
        }
        
        
        bool found = false;
        route.Add(currentNode.Id);
        foreach (string connection in ReverseNodeLookup[currentNode.Id])
        {
            if (FindRouteReverse(NodeLookup[connection], targetNode,route,depth+1))
            {
                found = true;
            }
        }
        route.Remove(route.Last());
        return found;
    }

    

    public class Node
    {
        public string Id;
        public List<string> Connections = new List<string>();
    }
}