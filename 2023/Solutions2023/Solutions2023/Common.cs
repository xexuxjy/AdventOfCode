﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


public static class Helper
{
    public static void ReadInts(string input, List<int> store)
    {
        string[] tokens = input.Split(new char[] { ' ', ',' },
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        foreach (string token in tokens)
        {
            store.Add(int.Parse(token));
        }
    }

    public static void ReadLongs(string input, List<long> store)
    {
        string[] tokens = input.Split(new char[] { ' ', ',' },
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        foreach (string token in tokens)
        {
            store.Add(long.Parse(token));
        }
    }

    public static String DrawGrid(char[] data, int width, int height)
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                sb.Append(data[(y * width) + x]);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public static String DrawGrid(char[] data, long width, long height)
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                sb.Append(data[(y * width) + x]);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public static String DrawGrid(bool[] data, long width, long height)
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                sb.Append(data[(y * width) + x]?"1":"0");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public static int[] GetNumGrid(List<string> data, ref int width, ref int height)
    {
        width = data[0].Length;
        height = data.Count;

        int[] numGrid = new int[data.Count * data[0].Length];
        for (int y = 0; y < data.Count; ++y)
        {
            for (int x = 0; x < data[0].Length; ++x)
            {
                numGrid[(y * width) + x] = int.Parse("" + data[y][x]);
            }
        }

        return numGrid;
    }

    public static char[] GetCharGrid(List<string> data, ref int width, ref int height)
    {
        width = data[0].Length;
        height = data.Count;

        char[] numGrid = new char[data.Count * data[0].Length];
        for (int y = 0; y < data.Count; ++y)
        {
            for (int x = 0; x < data[0].Length; ++x)
            {
                numGrid[(y * width) + x] = data[y][x];
            }
        }

        return numGrid;
    }


    public static long GCD(long n1, long n2)
    {
        if (n2 == 0)
        {
            return n1;
        }
        else
        {
            return GCD(n2, n1 % n2);
        }
    }

    public static long LCM(List<long> list)
    {
        return list.Aggregate((S, val) => S * val / Helper.GCD(S, val));
    }

    // Helpful function as lazy :)
    //https://stackoverflow.com/questions/4243042/c-sharp-point-in-polygon
    public static bool IsPointInPolygon(List<IntVector2> polygon, IntVector2 testPoint)
    {
        bool result = false;
        int j = polygon.Count - 1;
        for (int i = 0; i < polygon.Count; i++)
        {
            if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y ||
                polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
            {
                if (polygon[i].X + (testPoint.Y - polygon[i].Y) /
                    (polygon[j].Y - polygon[i].Y) *
                    (polygon[j].X - polygon[i].X) < testPoint.X)
                {
                    result = !result;
                }
            }

            j = i;
        }

        return result;
    }

    // with help from : https://github.com/Acc3ssViolation/advent-of-code-2023/blob/main/Advent/Advent/Shared/MathExtra.cs

    public static long Shoelace(List<IntVector2> vertices)
    {
        var sum = 0L;
        for (var i = 0; i < vertices.Count; i++)
        {
            var a = vertices[i];
            var b = vertices[(i + 1) % vertices.Count];
            sum += ((long)b.X + a.X) * ((long)b.Y - a.Y);
        }

        return sum / 2;
    }

    public static long Shoelace(List<LongVector2> vertices)
    {
        var sum = 0L;
        for (var i = 0; i < vertices.Count; i++)
        {
            var a = vertices[i];
            var b = vertices[(i + 1) % vertices.Count];
            sum += ((long)b.X + a.X) * ((long)b.Y - a.Y);
        }

        return sum / 2;
    }

    public static double Shoelace(List<DoubleVector2> vertices)
    {
        double sum = 0;
        for (var i = 0; i < vertices.Count; i++)
        {
            var a = vertices[i];
            var b = vertices[(i + 1) % vertices.Count];
            sum += (b.X + a.X) * (b.Y - a.Y);
        }

        return sum / 2;
    }

    /// <summary>
    /// Calculates the area of a polygon using the shoelace algorithm.
    /// </summary>
    public static double Shoelace(List<Vector2> vertices)
    {
        var sum = 0.0;
        for (var i = 0; i < vertices.Count; i++)
        {
            var a = vertices[i];
            var b = vertices[(i + 1) % vertices.Count];
            sum += (b.X + a.X) * (b.X - a.X);
        }

        return sum / 2;
    }


    public static List<DoubleVector2> Expand(List<DoubleVector2> vertices, double distance)
    {
        var edgeNormals = new List<DoubleVector2>(vertices.Count);
        for (var i = 0; i < vertices.Count; i++)
        {
            var start = vertices[i];
            var end = vertices[(i + 1) % vertices.Count];
            var normal = end - start;
            normal /= normal.Magnitude;

            // rotate
            normal = new DoubleVector2(normal.Y, -normal.X);

            edgeNormals.Add(normal);
        }

        var offsetVertices = new List<DoubleVector2>(vertices.Count);
        for (var i = 0; i < vertices.Count; i++)
        {
            var prevEdgeIndex = (i + edgeNormals.Count - 1) % edgeNormals.Count;
            var nextEdgeIndex = (i) % edgeNormals.Count;

            var prevNormal = edgeNormals[prevEdgeIndex];
            var nextNormal = edgeNormals[nextEdgeIndex];
            var normal = (prevNormal + nextNormal);
            var offset = new DoubleVector2(Math.Sign(normal.X) * distance, Math.Sign(normal.Y) * distance);

            offsetVertices.Add(vertices[i] + offset);
        }

        return offsetVertices;
    }

    static Tuple<Complex, Complex> SolveQuadratic(double a, double b, double c)
    {
        if (a == 0) throw new ArgumentException("The coefficient of x squared can't be zero");
        double discriminant = b * b - 4.0 * a * c;
        Complex temp = Complex.Sqrt(discriminant);
        Complex root1 = (-b + temp) / (2.0 * a);
        Complex root2 = (-b - temp) / (2.0 * a);
        return Tuple.Create(root1, root2);
    }

    public static double GetQuadratic(double f, double val1, double val2, double val3)
    {
        var c = val1;
        var aPlusB = val2 - c;
        var fourAPlusTwoB = val3 - c;
        var twoA = fourAPlusTwoB - (2 * aPlusB);
        var a = twoA / 2;
        var b = aPlusB - a;

        return a * (f * f) + b * f + c;
    }
}

//https://stackoverflow.com/questions/29188686/finding-the-intersect-location-of-two-rays
public static class IntersectionHelper
{
    public struct Ray
    {
        public DoubleVector3 Position;
        public DoubleVector3 Direction;

        public Ray(DoubleVector3 p, DoubleVector3 d)
        {
            Position = p;
            Direction = d;
        }
    }

    public static Ray FindShortestDistance(Ray ray1, Ray ray2)
    {
        if (ray1.Position == ray2.Position) // same position - that is the point of intersection
        {
            return new Ray(ray1.Position, DoubleVector3.Zero);
        }

        var d3 = DoubleVector3.Cross(ray1.Direction, ray2.Direction);

        if (d3 != DoubleVector3.Zero) // lines askew (non - parallel)
        {
            //d3 is a cross product of ray1.Direction (d1) and ray2.Direction(d2)
            //    that means d3 is perpendicular to both d1 and d2 (since it's not zero - we checked that)    
            //
            //If we would look at our lines from the direction where they seem parallel 
            //    (such projection must always exist for lines that are askew)
            //    we would see something like this
            //
            //   p1   a*d1
            //   +----------->x------
            //                |
            //                | c*d3
            //       p2  b*d2 v 
            //       +------->x----
            //
            //p1 and p2 are positions ray1.Position and ray2.Position - x marks the points of intersection.
            //    a, b and c are factors we multiply the direction vectors with (d1, d2, d3)
            //
            //From the illustration we can the shortest distance equation
            //    p1 + a*d1 + c*d3 = p2 + b*d2
            //
            //If we rearrange it so we have a b and c on the left:
            //    a*d1 - b*d2 + c*d3 = p2 - p1
            //
            //And since all of the know variables (d1, d2, d3, p2 and p1) have 3 coordinates (x,y,z)
            //    now we have a set of 3 linear equations with 3 variables.
            //   
            //    a * d1.X - b * d2.X + c * d3.X = p2.X - p1.X
            //    a * d1.Y - b * d2.Y + c * d3.Y = p2.Y - p1.Y
            //    a * d1.Z - b * d2.Z + c * d3.Z = p2.Z - p1.Z
            //
            //If we use matrices, it would be
            //    [d1.X  -d2.X  d3.X ]   [ a ]   [p2.X - p1.X]
            //    [d1.Y  -d2.Y  d3.Y ] * [ a ] = [p2.Y - p1.Y]
            //    [d1.Z  -d2.Z  d3.Z ]   [ a ]   [p2.Z - p1.Z]
            //
            //Or in short notation
            //
            //   [d1.X  -d2.X  d3.X | p2.X - p1.X]
            //   [d1.Y  -d2.Y  d3.Y | p2.Y - p1.Y]
            //   [d1.Z  -d2.Z  d3.Z | p2.Z - p1.Z]
            //
            //After Gaussian elimination, the last column will contain values a b and c

            double[] matrix = new double[12];

            matrix[0] = ray1.Direction.X;
            matrix[1] = -ray2.Direction.X;
            matrix[2] = d3.X;
            matrix[3] = ray2.Position.X - ray1.Position.X;

            matrix[4] = ray1.Direction.Y;
            matrix[5] = -ray2.Direction.Y;
            matrix[6] = d3.Y;
            matrix[7] = ray2.Position.Y - ray1.Position.Y;

            matrix[8] = ray1.Direction.Z;
            matrix[9] = -ray2.Direction.Z;
            matrix[10] = d3.Z;
            matrix[11] = ray2.Position.Z - ray1.Position.Z;

            var result = Solve(matrix, 3, 4);

            double a = result[3];
            double b = result[7];
            double c = result[11];

            if (a >= 0 && b >= 0) // normal shortest distance (between positive parts of the ray)
            {
                DoubleVector3 position = ray1.Position + (ray1.Direction * a);
                DoubleVector3 direction = d3 * c;

                return new Ray(position, direction);
            }
            //else will fall through below:
            //    the shortest distance was between a negative part of a ray (or both rays)
            //    this means the shortest distance is between one of the ray positions and another ray 
            //    (or between the two positions)
        }

        //We're looking for the distance between a point and a ray, so we use dot products now
        //Projecting the difference between positions (dP) onto the direction vectors will
        //   give us the position of the shortest distance ray.
        //The magnitude of the shortest distance ray is the the difference between its 
        //    position and the other rays position

        ray1.Direction.Normalize(); //needed for dot product - it works with unit vectors
        ray2.Direction.Normalize();

        DoubleVector3 dP = ray2.Position - ray1.Position;

        //shortest distance ray position would be ray1.Position + a2 * ray1.Direction
        //                                     or ray2.Position + b2 * ray2.Direction (if b2 < a2)
        //                                     or just distance between points if both (a and b) < 0
        //if either a or b (but not both) are negative, then the shortest is with the other one
        double a2 = DoubleVector3.Dot(ray1.Direction, dP);
        double b2 = DoubleVector3.Dot(ray2.Direction, -dP);

        if (a2 < 0 && b2 < 0)
            return new Ray(ray1.Position, dP);


        DoubleVector3 p3a = ray1.Position + (ray1.Direction * a2);
        DoubleVector3 d3a = ray2.Position - p3a;

        DoubleVector3 p3b = ray1.Position;
        DoubleVector3 d3b = ray2.Position + (ray2.Direction * b2) - p3b;

        if (b2 < 0)
            return new Ray(p3a, d3a);

        if (a2 < 0)
            return new Ray(p3b, d3b);

        if (d3a.Magnitude <= d3b.Magnitude)
            return new Ray(p3a, d3a);

        return new Ray(p3b, d3b);
    }

//Solves a set of linear equations using Gaussian elimination
    public static double[] Solve(double[] matrix, int rows, int cols)
    {
        for (int i = 0; i < cols - 1; i++)
        for (int j = i; j < rows; j++)
            if (matrix[i + j * cols] != 0)
            {
                if (i != j)
                    for (int k = i; k < cols; k++)
                    {
                        double temp = matrix[k + j * cols];
                        matrix[k + j * cols] = matrix[k + i * cols];
                        matrix[k + i * cols] = temp;
                    }

                j = i;

                for (int v = 0; v < rows; v++)
                    if (v == j)
                        continue;
                    else
                    {
                        double factor = matrix[i + v * cols] / matrix[i + j * cols];
                        matrix[i + v * cols] = 0;

                        for (int u = i + 1; u < cols; u++)
                        {
                            matrix[u + v * cols] -= factor * matrix[u + j * cols];
                            matrix[u + j * cols] /= matrix[i + j * cols];
                        }

                        matrix[i + j * cols] = 1;
                    }

                break;
            }

        return matrix;
    }
}


public class FixedSizedQueue<T> : Queue<T>
{
    private readonly object syncObject = new object();

    public int Size { get; private set; }

    public FixedSizedQueue(int size)
    {
        Size = size;
    }

    public new void Enqueue(T obj)
    {
        base.Enqueue(obj);
        lock (syncObject)
        {
            while (base.Count > Size)
            {
                T outObj = base.Dequeue();
            }
        }
    }
}


public class LongBounds
{
    private LongVector2 m_min;
    private LongVector2 m_max;
    private LongVector2 m_center;
    private long m_distance;

    public LongBounds(LongVector2 center, long distance)
    {
        m_center = center;
        m_distance = distance;
        m_min = new LongVector2(center.X - distance, center.Y - distance);
        m_max = new LongVector2(center.X + distance, center.Y + distance);
    }

    public LongBounds(LongVector2 min, LongVector2 max)
    {
        m_min = min;
        m_max = max;
        m_center = (min + (min + max) / 2);
        m_distance = min.ManhattanDistance(max);
    }

    public bool Encapsulates(LongBounds bounds)
    {
        return m_min.X <= bounds.m_min.X &&
               m_min.Y <= bounds.m_min.Y &&
               m_max.X >= bounds.m_max.X &&
               m_max.Y >= bounds.m_max.Y;
    }

    public bool Intersects(LongBounds bounds)
    {
        return (m_min.X <= bounds.m_max.X) && (m_max.X >= bounds.m_min.X) &&
               (m_min.Y <= bounds.m_max.Y) && (m_max.Y >= bounds.m_min.Y);
    }

    public bool Contains(long x, long y)
    {
        if ((x >= m_min.X && x <= m_max.X) && (y >= m_min.Y && y <= m_max.Y))
        {
            return m_center.ManhattanDistance(new LongVector2(x, y)) <= m_distance;
        }

        return false;
    }

    public LongBounds Clip(int min, int max)
    {
        LongVector2 minclip = new LongVector2(Math.Max(m_min.X, min), Math.Max(m_min.Y, max));
        LongVector2 maxclip = new LongVector2(Math.Min(m_max.X, min), Math.Min(m_max.Y, max));

        return new LongBounds(minclip, maxclip);
    }
}

// thanks to : https://www.koderdojo.com/blog/breadth-first-search-and-shortest-path-in-csharp-and-net-core

public class Graph<T>
{
    public Graph()
    {
    }

    public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T, T>> edges)
    {
        foreach (var vertex in vertices)
            AddVertex(vertex);

        foreach (var edge in edges)
            AddEdge(edge);
    }

    public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>>();

    public void AddVertex(T vertex)
    {
        AdjacencyList[vertex] = new HashSet<T>();
    }

    public void AddEdge(Tuple<T, T> edge)
    {
        if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
        {
            AdjacencyList[edge.Item1].Add(edge.Item2);
            AdjacencyList[edge.Item2].Add(edge.Item1);
        }
    }
}

public static class Algorithms
{
    public static HashSet<T> BFS<T>(Graph<T> graph, T start)
    {
        var visited = new HashSet<T>();

        if (!graph.AdjacencyList.ContainsKey(start))
            return visited;

        var queue = new Queue<T>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var vertex = queue.Dequeue();

            if (visited.Contains(vertex))
                continue;

            visited.Add(vertex);

            foreach (var neighbor in graph.AdjacencyList[vertex])
                if (!visited.Contains(neighbor))
                    queue.Enqueue(neighbor);
        }

