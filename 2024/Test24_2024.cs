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
            Wire wire = new Wire() { Id = inputId, Value = inputValue, InitialValue = inputValue };
            WireDictionary[inputId] = wire;


        }
        lineNo++;
        for (; lineNo < m_dataFileContents.Count; ++lineNo)
        {
            string[] tokens = m_dataFileContents[lineNo].Split(' ');

            Wire input1 = GetOrCreateWire(tokens[0]);
            Wire input2 = GetOrCreateWire(tokens[2]);
            Wire output = GetOrCreateWire(tokens[4]);

            Gate gate = new Gate() { m_input1 = input1, m_input2 = input2, GateType = tokens[1], Output = output };
            allGates.Add(gate);


        }
        if (IsPart2)
        {
            Reset();
            //DebugOutput(DisplayState("",true));

            int val1 = 5;
            int val2 = 7;

            SetInputGateToValue(val1,"x");
            SetInputGateToValue(val2,"y");
            DebugOutput(DisplayState("",true));

            long result = 0;
            int escape = 10000;
            int count = 0;
            while(result != val1+val2)
            {
                foreach (Gate g in allGates)
                {
                    g.Calculate();
                }
                result = GetGateValue("z");
                count++;
                if(count > escape)
                {
                    break;
                }
            }
            DebugOutput(DisplayState("",true));

            if(result == val1+val2)
            {
                DebugOutput($"Gates worked for {val1} + {val2} = {(val1+val2)}");
            }



        }
        else
        {

            //string desiredResult = "100";
            //string currentResult = "";
            int numIterations = 0;
            bool haveFinalResult = false;
            string gateResult = "";
            while (!haveFinalResult)
            {
                foreach (Gate g in allGates)
                {
                    g.Calculate();
                }

                bool complete = true;
                foreach (String key in WireDictionary.Keys.Where(x => x.StartsWith("z")))
                {
                    if (!WireDictionary[key].Value.HasValue)
                    {
                        complete = false;
                        break;
                    }
                }

                if (complete)
                {
                    haveFinalResult = true;
                }

                numIterations++;
            }
            DebugOutput($"After all z wires have value we had result of {Convert.ToInt64(DisplayState(), 2)} reached after {numIterations} steps");
            //string[] input = line
            int ibreak = 0;
        }

    }

    public void SetInputGateToValue(long value, string gatePrefix)
    {
        int maxIndex = int.Parse(WireDictionary.Keys.Where(x => x.StartsWith(gatePrefix)).OrderDescending().First().Replace(gatePrefix, ""));
        maxIndex+=1;

        string stringValue = Convert.ToString(value, 2);
        stringValue = new string(stringValue.Reverse().ToArray());
        for (int i = 0; i < maxIndex; i++)
        {
            string id = gatePrefix + i.ToString("D2");

            if(i < stringValue.Length)
            {
                WireDictionary[id].Value = stringValue[i] == '1' ? true : false;
            }
            else
            {
                WireDictionary[id].Value = false;
            }

        }


    }

    public long GetGateValue(string gatePrefix)
    {
        string temp = DisplayState(gatePrefix);
        temp = temp.Replace("*","");
        return Convert.ToInt64(temp,2);
    }


    public void Reset()
    {
        foreach (Wire w in WireDictionary.Values)
        {
            w.Reset();
        }
    }

    public string DisplayState(string prefix="z",bool full=false)
    {
        string result = "";
        foreach (String key in WireDictionary.Keys.Where(x => x.StartsWith(prefix)).OrderDescending())
        {
            string resultChar = "*";
            if (WireDictionary[key].Value.HasValue)
            {
                resultChar = WireDictionary[key].Value.Value ? "1" : "0";
            }
            //result += $"{key} = {resultChar} ,";
            result += (full?$"{key} = {resultChar}\n":resultChar);
        }

        //DebugOutput(result);
        return result;
    }

    public Wire GetOrCreateWire(string id)
    {
        if (!WireDictionary.TryGetValue(id, out Wire wire))
        {
            wire = new Wire() { Id = id };
            WireDictionary[id] = wire;
        }
        return wire;
    }

    public class Wire
    {
        public string Id;
        public bool? InitialValue;
        public bool? Value;


        public void Reset()
        {
            Value = InitialValue;
        }
    }


    public class Gate
    {
        public Wire m_input1;
        public Wire m_input2;

        public bool Swapped;

        public string GateType;
        public Wire Output;


        public Wire Input1
        {
            get { return Swapped ? m_input2 : m_input1; }
        }

        public Wire Input2
        {
            get { return Swapped ? m_input1 : m_input2; }
        }


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