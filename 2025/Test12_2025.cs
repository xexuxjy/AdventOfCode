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
                presentShape.BuildVariations();
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

        foreach (PresentShape presentShape in presentShapes)
        {
            presentShape.DrawVariations(this);
        }
        
        int ibreak = 0;
    }

    public class PresentShape
    {
        public const int PatternDims = 3;
        public bool[] Pattern;
        
        public List<(string,bool[])> Variations = new List<(string,bool[])>();
        
        
        public int ShapeSize = 0;
        
        public void ReadPattern(List<string> rows, int index)
        {
            Pattern = new bool[PatternDims * PatternDims];
            int count = 0;
            for (int i = 0; i < PatternDims; i++)
            {
                string line = rows[index + i + 1];
                foreach (char c in line)
                {
                    Pattern[count++] = c == '#' ? true : false;
                    ShapeSize++;
                }
            }
        }

        public void BuildVariations()
        {
            Variations.Add(("Original",Pattern));

            bool[] Rotate90 = new bool[PatternDims * PatternDims];
            bool[] Rotate180 = new bool[PatternDims * PatternDims];
            bool[] Rotate270 = new bool[PatternDims * PatternDims];

            int[,] matrix = new int[PatternDims, PatternDims];
            int count = 0;
            for (int x = 0; x < PatternDims; x++)
            {
                for (int y = 0; y < PatternDims; y++)
                {
                    //matrix[x, y] = count++;
                    matrix[y, x] = count++;
                }
                    
            }

            int[,] matrix90 = Helper.RotateMatrixCounterClockwise(matrix);
            int[,] matrix180 = Helper.RotateMatrixCounterClockwise(matrix90);
            int[,] matrix270 = Helper.RotateMatrixCounterClockwise(matrix180);

            for (int x = 0; x < PatternDims; x++)
            {
                for(int y = 0; y < PatternDims; y++)
                {
                    Rotate90[matrix90[x,y]] = Pattern[(y*PatternDims)+x];
                    Rotate180[matrix180[x,y]] = Pattern[(y*PatternDims)+x];
                    Rotate270[matrix270[x,y]] = Pattern[(y*PatternDims)+x];
                }
            }
            
            Variations.Add(("Rotate90",Rotate90));
            Variations.Add(("Rotate180", Rotate180));
            Variations.Add(("Rotate270", Rotate270));
        }

        public void DrawVariations(BaseTest baseTest)
        {
            foreach (var variation in Variations)
            {
                baseTest.DebugOutput(variation.Item1);
                baseTest.DebugOutput(Helper.DrawGridHash(variation.Item2,PatternDims,PatternDims));    
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