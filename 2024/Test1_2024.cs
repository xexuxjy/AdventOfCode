using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test1_2024 : BaseTest
{
    public override void Initialise()
    {
        Year = 2024;
        TestID = 1;
    }

    public override void Execute()
    {
        List<int> side1List = new List<int>();
        List<int> side2List = new List<int>();

        Dictionary<int, int> FrequencyDictionary = new Dictionary<int, int>();

        foreach (string line in m_dataFileContents)
        {
            string[] split = line.Split(new char[] { ' ' }, 2);
            int val1 = int.Parse(split[0]);
            int val2 = int.Parse(split[1]);
            side1List.Add(val1);
            side2List.Add(val2);

            if (!FrequencyDictionary.ContainsKey(val2))
            {
                FrequencyDictionary[val2] = 1;
            }
            else
            {
                FrequencyDictionary[val2]++;
            }
        }

        side1List.Sort();
        side2List.Sort();

        int diff = 0;

        if (IsPart2)
        {
            for (int i = 0; i < side1List.Count; i++)
            {
                int numOccurrences = 0;
                if (FrequencyDictionary.TryGetValue(side1List[i], out numOccurrences))
                {
                    diff += (side1List[i] * numOccurrences);
                }
            }
        }
        else
        {
            for (int i = 0; i < side1List.Count; i++)
            {
                diff += Math.Abs(side2List[i] - side1List[i]);
            }
        }

        DebugOutput("Total difference is  : " + Math.Abs(diff));

    }
}
