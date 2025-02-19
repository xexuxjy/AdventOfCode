using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test1_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 1;
    }

    public override void Execute()
    {
        if (IsPart2)
        {
            int current = 0;
            for(int i=0;i<m_dataFileContents[0].Length;++i)
            {
                char c = m_dataFileContents[0][i];
                if (c == '(')
                {
                    current++;

                }
                else
                {
                    current--;
                }

                if(current <0)
                {
                    DebugOutput("We entered the basement at position : "+(i+1));
                    break;
                }
            }
        }
        else
        {

            int numOpen = m_dataFileContents[0].Count(x => x == '(');
            int numClosed = m_dataFileContents[0].Count(x => x == ')');

            DebugOutput("Final position is : " + (numOpen - numClosed));
        }
    }

}