        return visited;
    }

    public static HashSet<T> DFS<T>(Graph<T> graph, T start)
    {
        var visited = new HashSet<T>();

        if (!graph.AdjacencyList.ContainsKey(start))
            return visited;

        var stack = new Stack<T>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            var vertex = stack.Pop();

            if (visited.Contains(vertex))
                continue;

            visited.Add(vertex);

            foreach (var neighbor in graph.AdjacencyList[vertex])
                if (!visited.Contains(neighbor))
                    stack.Push(neighbor);
        }

        return visited;
    }

    public static void CalculateLineLength(IntVector2 from, IntVector2 to, PlotFunction plotFunction)
    {
        Line(from.X, from.Y, to.X, to.Y, plotFunction);
    }


    private static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp;
        temp = lhs;
        lhs = rhs;
        rhs = temp;
    }

    //https://www.roguebasin.com/index.php/Bresenham's_Line_Algorithm

    /// <summary>
    /// The plot function delegate
    /// </summary>
    /// <param name="x">The x co-ord being plotted</param>
    /// <param name="y">The y co-ord being plotted</param>
    /// <returns>True to continue, false to stop the algorithm</returns>
    public delegate bool PlotFunction(int x, int y);

    /// <summary>
    /// Plot the line from (x0, y0) to (x1, y10
    /// </summary>
    /// <param name="x0">The start x</param>
    /// <param name="y0">The start y</param>
    /// <param name="x1">The end x</param>
    /// <param name="y1">The end y</param>
    /// <param name="plot">The plotting function (if this returns false, the algorithm stops early)</param>
    public static void Line(int x0, int y0, int x1, int y1, PlotFunction plot)
    {
        bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        if (steep)
        {
            Swap<int>(ref x0, ref y0);
            Swap<int>(ref x1, ref y1);
        }

        if (x0 > x1)
        {
            Swap<int>(ref x0, ref x1);
            Swap<int>(ref y0, ref y1);
        }

        int dX = (x1 - x0), dY = Math.Abs(y1 - y0), err = (dX / 2), ystep = (y0 < y1 ? 1 : -1), y = y0;

        for (int x = x0; x <= x1; ++x)
        {
            if (!(steep ? plot(y, x) : plot(x, y))) return;
            err = err - dY;
            if (err < 0)
            {
                y += ystep;
                err += dX;
            }
        }
    }
}

