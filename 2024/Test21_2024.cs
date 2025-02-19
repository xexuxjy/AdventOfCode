

using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text;
using static System.Formats.Asn1.AsnWriter;
using static Test8_2023;

public class Test21_2024 : BaseTest
{

    public char[] NumericPad = "789456123 0A".ToCharArray();
    public int NumericPadWidth = 3;
    public int NumericPadHeight = 4;



    public char[] ControlPad = " ^A<v>".ToCharArray();
    public int ControlPadWidth = 3;
    public int ControlPadHeight = 2;


    public IntVector2 NumericPadRobotArmPosition;
    public IntVector2 ControlPad1RobotArmPosition;
    public IntVector2 ControlPad2RobotArmPosition;

    public override void Initialise()
    {
        Year = 2024;
        TestID = 21;
    }


    public List<char> Array1 = new List<char>();
    public List<char> Array2 = new List<char>();


    public override void Execute()
    {
        int[] arr1 = new int[]{1,2,3 };
        int[] arr2 = new int[]{4,5,6 };

        int[][] arrs = new int[][] {arr1,arr2 };

            //    return options2
            //.CartesianProduct()
            //.Select(arr => string.Join("", arr))
            //.ToArray();


        var data = arrs.CartesianProduct().Select(x=>x.ToList()).ToList();

        



        NumericPadRobotArmPosition = Helper.GetPosition(Array.FindIndex<char>(NumericPad, x => x == 'A'), NumericPadWidth);
        ControlPad1RobotArmPosition = Helper.GetPosition(Array.FindIndex<char>(ControlPad, x => x == 'A'), ControlPadWidth);
        ControlPad2RobotArmPosition = ControlPad1RobotArmPosition;

        int numIterations = IsPart2 ? 25 : 2;

        SetupMaps();

        List<char> sourceList = Array1;
        List<char> targetList = Array2;

        List<char> tempList = null;
        
        long totalScore2 = 0;
        foreach (string desiredCode in m_dataFileContents)
        {
            long num = long.Parse(desiredCode.Replace("A", ""));

            sourceList.Clear();
            sourceList.AddRange(desiredCode.ToCharArray());

            string[] options  = GetMovesOnNumericKeypad( desiredCode);


            long bestCost = long.MaxValue;
            foreach(string option in options)
            {
                List<char> charList = new List<char>();
                charList.Add('A');
                charList.AddRange(option.ToCharArray());

                long cost = 0;
                for (int i = 0; i < charList.Count - 1; ++i)
                {
                    cost += Cost(charList[i], charList[i + 1], numIterations - 1, null);
                }
                bestCost= Math.Min(cost, bestCost);
            }

            DebugOutput($"score2 calc is : code {desiredCode}  is {bestCost} * {num} = {bestCost * num}");
            totalScore2 += (bestCost * num);
        }

        DebugOutput($"The final score is : {totalScore2} ");


    }

    public string[] GetMovesOnNumericKeypad(string s)
    {
        List<string[]> options2 = [];

        for (var i = -1; i < s.Length - 1; i++)
        {
            var x = i > -1 ? s[i] : 'A';
            List<List<char>> values = NumberPadOptions[(x, s[i + 1])];

            List<string> stringList = new List<string>();
            foreach(var t in values)
            {
                string ts = new string(t.ToArray());
                ts+="A";
                stringList.Add(ts);
            }

            options2.Add(stringList.ToArray());
        }

        return options2
            .CartesianProduct()
            .Select(arr => string.Join("", arr))
            .ToArray();
            
    }



    Dictionary<(char,char),List<List<char>>> NumberPadOptions = new Dictionary<(char, char), List<List<char>>>();
    Dictionary<(char,char),List<List<char>>> ControlPadOptions = new Dictionary<(char, char), List<List<char>>>();

    public void SetupMaps()
    {
        string keys = "0123456789A";
        for (int i = 0; i < keys.Length; i++)
        {
            IntVector2 a = Helper.GetPosition(Array.FindIndex<char>(NumericPad, x => x == keys[i]), NumericPadWidth);
            char from = keys[i];

            for (int j = 0; j < keys.Length; j++)
            {
                char to = keys[j];

                GetNumberPadMoves(from, to,NumberPadOptions);
            }
        }

        keys = "^A<v>";
        for (int i = 0; i < keys.Length; i++)
        {
            IntVector2 a = Helper.GetPosition(Array.FindIndex<char>(ControlPad, x => x == keys[i]), ControlPadWidth);
            char from = keys[i];

            for (int j = 0; j < keys.Length; j++)
            {
                char to = keys[j];

                GetControlPadMoves(from, to,ControlPadOptions);
            }
        }

    }


