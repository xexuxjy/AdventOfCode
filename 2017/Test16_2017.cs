using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Test16_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 16;
    }

    public override void Execute()
    {
        int numPrograms = IsTestInput?5:16;
        List<char> programs = new List<char>();
        for (int i = 0; i < numPrograms; i++)
        {
            programs.Add((char)(((int)'a')+i));
        }

        string[] commands = m_dataFileContents[0].Split(',');

        ProcessCommands(commands,programs);
        
        DebugOutput($"After the first dance {string.Join("", programs)}");

        if (IsPart2)
        {
            int numIterations = IsPart1 ? 1 : 1000000000;

            List<char> initialState = new List<char>();
            initialState.AddRange(programs);
            int repeatsAt = 0;
            // need to look and see at what point these repeat...
            for (int count = 0; count < numIterations; count++)
            {
                ProcessCommands(commands, programs);

                if (Enumerable.SequenceEqual(programs, initialState))
                {
                    // got back to start state after 
                    int ibreak = 0;
                    DebugOutput($"Got back to start state after {count+1} iterations");
                    repeatsAt = count + 1;
                    break;
                }
                
            }

            
            programs.Clear();
            programs.AddRange(initialState);
            numIterations = numIterations % repeatsAt;
            numIterations--;

            for (int count = 0; count < numIterations; count++)
            {
                ProcessCommands(commands, programs);
            }

            DebugOutput($"After the part2 dance {string.Join("", programs)}");
        }
    }

    void ProcessCommands(string[] commands, List<char> programs)
    {
        foreach (string line in commands)
        {
            if (line.StartsWith("s"))
            {
                int numPlaces = int.Parse(line.Remove(0, 1));
                for (int i = 0; i < numPlaces; i++)
                {
                    char lastChar = programs.Last();
                    programs.RemoveAt(programs.Count - 1);
                    programs.Insert(0, lastChar);
                }
            }
            else if (line.StartsWith("x"))
            {
                string[] tokens = line.Remove(0, 1).Split('/');
                int pos1 = int.Parse(tokens[0]);
                int pos2 = int.Parse(tokens[1]);
                programs.Swap(pos1, pos2);
            }
            else if (line.StartsWith("p"))
            {
                string[] tokens = line.Remove(0, 1).Split('/');
                int pos1 = programs.IndexOf(tokens[0][0]);
                int pos2 = programs.IndexOf(tokens[1][0]);
                programs.Swap(pos1, pos2);
            }
            else
            {
                Debug.Assert(false, "should not happen");
            }
        }        
    }
    
    
}