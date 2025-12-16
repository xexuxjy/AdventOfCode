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
                presentShape.BaseTest = this;
                presentShapes.Add(presentShape);
                presentShape.Id = ("" + (presentShapes.Count - 1))[0];
                
                presentShape.ReadPattern(m_dataFileContents,i);
                presentShape.BuildVariations();
                i += ShapeRows;
            }

            dataIndex = i;
        }
        
        for(int i=dataIndex; i<m_dataFileContents.Count;i++)
        {
            PresentArea presentArea = new PresentArea();
            presentArea.BaseTest = this;
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

        // foreach (PresentShape presentShape in presentShapes)
        // {
        //     presentShape.DrawVariations();
        // }


        int fitCount = 0;
        foreach (PresentArea presentArea in presentAreas)
        {
            if (presentArea.TryFit(presentShapes))
            {
                fitCount++;
            }
        }
        
        
        DebugOutput($"Managed to fit {fitCount} out of {presentAreas.Count} present areas.");
        int ibreak = 0;
    }

    public class PresentShape
    {
        public char Id;
        public const int PatternDims = 3;
        public bool[] Pattern;

        public BaseTest BaseTest;
        
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
                    if (c == '#')
                    {
                        ShapeSize++;
                    }
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

        public void DrawVariations()
        {
            foreach (var variation in Variations)
            {
                BaseTest.DebugOutput(variation.Item1);
                BaseTest.DebugOutput(Helper.DrawGridHash(variation.Item2,PatternDims,PatternDims));    
            }
        }
        
    }

    public class PresentArea
    {
        public int Width;
        public int Height;

        public List<int> PresentRequirements = new List<int>();

        public char[] Grid;

        public BaseTest BaseTest;
        
        public bool TryFit(List<PresentShape> presentShapes)
        {
            Grid = new char[Width * Height];
            Array.Fill(Grid, '.');
            
            List<PresentShape> toFit =  new List<PresentShape>();
            for (int i = 0; i < PresentRequirements.Count; i++)
            {
                if (PresentRequirements[i] >= 0)
                {
                    for (int j = 0; j < PresentRequirements[i]; j++)
                    {
                        toFit.Add(presentShapes[i]);
                    }
                }
            }
            
            // simple check.
            int totalSpaceNeeded = 0;
            foreach (PresentShape presentShape in toFit)
            {
                totalSpaceNeeded += presentShape.ShapeSize;
            }

            if (totalSpaceNeeded > Grid.Length)
            {
                return false;
            }

            bool result = CheckFitRecursive(toFit, 0, Grid, 0);
            
            //BaseTest.DebugOutput(Helper.DrawGrid(Grid,Width,Height));

            return result;
        }

        public bool CheckFitRecursive(List<PresentShape> presentShapes, int shapeIndex, char[] grid,int depth)
        {
            if (shapeIndex == presentShapes.Count)
            {
                BaseTest.DebugOutput(Helper.DrawGrid(grid,Width,Height));
                return true;
            }
            
            PresentShape presentShape = presentShapes[shapeIndex];

            bool success = false;
            
            foreach (var variation in presentShape.Variations)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        if (CanFillGrid(grid, x, y, Width, Height, variation.Item2))
                        {
                            char[] gridCopy = new char[grid.Length];
                            grid.CopyTo(gridCopy, 0);
                            FillGrid(gridCopy, x, y, Width, Height, presentShape.Id, variation.Item2);
                            
                            success = CheckFitRecursive(presentShapes, shapeIndex+1, gridCopy, depth+1);
                            if (success)
                            {
                                return success;
                            }
                        }
                    }
                }
            }
            return success;
        }



        public bool CanFillGrid(char[] gridData,int x, int y,int width,int height,bool[] shapeData)
        {
            //if (x + PresentShape.PatternDims >= width || y + PresentShape.PatternDims >= height)
            if (x + PresentShape.PatternDims > width || y + PresentShape.PatternDims > height)
            {
                return false;
            }

            for (int ay = 0; ay < PresentShape.PatternDims; ay++)
            {
                for (int ax = 0; ax < PresentShape.PatternDims; ax++)
                {
                    int shapeIndex = (ay*PresentShape.PatternDims)+ax;
                    int gridIndex = (y+ay) * width + x+ax;

                    // square already filled.
                    if (shapeData[shapeIndex] && gridData[gridIndex] != '.')
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    
        public bool FillGrid(char[] gridData,int x, int y,int width,int height,char shapeId,bool[] shapeData)
        {
            if (x + PresentShape.PatternDims > width || y + PresentShape.PatternDims > height)
            {
                return false;
            }

            for (int ay = 0; ay < PresentShape.PatternDims; ay++)
            {
                for (int ax = 0; ax < PresentShape.PatternDims; ax++)
                {
                    int shapeIndex = (ay*PresentShape.PatternDims)+ax;
                    int gridIndex = (y+ay) * width + x+ax;
                    if (shapeData[shapeIndex])
                    {
                        gridData[gridIndex] = shapeId;
                    }
                }
            }
            return true;
        }


        
    }
    
    

    
    
}