// help from : https://www.puresourcecode.com/dotnet/net-general/dijkstra-s-algorithm-in-c-with-generics/

namespace DijkstraAlgorithm
{
    public class Path<T>
    {
        public T Source { get; set; }

        public T Destination { get; set; }

        /// <summary>
        /// Cost of using this path from Source to Destination
        /// </summary>
        /// <remarks>
        /// Lower costs are preferable to higher costs
        /// </remarks>
        public int Cost { get; set; }
    }

    public static class ExtensionMethods
    {
        /// <summary>
        /// Adds or Updates the dictionary to include the destination and its associated cost 
        /// and complete path (and param arrays make paths easier to work with)
        /// </summary>
        public static void Set<T>(this Dictionary<T, KeyValuePair<int, LinkedList<Path<T>>>> Dictionary,
            T destination, int Cost, params Path<T>[] paths)
        {
            var CompletePath = paths == null ? new LinkedList<Path<T>>() : new LinkedList<Path<T>>(paths);
            Dictionary[destination] = new KeyValuePair<int, LinkedList<Path<T>>>(Cost, CompletePath);
        }
    }

    public static class Engine
    {
        public static LinkedList<Path<T>> CalculateShortestPathBetween<T>(T source, T destination,
            IEnumerable<Path<T>> Paths)
        {
            if (source.Equals(destination))
            {
                return null;
            }

            return CalculateFrom(source, Paths)[destination];
        }

