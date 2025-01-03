

using System.Numerics;
using System.Text;
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

        NumericPadRobotArmPosition = Helper.GetPosition(Array.FindIndex<char>(NumericPad, x => x == 'A'), NumericPadWidth);
        ControlPad1RobotArmPosition = Helper.GetPosition(Array.FindIndex<char>(ControlPad, x => x == 'A'), ControlPadWidth);
        ControlPad2RobotArmPosition = ControlPad1RobotArmPosition;

        int numIterations = IsPart2 ? 25 : 5;

        SetupMaps(numIterations);


        List<char> sourceList = Array1;
        List<char> targetList = Array2;

        List<char> tempList = null;
        long totalScore = 0;
        foreach (string desiredCode in m_dataFileContents)
        {
            sourceList.Clear();
            sourceList.AddRange(desiredCode.ToCharArray());


            GetMapCommandsForCode(sourceList, targetList, ref NumericPadRobotArmPosition, NumericPad, NumericPadWidth, NumericPadMoveMap, false);
            SwapList(ref sourceList, ref targetList);

            DebugOutput($"First level list is is {string.Join(',', sourceList)}");


            for (int i = 0; i < numIterations; i++)
            {
                GetMapCommandsForCode(sourceList, targetList, ref ControlPad1RobotArmPosition, ControlPad, ControlPadWidth, ControlPadMoveMap, false);
                SwapList(ref sourceList, ref targetList);
                DebugOutput($"Iteration  {i}  is {string.Join(',', sourceList)}");

            }

            // and back 
            SwapList(ref sourceList, ref targetList);

            long num = long.Parse(desiredCode.Replace("A", ""));
            long score = targetList.Count * num;


            DebugOutput($"code {desiredCode}  is {targetList.Count} * {num} = {score}");
            DebugOutput($"{desiredCode}: {string.Join(',', targetList)}");


            totalScore += score;


            sourceList.Clear();
            sourceList.AddRange(desiredCode.ToCharArray());

            GetMapCommandsForCode(sourceList, targetList, ref NumericPadRobotArmPosition, NumericPad, NumericPadWidth, NumericPadMoveMap);
            SwapList(ref sourceList, ref targetList);

            Dictionary<int,List<char>> debug = new Dictionary<int,List<char>>();
            long score2 = 0;
            sourceList.Insert(0,'A');

            for (int i = 0; i < sourceList.Count - 1; ++i)
            {
                score2 += Cost(sourceList[i], sourceList[i + 1], numIterations-1, debug);
            }
            
            foreach(int key in debug.Keys)
            {
                DebugOutput($"code2 {desiredCode}  level {key} is  :   {string.Join(',', debug[key])}");
            }
        }

        DebugOutput("The final score is : " + totalScore);


    }




    public Dictionary<(char, char), char[]> NumericPadMoveMap = new Dictionary<(char, char), char[]>();
    public Dictionary<(char, char), char[]> ControlPadMoveMap = new Dictionary<(char, char), char[]>();

    public Dictionary<(char, int), long> ValueCache = new Dictionary<(char, int), long>();



    public void SetupMaps(int numIterations)
    {

        ControlPadMoveMap[('A', 'A')] = new char[] { };
        ControlPadMoveMap[('A', '^')] = new char[] { '<' };
        ControlPadMoveMap[('A', '>')] = new char[] { 'v' };
        ControlPadMoveMap[('A', 'v')] = new char[] { 'v', '<' };
        ControlPadMoveMap[('A', '<')] = new char[] { 'v', '<', '<' };

        ControlPadMoveMap[('>', '>')] = new char[] { };
        ControlPadMoveMap[('>', 'A')] = new char[] { '^' };
        ControlPadMoveMap[('>', 'v')] = new char[] { '<' };
        ControlPadMoveMap[('>', '^')] = new char[] { '^', '<' };
        ControlPadMoveMap[('>', '<')] = new char[] { '<', '<' };

        ControlPadMoveMap[('^', '^')] = new char[] { };
        ControlPadMoveMap[('^', 'A')] = new char[] { '>' };
        ControlPadMoveMap[('^', '>')] = new char[] { 'v', '>' };
        ControlPadMoveMap[('^', 'v')] = new char[] { 'v', };
        ControlPadMoveMap[('^', '<')] = new char[] { 'v', '<', };

        ControlPadMoveMap[('v', 'v')] = new char[] { };
        ControlPadMoveMap[('v', 'A')] = new char[] { '>', '^' };
        ControlPadMoveMap[('v', '>')] = new char[] { '>' };
        ControlPadMoveMap[('v', '^')] = new char[] { '^', };
        ControlPadMoveMap[('v', '<')] = new char[] { '<', };

        ControlPadMoveMap[('<', '<')] = new char[] { };
        ControlPadMoveMap[('<', 'A')] = new char[] { '>', '>', '^' };
        ControlPadMoveMap[('<', '>')] = new char[] { '>', '>' };
        ControlPadMoveMap[('<', '^')] = new char[] { '>', '^' };
        ControlPadMoveMap[('<', 'v')] = new char[] { '>', };

        List<List<IntVector2>> allPaths = new List<List<IntVector2>>();

        IntVector2 originalStartPos = Helper.GetPosition(Array.FindIndex<char>(ControlPad, x => x == 'A'), ControlPadWidth);

        //foreach (char c in ControlPad)
        //{
        //    if (c == ' ')
        //    {
        //        continue;
        //    }

        //    char[] values = ControlPadMoveMap[('A', c)];
        //    List<char> sourceList = Array1;
        //    List<char> targetList = Array2;

        //    sourceList.Clear();
        //    sourceList.AddRange(values);
        //    IntVector2 startPos = originalStartPos;

        //    for (int i = 0; i < numIterations; i++)
        //    {
        //        GetMapCommandsForCode(sourceList, targetList, ref startPos, ControlPad, ControlPadWidth, ControlPadMoveMap);
        //        ValueCache[(c, i)] = targetList.Count;
        //        SwapList(ref sourceList, ref targetList);
        //    }

        //}


        string keys = "0123456789A";
        for (int i = 0; i < keys.Length; i++)
        {
            IntVector2 a = Helper.GetPosition(Array.FindIndex<char>(NumericPad, x => x == keys[i]), NumericPadWidth);
            char from = keys[i];

            for (int j = 0; j < keys.Length; j++)
            {
                char to = keys[j];

                allPaths.Clear();
                IntVector2 b = Helper.GetPosition(Array.FindIndex<char>(NumericPad, x => x == keys[j]), NumericPadWidth);


                BuildAllPaths(a, b, 7, NumericPad, NumericPadWidth, NumericPadHeight, new List<IntVector2>(), allPaths);

                int shortestLen = int.MaxValue;
                string shortestTemp = "";

                foreach (List<IntVector2> path in allPaths)
                {
                    string temp = "";
                    for (int k = 1; k < path.Count; k++)
                    {
                        IntVector2 diff = path[k] - path[k - 1];
                        temp += (Helper.PointerFromDirection(diff));
                    }


                    IntVector2 startPos = originalStartPos;


                    List<char> sourceList = Array1;
                    List<char> targetList = Array2;

                    sourceList.Clear();
                    sourceList.AddRange(temp.ToCharArray());

                    for (int c = 0; c < numIterations; c++)
                    {
                        GetMapCommandsForCode(sourceList, targetList, ref startPos, ControlPad, ControlPadWidth, ControlPadMoveMap);
                        SwapList(ref sourceList, ref targetList);
                        startPos = originalStartPos;
                    }


                    SwapList(ref sourceList, ref targetList);

                    if (targetList.Count < shortestLen)
                    {
                        shortestLen = targetList.Count;
                        shortestTemp = temp;

                        char[] shortestCA = shortestTemp.ToCharArray();

                        NumericPadMoveMap[(from, to)] = shortestCA;
                    }
                }
            }
        }




        int ibreak = 0;

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
            validPaths.Add(currentPath);
            return true;
        }


        bool pathExists = false;
        foreach (IntVector2 direction in IntVector2.Directions)
        {
            IntVector2 updated = current + direction;
            if (Helper.InBounds(updated, width, height))
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




    public string GetCommandsForCode(string code, ref IntVector2 startPosition, char[] dataGrid, int width, int height, bool allAllowed = false)
    {
        List<char> commands = new List<char>();
        foreach (char c in code)
        {
            List<IntVector2> moves = GetCommandListForPress(startPosition, c, dataGrid, width, height, allAllowed);
            List<char> temp = new List<char>();
            if (moves.Count > 1)
            {
                startPosition = moves.Last();

                for (int i = 1; i < moves.Count; i++)
                {
                    IntVector2 diff = moves[i] - moves[i - 1];
                    temp.Add(Helper.PointerFromDirection(diff));
                }
            }
            else
            {
                int ibreak = 0;
            }
            temp.Add('A');
            //DebugOutput(string.Join("", temp.ToArray()));
            commands.AddRange(temp);
        }

        string original = new string(commands.ToArray());
        return original;
    }

    public void GetMapCommandsForCode(List<char> sourceList, List<char> targetList, ref IntVector2 startPosition, char[] dataGrid, int width, Dictionary<(char, char), char[]> map, bool debug = false)
    {
        targetList.Clear();
        foreach (char c in sourceList)
        {
            char currentChar = dataGrid[Helper.GetIndex(startPosition, width)];
            char[] moves = map[(currentChar, c)];
            targetList.AddRange(moves);
            targetList.Add('A');

            startPosition = Helper.GetPosition(Array.FindIndex<char>(dataGrid, x => x == c), width);

            if (debug)
            {
                //DebugOutput($"From char {currentChar} to char {c} adding {string.Join(",", moves)},A");
            }
        }

    }

    public Dictionary<(char, int), long> CostDictionary = new Dictionary<(char, int), long>();
    public long Cost(char from, char to, int level, Dictionary<int,List<char>>? debug = null)
    {
        var key = (from, to);
        long totalCost = 0;
        bool firstForLevel = false;

        if(false)
            {
        DebugOutput($"From char {from} to char {to} adding {string.Join(",", ControlPadMoveMap[(from, to)])},A");
        }

        List<char> debugLine = null;
        if(debug != null)
        {
            if(!debug.TryGetValue(level,out debugLine))
            {
                debugLine = new List<char>();
                debug[level] = debugLine;
                firstForLevel = true;
            }
        }

        if (debug != null)
        {
            debugLine.AddRange(ControlPadMoveMap[(from, to)]);
            debugLine.Add('A');
        }


        //if (CostDictionary.TryGetValue(key, out totalCost))
        if(false)
        {
            //return totalCost;
        }
        else
        {
            if (level == 0)
            {
                totalCost = ControlPadMoveMap[(from, to)].LongLength;
            }
            else
            {
                List<char> temp = new List<char>();
                //if(firstForLevel)
                {
                    temp.Add('A');
                }
                temp.AddRange(ControlPadMoveMap[(from, to)]);
                temp.Add('A');
                for (int i = 0; i < temp.Count - 1; i++)
                {
                    long cost = Cost(temp[i], temp[i + 1], level - 1, debug);
                    totalCost += cost;
                }
            }
        }

        CostDictionary[key] = totalCost;
        return totalCost;
    }



    public List<IntVector2> GetCommandListForPress(IntVector2 currentPosition, char desiredButton, char[] layout, int width, int height, bool allAllowed = false)
    {
        List<IntVector2> results = new List<IntVector2>();
        IntVector2 targetPosition = Helper.GetPosition(Array.FindIndex<char>(layout, x => x == desiredButton), width);

        AStarInt astar = new AStarInt();
        astar.Initialize(new PadMapData(layout, width, height, currentPosition, targetPosition, allAllowed));

        astar.FindPath(currentPosition, targetPosition, results);

        return results;
    }


    private void SwapList(ref List<char> list1, ref List<char> list2)
    {
        List<char> t = list1;
        list1 = list2;
        list2 = t;
    }


    public class PadMapData : IMapDataInt
    {
        private char[] m_dataGrid;
        private int m_width;
        private int m_height;

        private IntVector2 m_startPosition;
        private IntVector2 m_endPosition;
        private bool m_allAllowed;


        public PadMapData(char[] dataGrid, int width, int height, IntVector2 start, IntVector2 end, bool allAllowed)
        {

            m_dataGrid = dataGrid;
            m_width = width;
            m_height = height;
            m_startPosition = start;
            m_endPosition = end;
            m_allAllowed = allAllowed;
        }


        public bool CanMove(IntVector2 from, IntVector2 to)
        {
            return Helper.InBounds(to, m_width, m_height) && (m_allAllowed || m_dataGrid[Helper.GetIndex(to, m_width)] != ' ');
        }

        public float DistanceToTarget(IntVector2 v, IntVector2 t)
        {
            return v.ManhattanDistance(t);
        }


        IntVector2[] d = new IntVector2[] { IntVector2.Up, IntVector2.Down, IntVector2.Left, IntVector2.Right };
        public IntVector2[] GetDirections()
        {
            return d;
        }

    }


}