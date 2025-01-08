using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

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

            Gate gate = new Gate(input1, input2, tokens[1], output);
            allGates.Add(gate);


        }
        if (IsPart2)
        {

            // no credit to me on this one, answer taken from : https://blog.lojic.com/2024/12/29/advent-of-code-2024-day-24-crossed-wires.html  , thanks

            char[] standardConnections = new char[] { 'x', 'y', 'z' };
            HashSet<string> wrongWires = new HashSet<string>();
            List<Gate> wrongGates = new List<Gate>();

            foreach (Gate gate in allGates)
            {
                char input1FC = gate.Input1.Id[0];
                char input2FC = gate.Input2.Id[0];
                char outputFC = gate.Output.Id[0];

                if (outputFC == 'z' && gate.GateType != "XOR" && gate.Output.Id != "z45")
                {
                    wrongWires.Add(gate.Output.Id);
                }
                
                if (gate.GateType == "XOR" && !standardConnections.Contains(input1FC) && !standardConnections.Contains(input2FC) && !standardConnections.Contains(outputFC))
                {
                    wrongWires.Add(gate.Output.Id);
                }
                
                if (gate.GateType == "AND" && !(gate.Input1.Id == "x00" || gate.Input2.Id == "x00"))
                {
                    foreach (Gate gate2 in allGates)
                    {
                        if ((gate.Output == gate2.Input1 || gate.Output == gate2.Input1) && gate2.GateType != "OR")
                        {
                            wrongWires.Add(gate.Output.Id);
                        }

                    }

                }
                
                if (gate.GateType == "XOR")
                {
                    foreach (Gate gate2 in allGates)
                    {
                        {
                            if ((gate.Output == gate2.Input1 || gate.Output == gate2.Input2) && gate2.GateType == "OR")
                            {
                                wrongWires.Add(gate.Output.Id);
                            }
                        }
                    }
                }
            }

            int ibreak  =0;

            
            DebugOutput(string.Join(',',wrongWires.Order()));


            //CreateGraphViz(allGates);

            //Reset();
            ////DebugOutput(DisplayState("",true));

            //int val1 = 5;
            //int val2 = 7;

            //SetInputGateToValue(0, "z");
            //SetInputGateToValue(val1, "x");
            //SetInputGateToValue(val2, "y");
            //DebugOutput(DisplayState("", true));

            //long result = 0;

            //int count = 0;
            //bool stable = false;
            //while (!stable)
            //{
            //    foreach (Gate g in allGates)
            //    {
            //        g.Calculate();
            //    }
            //    result = GetGateValue("z");

            //    stable = true;
            //    foreach (Wire w in WireDictionary.Values)
            //    {
            //        if (!w.Stable)
            //        {
            //            stable = false;
            //            break;
            //        }
            //    }
            //    count++;
            //}
            //DebugOutput(DisplayState("", true));

            //if (result == val1 + val2)
            //{
            //    DebugOutput($"Gates worked for {val1} + {val2} = {(val1 + val2)}");
            //}



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
        maxIndex += 1;

        string stringValue = Convert.ToString(value, 2);
        stringValue = new string(stringValue.Reverse().ToArray());
        for (int i = 0; i < maxIndex; i++)
        {
            string id = gatePrefix + i.ToString("D2");

            if (i < stringValue.Length)
            {
                // do twice to set as stable
                WireDictionary[id].Value = stringValue[i] == '1' ? true : false;
                WireDictionary[id].Value = stringValue[i] == '1' ? true : false;
            }
            else
            {
                // do twice to set as stable
                WireDictionary[id].Value = false;
                WireDictionary[id].Value = false;
            }

        }


    }

    public long GetGateValue(string gatePrefix)
    {
        string temp = DisplayState(gatePrefix);
        if (temp.Contains("*"))
        {
            return long.MaxValue;
        }
        return Convert.ToInt64(temp, 2);
    }


    public void Reset()
    {
        foreach (Wire w in WireDictionary.Values)
        {
            w.Reset();
        }
    }

    public string DisplayState(string prefix = "z", bool full = false)
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
            result += (full ? $"{key} = {resultChar}\n" : resultChar);
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


    public void CreateGraphViz(List<Gate> allGates)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("graph{");


        foreach (Gate gate in allGates)
        {
            string gateName = gate.Input1.Id + "_" + gate.Input2.Id;
            sb.AppendLine($"{gateName} [shape = triangle,label = \"{gateName + "-" + gate.GateType}\"]");
        }



        foreach (Wire w in WireDictionary.Values)
        {
            if (w.GateFrom != null & w.GateTo != null)
            {
                sb.AppendLine($"{w.GateFrom.Input1.Id}_{w.GateFrom.Input2.Id} -- {w.GateTo.Input1.Id}_{w.GateTo.Input2.Id}");
            }
            else if (w.GateFrom == null)
            {
                sb.AppendLine($"{w.Id} -- {w.GateTo.Input1.Id}_{w.GateTo.Input2.Id}");
            }
            else
            {
                sb.AppendLine($"{w.GateFrom.Input1.Id}_{w.GateFrom.Input2.Id} -- {w.Id}");
            }

        }

        sb.AppendLine("}");

        DebugOutput(sb.ToString());

    }

    public class Wire
    {
        public string Id;
        public bool? InitialValue;
        public Gate GateFrom;
        public Gate GateTo;


        private bool? m_value;
        private bool? m_lastValue;

        public bool? Value
        {
            get { return m_value; }
            set
            {
                m_lastValue = m_value;
                m_value = value;
            }
        }

        public bool Stable
        {
            get { return m_lastValue == m_value; }
        }

        public void Reset()
        {
            Value = InitialValue;
            m_lastValue = InitialValue;
        }



    }


    public class Gate
    {
        public Wire m_input1;
        public Wire m_input2;

        public bool Swapped;

        public string GateType;
        public Wire Output;


        public Gate(Wire input1, Wire input2, string gateType, Wire output)
        {
            m_input1 = input1;
            m_input2 = input2;
            GateType = gateType;
            Output = output;

            if (m_input1 != null)
            {
                m_input1.GateTo = this;
            }
            if (m_input2 != null)
            {
                m_input2.GateTo = this;
            }
            if (output != null)
            {
                output.GateFrom = this;
            }
        }

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