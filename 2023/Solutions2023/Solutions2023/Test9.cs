﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

public class Test9 : BaseTest
{
    public override void Initialise()
    {
        TestID = 9;
        IsTestInput = false;
        IsPart2 = false;
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
            numberPyramid.Solve();
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

        public void Solve()
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

            NumberDiffs.Last().Add(0);

            for (int i = (NumberDiffs.Count - 1); i > 0; --i)
            {
                NumberDiffs[i - 1].Add(NumberDiffs[i - 1].Last() + NumberDiffs[i].Last());
            }

            Score = NumberDiffs[0].Last();
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