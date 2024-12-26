using System.Net.Sockets;
using static Test22_2024;

public class Test22_2024 : BaseTest
{

    public override void Initialise()
    {
        Year = 2024;
        TestID = 22;
    }

    public override void Execute()
    {
        List<Buyer> buyers = new List<Buyer>();
        long total = 0;
        foreach (string line in m_dataFileContents)
        {
            Buyer buyer = new Buyer();
            buyers.Add(buyer);

            long startNumber = long.Parse(line);
            long startNumberOriginal = startNumber;

            int numIterations = 2000;


            for (int i = 0; i < numIterations; i++)
            {
                buyer.SecretNumbers.Add(startNumber);
                startNumber = ApplyRules(startNumber);
            }

            DebugOutput($"{startNumberOriginal}: {startNumber}");
            total += startNumber;
        }

        DebugOutput("Total is : " + total);

        if (IsPart2)
        {
            foreach (Buyer buyer in buyers)
            {
                buyer.Calc();
            }

            int[] bestNumbers = new int[4];
            int[] currentNumbers = new int[4];

            // generate a set of numbers
            int bestScore = 0;


            int range = 9;

            for (int a = -range; a < range; a++)
            {
                for (int b = -range; b < range; b++)
                {
                    for (int c = -range; c < range; c++)
                    {
                        for (int d = -range; d < range; d++)
                        {
                            int score = 0;
                            currentNumbers[0] = a;
                            currentNumbers[1] = b;
                            currentNumbers[2] = c;
                            currentNumbers[3] = d;

                            foreach (Buyer buyer in buyers)
                            {
                                int r = buyer.TestSellPoint(currentNumbers);
                                if (r != -1)
                                {
                                    score += r;
                                }
                            }
                            if (score > bestScore)
                            {

                                bestScore = score;
                                Array.Copy(currentNumbers, bestNumbers, bestNumbers.Length);
                            }
                        }
                    }
                }
            }

            DebugOutput($"The best pattern was {string.Join(", ", bestNumbers)} with a score of {bestScore}");

        }
    }


    public long ApplyRules(long secretValue)
    {
        // these will probably be shifts
        long step1 = secretValue * 64;
        // mix result into secret number (value)

        secretValue = Mix(secretValue, step1);
        secretValue = Prune(secretValue);

        long step2 = secretValue / 32; // round to neearest integer 
        // mix result into secret number (value)
        secretValue = Mix(secretValue, step2);
        //prune this result
        secretValue = Prune(secretValue);

        long step3 = secretValue * 2048;
        // mix result into secret number (value)
        secretValue = Mix(secretValue, step3);

        //prune this result
        secretValue = Prune(secretValue);

        return secretValue;
    }

    public long Mix(long secret, long value)
    {
        return (long)(secret ^ value);
    }

    public long Prune(long value)
    {
        return value % 16777216;
    }


    public class Buyer
    {
        public List<long> SecretNumbers = new List<long>();
        public List<int> LastDigits = new List<int>();
        public List<int> Diffs = new List<int>();

        public void Calc()
        {
            foreach (long n in SecretNumbers)
            {
                LastDigits.Add((int)(n % 10));
            }

            for (int i = 1; i < LastDigits.Count; i++)
            {
                Diffs.Add(LastDigits[i] - LastDigits[i - 1]);
            }
        }

        public int TestSellPoint(int[] numbers)
        {
            int foundIndex = -1;
            for (int i = 0; i < Diffs.Count-numbers.Length; i++)
            {
                bool found = true;
                for(int j=0; j < numbers.Length; j++)
                {
                    if(Diffs[i+j] != numbers[j]) 
                     {
                        found = false;
                        break;
                    }
                }
                if(found)
                {
                    // need to do it _after_ we've seen the sequence
                    foundIndex = i+numbers.Length;
                    break;
                }
                        
            }

            if (foundIndex != -1)
            {
                return LastDigits[foundIndex];
            }
            return -1;

        }

    }

}