        public static Dictionary<T, LinkedList<Path<T>>> CalculateShortestFrom<T>(T source, IEnumerable<Path<T>> Paths)
        {
            return CalculateFrom(source, Paths);
        }

        private static Dictionary<T, LinkedList<Path<T>>> CalculateFrom<T>(T source, IEnumerable<Path<T>> Paths)
        {
            // validate the paths
            if (Paths.Any(p => p.Source.Equals(p.Destination)))
                throw new ArgumentException("No path can have the same source and destination");

            // keep track of the shortest paths identified thus far
            Dictionary<T, KeyValuePair<int, LinkedList<Path<T>>>> ShortestPaths =
                new Dictionary<T, KeyValuePair<int, LinkedList<Path<T>>>>();

            // keep track of the locations which have been completely processed
            List<T> LocationsProcessed = new List<T>();

            // include all possible steps, with Int.MaxValue cost
            Paths.SelectMany(p => new T[] { p.Source, p.Destination }) // union source and destinations
                .Distinct() // remove duplicates
                .ToList() // ToList exposes ForEach
                .ForEach(s => ShortestPaths.Set(s, Int32.MaxValue, null)); // add to ShortestPaths with MaxValue cost

            // update cost for self-to-self as 0; no path
            ShortestPaths.Set(source, 0, null);

            // keep this cached
            var LocationCount = ShortestPaths.Keys.Count;

            while (LocationsProcessed.Count < LocationCount)
            {
                T _locationToProcess = default(T);

                //Search for the nearest location that isn't handled
                foreach (T _location in ShortestPaths.OrderBy(p => p.Value.Key).Select(p => p.Key).ToList())
                {
                    if (!LocationsProcessed.Contains(_location))
                    {
                        if (ShortestPaths[_location].Key == Int32.MaxValue)
                            return ShortestPaths.ToDictionary(k => k.Key,
                                v => v.Value.Value); //ShortestPaths[destination].Value;

                        _locationToProcess = _location;
                        break;
                    }
                } // foreach

                var _selectedPaths = Paths.Where(p => p.Source.Equals(_locationToProcess));

                foreach (Path<T> path in _selectedPaths)
                {
                    if (ShortestPaths[path.Destination].Key > path.Cost + ShortestPaths[path.Source].Key)
                    {
                        ShortestPaths.Set(
                            path.Destination,
                            path.Cost + ShortestPaths[path.Source].Key,
                            ShortestPaths[path.Source].Value.Union(new Path<T>[] { path }).ToArray());
                    }
                }

                //Add the location to the list of processed locations
                LocationsProcessed.Add(_locationToProcess);
            } // while

            return ShortestPaths.ToDictionary(k => k.Key, v => v.Value.Value);
            //return ShortestPaths[destination].Value;
        }
    }
}


