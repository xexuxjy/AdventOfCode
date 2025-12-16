using System;
using System.Collections.Generic;

public class Test9_2025 : BaseTest
{
    public override void Initialise()
    {
        Year = 2025;
        TestID = 9;
    }

    public override void Execute()
    {
        List<LongVector2> points = new List<LongVector2>();
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(',');
            LongVector2 point = new LongVector2(long.Parse(tokens[0]), long.Parse(tokens[1]));
            points.Add(point);
        }

        if (IsPart1)
        {
            ExecutePart1(points);
        }
        else
        {
            ExecutePart2(points);
        }
        
    }

    public void ExecutePart1(List<LongVector2> points)
    {
        List<(LongVector2, LongVector2)> pairs = new List<(LongVector2, LongVector2)>();
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                LongVector2 point1 = points[i];
                LongVector2 point2 = points[j];

                pairs.Add((point1, point2));
            }
        }

        double largestArea = double.MinValue;
        (LongVector2,LongVector2) closestPair =  (new LongVector2(0L,0L), new LongVector2(0L,0L));
        foreach ((LongVector2, LongVector2) pair in pairs)
        {
            if (pair.Item1 == new LongVector2(11, 1) && pair.Item2 == new LongVector2(2, 5))
            {
                int ibreak = 0;
            }
            double area = (Math.Abs(pair.Item1.X - pair.Item2.X)+1) * (Math.Abs(pair.Item1.Y - pair.Item2.Y)+1);
            if (area > largestArea)
            {
                largestArea = area;
                closestPair = pair;
            }
        }

        DebugOutput($"Largest area is {largestArea}");
        
    }

    public void ExecutePart2(List<LongVector2> points)
    {
        // build lines.
        DateTime startTime = DateTime.Now;

        DoubleVector2 rectTL = new DoubleVector2(3, 3);
        DoubleVector2 rectBR = new DoubleVector2(9, 9);
        
        DoubleVector2 line1  = new DoubleVector2(1, 3);
        DoubleVector2 line2  = new DoubleVector2(11, 3);
        
        
        bool intersects = LineIntersectsRect(line1,line2,rectTL, rectBR);

        line1 = new DoubleVector2(1, 1);
        line2 = new DoubleVector2(1, 11);
        
        bool intersects2 = LineIntersectsRect(line1,line2,rectTL, rectBR);
        
        List<(DoubleVector2, DoubleVector2)> lines = new List<(DoubleVector2, DoubleVector2)>();
        
        for (int i = 0; i < points.Count; i++)
        {
            DoubleVector2 point = new DoubleVector2(points[i].X,points[i].Y);
            DoubleVector2 nextPoint = i < points.Count-1?new DoubleVector2(points[i+1].X,points[i+1].Y):new DoubleVector2(points[0].X,points[0].Y);
            lines.Add((point, nextPoint));
        }
            
        
        List<(DoubleVector2, DoubleVector2)> pairs = new List<(DoubleVector2, DoubleVector2)>();
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                DoubleVector2 point1 = new DoubleVector2(points[i].X, points[i].Y);
                DoubleVector2 point2 = new DoubleVector2(points[j].X, points[j].Y);

                DoubleVector2 min = point1;
                min.Min(point2);
                
                DoubleVector2 max = point2;
                max.Max(point1);
                
                // shrink the rectangle by 1 in each dimension
                min += new DoubleVector2(1, 1);
                max -= new DoubleVector2(1, 1);
                
                //pairs.Add((point1, point2));
                pairs.Add((min, max));
            }
        }

        double largestArea = double.MinValue;
        (DoubleVector2,DoubleVector2) closestPair =  (new DoubleVector2(0,0), new DoubleVector2(0,0));
        
        
        foreach (var pair in pairs)
        {
            // check to see if the shrunken rectangle crosses any of the lines defined above. If not then it's a valid option
            bool valid = true;
            foreach (var line in lines)
            {
                if (LineIntersectsRect(line.Item1, line.Item2, pair.Item1, pair.Item2))
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                // regrow shrunken rectangle to calculate area.
                double area = (Math.Abs(pair.Item1.X - pair.Item2.X)+3) * (Math.Abs(pair.Item1.Y - pair.Item2.Y)+3);
                if (area > largestArea)
                {
                    largestArea = area;
                    closestPair = pair;
                }

            }
        }
        DebugOutput($"Largest area is {largestArea}");
    
        DateTime endTime = DateTime.Now;
        double bpElapsed = DateTime.Now.Subtract(startTime).TotalMilliseconds;
        


    }
    public static bool LineIntersectsRect(DoubleVector2 lineMinA, DoubleVector2 lineMaxA, DoubleVector2 rectangleMinA, DoubleVector2 rectangleMaxA)
    {
        DoubleVector2 lineMin = lineMinA;
        lineMin.Min(lineMaxA);
        
        DoubleVector2 lineMax = lineMaxA;
        lineMax.Max(lineMinA);

        DoubleVector2 rectangleMin = rectangleMinA;
        rectangleMin.Min(rectangleMaxA);
        
        DoubleVector2 rectangleMax = rectangleMaxA;
        rectangleMax.Max(rectangleMinA);
        
        
        
        // return LineIntersectsLine(lineMin, lineMax, new DoubleVector2(rectangleMin.X, rectangleMin.Y), new DoubleVector2(rectangleMax.X, rectangleMin.Y)) ||
        //        LineIntersectsLine(lineMin, lineMax, new DoubleVector2(rectangleMax.X, rectangleMin.Y), new DoubleVector2(rectangleMax.X, rectangleMax.Y)) ||
        //        LineIntersectsLine(lineMin, lineMax, new DoubleVector2(rectangleMax.X, rectangleMax.Y), new DoubleVector2(rectangleMin.X, rectangleMax.Y)) ||
        //        LineIntersectsLine(lineMin, lineMax, new DoubleVector2(rectangleMin.X, rectangleMax.Y), new DoubleVector2(rectangleMin.X, rectangleMin.Y)) ||
        //        Contains(rectangleMin,rectangleMax,lineMin) && Contains(rectangleMin,rectangleMax,lineMax);
        
        return LineIntersectsLine(lineMin, lineMax, new DoubleVector2(rectangleMin.X, rectangleMin.Y), new DoubleVector2(rectangleMax.X, rectangleMin.Y)) ||
               LineIntersectsLine(lineMin, lineMax, new DoubleVector2(rectangleMax.X, rectangleMin.Y), new DoubleVector2(rectangleMax.X, rectangleMax.Y)) ||
               LineIntersectsLine(lineMin, lineMax, new DoubleVector2(rectangleMax.X, rectangleMax.Y), new DoubleVector2(rectangleMin.X, rectangleMax.Y)) ||
               LineIntersectsLine(lineMin, lineMax, new DoubleVector2(rectangleMin.X, rectangleMax.Y), new DoubleVector2(rectangleMin.X, rectangleMin.Y));
        
    }

    public static bool Contains(DoubleVector2 min, DoubleVector2 max, DoubleVector2 point)
    {
        return point.X >= min.X && point.Y >= min.Y && point.X <= max.X && point.Y <= max.Y;
    }
    
    public static bool LineIntersectsLine(DoubleVector2 l1p1, DoubleVector2 l1p2, DoubleVector2 l2p1, DoubleVector2 l2p2)
    {
        double q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
        double d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);

        if( d == 0 )
        {
            return false;
        }

        double r = q / d;

        q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
        double s = q / d;

        if( r < 0 || r > 1 || s < 0 || s > 1 )
        {
            return false;
        }

        return true;
    }    
    
}