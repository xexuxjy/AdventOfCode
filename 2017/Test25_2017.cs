using System;
using System.Collections.Generic;

public class Test25_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 25;
    }

    public override void Execute()
    {
        if (IsTestInput)
        {
            ExecuteTestSteps();
        }
        else
        {
            ExecuteSteps();
        }
    }

    Dictionary<long, int> Tape = new Dictionary<long, int>();


    public void ExecuteTestSteps()
    {
        long currentPosition = 0;
        int numSteps = 6;
        char currentState = 'A';

        for (int i = 0; i < numSteps; i++)
        {
            if (currentState == 'A')
            {
                if (ReadTape(currentPosition) == 0)
                {
                    WriteTape(currentPosition, 1);
                    currentPosition++;
                    currentState = 'B';
                }
                else
                {
                    WriteTape(currentPosition, 0);
                    currentPosition--;
                    currentState = 'B';
                }
            }
            else if (currentState == 'B')
            {
                if (ReadTape(currentPosition) == 0)
                {
                    WriteTape(currentPosition, 1);
                    currentPosition--;
                    currentState = 'A';
                }
                else
                {
                    WriteTape(currentPosition, 1);
                    currentPosition++;
                    currentState = 'B';
                }
            }
        }

        int count = Tape.Where(x => x.Value == 1).Count();
        DebugOutput($"After test, checksum is {count}");
    }

    public void ExecuteSteps()
    {
        long currentPosition = 0;
        long numSteps = 12629077;
        char currentState = 'A';

        for (long i = 0; i < numSteps; i++)
        {
            if (currentState == 'A')
            {
                if (ReadTape(currentPosition) == 0)
                {
                    WriteTape(currentPosition, 1);
                    currentPosition++;
                    currentState = 'B';
                }
                else
                {
                    WriteTape(currentPosition, 0);
                    currentPosition--;
                    currentState = 'B';
                }
            }
            else if (currentState == 'B')
            {
                if (ReadTape(currentPosition) == 0)
                {
                    WriteTape(currentPosition, 0);
                    currentPosition++;
                    currentState = 'C';
                }
                else
                {
                    WriteTape(currentPosition, 1);
                    currentPosition--;
                    currentState = 'B';
                }
            }
            else if (currentState == 'C')
            {
                if (ReadTape(currentPosition) == 0)
                {
                    WriteTape(currentPosition, 1);
                    currentPosition++;
                    currentState = 'D';
                }
                else
                {
                    WriteTape(currentPosition, 0);
                    currentPosition--;
                    currentState = 'A';
                }
            }
            else if (currentState == 'D')
            {
                if (ReadTape(currentPosition) == 0)
                {
                    WriteTape(currentPosition, 1);
                    currentPosition--;
                    currentState = 'E';
                }
                else
                {
                    WriteTape(currentPosition, 1);
                    currentPosition--;
                    currentState = 'F';
                }
            }
            else if (currentState == 'E')
            {
                if (ReadTape(currentPosition) == 0)
                {
                    WriteTape(currentPosition, 1);
                    currentPosition--;
                    currentState = 'A';
                }
                else
                {
                    WriteTape(currentPosition, 0);
                    currentPosition--;
                    currentState = 'D';
                }
            }
            else if (currentState == 'F')
            {
                if (ReadTape(currentPosition) == 0)
                {
                    WriteTape(currentPosition, 1);
                    currentPosition++;
                    currentState = 'A';
                }
                else
                {
                    WriteTape(currentPosition, 1);
                    currentPosition--;
                    currentState = 'E';
                }
            }
        }

        int count = Tape.Where(x => x.Value == 1).Count();
        DebugOutput($"After test, checksum is {count}");
    }


    public int ReadTape(long position)
    {
        if (Tape.ContainsKey(position))
        {
            return Tape[position];
        }

        return 0;
    }

    public void WriteTape(long position, int value)
    {
        Tape[position] = value;
    }
}