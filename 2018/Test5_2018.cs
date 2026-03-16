using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Test5_2018 : BaseTest
{
    public override void Initialise()
    {
        Year = 2018;
        TestID = 5;
    }

    public override void Execute()
    {
        if (IsPart1)
        {
            ExecutePart1();
        }
        else
        {
            ExecutePart2();
        }
    }

    public void ExecutePart1()
    {
        LinkedList<char> list = new LinkedList<char>();
        
        LinkedListNode<char> rootNode = list.First;
        
        foreach(char c in m_dataFileContents[0])
        {
            list.AddLast(c);
        }

        FullyReact(list);

        string result = "";
        foreach (char c in list)
        {
            result += c;
        }
        DebugOutput($"The final result is {result.Length}");
        DebugOutput($"The final result text is {result}");
        
    }

    public void FullyReact(LinkedList<char> list)
    {
        // aA diff = ?
        int matchDiff = 'a' - 'A';

        bool cleaned = false;
        while (!cleaned)
        {
            cleaned = true;
            LinkedListNode<char> currentNode = list.First;
            while (currentNode != null && currentNode.Next != null)
            {
                int diff = Math.Abs(currentNode.Value - currentNode.Next.Value);
                // 
                if (diff == matchDiff)
                {
                    LinkedListNode<char> previousNode = currentNode.Previous;
                    list.Remove(currentNode.Next);
                    list.Remove(currentNode);
                    cleaned = false;
                    currentNode = previousNode;
                    //break;
                }
                else
                {
                    currentNode = currentNode.Next;
                }
            }
        }
        
    }
    
    public void ExecutePart2()
    {
        // aA diff = ?

        int matchDiff = 'a' - 'A';


        int shortest = int.MaxValue;
        int shortestReacted = int.MaxValue;
        char shortestChar = 'a';


        for (char matchChar = 'a'; matchChar <= 'z'; matchChar++)
        {
            LinkedList<char> list = new LinkedList<char>();

            LinkedListNode<char> rootNode = list.First;

            foreach (char c in m_dataFileContents[0])
            {
                list.AddLast(c);
            }

            bool cleaned = false;
            while (!cleaned)
            {
                cleaned = true;
                LinkedListNode<char> currentNode = list.First;
                while (currentNode != null)
                {
                    if (currentNode.Value == matchChar || currentNode.Value == matchChar-matchDiff)
                    {
                        LinkedListNode<char> previousNode = currentNode.Previous;
                        list.Remove(currentNode);
                        cleaned = false;
                        currentNode = previousNode;
                        //break;
                    }
                    else
                    {
                        currentNode = currentNode.Next;
                    }
                }
            }

            FullyReact(list);
            if (list.Count < shortestReacted)
            {
                shortestChar = matchChar;
                shortestReacted = list.Count;
            }
        }

        DebugOutput($"The final result is {shortestReacted} for character {shortestChar}");
    }
    
}