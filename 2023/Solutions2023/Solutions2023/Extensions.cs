using System.Numerics;

public static class Extensions
{

    public static float ManhattanDistance(this Vector2 v,Vector2 v2)
    {
        float distanceX = Math.Abs(v.X - v2.X);
        float distanceY = Math.Abs(v.Y - v2.Y);

        return distanceX + distanceY;
    }

    
    static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>()};
        return sequences.Aggregate(
            emptyProduct,
            (accumulator, sequence) => 
                from accseq in accumulator 
                from item in sequence 
                select accseq.Concat(new[] {item})                          
        );
    }

}