    public bool BuildAllPaths(IntVector2 current, IntVector2 target, int depth, char[] dataGrid, int width, int height, List<IntVector2> currentPath, List<List<IntVector2>> validPaths)
    {
        if (depth == 0)
        {
            return false;
        }

        currentPath.Add(current);

        if (current == target)
        {
            if (validPaths.Count > 0 && currentPath.Count < validPaths.Max(x => x.Count))
            {
                validPaths.Clear();
            }
            if (validPaths.Count == 0 || currentPath.Count == validPaths.Max(x => x.Count))
            {
                validPaths.Add(currentPath);
            }
            return true;
        }


        bool pathExists = false;
        foreach (IntVector2 direction in IntVector2.Directions)
        {
            IntVector2 updated = current + direction;
            if (Helper.InBounds(updated, width, height) && !currentPath.Contains(updated))
            {
                int index = Helper.GetIndex(updated, width);
                if (dataGrid[index] != ' ')
                {
                    List<IntVector2> pathCopy = new List<IntVector2>();
                    pathCopy.AddRange(currentPath);

                    pathExists |= BuildAllPaths(updated, target, depth - 1, dataGrid, width, height, pathCopy, validPaths);
                }
            }
        }
        return pathExists;
    }


    public Dictionary<(char, char, int), long> CostDictionary = new Dictionary<(char, char, int), long>();
    public long Cost(char from, char to, int level, Dictionary<int, List<char>>? debug = null)
    {
        var key = (from, to, level);
        long totalCost = 0;

        if (CostDictionary.TryGetValue(key, out totalCost))
        {
            return totalCost;
        }
        else
        {
            // find all paths from to.
            ControlPadOptions.TryGetValue((from,to),out List<List<char>> paths);

            long bestVal = long.MaxValue;
            foreach (List<char> path in paths)
            {
                long cost = 0;

                if (level == 0)
                {
                    //cost = ControlPadMoveMap[(from, to)].LongLength + 1;
                    cost = path.Count+1;
                }
                else
                {
                    List<char> temp = new List<char>();
                    temp.Add('A');
                    temp.AddRange(path);
                    temp.Add('A');
                    for (int i = 0; i < temp.Count - 1; i++)
                    {
                        cost += Cost(temp[i], temp[i + 1], level - 1, debug);
                    }
                }
                bestVal = Math.Min(bestVal, cost);
            }

            totalCost = bestVal;


        }

        CostDictionary[key] = totalCost;
        return totalCost;
    }

    public void GetControlPadMoves(char from, char to,Dictionary<(char, char), List<List<char>>> map)
    {
        List<List<IntVector2>> allPaths = new List<List<IntVector2>>();
        IntVector2 a = Helper.GetPosition(Array.FindIndex<char>(ControlPad, x => x == from), ControlPadWidth);
        IntVector2 b = Helper.GetPosition(Array.FindIndex<char>(ControlPad, x => x == to), ControlPadWidth);


        BuildAllPaths(a, b, 10, ControlPad, ControlPadWidth, ControlPadHeight, new List<IntVector2>(), allPaths);

        List<List<char>> resultList = new List<List<char>>();

        foreach (List<IntVector2> path in allPaths)
        {
            List<char> list = new List<char>();
            resultList.Add(list);
            for (int k = 1; k < path.Count; k++)
            {
                IntVector2 diff = path[k] - path[k - 1];
                list.Add(Helper.PointerFromDirection(diff));
            }
        }
        map[(from,to)] = resultList;
    }

    public void GetNumberPadMoves(char from, char to,Dictionary<(char, char), List<List<char>>> map)
    {
        if(from == 'A' && to == '8')
        {
            int ibreak =0;
        }

        List<List<IntVector2>> allPaths = new List<List<IntVector2>>();
        IntVector2 a = Helper.GetPosition(Array.FindIndex<char>(NumericPad, x => x == from), NumericPadWidth);
        IntVector2 b = Helper.GetPosition(Array.FindIndex<char>(NumericPad, x => x == to), NumericPadWidth);

        BuildAllPaths(a, b, 10, NumericPad, NumericPadWidth, NumericPadHeight, new List<IntVector2>(), allPaths);

        List<List<char>> resultList = new List<List<char>>();

        foreach (List<IntVector2> path in allPaths)
        {
            List<char> list = new List<char>();
            resultList.Add(list);
            for (int k = 1; k < path.Count; k++)
            {
                IntVector2 diff = path[k] - path[k - 1];
                list.Add(Helper.PointerFromDirection(diff));
            }
        }
        map[(from,to)] = resultList;
    }




    private void SwapList(ref List<char> list1, ref List<char> list2)
    {
        List<char> t = list1;
        list1 = list2;
        list2 = t;
    }
}



