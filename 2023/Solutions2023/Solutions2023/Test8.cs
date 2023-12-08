using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test8 : BaseTest
{
    public override void Initialise()
    {
        TestID = 8;
        IsTestInput = false;
        IsPart2 = false;
    }

    public override void Execute()
    {
        string instructions = m_dataFileContents[0];

        string START = "AAA";
        string GOAL = "ZZZ";

        Dictionary<String, Node> nodeDictionary = new Dictionary<string, Node>();
        for (int i = 2; i < m_dataFileContents.Count; ++i)
        {
            Node node = new Node(m_dataFileContents[i]);
            nodeDictionary[node.Id] = node;
        }

        Node currentNode = nodeDictionary[START];
        int instructionCount = 0;
        int totalCount = 0;
        while (currentNode != nodeDictionary[GOAL])
        {
            totalCount++;
            if (instructions[instructionCount] == 'L')
            {
                currentNode = nodeDictionary[currentNode.LeftId];
            }
            else
            {
                currentNode = nodeDictionary[currentNode.RightId];
            }

            instructionCount++;
            if (instructionCount == instructions.Length)
            {
                instructionCount = 0;
            }
            
            // stop loop forever...
            if (totalCount > 10000000)
            {
                DebugOutput(("Probably in a loop. breaking"));
                break;
            }
        }
        DebugOutput("Reached destination in : "+totalCount);
    }

    public class Node
    {
        public string Id;
        public string LeftId;
        public string RightId;

        public bool EndsA => Id[2] == 'A';
        public bool EndsZ => Id[2] == 'Z';
        
        public Node(string data)
        {
            string[] tokens = data.Split("=",StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries);
            Id = tokens[0];
            tokens[1] = tokens[1].Replace("(", "").Replace(")", "");
            string[] directionTokens = tokens[1].Split(",",StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries);
            LeftId = directionTokens[0];
            RightId = directionTokens[1];

        }
        
    }
    
}