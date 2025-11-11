using System;
using System.Collections.Generic;


public class Test19_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 19;
    }

    public override void Execute()
    {
        
        List<int> presentCounts = new List<int>();
        LinkedList<int> activeElves = new LinkedList<int>();
        List<int> removeList = new List<int>();

        int numElves = IsTestInput ? 5 : 3005290;

        for (int i = 0; i < numElves; i++)
        {
            presentCounts.Add(1);
            activeElves.AddLast(i);
        }

        int steps = (activeElves.Count) / 2;
        LinkedListNode<int> midNode = activeElves.First;
        for (int i = 0; i < steps; i++)
        {
            midNode = CircularLinkedList.NextOrFirst<int>(midNode);
        }

        while (true)
        {
            DateTime startTime = DateTime.Now;

            if (IsPart1)
            {
                for (LinkedListNode<int> node = activeElves.First; node != null; node = node.Next)
                {
                    LinkedListNode<int> targetElf = CircularLinkedList.NextOrFirst<int>(node);
                    if (targetElf != null)
                    {
                        activeElves.Remove(targetElf);
                    }
                }
            }
            else
            {
                LinkedListNode<int> nextMidNode = CircularLinkedList.NextOrFirst<int>(midNode);;
                
                if (activeElves.Count % 2 == 1)
                {
                    nextMidNode = CircularLinkedList.NextOrFirst<int>(nextMidNode);
                }
                activeElves.Remove(midNode);
                midNode = nextMidNode;
            }

            double bpElapsed = DateTime.Now.Subtract(startTime).TotalMilliseconds;

            //DebugOutput($"Active Elves : {activeElves.Count} time {bpElapsed}");
            if (activeElves.Count == 1)
            {
                goto WinnerFound;
            }
        }


        WinnerFound:
        DebugOutput($"The winner is Elf {activeElves.First.Value + 1} with {numElves} presents.");
    }

}

