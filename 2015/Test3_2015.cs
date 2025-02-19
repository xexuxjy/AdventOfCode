using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test3_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 3;
    }

    public override void Execute()
    {


        if (IsPart2)
        {
            HashSet<IntVector2> locations = new HashSet<IntVector2>();


            IntVector2 position = new IntVector2();
            IntVector2 position2 = new IntVector2();

            locations.Add(position);
            locations.Add(position2);

            string line = m_dataFileContents[0];
            for (int i = 0; i < line.Length; i += 2)
            {
                IntVector2 update1 = Helper.DirectionFromPointer(line[i]);
                IntVector2 update2 = Helper.DirectionFromPointer(line[i + 1]);

                position += update1;
                position2 += update2;
                locations.Add(position);
                locations.Add(position2);
            }
            DebugOutput("Total locations is : " + locations.Count);
        }
        else
        {
            HashSet<IntVector2> locations = new HashSet<IntVector2>();


            IntVector2 position = new IntVector2();
            locations.Add(position);
            foreach (char c in m_dataFileContents[0])
            {
                IntVector2 update = Helper.DirectionFromPointer(c);
                position += update;
                locations.Add(position);
            }

            DebugOutput("Total locations is : " + locations.Count);
        }
    }
}

