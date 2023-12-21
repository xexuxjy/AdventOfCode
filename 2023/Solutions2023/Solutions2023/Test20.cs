public class Test20 : BaseTest
{
    public override void Initialise()
    {
        TestID = 20;
        IsTestInput = false;
        IsPart2 = true;
    }

    public static bool RXLow = false;

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
                Module module = new Module(ModuleType.None, moduleName, moduleDictionary);
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

        List<string> kl = new List<string>();
        // kl.AddRange(moduleDictionary.Keys);
        // kl.Sort();

        
        //block 1  3792 reset 3793
        kl.Add("tf");
        kl.Add("gz");
        kl.Add("fl");
        kl.Add("mq");
        kl.Add("kb");
        kl.Add("hq");
        kl.Add("vr");
        kl.Add("zj");
        kl.Add("td");
        kl.Add("qn");
        kl.Add("jg");
        kl.Add("cb");
        kl.Add("gr");

       
        // block2 3880 reset 3881
        // kl.Add("lr");
        // kl.Add("gx");
        // kl.Add("xl");
        // kl.Add("rd");
        // kl.Add("hm");
        // kl.Add("gk");
        // kl.Add("bq");
        // kl.Add("hj");
        // kl.Add("rm");
        // kl.Add("xv");
        // kl.Add("dp");
        // kl.Add("dc");
        // kl.Add("js");

        
        // block 3846 reset 3847
        // kl.Add("sn");
        // kl.Add("tv");
        // kl.Add("tn");
        // kl.Add("xd");
        // kl.Add("bt");
        // kl.Add("nl");
        // kl.Add("sd");
        // kl.Add("nc");
        // kl.Add("qb");
        // kl.Add("th");
        // kl.Add("vf");
        // kl.Add("vp");
        // kl.Add("ng");

        
        // block 4 3760 reset 3761
        // kl.Add("hl");
        // kl.Add("tm");
        // kl.Add("px");
        // kl.Add("kg");
        // kl.Add("tl");
        // kl.Add("xb");
        // kl.Add("fp");
        // kl.Add("hs");
        // kl.Add("qk");
        // kl.Add("hb");
        // kl.Add("xx");
        // kl.Add("nj");
        // kl.Add("lb");

        
        
        // oddblock
        // kl.Add("lr");
        // kl.Add("tf");
        // kl.Add("hl");
        // kl.Add("sn");
        //
        // kl.Add("ss");
        // kl.Add("fz");
        // kl.Add("mf");
        // kl.Add("fh");
        //
        // kl.Add("ql");
        //
        
        int totalPushes = 0;

        if (IsPart2)
        {
            List<long> terms = new List<long>();
            terms.Add(3793);
            terms.Add(3881);
            terms.Add(3847);
            terms.Add(3761);

            long lcm = Helper.LCM(terms);

            DebugOutput("Part2 button count is : " + lcm);
        }
        else
        {
            long numButtonPushes = 3793;

            string debug = "";
            foreach (string key in kl)
            {
                //debug += $"{key} {(moduleDictionary[key].State?1:0)}  ";
                debug += $"{key} ";
            }
            DebugOutput(debug);

            
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

                 debug = "";
                 foreach (string key in kl)
                 {
                     //debug += $"{key} {(moduleDictionary[key].State?1:0)}  ";
                     debug += $"{(moduleDictionary[key].State?1:0)}  ";
                 }
                 DebugOutput(debug);
                string check = "fz";
                if (moduleDictionary[check].State)
                {
                    DebugOutput($"Reached {check} 1 at : " +i);
                    break;
                }
                
                
            }

            debug = "";
            foreach (string key in kl)
            {
                //debug += $"{key} {(moduleDictionary[key].State?1:0)}  ";
                debug += $"{(moduleDictionary[key].State?1:0)}  ";
            }
            DebugOutput(debug);

            
            // int totalLowSignalCount = moduleDictionary.Values.Sum(x => x.LowSignalCount);
            // int totalHighSignalCount = moduleDictionary.Values.Sum(x => x.HighSignalCount);
            //
            // totalPushes = (totalLowSignalCount + numButtonPushes) * totalHighSignalCount;
        }

        
        
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

    private Queue<(string, bool)> m_signalQueue = new Queue<(string, bool)>();

    public int LowSignalCount = 0;
    public int HighSignalCount = 0;


    public Module(ModuleType moduleType, string id, Dictionary<string, Module> moduleDictionary)
    {
        ModuleType = moduleType;
        Id = id;
        m_moduleDictionary = moduleDictionary;
    }

    public Module(string line, Dictionary<string, Module> moduleDictionary)
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


    public bool State
    {
        get { return m_state; }
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
                if (Id == "tf" && newSignal)
                {
                    int ibreak = 0;
                }

                
                SendSignal(newSignal);
            }
            else if (ModuleType == ModuleType.Broadcast)
            {
                SendSignal(isHigh);
            }
        }
    }

    public void ReceiveSignal(string senderId, bool isHigh)
    {
        // yuck.
        if (Id == "rx" && !isHigh)
        {
            Test20.RXLow = true;
        }

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


    public int LowItemsOnQueue
    {
        get { return m_signalQueue.Count(x => x.Item2 == false); }
    }

    public int HighItemsOnQueue
    {
        get { return m_signalQueue.Count(x => x.Item2 == true); }
    }
}


public enum ModuleType
{
    None,
    Button,
    Broadcast,
    FlipFlop,
    Conjunction,
}