public static class Combinations
{
    // Enumerate all possible m-size combinations of [0, 1, ..., n-1] array
    // in lexicographic order (first [0, 1, 2, ..., m-1]).
    private static IEnumerable<int[]> CombinationsRosettaWoRecursion(int m, int n)
    {
        int[] result = new int[m];
        Stack<int> stack = new Stack<int>(m);
        stack.Push(0);
        while (stack.Count > 0)
        {
            int index = stack.Count - 1;
            int value = stack.Pop();
            while (value < n)
            {
                result[index++] = value++;
                stack.Push(value);
                if (index != m) continue;
                yield return (int[])result.Clone(); // thanks to @xanatos
                //yield return result;
                break;
            }
        }
    }

    public static IEnumerable<T[]> CombinationsRosettaWoRecursion<T>(T[] array, int m)
    {
        if (array.Length < m)
            throw new ArgumentException("Array length can't be less than number of selected elements");
        if (m < 1)
            throw new ArgumentException("Number of selected elements can't be less than 1");
        T[] result = new T[m];
        foreach (int[] j in CombinationsRosettaWoRecursion(m, array.Length))
        {
            for (int i = 0; i < m; i++)
            {
                result[i] = array[j[i]];
            }

            yield return result;
        }
    }

    public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> sequence)
    {
        if (sequence == null)
        {
            yield break;
        }

        var list = sequence.ToList();

        if (!list.Any())
        {
            yield return Enumerable.Empty<T>();
        }
        else
        {
            var startingElementIndex = 0;

            foreach (var startingElement in list)
            {
                var index = startingElementIndex;
                var remainingItems = list.Where((e, i) => i != index);

                foreach (var permutationOfRemainder in remainingItems.Permute())
                {
                    yield return permutationOfRemainder.Prepend(startingElement);
                }

                startingElementIndex++;
            }
        }
    }
}
//https://github.com/iisfaq/CyrusBeck/blob/master/CyrusBeck.cs

