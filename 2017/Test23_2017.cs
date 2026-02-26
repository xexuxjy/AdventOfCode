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
        //LowLevel();
        //HighLevel();
        HighHighLevel();
    }

    public void LowLevel()
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
                    //registerSet.GetRegister("e").Value = registerSet.GetRegister("b").Value-1;
                }
            }

            // if (IsPart2 && arg1 == "e")
            // {
            //     DebugOutput($"e changed value = {registerSet.GetRegister(arg1).Value}   b [{registerSet.GetRegister("b").Value}]   c[{registerSet.GetRegister("c").Value}]  g [{registerSet.GetRegister("g").Value}]");
            // }
            //
            //
            // if (IsPart2 && arg1 == "f")
            // {
            //     DebugOutput($"f changed value = {registerSet.GetRegister(arg1).Value}   b [{registerSet.GetRegister("b").Value}]   c[{registerSet.GetRegister("c").Value}]  g [{registerSet.GetRegister("g").Value}]");
            // }


            if (IsPart2 && arg1 == "h")
            {
                DebugOutput(
                    $"H changed value = {registerSet.GetRegister(arg1).Value}   b [{registerSet.GetRegister("b").Value}]   c[{registerSet.GetRegister("c").Value}]  g [{registerSet.GetRegister("g").Value}]");
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

    public void HighLevel()
    {
        int a = IsPart1 ? 0 : 1;
        int b = 84;
        int c = b;
        int d = 2;
        int e = 0;
        int f = 1;
        int g = 0;
        int h = 0;

        int mulCount = 0;
        if (a != 0)
        {
            b = b * 100;
            mulCount++;
            b = b + 100000;
            c = b;
            c = c + 17000;
        }

        while (true)
        {
            f = 1;
            d = 2;
            do
            {
                e = 2;
                do
                {
                    g = d;
                    g = g * e;
                    mulCount++;
                    g = g - b;
                    if (g == 0)
                    {
                        f = 0;
                    }

                    e = e + 1;
                    g = e;
                    g = g - b;
                } while (g != 0);

                d = d + 1;
                g = d;
                g = g - b;
            } while (g != 0);

            if (f == 0)
            {
                h = h + 1;
                DebugOutput($"H Changed {h}");
                g = b;
                g = g - c;
                if (g == 0)
                {
                    goto EndRun;
                }

                b = b + 17;
            }
        }

        EndRun:
        int finished = 0;
        DebugOutput($"The multiply command was executed {mulCount} times");
    }

    public void HighHighLevel()
    {
        int from = 108400;
        int to = 125400;

        int count = 0;
        for (int i = from; i <= to; i+=17)
        {
            if (!Helper.IsPrime(i))
            {
                count++;
            }
        }
        DebugOutput($"There were {count} non prime numbers");
    }
    
}