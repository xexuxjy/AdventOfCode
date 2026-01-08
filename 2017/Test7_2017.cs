using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

public class Test7_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 7;
    }

    public override void Execute()
    {
        Dictionary<string, DiskNode> diskNodeDictionary = new Dictionary<string, DiskNode>();

        foreach (string line in m_dataFileContents)
        {
            string diskName = line.Substring(0, line.IndexOf(' '));
            string weight = Helper.SubstringBetween(line, "(", ")");
            int diskWeight = int.Parse(weight);

            DiskNode diskNode = new DiskNode() { Name = diskName, Weight = diskWeight };
            diskNodeDictionary.Add(diskName, diskNode);

            string holds = "->";
            if (line.Contains(holds))
            {
                string supportsString = line.Substring(line.IndexOf(holds) + holds.Length);
                string[] supports = supportsString.Split(',');
                foreach (string child in supports)
                {
                    diskNode.ChildNames.Add(child.Trim());
                }
            }
        }

        foreach (DiskNode diskNode in diskNodeDictionary.Values)
        {
            foreach (string child in diskNode.ChildNames)
            {
                diskNode.Children.Add(diskNodeDictionary[child]);
            }
        }


        foreach (DiskNode diskNode in diskNodeDictionary.Values)
        {
            foreach (DiskNode possibleParent in diskNodeDictionary.Values)
            {
                if (possibleParent.ChildNames.Contains(diskNode.Name))
                {
                    diskNode.HasParent = true;
                    break;
                }
            }
        }

        DiskNode rootNode = null;
        foreach (DiskNode diskNode in diskNodeDictionary.Values)
        {
            if (!diskNode.HasParent)
            {
                rootNode = diskNode;
                DebugOutput($"Found root node at {rootNode.Name}");
            }
        }

        if (IsPart2)
        {
            int lowestDepth = 0;
            int mismatch = 0;

            rootNode.AssignDepth(0);
            
            DiskNode unblancedNode = null;
            
            DebugOutput($"Total weight {rootNode.ChildWeight}");
            foreach (DiskNode diskNode in diskNodeDictionary.Values)
            {
                int? modeValue = diskNode.Children.Select(x => x.ChildWeight).GroupBy(x => x)
                    .OrderByDescending(x => x.Count()).ThenBy(x => x.Key)
                    .Select(x => (int?)x.Key)
                    .FirstOrDefault();

                if (modeValue.HasValue)
                {
                    foreach (DiskNode child in diskNode.Children)
                    {
                        if (child.ChildWeight != modeValue.Value)
                        {
                            if (child.Depth > lowestDepth)
                            {
                                unblancedNode = child;
                                lowestDepth = child.Depth;
                                mismatch = unblancedNode.Weight + (modeValue.Value - child.ChildWeight);
                            }
                        }
                    }
                }
            }

            if (unblancedNode != null)
            {
                DebugOutput($"Found unbalanced child at {unblancedNode.Name} need to have weight  {mismatch}");

            }
            
        }
    }
}

public class DiskNode
{
    public string Name;
    public int Weight;
    public bool HasParent = false;
    public List<string> ChildNames = new List<string>();
    public List<DiskNode> Children = new List<DiskNode>();
    public int Depth;
    
    private int m_childWeight = 0;

    public void AssignDepth(int depth)
    {
        Depth = depth;
        foreach (DiskNode child in Children)
        {
            child.AssignDepth(depth + 1);
        }
    }
    
    public int ChildWeight
    {
        get
        {
            if (m_childWeight == 0)
            {
                m_childWeight = Weight;
                foreach (DiskNode child in Children)
                {
                    m_childWeight += child.ChildWeight;
                }
            }

            return m_childWeight;
        }
    }
}