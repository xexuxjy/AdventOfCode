using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;

public class Test7 : BaseTest
{
    public static string CardOrder = "23456789TJQKA";
    public override void Initialise()
    {
        TestID = 7;
        IsTestInput = false;
        IsPart2 = false;
    }

    public override void Execute()
    {
        if (IsPart2)
        {
            CardOrder = "J23456789TQKA";
        }
        
        List<Hand> hands = new List<Hand>();
        List<int> bids = new List<int>();

        int count = 0;
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(' ');
            string handCards = tokens[0];
            int bid = int.Parse(tokens[1]);

            Hand hand = new Hand();
            hand.Cards.AddRange(handCards.ToCharArray());
            hand.Id = count++;
            hand.ScoreHand(IsPart2);
            hands.Add(hand);
            bids.Add(bid);
        }

        hands.Sort();
       
        int totalScore = 0;
        for(int i=0;i<hands.Count;++i)
        {
            int bidScore =(i + 1) * bids[hands[i].Id];
            totalScore += bidScore;

            //if ((hands[i].OriginalRanking != hands[i].FinalRanking))
            {
                String output = " " + hands[i].FinalRanking;
                output += "  " + hands[i].OriginalRanking;
                output += (hands[i].OriginalRanking != hands[i].FinalRanking) ? "  **DIFF**" : "";
                output += " AllCards :" + new String(hands[i].Cards.ToArray());
                output += " JokerCount :" + hands[i].JokerCount;
                output += "  Id : " + hands[i].Id + "  " + bids[hands[i].Id] + " * " + (i + 1) + " = " + bidScore;

                DebugOutput(output);
            }

        }

        //  251121738
        DebugOutput("Total score is " + totalScore);
        if (totalScore != 251121738)
        {
            int ibreak = 0;
        }
    }


    enum ScoreRanking
    {
        FiveOfAKind,
        FourOfAKind,
        FullHouse,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        Highest,
        None
    }

    
    
    
    class Hand : IComparable<Hand>
    {
        public int Id;
        public List<char> Cards = new List<char>();
        public List<char> ScoringCards = new List<char>();

        public ScoreRanking OriginalRanking;
        public ScoreRanking FinalRanking;

        public int JokerCount = 0;
        
        public void ScoreHand(bool isPart2)
        {

            List<char> cardsCopy = new List<char>();
            cardsCopy.AddRange(Cards);

            
            if (isPart2)
            {
                JokerCount = Cards.Count(x => x == 'J');
                cardsCopy.RemoveAll(x=>x=='J');
            }
           
            ScoreRanking ranking = RankCards(cardsCopy);
            OriginalRanking = ranking;
            FinalRanking = ranking;
            
            if (isPart2)
            {
                if(JokerCount >= 4)
                {
                    ranking = ScoreRanking.FiveOfAKind;
                }
                else if (JokerCount == 3)
                {
                    if (ranking == ScoreRanking.OnePair)
                    {
                        ranking = ScoreRanking.FiveOfAKind;
                    }
                    else
                    {
                        ranking = ScoreRanking.FourOfAKind;
                    }
                }
                else if (JokerCount == 2)
                {
                    if (ranking == ScoreRanking.ThreeOfAKind)
                    {
                        ranking = ScoreRanking.FiveOfAKind;
                    }
                    else if (ranking == ScoreRanking.OnePair)
                    {
                        ranking = ScoreRanking.FourOfAKind;
                    }
                    else
                    {
                        ranking = ScoreRanking.ThreeOfAKind;
                    }
                }
                else if (JokerCount == 1)
                {
                    if (ranking == ScoreRanking.FourOfAKind)
                    {
                        ranking = ScoreRanking.FiveOfAKind;
                    }
                    else if (ranking == ScoreRanking.ThreeOfAKind)
                    {
                        ranking = ScoreRanking.FourOfAKind;
                    }
                    else if (ranking == ScoreRanking.TwoPair)
                    {
                        ranking = ScoreRanking.FullHouse;
                    }
                    else if (ranking == ScoreRanking.OnePair)
                    {
                        ranking = ScoreRanking.ThreeOfAKind;
                    }
                    else
                    {
                        ranking = ScoreRanking.OnePair;
                    }
                }

                FinalRanking = ranking;
            }


        }

        public ScoreRanking RankCards(List<char> cards)
        {
            HashSet<char> uniques = new HashSet<char>();
            foreach (char c in cards)
            {
                uniques.Add(c);
            }

            int highest = 0;
            foreach (char card in uniques)
            {
                int count = Cards.Count(x => x == card);
                if (count > highest)
                {
                    highest = count;
                }
            }


            var groups = cards.GroupBy(c => c);

            ScoreRanking ranking = ScoreRanking.None;
            switch (highest)
            {
                case 5:
                    ranking = ScoreRanking.FiveOfAKind;
                    break;
                case 4:
                    ranking = ScoreRanking.FourOfAKind;
                    break;
                case 3:
                    ranking = ScoreRanking.ThreeOfAKind;
                    
                    var bigGroup2 = groups.Where(x => x.Count() == 3).FirstOrDefault();
                    var smallGroup2 = groups.Where(x => x.Count() == 2).FirstOrDefault();

                    if (smallGroup2 != null)
                    {
                        ranking = ScoreRanking.FullHouse;
                    }
                    break;
                case 2:

                    var pairGroups = groups.Where(x => x.Count() == 2);
                    int count = 0;
                    foreach (var pairGroup in pairGroups)
                    {
                        count++;
                    }
                    if (count == 2)
                    {
                        ranking = ScoreRanking.TwoPair;
                    }
                    else
                    {
                        ranking = ScoreRanking.OnePair;
                    }
                    break;
                case 1:
                    ranking = ScoreRanking.Highest;
                    break;
            }

            return ranking;
        }
        
        
        public int CompareTo(Hand other)
        {
            if (FinalRanking < other.FinalRanking)
            {
                return 1;
            }
            else if (FinalRanking > other.FinalRanking)
            {
                return -1;
            }
            else
            {
                for (int i = 0; i < Cards.Count; ++i)
                {
                    if (CardOrder.IndexOf(Cards[i]) != CardOrder.IndexOf(other.Cards[i]))
                    {
                        return CardOrder.IndexOf(Cards[i]) > CardOrder.IndexOf(other.Cards[i]) ? 1 : -1;
                    }
                }
            }
            return 0;
        }
        
    }
    
}