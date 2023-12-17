using System.Text;

public class Test15 : BaseTest
{
    public List<Box> m_boxList = new List<Box>();

    public override void Initialise()
    {
        TestID = 15;
        IsTestInput = false;
        IsPart2 = true;
    }

    public int GenerateHASH(string val)
    {
        int total = 0;
        foreach (char c in val)
        {
            int asciiVal = (int)c;
            total += c;
            total *= 17;
            total = total % 256;
        }

        return total;
    }

    public override void Execute()
    {
        for (int i = 0; i < 256; ++i)
        {
            m_boxList.Add(new Box(i));
        }

        if (IsPart2)
        {
            Part2();
        }
        else
        {
            Part1();
        }
    }

    public void Part1()
    {
        long finalTotal = 0;
        List<string> parseResuls = new List<string>();
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            foreach (string token in tokens)
            {
                finalTotal += GenerateHASH(token);
            }
        }

        DebugOutput("Final total is " + finalTotal);
        
    }

    public void Part2()
    {
        List<string> parseResuls = new List<string>();
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            foreach (string token in tokens)
            {
                int hashValue = GenerateHASH(token);
                bool remove = token.Last() == '-';
                if (remove)
                {
                    string lensId = token.Substring(0, token.Length - 1);
                    int boxId = GenerateHASH(lensId);
                    m_boxList[boxId].RemoveLens(lensId);
                }
                else
                {
                    string[] token2 = token.Split('=');
                    string lensId = token2[0];
                    int focalLength = int.Parse(token2[1]);

                    int boxId = GenerateHASH(lensId);
                    m_boxList[boxId].AddLens(lensId,focalLength);

                }
            }
        }

        int total = 0;
        for (int i = 0; i < m_boxList.Count; ++i)
        {
            total += m_boxList[i].FocusingPower();
            // Box b = m_boxList[i];
            // if (b.HasLenses)
            // {
            //     DebugOutput($"Box {i} has {b.DebugInfo()}");
            // }
        }

        DebugOutput("Full focusing power is : " + total);
    }
    
    
    public class Box
    {
        private int m_id;
        public Box(int id)
        {
            m_id = id;
        }
        
        private List<Tuple<string,int>> m_lenses = new List<Tuple<string,int>>();

        public void AddLens(string lensId,int focalLength)
        {
            int existingIndex = m_lenses.FindIndex(x => x.Item1 == lensId);
            if (existingIndex != -1)
            {
                m_lenses[existingIndex] = new Tuple<string, int>(lensId, focalLength);
            }
            else
            {
                m_lenses.Add(new Tuple<string, int>(lensId, focalLength));
            }
        }

        public void RemoveLens(string lensId)
        {
            m_lenses.RemoveAll(x => x.Item1 == lensId);
        }

        public bool HasLenses => m_lenses.Count() > 0;

        public int FocusingPower()
        {
            int total = 0;
            for (int lensId = 0; lensId < m_lenses.Count; ++lensId)
            {
                int lensTotal = m_id + 1;
                lensTotal *= (lensId + 1);
                lensTotal *= m_lenses[lensId].Item2;

                total += lensTotal;
            }
            return total;
        }
        
        public string DebugInfo()
        {
            return string.Join(',',m_lenses);
        }
        
    }
    
    
    
}