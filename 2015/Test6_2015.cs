using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test6_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 6;
    }

    int m_gridWidth = 1000;
    int[] m_lightGrid;

    public override void Execute()
    {

        m_lightGrid = new int[m_gridWidth*m_gridWidth];

        foreach(string line in m_dataFileContents)
        {
            if(line.StartsWith("turn on "))
            {
                string[] tokens = line.Replace("turn on ","").Replace("through","").Split(new char[]{',',' ' },StringSplitOptions.RemoveEmptyEntries);
                IntVector2 from = new IntVector2(int.Parse(tokens[0]),int.Parse(tokens[1]));
                IntVector2 to = new IntVector2(int.Parse(tokens[2]),int.Parse(tokens[3]));
                SetValue(from,to,1);

            }
            else if(line.StartsWith("turn off "))
            {
                string[] tokens = line.Replace("turn off ","").Replace("through","").Split(new char[]{',',' ' },StringSplitOptions.RemoveEmptyEntries);
                IntVector2 from = new IntVector2(int.Parse(tokens[0]),int.Parse(tokens[1]));
                IntVector2 to = new IntVector2(int.Parse(tokens[2]),int.Parse(tokens[3]));
                SetValue(from,to,-1);
            }
            else if(line.StartsWith("toggle "))
            {
                string[] tokens = line.Replace("toggle ","").Replace("through","").Split(new char[]{',',' ' },StringSplitOptions.RemoveEmptyEntries);
                IntVector2 from = new IntVector2(int.Parse(tokens[0]),int.Parse(tokens[1]));
                IntVector2 to = new IntVector2(int.Parse(tokens[2]),int.Parse(tokens[3]));
                ToggleValue(from,to);

            }

        }

        int count = 0;
        foreach(int val in m_lightGrid)
        {
            count+=val;
        }
        if(IsPart2)
        {
            DebugOutput($"The total brightness is : {count}");
        }
        else
        {
        DebugOutput($"There are : {count} lights on");
        }
        
    }

    private void SetValue(IntVector2 from, IntVector2 to,int val)
    {
        for(int x = from.X ; x <= to.X; x++)
        {
            for(int y = from.Y ; y <= to.Y; y++)
            {
               int index = (y*m_gridWidth)+x;

                if(IsPart2)
                {
                    m_lightGrid[index] = Math.Max(0,m_lightGrid[index]+val);
                }
                else
                {
                    m_lightGrid[index] = Math.Max(0,val);
                }
            }
        }
    }

    private void ToggleValue(IntVector2 from, IntVector2 to)
    {
        for(int x = from.X ; x <= to.X; x++)
        {
            for(int y = from.Y ; y <= to.Y; y++)
            {
               int index = (y*m_gridWidth)+x;
                if(IsPart2)
                {
                    m_lightGrid[index] = m_lightGrid[index]+2;
                }
                else
                {
                    if(m_lightGrid[index] == 0)
                    {
                        m_lightGrid[index] = 1;
                    }
                    else
                    {
                        m_lightGrid[index] = 0;
                    }
                }
            }
        }
    }


}

