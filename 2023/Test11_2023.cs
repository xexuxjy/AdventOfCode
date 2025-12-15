using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

public class Test11_2023 : BaseTest
{
    public override void Initialise()
    {
        Year = 2023;
        TestID = 11;
        IsTestInput = false;
        IsPart2 = true;
    }

    public override void Execute()
    {
        Galaxy galaxy = new Galaxy(m_dataFileContents, IsPart2 ? 1000000 : 2);

        int start = 2;
        int end = 7;
        long distance = galaxy.InflatedStarList[start].ManhattanDistance(galaxy.InflatedStarList[end]);
        DebugOutput($"Distance between Star {start + 1} {galaxy.InflatedStarList[start]} and Star {end + 1} {galaxy.InflatedStarList[end]} is {distance}");

        List<int> indices = new List<int>();
        for (int i = 0; i < galaxy.InflatedStarList.Count; ++i)
        {
            indices.Add(i);
        }

        long total = 0;
        var pairList = indices.SelectMany((fst, i) => indices.Skip(i + 1).Select(snd => (fst, snd)));
        foreach (var pair in pairList)
        {
            long d = galaxy.InflatedStarList[pair.fst].ManhattanDistance(galaxy.InflatedStarList[pair.snd]);
            total += d;
        }

        DebugOutput($"Total distance between all inflated stars is : " + total);

    }


    public class Galaxy
    {
        public List<string> OriginalPositions = new List<string>();
        public List<int> EmptyColumns = new List<int>();
        public List<int> EmptyRows = new List<int>();

        public List<LongVector2> OriginalStarList = new List<LongVector2>();
        public List<LongVector2> InflatedStarList = new List<LongVector2>();

        public Galaxy(List<string> data, int multiplier)
        {
            OriginalPositions.AddRange(data);

            for (int i = 0; i < OriginalPositions.Count; ++i)
            {
                if (!OriginalPositions[i].Contains('#'))
                {
                    EmptyRows.Add(i);
                }
            }

            for (int i = 0; i < OriginalPositions[0].Length; ++i)
            {
                bool empty = true;
                for (int j = 0; j < OriginalPositions.Count; ++j)
                {
                    if (OriginalPositions[j][i] == '#')
                    {
                        empty = false;
                        break;
                    }
                }

                if (empty)
                {
                    EmptyColumns.Add(i);
                }
            }


            for (int i = 0; i < OriginalPositions.Count; ++i)
            {
                for (int j = 0; j < OriginalPositions[i].Length; ++j)
                {
                    if (OriginalPositions[i][j] == '#')
                    {
                        OriginalStarList.Add(new LongVector2(j, i));
                    }
                }
            }


            foreach (LongVector2 star in OriginalStarList)
            {
                InflatedStarList.Add(GetInflatedPosition(star, multiplier));
            }



        }


        public LongVector2 GetInflatedPosition(LongVector2 original, int multiplier)
        {

            long rightShift = EmptyColumns.FindAll(x => x < original.X).Count;
            long downShift = EmptyRows.FindAll(x => x < original.Y).Count;

            rightShift *= (multiplier - 1);
            downShift *= (multiplier - 1);

            return original + new LongVector2(rightShift, downShift);

        }





    }

}