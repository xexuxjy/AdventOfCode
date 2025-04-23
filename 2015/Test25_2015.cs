using AdventOfCode.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Test8_2023;

public class Test25_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 25;
    }


    public override void Execute()
    {
        int rowLength = 6;
        int numEntries = 100;
        int[] codeData = new int[numEntries];
        int[] codeData2 = new int[numEntries];


        int k = 0;
        int row = 0;
        int rowIncrement = 0;


        int counter = 1;
        int counterIncrement = 2;
        for(int i=0;i<rowLength;++i)
        {
            codeData[i] = counter;
            counter += counterIncrement;
            counterIncrement++;
            k++;
        }

        while(k < numEntries)
        {
            for(int i=0;i<rowLength;++i)
            {
                if(k<numEntries)
                {
                    codeData[k] = codeData[k-rowLength]  + (i+1+row);
                    k++;
                }
            }
            row++;
        }


        long startNumber = 20151125;

        int targetRow = 2978;
        int targetCol = 3083;

        int iterations = 0;
        int iterationBreak = 100;

        List<List<long>> debugInfo = new List<List<long>>();

        foreach(var pair in GetNextTrianglePosition())
        {

            if(targetRow == pair.Item1+1 && targetCol == pair.Item2+1)
            {
                break;
            }


            //iterations++;
            //if(iterationBreak >0 && iterations > iterationBreak)
            //{
            //    break;
            //}
            //DebugOutput($"The value at ({pair.Item1+1},{pair.Item2+1}) is {startNumber}");
            //DebugOutput($"The value at ({pair.Item1+1},{pair.Item2+1}) is {iterations}");

            //List<long> newList = null;
            //if(pair.Item1 < debugInfo.Count)
            //{
            //    newList = debugInfo[pair.Item1];
            //}
            //else
            //{   
            //    newList = new List<long>();
            //    debugInfo.Add(newList);
            //}

            //newList.Add(startNumber);


            startNumber *= 252533;
            startNumber = startNumber % 33554393;

        }


        foreach(List<long> intRow in debugInfo)
        {
            DebugOutput(string.Join(", ", intRow));
        }

        //for(int i=0;i<maxRow;i++)
        //{
        //    string line = "";
        //    for(int j = 0;j<maxCol;j++)
        //    {
        //        if(resultsDictionary.ContainsKey((j,i)))
        //        {
        //            line = line + resultsDictionary[(j,i)]+",";
        //        }
        //    }
        //    DebugOutput(line);
        //}
        DebugOutput($"The value at ({targetRow},{targetCol}) is {startNumber}");
    }


    public IEnumerable<(int,int)> GetNextTrianglePosition()
    {
        int row = 0;
        int col = 0;
        List<int> rowLengths = new List<int>();
        
        while(true)
        {
            if(rowLengths.Count == 0)
            {
                rowLengths.Add(0);
                yield return (0,0);
            }
            
            for(int i = rowLengths.Count-1;i > 0;i--) 
            {
                if(rowLengths[i] == rowLengths[i-1])
                {
                    rowLengths[i-1] +=1 ;
                    yield return (i-1,rowLengths[i-1]);
                }
            }

            rowLengths.Add(0);
            yield return (rowLengths.Count-1,0);
        }


    }

}