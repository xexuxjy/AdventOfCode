using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

public class Test25_2016 : BaseTest
{
    public Dictionary<string, Register> Registers = new Dictionary<string, Register>();
    public int ProgramCounter = 0;

    public int AlternatingCount = 0;
    public int StartVal = 0;

    public bool StopSearching = false;
    
    public override void Initialise()
    {
        Year = 2016;
        TestID = 25;
    }

    public override void Execute()
    {
        Registers["a"] = new Register() { Id = "a" };
        Registers["b"] = new Register() { Id = "b" };
        Registers["c"] = new Register() { Id = "c" };
        Registers["d"] = new Register() { Id = "d" };

        if (!IsTestInput)
        {
            Registers["a"].Value = 1;
        }

        if (IsPart2)
        {
            Registers["a"].Value = 12;
        }

        foreach (Register reg in Registers.Values)
        {
            reg.Test = this;
        }

        RunProgram();

        DebugOutput("Value in Register a is : " + Registers["a"].Value);
    }

    public void RunProgram()
    {
        int escape = 0;
        int count = 0;
        List<string> instructions = m_dataFileContents;

        // for part 2 the answer can be jumped to by multiplying the values
        // at instructions 20 & 21 
        // cpy 95 c
        // jnz 99 d
        // (95 * 99) + numEggs!  
        StartVal = 0;
        while (StartVal < int.MaxValue)
        {
            Registers["a"].Value = StartVal;
            Registers["b"].Value = 0;
            Registers["c"].Value = 0;
            Registers["d"].Value = 0;
            
            count = 0;
            LastSignalValue = true;
            AlternatingCount = 0;
            ProgramCounter = 0;

            while (ProgramCounter < instructions.Count)
            {
                string[] tokens = instructions[ProgramCounter].Split(' ');
                
                InterpretInstruction(tokens, instructions,out bool invalidClock);
                if (invalidClock)
                {
                    break;
                }

                if (StopSearching)
                {
                    goto Finished;
                }
                
                count++;
                if (escape > 0 && count >= escape)
                {
                    break;
                }
            }
            StartVal++;
        }
        
        Finished:
        DebugOutput("Complete");
    }

    public bool ApplyPeephole(List<string> instructions)
    {
        return false;
        if (instructions[ProgramCounter] == "inc a" && instructions[ProgramCounter + 1] == "dec c" &&
            instructions[ProgramCounter + 2] == "jnz c -2")
        {
            DebugOutput("Optimised.");
            Registers["a"].Value = Registers["a"].Value * Registers["c"].Value;
            Registers["c"].Value = 0;
            ProgramCounter += 2;
            return true;
        }

        return false;
    }

    public void DumpRegisters(string[] instruction)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"*** {string.Join(' ',instruction)} ***");
        foreach (Register reg in Registers.Values.OrderBy(reg=>reg.Id))
        {
            sb.Append($"[{reg.Id}] = {reg.Value} , ");
        }

        sb.AppendLine();
        DebugOutput(sb.ToString());
    }
    

    public bool LastSignalValue = true;
    
    public bool InterpretInstruction(string[] tokens, List<string> instructions,out bool invalidClock)
    {
        bool jumped = false;
        invalidClock = false;
        bool optimised = ApplyPeephole(instructions);
        if (!optimised)
        {
            //DebugOutput($"PC {ProgramCounter} Interpreting instruction: " + string.Join(' ', tokens));
            switch (tokens[0])
            {
                case "cpy":
                    if (!char.IsDigit(tokens[2][0]))
                    {
                        if (int.TryParse(tokens[1], out int intVal))
                        {
                            Registers[tokens[2]].Value = intVal;
                        }
                        else
                        {
                            Registers[tokens[2]].Value = Registers[tokens[1]].Value;
                        }
                    }
                    else
                    {
                        int ibreak = 0;
                    }

                    break;
                case "inc":
                    Registers[tokens[1]].Increment();
                    break;
                case "dec":
                    Registers[tokens[1]].Decrement();

                    break;
                case "tgl":
                    if (!int.TryParse(tokens[1], out int amount))
                    {
                        amount = Registers[tokens[1]].Value;
                    }

                    int tglAddress = ProgramCounter + amount;
                    ToggleInstruction(tglAddress, instructions);
                    break;
                case "jnz":
                    int testValue = 0;
                    if (!int.TryParse(tokens[1], out testValue))
                    {
                        testValue = Registers[tokens[1]].Value;
                    }

                    if (testValue != 0)
                    {
                        if (!int.TryParse(tokens[2], out int address))
                        {
                            address = Registers[tokens[2]].Value;
                        }

                        jumped = true;
                        ProgramCounter += address;
                    }

                    break;
                case "out":
                    bool signalValue = false;
                    if (!int.TryParse(tokens[1], out int signalValueInt))
                    {
                        signalValueInt = Registers[tokens[1]].Value;
                        signalValue = signalValueInt != 0 ? true : false;
                    }

                    if (signalValue == !LastSignalValue)
                    {
                        LastSignalValue = signalValue;
                        AlternatingCount++;
                        if (AlternatingCount > 1000)
                        {
                            DebugOutput("Found high alternating count with start value : "+StartVal);
                            StopSearching = true;
                        }
                    }
                    else
                    {
                        invalidClock = true;
                    }
                    
                    break;
            }
            //DumpRegisters(tokens);            
        }

        if (!jumped)
        {
            ProgramCounter++;
        }

        return jumped;
    }

    public List<string> ToggleInstruction(int address, List<string> instructions)
    {
        if (address < 0 || address >= instructions.Count)
        {
            return instructions;
        }

        string[] tokens = instructions[address].Split(' ');

        string newInstruction = "";
        if (tokens.Length == 2)
        {
            //For one-argument instructions, inc becomes dec, and all other one-argument instructions become inc.
            if (tokens[0] == "inc")
            {
                newInstruction = "dec";
            }
            else
            {
                newInstruction = "inc";
            }
        }
        else if (tokens.Length == 3)
        {
            //For two-argument instructions, jnz becomes cpy, and all other two-instructions become jnz.
            if (tokens[0] == "jnz")
            {
                newInstruction = "cpy";
            }
            else
            {
                newInstruction = "jnz";
            }
        }

        string before = string.Join(' ', tokens);
        tokens[0] = newInstruction;
        instructions[address] = string.Join(' ', tokens);
        string after = string.Join(' ', tokens);
        DebugOutput($"Changing instruction at {address} from {before} to {after}");


        return instructions;
    }


    public class Register
    {
        public string Id;
        public int Value;
        public BaseTest Test;

        public void Increment()
        {
            if (Test.IsPart1)
            {
                Value++;
            }
            else
            {
                Value *= Value;
            }
        }

        public void Decrement()
        {
            Value--;
        }
    }
}