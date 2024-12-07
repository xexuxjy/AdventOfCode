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
        List<PossibleSum> possibleSumList = new List<PossibleSum>();
        foreach (string line in m_dataFileContents)
        {
            possibleSumList.Add(PossibleSum.FromString(line));
        }


        var possibleSumsBag = new ConcurrentBag<PossibleSum>();

        //Parallel.ForEach(possibleSumList, possibleSum =>
        //{
        //    possibleSum.SolveBruteForce();
        //    if (possibleSum.IsPossible)
        //    {
        //        possibleSumsBag.Add(possibleSum);
        //    }
        //});


        foreach (PossibleSum possibleSum in possibleSumList)
        {
            possibleSum.SolveBruteForce();
            if (possibleSum.IsPossible)
            {
                possibleSumsBag.Add(possibleSum);
            }
        }


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

        public void SolveBruteForce()
        {
            long[] operands = new long[Terms.Length - 1];
            long[] operations = new long[] { ADD, MUL };


            List<long[]> possiblePermutations = Combinations.BuildOptions<long>(Terms.Length - 1, operations);

            foreach (var result in possiblePermutations)
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
            }
            return calcTotal;
        }
    }
}