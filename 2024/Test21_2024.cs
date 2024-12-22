

using System.Numerics;

public class Test21_2024 : BaseTest
{

    public char[] NumericPad = "789456123 0A".ToCharArray();
    public int NumericPadWidth = 3;
    public int NumericPadHeight = 4;



    public char[] ControlPad = " ^ A<v>".ToCharArray();
    public int ControlPadWidth = 3;
    public int ControlPadHeight = 2;


    public IntVector2 NumericPadRobotArmPosition;
    public IntVector2 ControlPad1RobotArmPosition;
    public IntVector2 ControlPad2RobotArmPosition;

    public override void Initialise()
    {
        Year = 2024;
        TestID = 21;


        NumericPadRobotArmPosition = Helper.GetPosition(Array.FindIndex<char>(NumericPad, x => x == 'A'), NumericPadWidth);
        ControlPad1RobotArmPosition = Helper.GetPosition(Array.FindIndex<char>(ControlPad, x => x == 'A'), NumericPadWidth);
        ControlPad2RobotArmPosition = ControlPad1RobotArmPosition;


        string desiredCode = "029A";
        List<char> commands = GetCommandsForCode(desiredCode, ref NumericPadRobotArmPosition, NumericPad, NumericPadWidth, NumericPadHeight);


        DebugOutput(string.Join("", commands));
    }


    public List<char> GetCommandsForCode(string code, ref IntVector2 startPosition, char[] dataGrid, int width, int height)
    {
        List<char> commands = new List<char>();
        foreach (char c in code)
        {
            List<IntVector2> moves = GetCommandListForPress(startPosition, c, dataGrid, width, height);

            if (moves.Count > 1)
            {
                startPosition = moves.Last();

                for (int i = 1; i < moves.Count; i++)
                {
                    IntVector2 diff = moves[i] - moves[i - 1];
                    commands.Add(Helper.PointerFromDirection(diff));
                }
                commands.Add('A');
            }
        }
        return commands;
    }


    public List<IntVector2> GetCommandListForPress(IntVector2 currentPosition, char desiredButton, char[] layout, int width, int height)
    {
        List<IntVector2> results = new List<IntVector2>();
        IntVector2 targetPosition = Helper.GetPosition(Array.FindIndex<char>(layout, x => x == desiredButton), width);

        AStarInt astar = new AStarInt();
        astar.Initialize(new PadMapData(layout, width, height, currentPosition, targetPosition));

        astar.FindPath(currentPosition, targetPosition, results);

        return results;
    }


    public override void Execute()
    {



    }


    public class PadMapData : IMapDataInt
    {
        private char[] m_dataGrid;
        private int m_width;
        private int m_height;

        private IntVector2 m_startPosition;
        private IntVector2 m_endPosition;


        public PadMapData(char[] dataGrid, int width, int height, IntVector2 start, IntVector2 end)
        {

            m_dataGrid = dataGrid;
            m_width = width;
            m_height = height;
            m_startPosition = start;
            m_endPosition = end;
        }


        public bool CanMove(IntVector2 from, IntVector2 to)
        {
            return Helper.InBounds(to, m_width, m_height) && m_dataGrid[Helper.GetIndex(to, m_width)] != ' ';
        }

        public float DistanceToTarget(IntVector2 v, IntVector2 t)
        {
            return v.ManhattanDistance(t);
        }


        public IntVector2[] GetDirections()
        {
            return IntVector2.Directions;
        }

    }


}