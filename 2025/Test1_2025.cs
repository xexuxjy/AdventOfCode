using System;
using System.Collections.Generic;

public class Test1_2025 : BaseTest
{
    public override void Initialise()
    {
        Year = 2025;
        TestID = 1;
    }

    int ZeroCount=0;
    int StartPosition = 50;
    int DialSize = 100;

    public override void Execute()
    {
        int currentPosition = StartPosition;
        foreach (string line in m_dataFileContents)
        {
            bool turnLeft = line.StartsWith("L");
            int number = int.Parse(line.Substring(1));

            ZeroCount += TurnDial(ref currentPosition, number, turnLeft);
        }
        DebugOutput($"The number of times zero occured is {ZeroCount}");
    }

    public void OldMethod()
    {
        int currentPosition = StartPosition;
        foreach (string line in m_dataFileContents)
        {
            bool turnLeft = line.StartsWith("L");
            int number = int.Parse(line.Substring(1));
            bool startedAtZero = currentPosition == 0;

            if (turnLeft)
            {
                currentPosition -= number;
                while (currentPosition < 0)
                {
                    currentPosition += DialSize;
                    if (IsPart2)
                    {
                        if (currentPosition != 0)
                        {
                            if (startedAtZero)
                            {
                                startedAtZero = false;
                            }
                            else
                            {
                                ZeroCount++;                                
                            }
                            DebugOutput($"*** {line}  CurrentPosition {currentPosition} zc {ZeroCount}");
                        }
                    }
                }
            }
            else
            {
                currentPosition += number;
                while(currentPosition >= DialSize)
                {
                    currentPosition -= DialSize;
                    if (IsPart2)
                    {
                        if (currentPosition != 0)
                        {
                            if (startedAtZero)
                            {
                                startedAtZero = false;
                            }
                            else
                            {
                                ZeroCount++;                                
                            }
                            
                            DebugOutput($"*** {line}  CurrentPosition {currentPosition} zc {ZeroCount}");                            
                        }
                    }
                }
            }

            if (currentPosition == 0)
            {
                ZeroCount++;
            }
            
            DebugOutput($" {line}  CurrentPosition {currentPosition} zc {ZeroCount}");
            
        }
        
    }
    
    public int TurnDial(ref int currentPosition, int amount,bool left)
    {
        int zeroCount = 0;
        for (int i = 0; i < amount; i++)
        {
            currentPosition += left ? -1 : 1;
 
            if (currentPosition < 0)
            {
                currentPosition += DialSize;
            }

            if (currentPosition >= DialSize)
            {
                currentPosition -= DialSize;
            }
            if (currentPosition == 0)
            {
                zeroCount++;
            }

        }
        return zeroCount;
    }
    
}