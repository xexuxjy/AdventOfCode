using AdventOfCode.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Test8_2023;

public class Test18_2015 : BaseTest
{
    public const char ON = '#';
    public const char OFF = '.';
    public override void Initialise()
    {
        Year = 2015;
        TestID = 18;
    }


    public override void Execute()
    {
        int width = 0;
        int height = 0;
        char[] dataGrid = Helper.GetCharGrid(m_dataFileContents,ref width,ref height);
        char[] dataGrid2 = Helper.GetCharGrid(m_dataFileContents,ref width,ref height);

        // Corners are on.
        if(IsPart2)
        {
            dataGrid[0] = ON;
            dataGrid[(width-1)] = ON;
            dataGrid[((height-1)*width)] = ON;
            dataGrid[((height-1)*width)+width-1] = ON;
        }

        List<char[]> dataGrids = new List<char[]>();
        dataGrids.Add(dataGrid);
        dataGrids.Add(dataGrid2);

        bool flipFlop = true;

        int numIterations = IsTestInput?5:100;

        DebugOutput(Helper.DrawGrid(dataGrids[0],width,height));

        for(int i=0;i<numIterations;i++)
        {
            UpdateGrid(dataGrids[flipFlop?0:1],dataGrids[flipFlop?1:0],width,height);
            //DebugOutput(Helper.DrawGrid(dataGrids[flipFlop?1:0],width,height));
            if(i<numIterations-1)
            {
                flipFlop = !flipFlop;
            }
        }

        int lightsOn = dataGrids[flipFlop?1:0].Count(x=> x == ON);

        DebugOutput($"After {numIterations} there are {lightsOn} lights on");


    }

    public void UpdateGrid(char[] src,char[] dst,int width,int height)
    {
        for(int x=0;x<width;x++)
        {
            for(int y=0;y<height;y++)
            {
                char currentState = src[(y*width)+x];
                char[] surrounds = new char[8];

                surrounds[0] = ((x>0 && y>0) ? src[((y-1)*width)+(x-1)] : OFF);
                surrounds[1] = ((y>0) ? src[((y-1)*width)+(x)] : OFF);
                surrounds[2] = ((y>0 && x < width-1) ? src[((y-1)*width)+(x+1)] : OFF);

                surrounds[3] =((x>0 ) ? src[((y)*width)+(x-1)] : OFF);
                surrounds[4] =((x<width-1 ) ? src[((y)*width)+(x+1)] : OFF);

                surrounds[5] = ((x>0 && y<height-1) ? src[((y+1)*width)+(x-1)] : OFF);
                surrounds[6] = ((y<height-1) ? src[((y+1)*width)+(x)] : OFF);
                surrounds[7] = ((y<height-1 && x < width-1) ? src[((y+1)*width)+(x+1)] : OFF);





                int neighboursOn = surrounds.Count(x=>x== ON);

                //The state a light should have next is based on its current state (on or off) plus the number of neighbors that are on:

                //    A light which is on stays on when 2 or 3 neighbors are on, and turns off otherwise.
                //    A light which is off turns on if exactly 3 neighbors are on, and stays off otherwise.

                char targetState = currentState;
                if(currentState == ON)
                {
                    if(!(neighboursOn == 2 || neighboursOn == 3))
                    {
                        targetState = OFF;
                    }
                }
                else
                {
                    if(neighboursOn == 3)
                    {
                        targetState = ON;
                    }
                }

                if(IsPart2)
                {
                    bool isCorner = (x==0 && y==0) || (x==width-1 && y == 0) || (x==0 && y==height-1) || (x==width-1 &&y==height-1);
                    if (isCorner)
                    {
                        targetState = ON;
                    }
                }

                
                dst[(y*width)+x] = targetState;
            }
        }


    }

}