using AdventOfCode.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Test8_2023;

public class Test23_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 23;
    }


    public override void Execute()
    {
        int programCounter = 0;
        ulong rega=IsPart2?(ulong)1:0;
        ulong regb=0;


        while(programCounter < m_dataFileContents.Count)
        {
            string line = m_dataFileContents[programCounter];
            string[] tokens = line.Split(new char[]{' ',',' },StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries);

            string instruction = tokens[0];
            string register = tokens[1];
             
            //DebugOutput($"Program counter : {programCounter}   Instruction : {line}");

            if(instruction == "inc")
            {
                if(register == "a")
                {
                    rega++;
                }
                else if(register == "b")
                {
                    regb++;
                }
                programCounter++;
            }
            else if(instruction == "hlf")
            {
                if(register == "a")
                {
                    rega = rega/2;
                }
                else if(register == "b")
                {
                    regb = regb /2;
                }
                programCounter++;
            }
            else if(instruction == "tpl")
            {
                if(register == "a")
                {
                    rega = rega * 3;
                }
                else if(register == "b")
                {
                    regb = regb * 3;
                }
                programCounter++;
            }
            else if (instruction == "jmp")
            {
                int amount = int.Parse(register);
                if(amount < 0)
                {
                    int ibreak = 0;
                }

                programCounter += amount;
            }
            else if (instruction == "jie")
            {
                int amount = int.Parse(tokens[2]);
                if(amount < 0)
                {
                    int ibreak = 0;
                }
                if(register == "a")
                {
                    if(rega % 2 == 0)
                    {
                        programCounter += amount;
                    }
                    else
                    {
                        programCounter++;
                    }
                }
                else if(register == "b")
                {
                    if(regb % 2 == 0)
                    {
                        programCounter += amount;
                    }
                    else
                    {
                        programCounter++;
                    }
                }
            }
            else if (instruction == "jio")
            {
                int amount = int.Parse(tokens[2]);
                if(amount < 0)
                {
                    int ibreak = 0;
                }
                if(register == "a")
                {
                    if(rega == 1)
                    {
                        programCounter += amount;
                    }
                    else
                    {
                        programCounter++;
                    }
                }
                else if(register == "b")
                {
                    if(regb == 1)
                    {
                        programCounter += amount;
                    }
                    else
                    {
                        programCounter++;
                    }
                }
            }
            else
            {
                int ibreak = 0;
            }

            //DebugOutput($"New Program counter : {programCounter}");

        }

        DebugOutput($"Final values of registers : a [{rega}]  b [{regb}] ");

    }
}