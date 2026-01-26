using System;
using System.Collections.Generic;

public class Test14_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 14;
    }

    public override void Execute()
    {
        string input = m_dataFileContents[0];
        int numRows = 128;
        int total = 0;

        List<char> grid = new List<char>();

        for (int i = 0; i < numRows; i++)
        {
            string binaryRep = Convert1(input, i);
            total += binaryRep.Count(x => x == '1');
            if (IsPart2)
            {
                foreach (char c in binaryRep)
                {
                    grid.Add(c);
                }
            }

        }

        DebugOutput($"There are {total} squares used");
        
        if (IsPart2)
        {
            int groupCount = 0;
            for (int x = 0; x < numRows; x++)
            {
                for (int y = 0; y < numRows; y++)
                {
                    if (grid[(y * numRows) + x] == '1')
                    {
                        groupCount++;
                        Fill(grid,numRows, numRows,new IntVector2(x,y),'0','0');
                    }
                }
            }
        
            DebugOutput($"There are {groupCount} different groups");
        }
        
        
        

    }

    public string Convert1(string input, int index)
    {
        string fullInput = input+"-"+index;
        string hashedValue = Helper.CalculateKnotHash(fullInput);
        string binaryRep = String.Join(String.Empty,
            hashedValue.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
       
        return binaryRep;
    }


    public void Fill(List<char> grid, int width, int height, IntVector2 position,char emptyChar,char fillChar)
    {
        if (grid[(position.Y * width) + position.X] != emptyChar)
        {
            grid[(position.Y * width) + position.X] = fillChar;
            foreach (IntVector2 offset in IntVector2.Directions)
            {
                IntVector2 newPosition = position+offset;
                if (Helper.InBounds(newPosition, width, height))
                {
                    Fill(grid, width, height, position + offset, emptyChar, fillChar);
                }
            }
        }
    }
    
}