using System;
using System.Collections.Generic;

public class Test15_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 15;
    }

    public override void Execute()
    {
        long generatorAStart = IsTestInput ? 65 : 512;
        long generatorBStart = IsTestInput ? 8921 : 191;

        long factorA = 16807;
        long factorB = 48271;

        long currentA = generatorAStart;
        long currentB = generatorBStart;

        long mask = 0xFFFF;

        int numIterations = IsPart1?40000000:5000000;

        //numIterations = 5;

        int matchCount = 0;

        if (IsPart1)
        {
            for (int i = 0; i < numIterations; i++)
            {
                currentA = GenerateNext(currentA, factorA);
                currentB = GenerateNext(currentB, factorB);

                // compare lowest x bits
                long maskedA = currentA & mask;
                long maskedB = currentB & mask;

                if (maskedA == maskedB)
                {
                    matchCount++;
                }
            }
        }
        else
        {
            for (int i = 0; i < numIterations; i++)
            {
                bool isValidA = false;
                while (!isValidA)
                {
                    currentA = GenerateNext(currentA, factorA);
                    if (currentA % 4 == 0)
                    {
                        isValidA = true;
                    }
                }

                bool isValidB = false;
                while (!isValidB)
                {
                    currentB = GenerateNext(currentB, factorB);
                    if (currentB % 8 == 0)
                    {
                        isValidB = true;
                    }
                }
                
                //DebugOutput($"{currentA} -> {currentB}");
                
                // compare lowest x bits
                long maskedA = currentA & mask;
                long maskedB = currentB & mask;

                if (maskedA == maskedB)
                {
                    matchCount++;
                }
                
                
            }
        }

        DebugOutput($"There are {matchCount} matches");

    }

    public long GenerateNext(long input,long factor)
    {
        long result = input * factor;
        result = result % 2147483647L;
        return result;
    }
    
}