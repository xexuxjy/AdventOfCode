using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

public class Test9_2023 : BaseTest
{
    public override void Initialise()
    {
        Year = 2023;
        TestID = 9;
        IsTestInput = false;
        IsPart2 = true;
    }

    public override void Execute()
    {
        long total = 0;
        List<NumberPyramid> pyramidList = new List<NumberPyramid>();
        foreach (string line in m_dataFileContents)
        {
            List<long> ll = new List<long>();
            Helper.ReadLongs(line, ll);
            NumberPyramid numberPyramid = new NumberPyramid(ll);
            numberPyramid.Solve(IsPart2);
            pyramidList.Add(numberPyramid);
            //DebugOutput(numberPyramid.DebugInfo());
            total += numberPyramid.Score;
        }

        DebugOutput("Final score is : " + total);
    }

    public class NumberPyramid
    {
        public List<List<long>> NumberDiffs = new List<List<long>>();

        public NumberPyramid(List<long> ll)
        {
            NumberDiffs.Add(ll);
        }

        public long Score;

        public void Solve(bool part2)
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                int index = NumberDiffs.Count - 1;
                List<long> ll = new List<long>();
                for (int j = 0; j < NumberDiffs[index].Count - 1; ++j)
                {
                    ll.Add(NumberDiffs[index][j + 1] - NumberDiffs[index][j]);
                }

                NumberDiffs.Add(ll);
                if (ll.FindAll(x => x == 0).Count() == ll.Count)
                {
                    keepGoing = false;
                }
            }

            if (part2)
            {
                NumberDiffs.Last().Insert(0, 0);
                for (int i = (NumberDiffs.Count - 1); i > 0; --i)
                {
                    NumberDiffs[i - 1].Insert(0, NumberDiffs[i - 1].First() - NumberDiffs[i].First());
                }
                Score = NumberDiffs[0].First();
            }
            else
            {
                NumberDiffs.Last().Add(0);

                for (int i = (NumberDiffs.Count - 1); i > 0; --i)
                {
                    NumberDiffs[i - 1].Add(NumberDiffs[i - 1].Last() + NumberDiffs[i].Last());
                }

                Score = NumberDiffs[0].Last();
            }
        }

        public string DebugInfo()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var ll in NumberDiffs)
            {
                sb.AppendLine(String.Join(", ", ll));
            }

            return sb.ToString();
        }
    }
}