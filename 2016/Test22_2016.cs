using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

public class Test22_2016 : BaseTest 
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 22;
    }

    Dictionary<IntVector2, DiskNode> m_diskMap = new Dictionary<IntVector2, DiskNode>();

    public override void Execute()
    {
        foreach(string line in m_dataFileContents)
        {
            if (line.StartsWith("/dev"))
            {
                string[] tokens  = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                /*
                 * /dev/grid/node-x0-y0     85T   65T    20T   76%
                 */
                string pattern = "\\/dev\\/grid\\/node-x([0-9]+)-y([0-9]+)";
                pattern = @"[\d]+";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(tokens[0]);
                int x = int.Parse(match.Value);
                int y = int.Parse(match.NextMatch().Value);
                
                IntVector2 pos = new  IntVector2(x,y);
                int size = int.Parse(tokens[1].Replace("T",""));
                int used = int.Parse(tokens[2].Replace("T",""));
                DiskNode diskNode = new DiskNode(){Position=pos,Capacity=size,Used=used};
                
                m_diskMap[pos] = diskNode;
               
            }
        }


        var viablieList = BuildViableList(m_diskMap);

        DebugOutput($"Total viable nodes is : {viablieList.Count}");


        if (IsPart2)
        {
            IntVector2 goalNode = new  IntVector2();
            foreach (IntVector2 n in m_diskMap.Keys)
            {
                if (n.X > goalNode.X && n.Y == 0)
                {
                    goalNode = n;
                }
            }

            m_diskMap[goalNode].GoalNode = true;

            DiskNode g = m_diskMap[goalNode];
            DiskNode lg = m_diskMap[new IntVector2(goalNode.X-1, goalNode.Y)];
            DiskNode dg = m_diskMap[new IntVector2(goalNode.X, goalNode.Y+1)];

            List<DiskNode> hasSpaceList =  new List<DiskNode>();
            foreach (DiskNode n in m_diskMap.Values)
            {
                if (n != g && n.Capacity >= g.Used)
                {
                    hasSpaceList.Add(n);
                }
            }

            DebugOutput(DrawMap(m_diskMap));
            
            
            // List<(IntVector2,IntVector2)> moveList = new List<(IntVector2,IntVector2)>();
            // Search(m_diskMap, moveList,0);
            //
            // DebugOutput($"Shortest move list for Part2 is : "+m_shortest);
        }
        
        
    }

    private int m_longestViableList = 0;
    List<(DiskNode, DiskNode)> BuildViableList(Dictionary<IntVector2, DiskNode> diskMap)
    {
        List<(DiskNode, DiskNode)> viableList = new List<(DiskNode, DiskNode)>();
        foreach (DiskNode diskNodeA in diskMap.Values)
        {
            foreach (DiskNode diskNodeB in diskMap.Values)
            {
                if (IsViable(diskNodeA, diskNodeB))
                {
                    viableList.Add((diskNodeA, diskNodeB));
                }
            }
        }

        if (viableList.Count > m_longestViableList)
        {
            m_longestViableList = viableList.Count;
        }
        return viableList;
    }
    
    Dictionary<IntVector2, DiskNode> CloneData(Dictionary<IntVector2, DiskNode> data)
    {
        Dictionary<IntVector2, DiskNode> clone = new Dictionary<IntVector2, DiskNode>();
        foreach (IntVector2 pos in data.Keys)
        {
            clone[pos] = (DiskNode)data[pos].Clone();
        }

        return clone;
    }
    
    //Node A is not empty (its Used is not zero).
    //Nodes A and B are not the same node.
    //The data on node A (its Used) would fit on node B (its Avail).

    public bool IsViable(DiskNode nodeA,DiskNode nodeB)
    {
        if (nodeA.Position == nodeB.Position)
        {
            return false;
        }

        if (nodeA.Used == 0)
        {
            return false;
        }

        if (nodeB.Avail < nodeA.Used)
        {
            return false;
        }

        if (IsPart2)
        {
            int distance = nodeA.Position.ManhattanDistance(ref nodeB.Position); 

            if (nodeA.Position.Y == 25 && nodeB.Position == new IntVector2(22, 25))
            {
                int ibreak = 0;
            }
            if ( distance != 1)
            {
                return false;
            }
        }
        
        return true;
    }

    public void MoveData(DiskNode nodeA, DiskNode nodeB)
    {
        //Debug.Assert(IsViable(nodeA,nodeB));
        nodeB.Used += nodeA.Used;
        nodeA.Used = 0;
        if (nodeA.GoalNode)
        {
            nodeB.GoalNode = true;
            nodeA.GoalNode = false;
        }
    }

    public int m_shortest = int.MaxValue;
    List<(IntVector2,IntVector2)> m_shortestPath = new List<(IntVector2,IntVector2)>();
    
    public void Search(Dictionary<IntVector2, DiskNode> diskMap,List<(IntVector2,IntVector2)> moves,int depth)
    {
        if (depth > 100)
        {
            return;
        }

        if (moves.Count > m_shortest)
        {
            return;
        }
        
        if (diskMap[IntVector2.Zero].GoalNode)
        {
            if (moves.Count < m_shortest)
            {
                m_shortest = moves.Count;
                DebugOutput($"Shortest path found: {m_shortest}");
                m_shortestPath.Clear();
                m_shortestPath.AddRange(moves);
            }

            return;
        }

        //DebugOutput(DrawMap(diskMap));
        //DebugOutput("");
        
        List<(DiskNode,DiskNode)> viableList = BuildViableList(diskMap);
        DiskNode goalNode = null;
        foreach (DiskNode diskNode in diskMap.Values)
        {
            if (diskNode.GoalNode)
            {
                goalNode = diskNode;
                break;
            }
        }

        
        //viableList.OrderBy(tuple => (Math.Min(tuple.Item1.Position.ManhattanDistance(goalNode.Position),tuple.Item2.Position.ManhattanDistance(goalNode.Position))));
        viableList.OrderBy(tuple => tuple.Item2.Capacity);
        foreach (var newMove in viableList)
        {
            // don't go backwards...
            // if (moves.Count > 0 && newMove.Item1.Position == moves.Last().Item2.Position &&
            //     newMove.Item2.Position == moves.Last().Item1.Position)
            // {
            //     continue;
            // }

            if (moves.Contains((newMove.Item1.Position, newMove.Item2.Position)) ||
                moves.Contains((newMove.Item2.Position, newMove.Item1.Position)))
            {
                continue;
            }
            
            Dictionary<IntVector2, DiskNode> diskMapClone = CloneData(diskMap);
            MoveData(diskMapClone[newMove.Item1.Position], diskMapClone[newMove.Item2.Position]);
            moves.Add((newMove.Item1.Position, newMove.Item2.Position));
            Search(diskMapClone,moves,depth+1);
            moves.RemoveAt(moves.Count - 1);
        }
    }

    public string DrawMap(Dictionary<IntVector2, DiskNode> diskMap)
    {
        StringBuilder sb = new StringBuilder();
        int maxX = 0;
        int maxY = 0;

        foreach (IntVector2 pos in diskMap.Keys)
        {
            maxX = Math.Max(maxX, pos.X);
            maxY = Math.Max(maxY, pos.Y);
        }
        
        for (int y = 0; y <= maxY; y++)
        {
            StringBuilder row = new StringBuilder();
            for (int x = 0; x <= maxX; x++)
            {
                IntVector2 pos =  new IntVector2(x, y);
                if (diskMap.ContainsKey(pos))
                {
                    if (pos == IntVector2.Zero)
                    {
                        row.Append('*');
                    }
                    else if (diskMap[pos].Used == 0)
                    {
                        row.Append('_');
                    }
                    else if (diskMap[pos].GoalNode)
                    {
                        row.Append('G');
                    }
                    else if (diskMap[pos].Capacity > 20 && diskMap[pos].UsePct >= 85)
                    {
                        row.Append('#');
                    }
                    else
                    {
                        row.Append('.');
                    }

                }
            }

            sb.AppendLine(row.ToString());
        }
        return sb.ToString();
    }
    
    
    public class DiskNode : ICloneable
    {
        public IntVector2 Position;
        public int Capacity;
        public int Used;
        public bool GoalNode;
        
        public int Avail
        {
            get{return Capacity - Used;}
        }

        public int UsePct
        {
            get
            {
                return (int)(Capacity >0?((float)Used/(float)Capacity)*100f:0);
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        
    }
}