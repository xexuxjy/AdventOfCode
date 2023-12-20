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

    
    public static int ToHexInt(this string str)
    {
        bool negative = false;
        var num = 0;
        var index = 0;
        if (str[index] == '-')
        {
            negative = true;
            index++;
        }
        while (index < str.Length)
        {
            var chr = str[index];
            if (char.IsAsciiDigit(chr))
            {
                num *= 16;
                num += chr - '0';
            }
            else if (char.IsBetween(chr, 'a', 'f'))
            {
                num *= 16;
                num += chr - 'a' + 10;
            }
            else
            {
                break;
            }

            index++;
        }
        return negative ? -num : num;
    }

}