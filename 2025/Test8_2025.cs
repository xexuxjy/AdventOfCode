using System;
using System.Collections.Generic;

public class Test8_2025 : BaseTest
{
    public override void Initialise()
    {
        Year = 2025;
        TestID = 8;
    }

    public override void Execute()
    {
        List<IntVector3> points = new List<IntVector3>();
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(',');
            IntVector3 point = new IntVector3(int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]));
            points.Add(point);
        }

        var result = BuildCircuits(points);

        long total = 1;
        int testSize = 3;
        
        for(int i=0; i<testSize; i++)        
        {
            total *= result[i].PointCount;
        }
        
        DebugOutput($"Have a total circuit size of {total} ");
    }

    public List<Circuit> BuildCircuits(List<IntVector3> points)
    {
        List<IntVector3> pointsCopy = new List<IntVector3>();
        pointsCopy.AddRange(points.OrderBy(x=>x.Magnitude));
        
        double minDistance = double.MaxValue;
        double lastDistance = double.MaxValue;

        List<Circuit> circuits = new List<Circuit>();

        int unallocatedPoints = pointsCopy.Count;

        int numIterations = 12;
        for(int i=0;i<numIterations;i++)
        {
            (IntVector3,IntVector3) bestResult = (new IntVector3(),new IntVector3()); 
            
            foreach (IntVector3 point in pointsCopy)
            {
                foreach (IntVector3 point2 in pointsCopy)
                {
                    if (point != point2)
                    {
                        double distance = (point-point2).Magnitude;
                        if (distance < minDistance)
                        {
                            bool alreadyLinked = false;
                            foreach (Circuit circ in circuits)
                            {
                                if(circ.ContainsConnectedPoints(point,point2))
                                {
                                    alreadyLinked = true;
                                    break;
                                }
                            }

                            if (!alreadyLinked)
                            {
                                minDistance = distance;
                                bestResult = (point, point2);
                            }
                        }
                    }
                }
            }
            
            // find circuit for point. if it doesn't exist create a new one.
            Circuit targetCircuit = null;
            foreach (Circuit circuit in circuits)
            {
                if (circuit.ContainsPoint(bestResult.Item1) || circuit.ContainsPoint(bestResult.Item2))
                {
                    targetCircuit = circuit;
                    break;
                }
            }

            if (targetCircuit == null)
            {
                targetCircuit = new Circuit();
                circuits.Add(targetCircuit);
            }
            
            targetCircuit.ConnectPoints(bestResult.Item1, bestResult.Item2);
            minDistance = double.MaxValue;

            int allocatedPoints = 0;
            foreach (Circuit circuit in circuits)
            {
                allocatedPoints += circuit.PointCount;
            }
            unallocatedPoints = pointsCopy.Count-allocatedPoints;;
        }

        circuits.Sort((x,y) => (x.PointCount.CompareTo(y.PointCount)*-1));
        
        return circuits;
    }
        
}

public class Circuit
{
    private String Id;

    public void ConnectPoints(IntVector3 point, IntVector3 point2)
    {
        if (point < point2)
        {
            Points.Add(point);
            Points.Add(point2);
            Junctions.Add((point, point2));
            Distances.Add(point.ManhattanDistance(point2));
        }
        else
        {
            Points.Add(point);
            Points.Add(point2);
            Junctions.Add((point2, point));
            Distances.Add(point.ManhattanDistance(point2));
        }
    }

    public bool ContainsConnectedPoints(IntVector3 point, IntVector3 point2)
    {
        return Junctions.Contains((point, point2)) || Junctions.Contains((point2, point));
    }

    public bool ContainsPoint(IntVector3 point)
    {
        return Points.Contains(point);        
    }
 
    public int PointCount { get { return Points.Count; } }
    
    private HashSet<IntVector3> Points = new HashSet<IntVector3>();
    private List<(IntVector3,IntVector3)> Junctions = new List<(IntVector3,IntVector3)>();
    private List<int> Distances =  new List<int>();
}