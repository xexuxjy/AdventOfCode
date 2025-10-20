using System;
using System.Collections.Generic;

public class Test12_2016 : BaseTest
{
    public Dictionary<string,Register> Registers = new Dictionary<string, Register>();
    public int ProgramCounter = 0;
    
    public override void Initialise()
    {
        Year = 2016;
        TestID = 12;
    }

    public override void Execute()
    {
        Registers["a"] = new Register() { Id = "a" };
        Registers["b"] = new Register() { Id = "b" };
        Registers["c"] = new Register() { Id = "c" };
        Registers["d"] = new Register() { Id = "d" };

        if (IsPart2)
        {
            Registers["c"].Value = 1;
        }
        
        RunProgram();

        DebugOutput("Value in Register a is : " + Registers["a"].Value);
        
    }

    public void RunProgram()
    {
        while (ProgramCounter < m_dataFileContents.Count)
        {
            bool jumped = false;
            string[] tokens = m_dataFileContents[ProgramCounter].Split(' ');

            switch (tokens[0])
            {
                case "cpy":
                    if (int.TryParse(tokens[1], out int intVal))
                    {
                        Registers[tokens[2]].Value = intVal;
                    }
                    else
                    {
                        Registers[tokens[2]].Value = Registers[tokens[1]].Value;
                    }

                    break;
                case "inc":
                    Registers[tokens[1]].Increment();
                    break;
                case "dec":
                    Registers[tokens[1]].Decrement();
                    break;
                case "jnz":
                    int testValue = 0;
                    if (!int.TryParse(tokens[1], out testValue))
                    {
                        testValue = Registers[tokens[1]].Value;
                    }
                    
                    if (testValue != 0)
                    {
                        jumped = true;
                        int.TryParse(tokens[2], out int address);
                        ProgramCounter += address;
                    }

                    break;

            }

            if (!jumped)
            {
                ProgramCounter++;
            }
        }

    }
    


    public class Register
    {
        public string Id;
        public int Value;

        public void Increment()
        {
            Value++;
        }

        public void Decrement()
        {
            Value--;
        }

    }
    
}