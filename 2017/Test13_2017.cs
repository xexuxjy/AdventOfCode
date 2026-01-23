using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

public class Test13_2017 : BaseTest
{
    public List<FWLayer> m_layers = new List<FWLayer>();
    
    public override void Initialise()
    {
        Year = 2017;
        TestID = 13;
    }

    public override void Execute()
    {
        int layerId = 0;
        foreach (string line in m_dataFileContents)
        {
            string[] tokens =  line.Split(':', StringSplitOptions.RemoveEmptyEntries);
            FWLayer layer = new FWLayer();
            layer.LayerId = layerId++;
            layer.Depth = int.Parse(tokens[0]);
            layer.Range = int.Parse(tokens[1]);
            m_layers.Add(layer);
        }

        m_layers.Sort((x,y)=>x.Depth.CompareTo(y.Depth));

        if (IsPart1)
        {

            int maxLayer = m_layers.Max(l => l.Depth);
            int currentPosition = 0;
            int penalty = 0;
            while (currentPosition <= maxLayer)
            {
                foreach (FWLayer layer in m_layers)
                {
                    if (layer.Depth == currentPosition && layer.ScanPosition == 0)
                    {
                        penalty += (layer.Depth * layer.Range);
                    }
                }
                
                MoveLayers();

                currentPosition++;

            }


            DebugOutput($"The final penalty is {penalty}");
        }
        else
        {
            TestPart2Mod();
        }



    }

    public void TestPart2Mod()
    {
        int delay = 1;
        bool keepGoing = true;

        while (keepGoing)
        {
            int currentPosition = 0;
            bool safe = true;
            foreach (FWLayer layer in m_layers)
            {
                int mod = (layer.Range - 1) * 2;
                if ((delay+layer.Depth) % mod == 0)
                {
                    delay += 1;
                    safe = false;
                    break;
                }
            }

            if (safe)
            {
                keepGoing = false;
            }
        }
        
        DebugOutput($"A delay of {delay} gets through safely");
        
    }
    public void TestPart2()
    {
        int delay = 1;
        bool keepGoing = true;
        int maxLayer = m_layers.Max(l => l.Depth);

        while (keepGoing)
        {
            foreach (FWLayer layer in m_layers)
            {
                layer.Reset();
            }

            for (int i = 0; i < delay; i++)
            {
                MoveLayers();
            }
                
            int currentPosition = 0;

            bool caught = false;
            while (currentPosition <= maxLayer)
            {
                foreach (FWLayer layer in m_layers)
                {
                    if (layer.Depth == currentPosition && layer.ScanPosition == 0)
                    {
                        caught = true;
                    }
                }

                if (caught)
                {
                    delay++;
                    break;
                }
                    
                MoveLayers();

                currentPosition++;

            }

            if (caught == false)
            {
                DebugOutput($"A delay of {delay} gets through safely");
                keepGoing = false;
            }
        }

        // List<long> test = new List<long>();
        // foreach (FWLayer layer in m_layers)
        // {
        //     test.Add(layer.Depth+layer.Range);
        // }
        //
        // long result = Helper.LCM(test);
        // int ibreak = 0;

    }
    

    public void DrawLayers()
    {
        foreach(FWLayer layer in m_layers)
        {
            layer.DebugOutput(this);
        }

        DebugOutput("");
    }

    public void MoveLayers()
    {
        foreach (FWLayer layer in m_layers)
        {
            layer.Move();
        }
        
    }
    
}


public class FWLayer
{
    public int LayerId;
    public int Depth;
    public int Range;
    public int ScanPosition;
    public int Direction = 1;

    public void Move()
    {
        ScanPosition += Direction;

        if (ScanPosition == 0 || ScanPosition == Range-1)
        {
            Direction *= -1;
        }
    }

    public void Reset()
    {
        ScanPosition = 0;
        Direction = 1;
    }
    
    public void DebugOutput(BaseTest test)
    {
        StringBuilder sb =  new StringBuilder();
        sb.Append(Depth);
        sb.Append(" : ");
        for (int i = 0; i < Range; i++)
        {
            sb.Append("[");
            sb.Append(i==ScanPosition ? "S" : " ");
            sb.Append("]");
        }

        test.DebugOutput(sb.ToString());

    }
    
    
}