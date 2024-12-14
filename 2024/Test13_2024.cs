using static Test7_2024;
using System.Collections.Concurrent;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Test15_2023;
using System.Runtime.Intrinsics.X86;

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
        foreach (string line in m_dataFileContents)
        {
            if (clawGame == null)
            {
                clawGame = new ClawGame();
                clawGames.Add(clawGame);
            }

            if (line.StartsWith("Button"))
            {
                string[] tokens = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                int x = int.Parse(tokens[2].Substring(2));
                int y = int.Parse(tokens[3].Substring(2));

                clawGame.Buttons[tokens[1]] = new LongVector2(x, y);

            }
            else if (line.StartsWith("Prize"))
            {
                string[] tokens = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                int x = int.Parse(tokens[1].Substring(2));
                int y = int.Parse(tokens[2].Substring(2));


                clawGame.PrizeLocation = new LongVector2(x, y);
                clawGame = null;
            }

        }

        var validScoresBag = new ConcurrentBag<long>();

        Parallel.ForEach(clawGames, cg =>
        {
            long val = cg.SolveBruteForce(maxMove, IsPart2);
            if (val != long.MaxValue)
            {
                validScoresBag.Add(val);
            }
        });



        long total = 0;
        foreach (long i in validScoresBag)
        {
            total += i;
        }

        //foreach(ClawGame cg in clawGames)
        //{
        //    int result = cg.SolveBruteForce(maxMove);
        //    if(result != int.MaxValue)
        //    {
        //        total += result;
        //    }
        //}


        DebugOutput("The lowest cost is : " + total);


    }


    public class ClawGame
    {
        public Dictionary<string, LongVector2> Buttons = new Dictionary<string, LongVector2>();
        public LongVector2 PrizeLocation = new LongVector2();

        public long SolveBruteForce(int maxMoves, bool isPart2 = false)
        {

            LongVector2 AMove = Buttons["A:"];
            LongVector2 BMove = Buttons["B:"];

            int ACost = 3;
            int BCost = 1;

            long lowestCost = long.MaxValue;



            if (isPart2)
            {

                // great help provided by : https://www.reddit.com/r/adventofcode/comments/1hd7irq/2024_day_13_an_explanation_of_the_mathematics/?utm_source=share&utm_medium=web3x&utm_name=web3xcss&utm_term=1&utm_content=share_button
                long adjustment = 10000000000000;

                PrizeLocation += new LongVector2(adjustment,adjustment);

                long determinant = AMove.X * BMove.Y - AMove.Y * BMove.X;

                long a = ((PrizeLocation.X * BMove.Y) - (PrizeLocation.Y * BMove.X)) / determinant;
                long b = ((AMove.X * PrizeLocation.Y) - (AMove.Y * PrizeLocation.X)) / determinant;

                if((AMove * a) + (BMove * b) == PrizeLocation)
                {
                    return  (a * ACost) + (b * BCost);
                }
                else
                {
                    return long.MaxValue;
                }

            }
            else
            {

                for (int a = 0; a < maxMoves; a++)
                {
                    for (int b = 0; b < maxMoves; b++)
                    {
                        if ((AMove * a) + (BMove * b) == PrizeLocation)
                        {
                            int cost = (a * ACost) + (b * BCost);
                            if (cost < lowestCost)
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
}