//https://bluetoque.ca/2012/01/cohen-sutherland-line-clipping/
namespace CyrusBeckLineClipping
{
    public class CyrusBeck
    {
        public enum CyrusBeckResult
        {
            DoesNotIntersect,
            NotTrimmed,
            StartTrimmed,
            EndTrimmed,
            StartAndEndTrimmed
        }

        // Converted from c++ code at
        // https://www.geeksforgeeks.org/line-clipping-set-2-cyrus-beck-algorithm/
        public static bool LineClipping(List<DoubleVector3> vertices, DoubleVector3 startVector,
            DoubleVector3 endVector, out DoubleVector3 trimmedStartVector, out DoubleVector3 trimmedEndVector,
            out CyrusBeckResult results)
        {
            List<DoubleVector3> normals = new List<DoubleVector3>();

            // Calculating the normals 
            for (int i = 0; i < vertices.Count; i++)
            {
                normals.Add(new DoubleVector3(
                    vertices[i].Y - vertices[(i + 1) % vertices.Count].Y,
                    vertices[(i + 1) % vertices.Count].X - vertices[i].X,
                    0));
            }

            // Calculating P1 - P0 
            DoubleVector3 P1_P0 = endVector - startVector;

            // Initializing all values of P0 - PEi 
            List<DoubleVector3> P0_PEi = new List<DoubleVector3>();


            // Calculating the values of P0 - PEi for all edges 
            for (int i = 0; i < vertices.Count; i++)
            {
                // Calculating PEi - P0, so that the denominator won't have to multiply by -1 while calculating 't' 
                P0_PEi.Add(new DoubleVector3(vertices[i].X - startVector.X, vertices[i].Y - startVector.Y, 0));
            }

            List<double> numerator = new List<double>();
            List<double> denominator = new List<double>();

            // Calculating the numerator and denominators 
            // using the dot function 
            for (int i = 0; i < vertices.Count; i++)
            {
                numerator.Add(dotProduct(normals[i], P0_PEi[i]));
                denominator.Add(dotProduct(normals[i], P1_P0));
            }

            // Initializing the 't' values dynamically 
            List<double> t = new List<double>();

            // Making two vectors called 't entering' 
            // and 't leaving' to group the 't's 
            // according to their denominators 
            List<double> tEntering = new List<double>();
            List<double> tLeaving = new List<double>();

            // Calculating 't' and grouping them accordingly 
            for (int i = 0; i < vertices.Count; i++)
            {
                if (denominator[i] == 0)
                    t.Add(numerator[i]);
                else
                    t.Add(numerator[i] / denominator[i]);

                if (denominator[i] >= 0)
                    tEntering.Add(t[i]);
                else
                    tLeaving.Add(t[i]);
            }

            // Initializing the final two values of 't' 
            double tEnteringMax;
            double tLeavingMin;

            // Taking the max of all 'tE' and 0, so pushing 0 
            tEntering.Add(0);
            tEnteringMax = tEntering.Max();

            // Taking the min of all 'tL' and 1, so pushing 1 
            tLeaving.Add(1);
            tLeavingMin = tLeaving.Min();

            // Entering 't' value cannot be greater than exiting 't' value, hence, this is the case when the line 
            // is completely outside 
            if (tEnteringMax > tLeavingMin)
            {
                trimmedStartVector = new DoubleVector3();
                trimmedEndVector = new DoubleVector3();
                results = CyrusBeckResult.DoesNotIntersect;
                return false;
            }

            // Calculating the coordinates in terms of the trimmed x and y 
            trimmedStartVector = new DoubleVector3((float)(startVector.X + P1_P0.X * tEnteringMax),
                (float)(startVector.Y + P1_P0.Y * tEnteringMax), 0);
            trimmedEndVector = new DoubleVector3((float)(startVector.X + P1_P0.X * tLeavingMin),
                (float)(startVector.Y + P1_P0.Y * tLeavingMin), 0);

            bool StartTrimmed = IsSame(startVector, trimmedStartVector) ? false : true;
            bool EndTrimmed = IsSame(endVector, trimmedEndVector) ? false : true;

            if (StartTrimmed && EndTrimmed)
                results = CyrusBeckResult.StartAndEndTrimmed;
            else if (StartTrimmed)
                results = CyrusBeckResult.StartTrimmed;
            else if (EndTrimmed)
                results = CyrusBeckResult.EndTrimmed;
            else
                results = CyrusBeckResult.NotTrimmed;
            return true;
        }

        private static bool IsSame(DoubleVector3 p1, DoubleVector3 p2)
        {
            if (IsSame(p1.X, p2.X) == false)
                return false;
            if (IsSame(p1.Y, p2.Y) == false)
                return false;
            if (IsSame(p1.Z, p2.Z) == false)
                return false;
            return true;
        }

        private static bool IsSame(double a, double b)
        {
            // Compare to 5 decimal points
            return Math.Abs(a - b) < 0.00001;
        }


        private static double dotProduct(DoubleVector3 v1, DoubleVector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        private static DoubleVector3 crossProduct(DoubleVector3 v1, DoubleVector3 v2)
        {
            return new DoubleVector3(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
        }
    }
}

public class BinaryNode<T>
{
    public string? Name { get; set; }
    public T Data { get; set; }
    public BinaryNode<T>? Parent { get; set; }
    public BinaryNode<T>? Left { get; set; }
    public BinaryNode<T>? Right { get; set; }

    public override string ToString() => Name;
}