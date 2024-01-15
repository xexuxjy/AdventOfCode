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
                componentMap[destination].Connections.Add(c.Id);

            }
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