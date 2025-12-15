using System;
using System.Collections.Generic;

public class Test12_2025 : BaseTest
{
    public override void Initialise()
    {
        Year = 2025;
        TestID = 12;
    }

    public static int NumShapes = 6;
    public static int ShapeRows = 5;

    
    public override void Execute()
    {

        
        int shapeLines = NumShapes * ShapeRows;

        List<PresentShape> presentShapes = new List<PresentShape>();
        List<PresentArea> presentAreas = new List<PresentArea>();

        int dataIndex = 0;
        for (int i = 0; i < shapeLines; )
        {
            string line = m_dataFileContents[i];
            if (i % ShapeRows == 0)
            {
                PresentShape presentShape = new PresentShape();
                presentShapes.Add(presentShape);
                presentShape.ReadPattern(m_dataFileContents,i);
                i += ShapeRows;
            }

            dataIndex = i;
        }
        
        for(int i=dataIndex; i<m_dataFileContents.Count;i++)
        {
            PresentArea presentArea = new PresentArea();
            presentAreas.Add(presentArea);
            string[] tokens = m_dataFileContents[i].Split(' ');

            string[] areaTokens = tokens[0].Replace(":", "").Split('x');
            presentArea.Width =  int.Parse(areaTokens[0]);
            presentArea.Height = int.Parse(areaTokens[1]);
            for (int j = 1; j < tokens.Length; j++)
            {
                presentArea.PresentRequirements.Add(int.Parse(tokens[j]));
            }
        }

        int ibreak = 0;
    }

    public class PresentShape
    {
        public bool[] Pattern;

        public void ReadPattern(List<string> rows, int index)
        {
            Pattern = new bool[3 * (ShapeRows - 2)];
            int count = 0;
            for (int i = 0; i < ShapeRows - 2; i++)
            {
                string line = rows[index + i + 1];
                foreach (char c in line)
                {
                    Pattern[count++] = c == '#' ? true : false;
                }
            }
        }

    }

    public class PresentArea
    {
        public int Width;
        public int Height;

        public List<int> PresentRequirements = new List<int>();

    }
    
    
}