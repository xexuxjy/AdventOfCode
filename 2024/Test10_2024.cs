using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Test10_2024 : BaseTest
{
    public const int StartPosition = 0;
    public const int FinalGoal = 9;
    public override void Initialise()
    {
        Year = 2024;
        TestID = 10;
    }

    public override void Execute()
    { 
        int width = 0;
        int height = 0;
        
        width = m_dataFileContents[0].Length;
        height = m_dataFileContents.Count;

        int[] dataGrid = new int[width * height];;//Helper.GetNumGrid(m_dataFileContents,ref width, ref height);


        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                int val  = 0;
                if(m_dataFileContents[y][x] == '.')
                {
                    dataGrid[y * width + x] = -1;
                }
                else
                {
                    dataGrid[y * width + x] = int.Parse("" + m_dataFileContents[y][x]);
                }
            }
        }


        Dictionary<IntVector2,List<IntVector2>> scores = new Dictionary<IntVector2,List<IntVector2>>();

        for(int y=0;y<height;++y)
        {
            for(int x=0;x<width;++x)
            {

                int index = (y*width)+x;
                if(dataGrid[index] == StartPosition)
                {
                    IntVector2 p = new IntVector2(x,y);
                    FindTrail(p,p,dataGrid,width,height,scores,new List<IntVector2>());
                }
            }

        }

        int total = 0;
        foreach(var score in scores.Values)
        {
            if(IsPart2)
            {
                total += score.Count;
            }
            else
            {
                HashSet<IntVector2> tempHashSet = new HashSet<IntVector2>();
                foreach(IntVector2 v in score)
                {
                    tempHashSet.Add(v);
                }
                total += tempHashSet.Count;
            }

        }

        DebugOutput("Final score is : "+total);

        
    }


    public void FindTrail(IntVector2 startPosition,IntVector2 currentPosition,int[] dataGrid,int width,int height,Dictionary<IntVector2,List<IntVector2>> scores,List<IntVector2> history)
    {
        int currentIndex = (currentPosition.Y * width) + currentPosition.X;

        if(dataGrid[currentIndex] == FinalGoal)
        {
            HashSet<IntVector2> summits = null;
            if(!scores.ContainsKey(startPosition))
            {
                scores[startPosition] = new List<IntVector2>();

            }

            scores[startPosition].Add(currentPosition);
            //string result = $" {scores[startPosition]} : "+string.Join("   ",history);
            //DebugOutput(result);
        }
        else
        {
            foreach(IntVector2 move in IntVector2.Directions)
            {
                IntVector2 newPosition = currentPosition + move;


                if(Helper.InBounds(newPosition,width,height))
                {
                    int newIndex = (newPosition.Y * width) + newPosition.X;

                    // stepping up 1
                    if(dataGrid[newIndex] == dataGrid[currentIndex]+1)
                    {
                        List<IntVector2> historyCopy =new List<IntVector2>();
                        historyCopy.AddRange(history);
                        historyCopy.Add(newPosition);

                        FindTrail(startPosition,newPosition,dataGrid,width,height,scores,historyCopy);
                    }
                }
            }
        }

    }

}
