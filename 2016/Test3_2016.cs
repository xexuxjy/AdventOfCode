using System;
using System.Collections.Generic;
public class Test3_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 3;
    }
    public override void Execute()
    {
        if (IsTestInput)
        {
            int[] testArray = { 10, 5, 25 };
            DebugOutput($"Test Valid triangle {ValidTriangle(testArray[0],testArray[1],testArray[2])}");
        }
        else
        {
            int possible = 0;
            
            
            if(IsPart1)
            {
            foreach (string line in m_dataFileContents)
            {
                string[] tokens = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                int[] points = { int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]) };
                if (ValidTriangle(points[0],points[1],points[2]))
                {
                    possible++;
                }

            }
            }
            else
            {
                List<int> column1 = new List<int>();
                List<int> column2 = new List<int>();
                List<int> column3 = new List<int>();
                foreach (string line in m_dataFileContents)
                {
                    string[] tokens = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    column1.Add(int.Parse(tokens[0]));
                    column2.Add(int.Parse(tokens[1]));
                    column3.Add(int.Parse(tokens[2]));

                }
                for(int i=0; i<column1.Count; i+=3)
                {
                    possible += ValidTriangle(column1[i],column1[i+1],column1[i+2])?1:0;
                    possible += ValidTriangle(column2[i],column2[i+1],column2[i+2])?1:0;
                    possible += ValidTriangle(column3[i],column3[i+1],column3[i+2])?1:0;
                }
            }
            DebugOutput($"Total possible triangles is {possible}");
        }
    }

    private bool ValidTriangle(int a,int b,int c)
    {
        if (a + b <= c)
        {
            return false;
        }
        if (b + c  <= a)
        {
            return false;
        }
        if (c + a <= b)
        {
            return false;
        }
        return true;
    }
}
