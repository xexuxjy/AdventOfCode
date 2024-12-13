using static Test7_2024;
using System.Collections.Concurrent;

public class Test13_2024 : BaseTest
{
    public override void Initialise()
    {
        Year = 2024;
        TestID = 13;
    }

    public override void Execute()
    {
        int maxMove = 100;
        List<ClawGame> clawGames = new List<ClawGame>();
        ClawGame clawGame = null;
        foreach(string line in m_dataFileContents)
        {
            if(clawGame == null)
            {
                clawGame = new ClawGame();
                clawGames.Add(clawGame);
            }

            if(line.StartsWith("Button"))
            {
                string[] tokens = line.Split(new char[]{' ',',' },StringSplitOptions.RemoveEmptyEntries);
                int x = int.Parse(tokens[2].Substring(2));
                int y = int.Parse(tokens[3].Substring(2));

                clawGame.Buttons[tokens[1]] = new IntVector2(x,y);

            }
            else if(line.StartsWith("Prize"))
            {
                string[] tokens = line.Split(new char[]{' ',',' },StringSplitOptions.RemoveEmptyEntries);
                int x = int.Parse(tokens[1].Substring(2));
                int y = int.Parse(tokens[2].Substring(2));
                

                clawGame.PrizeLocation= new IntVector2(x,y);
                clawGame = null;
            }

        }

        var validScoresBag = new ConcurrentBag<int>();

        Parallel.ForEach(clawGames, cg =>
        {
            int val = cg.SolveBruteForce(maxMove);
            if(val != int.MaxValue)
            {
                validScoresBag.Add(val);
            }
        });


        int total = 0;
        foreach (int i in validScoresBag)
        {
            total+=i;
        }

        //foreach(ClawGame cg in clawGames)
        //{
        //    int result = cg.SolveBruteForce(maxMove);
        //    if(result != int.MaxValue)
        //    {
        //        total += result;
        //    }
        //}


        DebugOutput("The lowest cost is : "+total);
        

    }


    public class ClawGame
    {
        public Dictionary<string,IntVector2> Buttons = new Dictionary<string,IntVector2>();
        public IntVector2 PrizeLocation = new IntVector2();

        public int SolveBruteForce(int maxMoves)
        {
            
            IntVector2 AMove = Buttons["A:"];
            IntVector2 BMove = Buttons["B:"];

            int ACost = 3;
            int BCost = 1;

            int lowestCost = int.MaxValue;


            for(int a = 0; a < maxMoves; a++)
            {
                for(int b = 0; b < maxMoves; b++)
                {
                    if((AMove*a)+(BMove*b) == PrizeLocation)
                    {
                        int cost = (a*ACost)+(b*BCost);
                        if(cost < lowestCost)
                        {
                            lowestCost = cost;
                        }
                    }
                }
            }

            return lowestCost;
        }

    }
}