using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Test8_2017 : BaseTest
{
    Dictionary<string,Register> m_registers = new Dictionary<string,Register>();
    
    public override void Initialise()
    {
        Year = 2017;
        TestID = 8;
    }

    public override void Execute()
    {
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(' ');
            string name = tokens[0];
            string operation =  tokens[1];
            int amount = int.Parse(tokens[2]);
            string ifCondition = tokens[3];
            string lhs = tokens[4];
            string comparison = tokens[5];
            int rhs = int.Parse(tokens[6]);

            bool valid = false;
            Register lhsRegister = GetRegister(lhs);
            if (comparison == "==")
            {
                valid = lhsRegister.Value == rhs;
            }
            else if (comparison == "!=")
            {
                valid = lhsRegister.Value != rhs;
            }
            else if (comparison == ">")
            {
                valid = lhsRegister.Value > rhs;
            }
            else if (comparison == "<")
            {
                valid = lhsRegister.Value < rhs;
            }
            else if (comparison == ">=")
            {
                valid = lhsRegister.Value >= rhs;
            }
            else if (comparison == "<=")
            {
                valid = lhsRegister.Value <= rhs;
            }
            else
            {
                Debug.Assert(false);
            }

            if (valid)
            {
                Register register = GetRegister(name);
                if (operation == "inc")
                {
                    register.Value += amount;
                }
                else if (operation == "dec")
                {
                    register.Value -= amount;
                }
                else
                {
                    Debug.Assert(false);
                }
            }
        }
        
        int highestValue = m_registers.Values.Max(x => x.Value); 
        
        DebugOutput($"The highest value is {highestValue}");

        if (IsPart2)
        {
            int highestEverValue = m_registers.Values.Max(x => x.HighestValue);
            DebugOutput($"The highest ever value is {highestEverValue}");
        }
        
        
    }

    public Register GetRegister(string name)
    {
        if(!m_registers.TryGetValue(name,out Register register))
        {
            register = new Register();
            register.Name = name;
            m_registers[name] = register;
        }
        return register;
    }
}

