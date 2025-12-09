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
        List<LongVector3> points = new List<LongVector3>();
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(',');
            LongVector3 point = new LongVector3(long.Parse(tokens[0]), long.Parse(tokens[1]), long.Parse(tokens[2]));
            points.Add(point);
        }

        List<(LongVector3, LongVector3,double)> closestPairs = new List<(LongVector3, LongVector3,double)>();
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                LongVector3 point1 = points[i];
                LongVector3 point2 = points[j];
                double mag = (point1-point2).Magnitude;
                if (double.IsNaN(mag))
                {
                    int ibreak = 0;
                    double mag2 = (point1-point2).Magnitude2;
                }
                closestPairs.Add((point1,point2,mag));
            }
        }
        // sort all the closest pairs once for efficiency.
        closestPairs.Sort((a, b) => a.Item3.CompareTo(b.Item3));
        
        var result = BuildCircuits(points, closestPairs);

        long total = 1;
        int testSize = 3;
        
        for(int i=0; i<testSize; i++)        
        {
            total *= result[i].PointCount;
        }
        
        DebugOutput($"Have a total circuit size of {total} ");
    }

    public List<Circuit> BuildCircuits(List<LongVector3> points, List<(LongVector3, LongVector3, double)> pairs)
    {
        List<LongVector3> pointsCopy = new List<LongVector3>();
        pointsCopy.AddRange(points.OrderBy(x => x.Magnitude));
        List<Circuit> circuits = new List<Circuit>();
        foreach (LongVector3 point in pointsCopy)
        {
            Circuit circuit = new Circuit(point,this);
            circuits.Add(circuit);
        }

        int numIterations = IsTestInput?10:1000;
        for (int i = 0; i < numIterations; i++)
        {
            var pair = pairs[i];
            Circuit pointCircuit = null;
            foreach (Circuit circuit in circuits)
            {
                if(circuit.ContainsPoint(pair.Item1))
                {
                    pointCircuit = circuit;
                    break;
                }
            }

            Circuit pointCircuit2 = null;
            foreach (Circuit circuit in circuits)
            {
                if(circuit.ContainsPoint(pair.Item2))
                {
                    pointCircuit2 = circuit;
                    break;
                }
            }
            if (pointCircuit!= pointCircuit2)
            {
                pointCircuit.Merge(pointCircuit2);
            }
            
            circuits.RemoveAll(x => x.PointCount == 0);

        }
        circuits.Sort((x,y) => (x.PointCount.CompareTo(y.PointCount)*-1));
        
        return circuits;
    }

        
}

public class Circuit
{
    private BaseTest m_test;
    public Circuit(LongVector3 point,BaseTest test)
    {
        m_test = test;
        Points.Add(point);
    }

    public void Merge(Circuit circuit)
    {
        //m_test.DebugOutput($"Merging circuit {string.Join(' ', circuit.Points)} to {string.Join(' ', Points)} .");
        
        foreach (LongVector3 point in circuit.Points)
        {
            Points.Add(point);
        }
        circuit.Points.Clear();
    }
    
    public bool ContainsPoint(LongVector3 point)
    {
        return Points.Contains(point);        
    }
 
    public int PointCount { get { return Points.Count; } }
    private HashSet<LongVector3> Points = new HashSet<LongVector3>();

}