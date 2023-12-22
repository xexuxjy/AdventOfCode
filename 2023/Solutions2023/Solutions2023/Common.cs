using System;
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

    
    public static int[] GetNumGrid(List<string> data, ref int width, ref int height)
    {
        width = data[0].Length;
        height = data.Count;
        
        int [] numGrid = new int[data.Count * data[0].Length];
        for (int y = 0; y < data.Count; ++y)
        {
            for (int x = 0; x < data[0].Length; ++x)
            {
                numGrid[(y * width) + x] = int.Parse(""+data[y][x]);
            }
        }

        return numGrid;
    }

    public static char[] GetCharGrid(List<string> data, ref int width, ref int height)
    {
        width = data[0].Length;
        height = data.Count;
        
        char [] numGrid = new char[data.Count * data[0].Length];
        for (int y = 0; y < data.Count; ++y)
        {
            for (int x = 0; x < data[0].Length; ++x)
            {
                numGrid[(y * width) + x] =data[y][x];
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
    public static double Shoelace( List<Vector2> vertices)
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

            offsetVertices.Add(vertices[i]+offset);
        }
    
        return offsetVertices;
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

    public static void CalculateLineLength(IntVector2 from, IntVector2 to,PlotFunction plotFunction)
    {
        Line(from.X,from.Y,to.X,to.Y,plotFunction);
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