public class Test20 : BaseTest
{
    public override void Initialise()
    {
        TestID = 20;
        IsTestInput = false;
        IsPart2 = false;
    }

    public override void Execute()
    {
        Dictionary<string, Module> moduleDictionary = new Dictionary<string, Module>();
        
        foreach (string line in m_dataFileContents)
        {
            Module module = new Module(line, moduleDictionary);
        }

        // create any dummy modules.
        HashSet<string> modulesNames = new HashSet<string>();
        foreach (Module m in moduleDictionary.Values)
        {
            foreach (string target in m.Targets)
            {
                modulesNames.Add(target);
            }
        }

        foreach (string moduleName in modulesNames)
        {
            if (!moduleDictionary.ContainsKey(moduleName))
            {
                Module module = new Module(ModuleType.None,moduleName, moduleDictionary);
                moduleDictionary[moduleName] = module;
            }
        }
        

        foreach (Module m in moduleDictionary.Values)
        {
            if (m.ModuleType == ModuleType.Conjunction)
            {
                foreach (Module n in moduleDictionary.Values)
                {
                    if (n.Targets.Contains(m.Id))
                    {
                        m.ConjunctionInputModules.Add((n.Id, false));
                    }
                }
            }
        }


        
        

        int numButtonPushes = 1000;

        for (int i = 0; i < numButtonPushes; ++i)
        {
            moduleDictionary["broadcaster"].ReceiveSignal("button", false);
            while (moduleDictionary.Values.Count(x => x.IsBusy) > 0)
            {
                foreach (Module m in moduleDictionary.Values)
                {
                    m.Process();
                }
            }
        }

        int totalLowSignalCount = moduleDictionary.Values.Sum(x => x.LowSignalCount);
        int totalHighSignalCount = moduleDictionary.Values.Sum(x => x.HighSignalCount);

        int totalPushes = (totalLowSignalCount + numButtonPushes) * totalHighSignalCount;

        DebugOutput("Total pushes was : " + totalPushes);
        
        int ibreak = 0;

    }
}


public class Module
{
    public string Id;
    public ModuleType ModuleType = ModuleType.None;
    private Dictionary<string, Module> m_moduleDictionary;
    public List<string> Targets = new List<string>();

    private bool m_state;

    public List<(string, bool)> ConjunctionInputModules = new List<(string, bool)>();

    private Queue<(string,bool)> m_signalQueue = new Queue<(string,bool)>();

    public int LowSignalCount = 0;
    public int HighSignalCount = 0;



    public Module(ModuleType moduleType, string id, Dictionary<string, Module> moduleDictionary)
    {
        ModuleType = moduleType;
        Id = id;
        m_moduleDictionary = moduleDictionary;
    }
    public Module(string line,Dictionary<string, Module> moduleDictionary)
    {
        m_moduleDictionary = moduleDictionary;
        
        string type = line.Substring(0, line.IndexOf("->"));
        type = type.Trim();
        
        string targets = line.Substring(line.IndexOf("->") + 2);
        Targets.AddRange(targets.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
        
     
        if (type == "broadcaster")
        {
            Id = type;
            ModuleType = ModuleType.Broadcast;
        }
        else if (type.StartsWith("%"))
        {
            ModuleType = ModuleType.FlipFlop;
            Id = type.Replace("%", "");
        }
        else if (type.StartsWith("&"))
        {
            ModuleType = ModuleType.Conjunction;
            Id = type.Replace("&", "");
        }

        
        moduleDictionary[Id] = this;

    }

    public void Process()
    {
        if (m_signalQueue.Count > 0)
        {
            var work = m_signalQueue.Dequeue();
            string senderId = work.Item1;
            bool isHigh = work.Item2;


            if (ModuleType == ModuleType.FlipFlop)
            {
                if (!isHigh)
                {
                    m_state = !m_state;
                    SendSignal(m_state);
                }
            }
            else if (ModuleType == ModuleType.Conjunction)
            {
                int index = ConjunctionInputModules.FindIndex(x => x.Item1 == senderId);
                ConjunctionInputModules[index] = (senderId, isHigh);

                // if all the values are high then send a low signal
                bool newSignal = ConjunctionInputModules.Count(x => x.Item2) != ConjunctionInputModules.Count;
                SendSignal(newSignal);
            }
            else if (ModuleType == ModuleType.Broadcast)
            {
                SendSignal(isHigh);
            }
        }
    }    
    public void ReceiveSignal(string senderId,bool isHigh)
    {
        m_signalQueue.Enqueue((senderId, isHigh));
    }

    private void SendSignal(bool isHigh)
    {
        foreach (string target in Targets)
        {
            m_moduleDictionary[target].ReceiveSignal(Id, isHigh);
            if (isHigh)
            {
                HighSignalCount++;
            }
            else
            {
                LowSignalCount++;
            }
        }
    }

    public bool IsBusy
    {
        get { return m_signalQueue.Count > 0; }
    }

    
}


public enum ModuleType
{
    None,
    Button,
    Broadcast,
    FlipFlop,
    Conjunction
}