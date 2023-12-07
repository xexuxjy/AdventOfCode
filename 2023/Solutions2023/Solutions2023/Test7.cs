using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;

public class Test7 : BaseTest
{
    public static  string CardOrder = "23456789TJQKA";
    public override void Initialise()
    {
        TestID = 7;
        IsTestInput = false;
        IsPart2 = true;
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
            DebugOutput("Scoring : "+new String(hands[i].ScoringCards.ToArray())+"  " +hands[i].Ranking+"  AllCards :" +new String(hands[i].Cards.ToArray())+"  "+ bids[hands[i].Id] + " * " + (i + 1) + " = " + bidScore);
        }

        DebugOutput("Total score is " + totalScore);
        int ibreak = 0;
    }


    enum ScoreRanking
    {
        FiveOfAKind,
        FourOfAKind,
        FullHouse,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        Highest
    }

    
    
    
    class Hand : IComparable<Hand>
    {
        public int Id;
        public List<char> Cards = new List<char>();
        public List<char> ScoringCards = new List<char>();
        public ScoreRanking Ranking;
        public void ScoreHand(bool isPart2)
        {
            // Cards.Sort((x,y) => CardOrder.IndexOf(x).CompareTo(CardOrder.IndexOf(y)));
            // Cards.Reverse();
            
            int highest = 0;
            
            foreach (char card in Cards)
            {
                int count = Cards.Count(x => x == card);
                if (count > highest)
                {
                    highest = count;
                }
            }

            int jokerCount = 0;
            if (isPart2)
            {
                jokerCount = Cards.Count(x => x == 'J');
                highest += jokerCount;
            }
            
            
            
            var groups = Cards.GroupBy(c => c);

            switch (highest)
            {
                case 5:
                    Ranking = ScoreRanking.FiveOfAKind;
                    ScoringCards.AddRange(Cards);
                    break;
                case 4:
                    Ranking = ScoreRanking.FourOfAKind;
                    var bigGroup =groups.Where(x => x.Count() == 4).FirstOrDefault();
                    var smallGroup =groups.Where(x => x.Count() == 1).FirstOrDefault();
                    for (int i = 0; i < bigGroup?.Count(); ++i)
                    {
                        ScoringCards.Add(bigGroup.Key);
                    }
                    for (int i = 0; i < smallGroup?.Count(); ++i)
                    {
                        ScoringCards.Add(smallGroup.Key);
                    }
                    
                    break;
                case 3:
                    var bigGroup2 = groups.Where(x => x.Count() == 3).FirstOrDefault();
                    var smallGroup2 = groups.Where(x => x.Count() == 2).FirstOrDefault();

                    if (bigGroup2?.Count() == 3)
                    {
                        for (int i = 0; i < bigGroup2?.Count(); ++i)
                        {
                            ScoringCards.Add(bigGroup2.Key);
                        }

                        Ranking = ScoreRanking.ThreeOfAKind;
                        if (smallGroup2?.Count() == 2)
                        {
                            for (int i = 0; i < smallGroup2?.Count(); ++i)
                            {
                                ScoringCards.Add(smallGroup2.Key);
                            }
                            Ranking = ScoreRanking.FullHouse;
                        }
                    }
                    break;
                case 2:
                    var pairGroups = groups.Where(x => x.Count() == 2);
                    int count = 0;
                    foreach (var pairGroup in pairGroups)
                    {
                        count++;
                        ScoringCards.Add(pairGroup.Key);
                        ScoringCards.Add(pairGroup.Key);
                    }
                    if (count == 2)
                    {
                        Ranking = ScoreRanking.TwoPair;
                    }
                    else
                    {
                        Ranking = ScoreRanking.OnePair;
                    }
                    break;
                case 1:
                    Ranking = ScoreRanking.Highest;
                    ScoringCards.Add(Cards[0]);
                    break;
            }

        }

        public int CompareTo(Hand other)
        {
            if (Ranking < other.Ranking)
            {
                return 1;
            }
            else if (Ranking > other.Ranking)
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

            //     for (int i = 0; i < ScoringCards.Count; ++i)
            //     {
            //         if (CardOrder.IndexOf(ScoringCards[i]) != CardOrder.IndexOf(other.ScoringCards[i]))
            //         {
            //             return CardOrder.IndexOf(ScoringCards[i]) > CardOrder.IndexOf(other.ScoringCards[i]) ? 1 : -1;
            //         }
            //     }
            }
            return 0;
        }
        
    }
    
}