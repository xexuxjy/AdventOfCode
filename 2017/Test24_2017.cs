using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

public class Test24_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 24;
    }

    public int HighestScore = 0;
    public int MaxDepth = 0;

    public override void Execute()
    {
        List<Component> components = new List<Component>();

        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split('/');
            Component c = new Component();
            c.LHS = int.Parse(tokens[0]);
            c.RHS = int.Parse(tokens[1]);
            components.Add(c);
        }


        List<Component> path = new List<Component>();
        CheckNext(0, components, 0, 0, path);

        if (IsPart1)
        {
            DebugOutput($"The highest score is {HighestScore}");
        }
        else
        {
            DebugOutput($"The longest chain is {MaxDepth} with strength {HighestScore}");
        }
    }


    public void CheckNext(int match, List<Component> components, int total, int depth, List<Component> path)
    {
        if (IsPart1)
        {
            if (total > HighestScore)
            {
                HighestScore = total;
            }
        }
        else
        {
            if (depth >= MaxDepth)
            {
                MaxDepth = depth;
                if (total > HighestScore)
                {
                    HighestScore = total;
                }
            }
        }

        foreach (Component c in components)
        {
            if (!c.Used && (c.LHS == match || c.RHS == match))
            {
                int nextNumber = c.LHS == match ? c.RHS : c.LHS;

                c.Used = true;
                path.Add(c);

                CheckNext(nextNumber, components, total + c.Total, depth + 1, path);
                path.RemoveAt(path.Count - 1);

                c.Used = false;
            }
        }
    }
}

public class Component
{
    public int LHS;
    public int RHS;

    public int Total
    {
        get { return LHS + RHS; }
    }

    public string DebugInfo
    {
        get { return $"{LHS}/{RHS} "; }
    }

    public bool Used = false;
}