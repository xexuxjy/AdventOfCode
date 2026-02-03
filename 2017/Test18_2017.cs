using System;
using System.Collections.Generic;

public class Test18_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 18;
    }

    
    public override void Execute()
    {
        if (IsPart1)
        {
            ExecutePart1();
        }
        else
        {
            ExecutePart2();
        }
    }

    public void ExecutePart1()
    {
        LongRegisterSet registerSet = new LongRegisterSet();
        
        int programCounter = 0;

        long lastSoundPlayed = -1;
        long recoveredSound = -1;
        
        //while(programCounter < m_dataFileContents.Count)
        while(programCounter < m_dataFileContents.Count && recoveredSound == -1)
        {
            string line = m_dataFileContents[programCounter];    
            string[] tokens = line.Split(' ');
            bool jumped = false;
            switch (tokens[0])
            {
                case "snd":
                    lastSoundPlayed = registerSet.GetValue(tokens[1]);
                    //DebugOutput($"Last sound played: {lastSoundPlayed}");
                    break;
                case "set":
                    registerSet.GetRegister(tokens[1]).Value = registerSet.GetValue(tokens[2]);
                    break;
                case "add":
                    registerSet.GetRegister(tokens[1]).Value += registerSet.GetValue(tokens[2]);
                    break;
                case "mul":
                    registerSet.GetRegister(tokens[1]).Value *= registerSet.GetValue(tokens[2]);
                    break;
                case "mod":
                    long temp = registerSet.GetRegister(tokens[1]).Value;
                    long modded = temp % registerSet.GetValue(tokens[2]);
                    registerSet.GetRegister(tokens[1]).Value = modded;
                    break;
                case "rcv":
                    if (registerSet.GetValue(tokens[1]) > 0)
                    {
                        recoveredSound = lastSoundPlayed;
                    }
                    break;
                case "jgz":
                    if (registerSet.GetValue(tokens[1]) > 0)
                    {
                        programCounter += (int)registerSet.GetValue(tokens[2]);
                        jumped = true;
                    }
                    break;
                default:
                    DebugOutput($"Unexpected command {tokens[0]}");
                    break;

            }
            //DebugOutput($"Register A : {registerSet.GetValue("a")}");
            if (!jumped)
            {
                programCounter++;
            }

        }
        DebugOutput($"The last recovered sound was {recoveredSound}");
        
        
    }

    public void ExecutePart2()
    {
        SubMachine machine0 = new SubMachine();
        SubMachine machine1 = new SubMachine();

        machine0.Instructions = m_dataFileContents;
        machine1.Instructions = m_dataFileContents;

        machine0.ProgramId = 0;
        machine1.ProgramId = 1;

        machine0.Target = machine1;
        machine1.Target = machine0;

        bool keepGoing = true;
        while (keepGoing)
        {
            machine0.Process();
            machine1.Process();
            if (machine0.Waiting && machine1.Waiting)
            {
                keepGoing = false;
            }
        }
        DebugOutput($"Machine 1 sent {machine1.SendCount} values");

    }
    
    
}


public class SubMachine
{
    
    
    //public Queue<long> SendQueue =  new Queue<long>();
    public Queue<long> ReceiveQueue = new Queue<long>();

    private int m_programId;
    public int ProgramId
    {
        get{return m_programId;}
        set
        {
            m_programId = value;
    
            RegisterSet.GetRegister("p").Value = m_programId;
        }
    }
    
    
    
    private LongRegisterSet RegisterSet = new LongRegisterSet();

    public List<string> Instructions;

    public SubMachine Target;

    public bool Waiting = false;
    public bool Terminated = false;
    
    public int ProgramCounter = 0;

    public long SendCount;

    public long InstructionCount;
    
    public void Process()
    {
        if (ProgramCounter < 0 || ProgramCounter >= Instructions.Count)
        {
            Terminated = true;
            return;
        }
        
        InstructionCount++;
        string line = Instructions[ProgramCounter];    
        string[] tokens = line.Split(' ');
        bool jumped = false;
        
        switch (tokens[0])
        {
            case "snd":
                SendCount++;
                Target.ReceiveQueue.Enqueue(RegisterSet.GetValue(tokens[1]));
                break;
            case "set":
                RegisterSet.GetRegister(tokens[1]).Value = RegisterSet.GetValue(tokens[2]);
                break;
            case "add":
                RegisterSet.GetRegister(tokens[1]).Value += RegisterSet.GetValue(tokens[2]);
                break;
            case "mul":
                RegisterSet.GetRegister(tokens[1]).Value *= RegisterSet.GetValue(tokens[2]);
                break;
            case "mod":
                long temp = RegisterSet.GetRegister(tokens[1]).Value;
                long modded = temp % RegisterSet.GetValue(tokens[2]);
                RegisterSet.GetRegister(tokens[1]).Value = modded;
                break;
            case "rcv":
                Waiting = true;
                if (ReceiveQueue.Count > 0)
                {
                    long value = ReceiveQueue.Dequeue();
                    RegisterSet.GetRegister(tokens[1]).Value = value;
                    Waiting = false;
                }
                    
                break;
            case "jgz":
                if (RegisterSet.GetValue(tokens[1]) > 0)
                {
                    ProgramCounter += (int)RegisterSet.GetValue(tokens[2]);
                    jumped = true;
                }
                break;
            default:
            
                break;

        }

        if (!(jumped || Waiting))
        {
            ProgramCounter++;
        }
    }
}
