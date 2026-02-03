using System;
using System.Collections.Generic;

public class Test17_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 17;
    }

    public override void Execute()
    {
        int insertValue = 0;
        int numSteps = IsTestInput ? 3 : 377;
        
        int iterations = IsPart1?2017:50000000;

        int result = 0;
        
        LinkedList<int> buffer = new LinkedList<int>();
        LinkedListNode<int> current = buffer.AddLast(insertValue++);    
        
        LinkedListNode<int> zeroNode = current;
        
        for (int i = 0; i < iterations; i++)
        {
            for (int j = 0; j < numSteps; j++)
            {
                current = CircularLinkedList.NextOrFirst(current);
            }

            LinkedListNode<int> newNode = new LinkedListNode<int>(insertValue++);
            
            buffer.AddAfter(current, newNode);
            current = newNode;

            if (i % 1000000 == 0)
            {
                DebugOutput($"{i}");
            }
            
            if (i == iterations - 1)
            {
                result = CircularLinkedList.NextOrFirst(current).Value;
            }
        }

        if (IsPart2)
        {
            result = CircularLinkedList.NextOrFirst(zeroNode).Value;
        }

        DebugOutput($"The result is {result}");

    }
}