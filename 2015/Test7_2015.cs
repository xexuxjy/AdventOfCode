using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test7_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 7;
    }

    public List<Wire> m_wires = new List<Wire>();
    public List<Gate> m_gates = new List<Gate>();

    public override void Execute()
    {


        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(' ');
            if (tokens.Length == 3)
            {
                if (tokens[1] == "->")
                {
                    Wire targetWire = GetOrCreateWire(tokens[2]);
                    if (ushort.TryParse(tokens[0], out ushort numValue))
                    {
                        targetWire.FixedValue = numValue;
                        targetWire.ValueSet = true;
                        DebugOutput("Setting fixed value to : "+numValue);
                    }
                    else
                    {
                        Wire sourceWire = GetOrCreateWire(tokens[0]);
                        targetWire.ValueSet = true;
                        targetWire.SignalWire = sourceWire;
                    }
                }
            }
            else if (tokens.Length == 4)
            {
                if (tokens[0] == "NOT")
                {
                    Gate gate = new Gate();
                    gate.Operation = tokens[0];
                    gate.Input1 = GetOrCreateWire(tokens[1]);
                    gate.Output = GetOrCreateWire(tokens[3]);
                    m_gates.Add(gate);
                }

            }
            else if (tokens.Length == 5)
            {
                if (tokens[1] == "AND" || tokens[1] == "OR")
                {
                    Gate gate = new Gate();
                    gate.Operation = tokens[1];
                    gate.Input1 = GetOrCreateWire(tokens[0]);
                    gate.Input2 = GetOrCreateWire(tokens[2]);
                    gate.Output = GetOrCreateWire(tokens[4]);
                    m_gates.Add(gate);
                }
                else if (tokens[1] == "LSHIFT" || tokens[1] == "RSHIFT")
                {
                    Gate gate = new Gate();
                    gate.Operation = tokens[1];
                    gate.Input1 = GetOrCreateWire(tokens[0]);
                    gate.Argument = tokens[2];
                    gate.Output = GetOrCreateWire(tokens[4]);
                    m_gates.Add(gate);

                }
            }

        }


        foreach(Wire wire in m_wires)
        {
            if(!(wire.SignalGate != null || wire.SignalWire != null || wire.HasValue))
            {
                int ibreak = 0;
            }

        }




        bool allReady = false;
        while(!allReady)
        {
            allReady = true;
            foreach(Gate gate in m_gates)
            {

                gate.CheckAndUpdate();
                if(!gate.IsReady)
                {
                    allReady = false;
                }
            }
            int readyCount = m_gates.FindAll(gate => gate.IsReady).Count;

        }

        //foreach (Wire w in m_wires.OrderBy(x => x.Id))
        //{
        //    DebugOutput($"Wire {w.Id} has value {w.Value}");
        //}

        Wire aWire = m_wires.Find(x=>x.Id == "a");
        if(aWire != null)
        {
            DebugOutput($"Wire {aWire.Id} has value {aWire.Value}");
        }


        if(IsPart2)
        {
            Wire bWire = m_wires.Find(x=>x.Id == "b");
            ushort aWireValue = aWire.Value;
            foreach(Wire wire in m_wires)
            {
                wire.Reset();
            }
        
            foreach(Gate gate in m_gates)
            {
                gate.IsReady = false;
            }

            bWire.SignalWire = null;
            bWire.SignalGate = null;
            bWire.FixedValue = aWireValue;
            allReady = false;
            while(!allReady)
            {
                allReady = true;
                foreach(Gate gate in m_gates)
                {

                    gate.CheckAndUpdate();
                    if(!gate.IsReady)
                    {
                        allReady = false;
                    }
                }
                int readyCount = m_gates.FindAll(gate => gate.IsReady).Count;

            }
            DebugOutput($"Part2 Wire {aWire.Id} has value {aWire.Value}");
        }





    }



    public Wire GetOrCreateWire(string id)
    {
        Wire w = m_wires.Find(x => x.Id == id);
        if (w == null)
        {
            w = new Wire();
            w.Id = id;

            if(ushort.TryParse(id,out ushort num))
            {
                w.FixedValue = num;
                w.ValueSet = true;
            }

            m_wires.Add(w);
        }
        return w;

    }



    public class Wire
    {
        public string Id;

        private ushort m_fixedValue;
        private ushort m_originalValue;

        public void Reset()
        {
            m_fixedValue = m_originalValue;
        }


        public ushort FixedValue
        {
            get{return m_fixedValue; }
            set
            {
                m_fixedValue = value;
                m_originalValue = value;
                ValueSet = true;
            }
        }

        public Wire SignalWire;
        public Gate SignalGate;

        public bool ValueSet;


        public bool HasValue
        {
            get
            {
                if (SignalWire != null)
                {
                    return SignalWire.HasValue;
                }
                if (SignalGate != null)
                {
                    
                    return SignalGate.IsReady;
                }
                else
                {
                    return ValueSet;
                }
            }
        }

        public ushort Value
        {
            get
            {
                if (SignalWire != null)
                {
                    return SignalWire.Value;
                }
                if (SignalGate != null)
                {
                    return SignalGate.GateValue;
                }
                else
                {
                    return FixedValue;
                }
            }
        }
    }


    public class Gate
    {
        public bool Visited = false;

        public Wire Input1;
        public Wire Input2;

        private Wire m_outputWire;

        public string Operation;
        public string Argument;


        private ushort m_currentValue;
        public  bool IsReady;

        public Wire Output
        {
            get { return m_outputWire; }
            set
            {
                m_outputWire = value;
                value.SignalGate = this;
                value.ValueSet = true;
            }
        }


        public void CheckAndUpdate()
        {

            if(Visited)
            {
                int ibreak = 0;
            }
            Visited = true;

            if(IsReady)
            {
                return;
            }
            IsReady = false;
            if (Operation == "AND")
            {
                if (Input1.HasValue && Input2.HasValue)
                {
                    IsReady = true;
                    m_currentValue = (ushort)(Input1.Value & Input2.Value);
                }
            }
            else if (Operation == "OR")
            {
                if (Input1.HasValue && Input2.HasValue)
                {
                    IsReady = true;
                    m_currentValue = (ushort)(Input1.Value | Input2.Value);
                }
            }
            else if (Operation == "NOT")
            {
                if (Input1.HasValue)
                {
                    IsReady = true;
                    m_currentValue = (ushort)(~Input1.Value);
                }
            }
            else if (Operation == "LSHIFT")
            {
                if (Input1.HasValue)
                {
                    IsReady = true;
                    m_currentValue = (ushort)(Input1.Value << ushort.Parse(Argument));
                }
            }
            else if (Operation == "RSHIFT")
            {
                if (Input1.HasValue)
                {
                    IsReady = true;
                    m_currentValue = (ushort)(Input1.Value >> ushort.Parse(Argument));
                }
            }

        }

        public ushort GateValue
        {
            get
            {
                if(IsReady)
                {
                    return m_currentValue;
                }
                return 0;
            }

        }

    }

}