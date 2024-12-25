using System.Net.Sockets;

public class Test24_2024 : BaseTest
{

    public override void Initialise()
    {
        Year = 2024;
        TestID = 24;
    }

    Dictionary<string, Wire> WireDictionary = new Dictionary<string, Wire>();


    public override void Execute()
    {
        List<Gate> allGates = new List<Gate>();



        int lineNo = 0;
        for (lineNo = 0; lineNo < m_dataFileContents.Count; ++lineNo)
        {
            if (m_dataFileContents[lineNo] == "")
            {
                break;
            }

            string[] tokens = m_dataFileContents[lineNo].Split(':');
            string inputId = tokens[0];
            bool inputValue = int.Parse(tokens[1]) == 1;
            Wire wire = new Wire() { Id = inputId, Value = inputValue };
            WireDictionary[inputId] = wire;


        }
        lineNo++;
        for (; lineNo < m_dataFileContents.Count; ++lineNo)
        {
            string[] tokens = m_dataFileContents[lineNo].Split(' ');

            Wire input1 = GetOrCreateWire(tokens[0]);
            Wire input2 = GetOrCreateWire(tokens[2]);
            Wire output = GetOrCreateWire(tokens[4]);

            Gate gate = new Gate() { Input1 = input1, Input2 = input2, GateType = tokens[1], Output = output };
            allGates.Add(gate);


        }


        //string desiredResult = "100";
        //string currentResult = "";
        int numIterations = 0;
        bool haveFinalResult = false;
        string gateResult = "";
        while(!haveFinalResult)
        {
            foreach(Gate g in allGates)
            {
                g.Calculate();
            }

            gateResult = CalculateState();
            
            bool complete = true;
            foreach(String key in WireDictionary.Keys.Where(x=>x.StartsWith("z")))
            {
                if(!WireDictionary[key].Value.HasValue)
                {
                    complete =false;
                    break;
                }
            }

            if(complete)
            {
                haveFinalResult = true;
            }

            numIterations++;
        }
        DebugOutput($"After all z wires have value we had result of {Convert.ToInt64(gateResult,2)} reached after {numIterations} steps");
        //string[] input = line
        int ibreak = 0;

    }

    public string CalculateState()
    {
        string result = "";
        foreach(String key in WireDictionary.Keys.Where(x=>x.StartsWith("z")).OrderDescending())
        {
            string resultChar = "*";
            if(WireDictionary[key].Value.HasValue)
            {
                resultChar = WireDictionary[key].Value.Value?"1":"0";
            }
            //result += $"{key} = {resultChar} ,";
            result+=resultChar;
        }

        //DebugOutput(result);
        return result;
    }

    public Wire GetOrCreateWire(string id)
    {
        if(!WireDictionary.TryGetValue(id, out Wire wire))
        {
            wire = new Wire() { Id = id };
            WireDictionary[id] = wire;
        }
        return wire;
    }

    public class Wire
    {
        public string Id;
        public bool? Value;
    }


    public class Gate
    {
        public Wire Input1;
        public Wire Input2;
        public string GateType;
        public Wire Output;



        public void Calculate()
        {
            if (Input1.Value.HasValue && Input2.Value.HasValue)
            {
                if (GateType == "AND")
                {
                    Output.Value = Input1.Value.Value && Input2.Value.Value;
                }
                else if (GateType == "OR")
                {
                    Output.Value = Input1.Value.Value || Input2.Value.Value;
                }
                if (GateType == "XOR")
                {
                    Output.Value = Input1.Value.Value ^ Input2.Value.Value;
                }
            }
        }


    }
}