using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test8_2023 : BaseTest
{
    public override void Initialise()
    {
        Year = 2023;
        TestID = 8;
        IsTestInput = false;
        IsPart2 = true;
    }

    public override void Execute()
    {
        string instructions = m_dataFileContents[0];


        Dictionary<String, Node> nodeDictionary = new Dictionary<string, Node>();
        for (int i = 2; i < m_dataFileContents.Count; ++i)
        {
            Node node = new Node(m_dataFileContents[i]);
            nodeDictionary[node.Id] = node;
        }

        if (IsPart2)
        {
            ExecutePart2(instructions, nodeDictionary);
        }
        else
        {
            ExecutePart1(instructions, nodeDictionary);
        }

    }

    public void ExecutePart1(string instructions, Dictionary<string, Node> nodeDictionary)
    {
        DebugOutput("ExecutePart1");
        string START = "AAA";
        string GOAL = "ZZZ";
        Node currentNode = nodeDictionary[START];

        int instructionCount;
        int totalCount;

        FollowPaths(instructions, nodeDictionary, currentNode, IsPart2, out totalCount, out instructionCount);

        DebugOutput("Reached destination in : " + totalCount + "  instruction count was : " + instructionCount);
    }

    public Node FollowPaths(string instructions, Dictionary<string, Node> nodeDictionary, Node startNode, bool part2, out int totalCount,
        out int instructionCount)
    {
        Node currentNode = startNode;

        //DebugOutput("Start node is "+startNode.DebugInfo());

        instructionCount = 0;
        totalCount = 0;

        int instructionLoops = 0;

        while (part2 ? (!currentNode.EndsZ) : (currentNode != nodeDictionary["ZZZ"]))
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
                instructionLoops++;
            }

            // stop loop forever...
            if (totalCount > 1000000000)
            {
                DebugOutput(("Probably in a loop. breaking"));
                break;
            }
        }

        //DebugOutput("End node is " + currentNode.DebugInfo());


        return currentNode;
    }


    public void ExecutePart2(string instructions, Dictionary<string, Node> nodeDictionary)
    {
        DebugOutput("ExecutePart2");
        List<Node> currentStates = new List<Node>();
        foreach (Node n in nodeDictionary.Values)
        {
            if (n.EndsA)
            {
                currentStates.Add(n);
            }
        }

        List<long> stepCount = new List<long>();
        foreach (Node n in currentStates)
        {
            int totalCount;
            int instructionCount;
            //DebugOutput("Testing : "+n.Id);
            FollowPaths(instructions, nodeDictionary, n, IsPart2, out totalCount, out instructionCount);
            //DebugOutput("Reached destination in : " + totalCount + " instruction count was : "+instructionCount);
            stepCount.Add(totalCount);
        }


        long multiple = Helper.LCM(stepCount);

        DebugOutput("All paths are aligned with instruction count so we can just find each one individually and then find the GCD ");
        DebugOutput("Which in this case is : " + multiple);

        int ibreak = 0;
    }


    public class Node
    {
        public string Id;
        public string LeftId;
        public string RightId;


        public bool EndsA => Id[2] == 'A';
        public bool EndsZ => Id[2] == 'Z';

        public bool LeftCyclic => (Id == LeftId);
        public bool RightCyclic => (Id == RightId);

        public bool TotalCyclic => LeftCyclic && RightCyclic;

        public Node(string data)
        {
            string[] tokens = data.Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            Id = tokens[0];
            tokens[1] = tokens[1].Replace("(", "").Replace(")", "");
            string[] directionTokens =
                tokens[1].Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            LeftId = directionTokens[0];
            RightId = directionTokens[1];
        }

        public string DebugInfo()
        {
            return ($"Id : {Id}  Left = {LeftId}  Right = {RightId}");
        }


    }
}