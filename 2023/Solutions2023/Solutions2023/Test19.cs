public class Test19 : BaseTest
{
    public override void Initialise()
    {
        TestID = 19;
        IsTestInput = false;
        IsPart2 = false;
    }

    public override void Execute()
    {
        Dictionary<string, WorkFlow> workflowDictionary = new Dictionary<string, WorkFlow>();
        List<SortItem> acceptList = new List<SortItem>();

        List<WorkFlow> workflows = new List<WorkFlow>();
        int i = 0;
        for (i = 0; i < m_dataFileContents.Count; ++i)
        {
            if (m_dataFileContents[i] == "")
            {
                break;
            }
            workflows.Add(new WorkFlow(m_dataFileContents[i],workflowDictionary,acceptList));
        }

        i += 1;
        List<SortItem> items = new List<SortItem>();
        for (; i < m_dataFileContents.Count; ++i)
        {
            SortItem item = new SortItem(m_dataFileContents[i]);
            items.Add(item);
        }

        WorkFlow start = workflowDictionary["in"];

        foreach (SortItem sortItem in items)
        {
            start.Process(sortItem);
        }

        int total = acceptList.Sum(x => x.Score);
        DebugOutput("Accept list sum is : " + total);


    }

    public record SortItem
    {
        public int X;
        public int M;
        public int A;
        public int S;

        public SortItem(string data)
        {
            data = data.Replace("}", "").Replace("}", "");
            string[] tokens = data.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            X = int.Parse(
                tokens[0].Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]);
            M = int.Parse(
                tokens[1].Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]);
            A = int.Parse(
                tokens[2].Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]);
            S = int.Parse(
                tokens[3].Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]);

        }

        public int Score
        {
            get { return X + M + A + S; }
        }
    };



    public class WorkFlow
    {
        public string Id;
        private List<Rule> m_rules = new List<Rule>();
        private Dictionary<string, WorkFlow> m_workflowDictionary;
        private List<SortItem> m_acceptedList; 
        public WorkFlow(string data,Dictionary<string,WorkFlow> workflowDictionary,List<SortItem> acceptedList)
        {
            Id = data.Substring(0, data.IndexOf("{"));
            workflowDictionary[Id] = this;
            m_workflowDictionary = workflowDictionary;
            m_acceptedList = acceptedList;
            
            string ruleText = data.Substring(data.IndexOf("{") + 1);
            ruleText = ruleText.Substring(0,ruleText.IndexOf("}"));
            string[] tokens =
                ruleText.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            foreach (string token in tokens)
            {
                m_rules.Add(new Rule(token));
            }
        }

        public void Process(SortItem sortItem)
        {
            foreach (Rule rule in m_rules)
            {
                string destination;
                if (rule.Process(sortItem, out destination))
                {
                    if (destination == "A")
                    {
                        // accepted
                        m_acceptedList.Add(sortItem);
                    }
                    else if (destination == "R")
                    {
                        // rejected
                    }
                    else
                    {
                        m_workflowDictionary[destination].Process(sortItem);
                    }
                    break;
                }
            }
        }
        
    }

    public  class Rule
    {
        private string m_lhs;
        private int m_rhs;
        private string m_comparator;
        private string m_destination;
        
        public Rule(string ruleText)
        {
            int index = ruleText.IndexOf("<");
            if(index == -1)
            {
                index = ruleText.IndexOf(">");
            }

            if (index != -1)
            {
                m_comparator = ruleText.Substring(index, 1);
                m_lhs = ruleText.Substring(0, index );
                
                
                string temp  = ruleText.Substring(index+1);
                string[] temp2 = temp.Split(":",
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                m_rhs = int.Parse(temp2[0]);
                m_destination = temp2[1];
                
                int ibreak = 0;

            }
            else
            {
                m_destination = ruleText;

            }
            
        }

        public bool Process(SortItem sortItem,out string destination)
        {
            // default / last case.
            if (m_lhs == null)
            {
                destination = m_destination;
                return true;
            }
            
            
            int compareValue=0;
            if (m_lhs == "x") compareValue = sortItem.X;
            if (m_lhs == "m") compareValue = sortItem.M;
            if (m_lhs == "a") compareValue = sortItem.A;
            if (m_lhs == "s") compareValue = sortItem.S;


            if (m_comparator == "<")
            {
                if (compareValue < m_rhs)
                {
                    destination = m_destination;
                    return true;
                }
            }
            else if (m_comparator == ">")
            {
                if (compareValue > m_rhs)
                {
                    destination = m_destination;
                    return true;
                }
            }

            destination = "";
            return false;

        }
        
    }
    
    
}