using System.Diagnostics;
using System.Drawing;
using System.Numerics;

public class Test18 : BaseTest
{
    public override void Initialise()
    {
        TestID = 18;
        IsTestInput = false;
        IsPart2 = true;
    }

    public override void Execute()
    {
        List<Tuple<DoubleVector2, long, string>> dataList = new List<Tuple<DoubleVector2, long, string>>();
        foreach (string data in m_dataFileContents)
        {
            string[] tokens = data.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            string dirString = tokens[0];
            long distance = long.Parse(tokens[1]);

            if (IsPart2)
            {
                dirString = "" + tokens[2].Substring(tokens[2].Length - 2, 1);
                string hexVal = tokens[2].Substring(2, 5);
                distance = Convert.ToInt64(hexVal, 16);
            }

            DoubleVector2 direction = new DoubleVector2();
            if (dirString == "U" || dirString == "3") direction = DoubleVector2.Down;
            else if (dirString == "D" || dirString == "1") direction = DoubleVector2.Up;
            else if (dirString == "L" || dirString == "2") direction = DoubleVector2.Left;
            else if (dirString == "R" || dirString == "0") direction = DoubleVector2.Right;

            dataList.Add(new(direction, distance, tokens[2]));
        }


        DoubleVector2 currentPos = new DoubleVector2();
        List<DoubleVector2> points = new List<DoubleVector2>();

        foreach (var data in dataList)
        {
            currentPos += (data.Item1 * data.Item2);
            points.Add(currentPos);
        }

        List<DoubleVector2> expanded = Helper.Expand(points, 0.5);
        long total = (long)Helper.Shoelace(expanded);
        DebugOutput("Total area is : " + Math.Abs(total));

    }

}
