using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test2_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 2;
    }

    public override void Execute()
    {
        int paperTotal = 0;
        int ribbonTotal = 0;

        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split('x');


            int width = int.Parse(tokens[0]);
            int length = int.Parse(tokens[1]); ;
            int height = int.Parse(tokens[2]); ;

            int perimeter1 = (2*width)+(2*height);
            int perimeter2 = (2*width)+(2*length);
            int perimeter3 = (2*length)+(2*height);

            int smallestPerimeter = Math.Min(perimeter1, Math.Min(perimeter2,perimeter3));


            int area1 = width * length;
            int area2 = width * height;
            int area3 = height * length;
            int extra = Math.Min(area1, Math.Min(area2, area3));

            int boxTotal = (2 * area1) + (2 * area2) + (2 * area3) + extra;

            int bow = width*height*length;

            ribbonTotal += (smallestPerimeter+bow);
            paperTotal += boxTotal;
        }

        if (IsPart2)
        {
            DebugOutput("Total ribbopn needed is : "+ribbonTotal);
        }
        else
        {
            DebugOutput("Total paper needed is : "+paperTotal);
        }
    }
}

