public class Test25 : BaseTest
{
    public override void Initialise()
    {
        TestID = 25;
        IsTestInput = true;
        IsPart2 = false;
    }

    public override void Execute()
    {
        List<Component> components = new List<Component>();
        Dictionary<string, Component> componentMap = new Dictionary<string, Component>();

        foreach (string line in m_dataFileContents)
        {
            Component c = new Component(line);
            components.Add(c);
            componentMap[c.Id] = c;
        }

        foreach (Component c in components)
        {
            foreach (string destination in c.Connections)
            {
                if (!componentMap.ContainsKey(destination))
                {
                    Component newComponent = new Component(destination + ":");
                    componentMap[destination] = newComponent;

                }

                // add reverse connections?
                //componentMap[destination].Connections.Add(c.Id);

            }
        }

        bool[] adjacencytGrid = BuildAdjacencyMatrix(componentMap);
        bool[] adjacencytGridBothDirections = BuildAdjacencyMatrix(componentMap,true);
        bool[] reduced = ReduceAdjacencyMatrix(adjacencytGridBothDirections, componentMap.Keys.Count);
        
        DebugOutput((Helper.DrawGrid(adjacencytGrid, componentMap.Keys.Count, componentMap.Keys.Count)));
        DebugOutput((Helper.DrawGrid(adjacencytGridBothDirections, componentMap.Keys.Count, componentMap.Keys.Count)));
        DebugOutput((Helper.DrawGrid(reduced, componentMap.Keys.Count, componentMap.Keys.Count)));
            
        List<string> componentIds = new List<string>();
        componentIds.AddRange(componentMap.Keys);
        componentIds.Sort();

        for (int i = 0; i < componentIds.Count; ++i)
        {
            string line = componentIds[i] + "-> " ;
            for (int j = 0; j < componentIds.Count; ++j)
            {
                if (reduced[i + (j * componentIds.Count)])
                {
                    line += componentIds[j] + " ";
                }
            }
            DebugOutput(line);
        }
        
        
        
        
        Dictionary<string, HashSet<string>> reachables = new Dictionary<string, HashSet<string>>();
        foreach (string id in componentMap.Keys)
        {
            HashSet<string> reachable = new HashSet<string>();
            BuildReachableComponents(id, reachable, componentMap);
            reachables[id] = reachable;
        }

        int ibreak = 0;
    }

    public void BuildReachableComponents(string startId, HashSet<String> reachable, Dictionary<string, Component> map)
    {
        Component c = map[startId];
        if (!reachable.Contains(c.Id))
        {
            reachable.Add(c.Id);
            foreach (string destination in c.Connections)
            {
                BuildReachableComponents(destination, reachable, map);
            }
        }
    }

    public bool[] BuildAdjacencyMatrix(Dictionary<string, Component> map,bool bothDirections = false)
    {
        List<string> componentIds = new List<string>();
        componentIds.AddRange(map.Keys);
        componentIds.Sort();
        
        bool[] result = new bool[map.Keys.Count * map.Keys.Count];

        for (int i = 0; i < componentIds.Count; ++i)
        {
            foreach (string connection in map[componentIds[i]].Connections)
            {
                int connectionIndex = componentIds.IndexOf(connection);
                result[i + (connectionIndex * componentIds.Count)] = true;

                if (bothDirections)
                {
                    result[connectionIndex + (i * componentIds.Count)] = true;
                }
            }
        }
        
        
        
        return result;
    }


    public bool[] ReduceAdjacencyMatrix(bool[] original, int width)
    {
        bool[] result = new bool[original.Length];
        Array.Copy(original, result, original.Length);

        // reflexive reduction
        for (int i = 0; i < width; ++i)
        {
            result[(i * width) + i] = false;

        }

        // transitive reduction
        for (int j = 0; j < width; ++j)
        {
            for (int i = 0; i < width; ++i)
            {
                if (result[(i * width) + j])
                {
                    for (int k = 0; k < width; ++k)
                    {
                        if (result[(j * width) + k])
                        {
                            result[(i * width) + k] = false;
                        }
                    }
                }
            }
        }

        return result;
    }

    public class Component
    {
        public string Id;
        public HashSet<string> Connections = new HashSet<string>();
        private string m_raw;
        
        public Component(string dataLine)
        {
            m_raw = dataLine;
            string[] tokens = dataLine.Split(":");
            Id = tokens[0];
            string[] connections = tokens[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string c in connections)
            {
                Connections.Add(c);
            }
        }

        public override string ToString()
        {
            return m_raw;
        }
    }
}