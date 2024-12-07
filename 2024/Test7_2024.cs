using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using static Test7_2024;

public class Test7_2024 : BaseTest
{

    public override void Initialise()
    {
        Year = 2024;
        TestID = 7;
    }

    public override void Execute()
    {
        long finalTotal = 0;
        long maxTerms = 0;
        
        long[] operations = IsPart2?new long[] { PossibleSum.ADD, PossibleSum.MUL,PossibleSum.CAT } : new long[] { PossibleSum.ADD, PossibleSum.MUL} ;


        List<PossibleSum> possibleSumList = new List<PossibleSum>();
        foreach (string line in m_dataFileContents)
        {
            PossibleSum possibleSum = PossibleSum.FromString(line);
            if(possibleSum.Terms.Length > maxTerms)
            {
                maxTerms = possibleSum.Terms.Length;
            }
            possibleSumList.Add(possibleSum);
        }

        List<List<long[]>> possiblePermutations = new List<List<long[]>>();
        for(int i=1;i<=maxTerms;++i)
        {
            List<long[]> list = Combinations.BuildOptions<long>(i, operations);
            possiblePermutations.Add(list);
        }
       

        var possibleSumsBag = new ConcurrentBag<PossibleSum>();

        Parallel.ForEach(possibleSumList, possibleSum =>
        {
            possibleSum.SolveBruteForce(possiblePermutations, IsPart2);
            if (possibleSum.IsPossible)
            {
                possibleSumsBag.Add(possibleSum);
            }
        });



        //foreach (PossibleSum possibleSum in possibleSumList)
        //{
        //    possibleSum.SolveBruteForce(possiblePermutations, IsPart2);
        //    if (possibleSum.IsPossible)
        //    {
        //        possibleSumsBag.Add(possibleSum);
        //    }
        //}


        foreach (PossibleSum possibleSum in possibleSumsBag)
        {
            finalTotal += possibleSum.Result;
        }

        int ibreak = 0;
        DebugOutput("Have a final total of : " + finalTotal);

    }




    public class PossibleSum
    {
        public long Result;
        public long[] Terms;
        public long[] FinalOperands;


        public const int ADD = 0;
        public const int MUL = 1;
        public const int SUB = 2;
        public const int DIV = 3;
        public const int CAT = 4;

        public bool IsPossible
        { get { return FinalOperands != null; } }

        public static PossibleSum FromString(string line)
        {
            string[] tokens = line.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            PossibleSum possibleSum = new PossibleSum();
            possibleSum.Result = long.Parse(tokens[0]);
            possibleSum.Terms = new long[tokens.Length - 1];
            for (int i = 0; i < possibleSum.Terms.Length; i++)
            {
                possibleSum.Terms[i] = long.Parse(tokens[i + 1]);
            }


            return possibleSum;
        }

        public void SolveBruteForce(List<List<long[]>> possiblePermutations,bool part2)
        {
            long[] operands = null;;
            List<long[]> permutations = possiblePermutations[Terms.Length-2];

            foreach (var result in permutations )
            {
                operands = result;
                if (GetTotal(operands) == Result)
                {
                    // found result;
                    FinalOperands = operands;
                    return;
                }
            }
        }

        public long GetTotal(long[] operands)
        {
            long calcTotal = Terms[0];
            for (int i = 0; i < operands.Length; i++)
            {
                if (operands[i] == ADD)
                {
                    calcTotal = calcTotal + Terms[i + 1];
                }
                else if (operands[i] == MUL)
                {
                    calcTotal = calcTotal * Terms[i + 1];
                }
                else if(operands[i] == CAT)
                {
                    calcTotal = long.Parse(calcTotal.ToString()+ (Terms[i + 1]).ToString());
                }
            }
            return calcTotal;
        }
    }
}