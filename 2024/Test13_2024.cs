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

        //Parallel.ForEach(clawGames, cg =>
        //{
        //    long val = cg.SolveBruteForce(maxMove,IsPart2);
        //    if (val != long.MaxValue)
        //    {
        //        validScoresBag.Add(val);
        //    }
        //});

        clawGames[1].SolveBruteForce(maxMove, IsPart2);


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
                long adjustment = 10000000000000;

                PrizeLocation.X += adjustment;
                PrizeLocation.Y += adjustment;

                long xproduct = AMove.X * BMove.X;
                long xremainder = PrizeLocation.X % xproduct;

                long yproduct = AMove.Y * BMove.Y;
                long yremainder = PrizeLocation.Y % yproduct;


                long ax = (xremainder % AMove.X);
                long ay = (yremainder % AMove.Y);
                long bx = (xremainder % BMove.X);
                long by = (yremainder % BMove.Y);


                long a = ((PrizeLocation.X * BMove.Y) - (PrizeLocation.Y * BMove.X)) / (AMove.X * BMove.Y) - (AMove.Y * BMove.X);
                long b = ((AMove.X * PrizeLocation.Y) - (AMove.Y * PrizeLocation.X)) / (AMove.X * BMove.Y) - (AMove.Y * BMove.X);

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