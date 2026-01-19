using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Security.AccessControl;

public class Test11_2017 : BaseTest
{
    public HexNode m_centralNode;

    public override void Initialise()
    {
        Year = 2017;
        TestID = 11;
    }

    public override void Execute()
    {
        m_centralNode = new HexNode();
        HexNode.AllNodes[m_centralNode.Position] = m_centralNode;
        
        HexNode currentNode = m_centralNode;
        int maxDistance = 0;
        
        if (IsTestInput)
        {
            // currentNode = currentNode.Move("NE");
            // currentNode = currentNode.Move( "NE");
            // currentNode = currentNode.Move( "NE");

            // currentNode = currentNode.Move("NE");
            // currentNode = currentNode.Move("NE");
            // currentNode = currentNode.Move("SW");
            // currentNode = currentNode.Move("SW");
            
            // currentNode = currentNode.Move("NE");
            // currentNode = currentNode.Move("NE");
            // currentNode = currentNode.Move("S");
            // currentNode = currentNode.Move("S");

            
            currentNode = currentNode.Move("N");
            currentNode = currentNode.Move("N");
            currentNode = currentNode.Move("S");

            
            
            // currentNode = currentNode.Move("SE");
            // currentNode = currentNode.Move( "SW");
            // currentNode = currentNode.Move( "SE");
            // currentNode = currentNode.Move( "SW");
            // currentNode = currentNode.Move( "SW");
        }
        else
        {
            string[] moves = m_dataFileContents[0].Split(',');
            foreach (string move in moves)
            {
                currentNode = currentNode.Move(move.ToUpper());
                maxDistance = Math.Max(maxDistance,CalcDistance(currentNode, m_centralNode));
            }
        }

        DebugOutput($"Distance at node {currentNode.Position} is {CalcDistance(m_centralNode,currentNode)}");
        if (IsPart2)
        {
            DebugOutput($"Maximum distance reached is {maxDistance}");
        }
    }

    public int CalcDistance(HexNode node1,HexNode node2)
    {
        int x = Math.Abs(node2.Position.X - node1.Position.X);
        int y = Math.Abs(node2.Position.Y - node1.Position.Y);
        int z = Math.Abs(node2.Position.Z - node1.Position.Z);

        int distance = Math.Max(x, Math.Max(y, z));
        
        return distance;
    }

}


public class HexNode
{
    public IntVector3 Position;
    public static Dictionary<IntVector3,HexNode> AllNodes = new Dictionary<IntVector3,HexNode>();
    public HexNode Move(string direction)
    {
        HexNode result = null;
        IntVector3 newPosition = new IntVector3();
        
        switch (direction)
        {
            
            case "NW":
                newPosition = Position + new IntVector3(0,-1,1);
                break;
            case "SE":
                newPosition = Position + new IntVector3(0,1,-1);
                break;
            case "NE":
                newPosition = Position + new IntVector3(1,0,-1);
                break;
            case "SW":
                newPosition = Position + new IntVector3(-1,0,1);
                break;
            case "N":
                newPosition = Position + new IntVector3(1,-1,0);
                break;
            case "S":
                newPosition = Position + new IntVector3(-1,1,0);
                break;
            default:
                int ibreak = 0;
                break;
        }

        Debug.Assert((newPosition.X + newPosition.Y + newPosition.Z) == 0);
        
        if (!AllNodes.TryGetValue(newPosition, out result))
        {
            result = new HexNode();
            result.Position = newPosition;
            AllNodes.Add(result.Position, result);
        }
        
        return result;
    }
    
}