using System;
using System.Collections.Generic;

public class Test23_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 23;
    }

    public override void Execute()
    {
        
        RegisterSet registerSet = new RegisterSet();

        if (IsPart2)
        {
            registerSet.GetRegister("a").Value = 1;
        }
        
        
        int programCounter = 0;
        int mulCount = 0;
        bool keepGoing = true;
        while (keepGoing)
        {
            string line = m_dataFileContents[programCounter];

            string[] tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string command = tokens[0];
            string arg1 = tokens[1];
            string arg2 = tokens[2];

            bool jumped = false;


            switch (command)
            {
                case "set":
                    registerSet.GetRegister(arg1).Value = registerSet.GetValue(arg2);
                    break;
                case "sub":
                    registerSet.GetRegister(arg1).Value -= registerSet.GetValue(arg2);
                    break;
                case "mul":
                    mulCount++;
                    registerSet.GetRegister(arg1).Value *= registerSet.GetValue(arg2);
                    break;
                case "jnz":
                    if (registerSet.GetValue(arg1) != 0)
                    {
                        programCounter += registerSet.GetValue(arg2);
                        jumped = true;
                    }

                    break;
            }

            if (programCounter == 18)
            {
                // DebugOutput(
                //     $"g changed value = {registerSet.GetRegister(arg1).Value}   b [{registerSet.GetRegister("b").Value}]   c[{registerSet.GetRegister("c").Value}]");
                //if (registerSet.GetRegister(arg1).Value == -108397)
                {
                    //registerSet.GetRegister(arg1).Value = 0;
                    //registerSet.GetRegister("b").Value = registerSet.GetRegister("c").Value;
                    //DebugOutput(registerSet.GetValues());
                    registerSet.GetRegister("e").Value = registerSet.GetRegister("b").Value-1;
                }
            }


            if (programCounter == 30)
            {
                int ibreak = 0;
            }
            
            if (IsPart2 && arg1 == "f")
            {
                DebugOutput($"f changed value = {registerSet.GetRegister(arg1).Value}   b [{registerSet.GetRegister("b").Value}]   c[{registerSet.GetRegister("c").Value}]");
            }

            
            if (IsPart2 && arg1 == "h")
            {
                DebugOutput($"H changed value = {registerSet.GetRegister(arg1).Value}   b [{registerSet.GetRegister("b").Value}]   c[{registerSet.GetRegister("c").Value}]");
            }

            
            if (!jumped)
            {
                programCounter++;
            }

            if (programCounter < 0 || programCounter >= m_dataFileContents.Count)
            {
                keepGoing = false;
            }
        }
        
        DebugOutput($"The multiply command was executed {mulCount} times");
    }
}