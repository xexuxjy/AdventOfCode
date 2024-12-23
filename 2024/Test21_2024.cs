

using System.Numerics;

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
        SetupMaps();
    }

    public Dictionary<(char, char), char[]> NumericPadMoveMap = new Dictionary<(char, char), char[]>();
    public Dictionary<(char, char), char[]> ControlPadMoveMap = new Dictionary<(char, char), char[]>();




    public void SetupMaps()
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

        string keys= "0123456789A";
        for(int i=0; i<keys.Length; i++)
        {
            IntVector2 a = Helper.GetPosition(Array.FindIndex<char>(NumericPad, x => x == keys[i]), NumericPadWidth);
            char from = keys[i];

            for(int j=0; j<keys.Length; j++)
            {
                char to = keys[j];

                allPaths.Clear();
                IntVector2 b = Helper.GetPosition(Array.FindIndex<char>(NumericPad, x => x == keys[j]), NumericPadWidth);
        
                if(from == '9' && to == 'A')
                {
                    int ibreak3 = 0;
                }

                BuildAllPaths(a,b,8,NumericPad,NumericPadWidth,NumericPadHeight,new List<IntVector2>(),allPaths);

                string shortest = "                                                                                                                     ";
                string shortestTemp = "";

                foreach(List<IntVector2> path in allPaths)
                {
                    string temp ="";
                    for (int k = 1; k < path.Count; k++)
                    {
                        IntVector2 diff = path [k] - path[k - 1];
                        temp += (Helper.PointerFromDirection(diff));
                    }



                    IntVector2 startPos = Helper.GetPosition(Array.FindIndex<char>(ControlPad, x => x == 'A'), ControlPadWidth);


                    string controlPad1Commands = GetMapCommandsForCode(temp, ref startPos, ControlPad, ControlPadWidth, ControlPadMoveMap);

                    startPos = Helper.GetPosition(Array.FindIndex<char>(ControlPad, x => x == 'A'), ControlPadWidth);
                    controlPad1Commands = GetMapCommandsForCode(controlPad1Commands, ref startPos, ControlPad, ControlPadWidth, ControlPadMoveMap);

                    if(controlPad1Commands.Length < shortest.Length)
                    {
                        shortest = controlPad1Commands;
                        shortestTemp = temp;
                    }

                }

                NumericPadMoveMap[(keys[i],keys[j])] = shortestTemp.ToCharArray();

            }
        }



        int ibreak =0;

    }


    

    public bool BuildAllPaths(IntVector2 current,IntVector2 target,int depth,char[]dataGrid,int width,int height,List<IntVector2> currentPath,List<List<IntVector2>> validPaths)
    {

        if(depth == 0)
        {
            return false; 
        }

        currentPath.Add(current);

        if(current == target)
        {
            validPaths.Add(currentPath);
            return true; 
        }


        bool pathExists = false;
        foreach(IntVector2 direction in IntVector2.Directions)
        {
            IntVector2 updated = current + direction;
            if(Helper.InBounds(updated,width,height))
            {
                int index = Helper.GetIndex(updated,width);
                if(dataGrid[index] != ' ')
                {
                    List<IntVector2> pathCopy = new List<IntVector2>();
                    pathCopy.AddRange(currentPath);

                    pathExists |= BuildAllPaths(updated,target,depth-1,dataGrid,width,height,pathCopy,validPaths);
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


    public string GetMapCommandsForCode(string code, ref IntVector2 startPosition, char[] dataGrid, int width, Dictionary<(char, char), char[]> map)
    {
        List<char> commands = new List<char>();
        foreach (char c in code)
        {
            char currentChar = dataGrid[Helper.GetIndex(startPosition, width)];
            char[] moves = map[(currentChar, c)];
            commands.AddRange(moves);
            commands.Add('A');

            startPosition = Helper.GetPosition(Array.FindIndex<char>(dataGrid, x => x == c), width);

        }

        string original = new string(commands.ToArray());
        return original;
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


    public override void Execute()
    {

        NumericPadRobotArmPosition = Helper.GetPosition(Array.FindIndex<char>(NumericPad, x => x == 'A'), NumericPadWidth);
        ControlPad1RobotArmPosition = Helper.GetPosition(Array.FindIndex<char>(ControlPad, x => x == 'A'), ControlPadWidth);
        ControlPad2RobotArmPosition = ControlPad1RobotArmPosition;

        int totalScore = 0;
        foreach (string desiredCode in m_dataFileContents)
        {
            //string commands = GetCommandsForCode(desiredCode, ref NumericPadRobotArmPosition, NumericPad, NumericPadWidth, NumericPadHeight);
            string commands = GetMapCommandsForCode(desiredCode, ref NumericPadRobotArmPosition, NumericPad, NumericPadWidth, NumericPadMoveMap);

            DebugOutput(commands);
            string controlPad1Commands = GetMapCommandsForCode(commands, ref ControlPad1RobotArmPosition, ControlPad, ControlPadWidth, ControlPadMoveMap);
            DebugOutput(controlPad1Commands);

            string controlPad2Commands = GetMapCommandsForCode(controlPad1Commands, ref ControlPad2RobotArmPosition, ControlPad, ControlPadWidth, ControlPadMoveMap);
            DebugOutput(controlPad2Commands);

            int num = int.Parse(desiredCode.Replace("A", ""));
            int score = controlPad2Commands.Length * num;

            totalScore += score;

            DebugOutput($"code {desiredCode}  is {controlPad2Commands.Length} * {num} = {score}");
            DebugOutput($"{desiredCode}: {controlPad2Commands}");
        }

        DebugOutput("The final score is : " + totalScore);


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