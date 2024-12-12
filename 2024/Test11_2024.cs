using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static Test11_2024;
using static Test8_2023;

public class Test11_2024 : BaseTest
{
    public override void Initialise()
    {
        Year = 2024;
        TestID = 11;
    }

    public override void Execute()
    {

        SingleListNode<long> startNode = null;
        SingleListNode<long> currentNode = null;

        string[] tokens = m_dataFileContents[0].Split(' ');
        foreach (string token in tokens)
        {
            SingleListNode<long> val = new SingleListNode<long>(long.Parse(token));
            if(startNode == null)
            {
                startNode = val;
                currentNode = startNode;
            }
            else
            {
                currentNode.Next = val;
                currentNode = val;
            }

        }

        //DebugOutput(string.Join("  ", numberList));

        int numBlinks = IsPart2?75:25;
        long last = 0;

        long runningTotal = 0;

        var node = startNode;
            
        while(node != null)
        {
            runningTotal+=RecursiveCount(node.Value,numBlinks);
            node = node.Next;
        }



        //for (int b = 0; b < numBlinks; b++)
        //{

        //    var node = startNode;
            
        //    while(node != null)
        //    {
        //        node = ApplyRule(node);
        //    }


        //    string totalString1 = "";
        //    int t = 0;
        //    currentNode = startNode;
        //    while (currentNode != null)
        //    {
        //        //totalString1 += currentNode.Value + "  ";
        //        currentNode = currentNode.Next;
        //        t++;
        //    }
        //    //DebugOutput(totalString1);
        //    //DebugOutput(string.Join("  ", numberList));
        //    //DebugOutput($"Total stones after {b}  diff {t-last} : "+t);
        //    DebugOutput($"Total stones after {b} : {t} : diff {t-last} ");
        //    last = t;
        //}

        //int total = 0;
        ////string totalString = "";
        //currentNode = startNode;
        //while (currentNode != null)
        //{
        //    total++;
        //    //totalString += currentNode.Value + "  ";
        //    currentNode = currentNode.Next;

        //}

        //DebugOutput(totalString);
        DebugOutput($"Total stones after {numBlinks}  : "+runningTotal);
    }



    public SingleListNode<long> ApplyRule(SingleListNode<long> node)
    {
        long stoneValue = node.Value;
        int digitCount = CountDigit(stoneValue);
        bool added = false;
        

        if (stoneValue == 0)
        {
            stoneValue = 1;
            node.Value = stoneValue;
            return node.Next;
        }
        else if (digitCount % 2 == 0)
        {
            //string stoneStr = stoneValue.ToString();
            //int half = digitCount / 2;
            //long stoneVal1 = long.Parse(stoneStr.Substring(0, half));
            //long stoneVal2 = long.Parse(stoneStr.Substring(half));
            
            uint Base = 10;
            uint divisor = Base;
        
            while ( node.Value / divisor > divisor ) divisor *= Base;

            long stoneVal1 = node.Value/divisor;
            long stoneVal2 = node.Value%divisor;


            node.Value = stoneVal1;

            SingleListNode<long> insertedNode = new SingleListNode<long>(stoneVal2);
            insertedNode.Next = node.Next;
            node.Next = insertedNode;
            return insertedNode.Next;
        }
        else
        {
            stoneValue = stoneValue * 2024;
            node.Value = stoneValue;
            return node.Next;
        }
        return null;
    }

    //https://www.geeksforgeeks.org/program-count-digits-integer-3-different-methods/
    static int CountDigit(long n)
    {

        // Base case
        if (n == 0)
            return 1;

        int count = 0;

        // Iterate till n has digits remaining
        while (n != 0)
        {

            // Remove rightmost digit
            n = n / 10;

            // Increment digit count by 1
            ++count;
        }
        return count;
    }

    Dictionary<LongVector2,long> cache = new Dictionary<LongVector2,long>();

    public long RecursiveCount(long number,int steps)
    {
        if(steps == 0)
        {
            return 1;
        }
        long result = 0;
        LongVector2 key = new LongVector2(number,steps);
        if(cache.ContainsKey(key))
        {
            return cache[key];
        }

        int digitCount = CountDigit(number);

        if(number == 0)
        {
            result = RecursiveCount(1,steps-1);
           
        }
        else if (digitCount % 2 == 0)
        {
            uint Base = 10;
            uint divisor = Base;
        
            while ( number / divisor > divisor ) divisor *= Base;

            long stoneVal1 = number/divisor;
            long stoneVal2 = number%divisor;

            result = (RecursiveCount(stoneVal1,steps-1) + RecursiveCount(stoneVal2,steps-1));
        }
        else
        {
            result = RecursiveCount(number * 2024, steps-1);
        }
        cache[key] = result;
        return result;

    }


    public class SingleListNode<T> 
    {
        public T Value;
        public SingleListNode<T> Next;

        public SingleListNode(T value) { Value = value; }

    }

}