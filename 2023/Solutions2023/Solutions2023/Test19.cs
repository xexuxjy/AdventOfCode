public class Test19 : BaseTest
{
    public override void Initialise()
    {
        TestID = 19;
        IsTestInput = true;
        IsPart2 = true;
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

        if (IsPart2)
        {
            long total = 0;

            // foreach (WorkFlow wf in workflows)
            // {
            //     SortItem minSortItem = new SortItem();
            //     SortItem maxSortItem = new SortItem();
            //
            //     wf.MinSortItem = minSortItem;
            //     wf.MaxSortItem = maxSortItem;
            //     
            //     for (int j = 0; j < minSortItem.Values.Length;j++)
            //     {
            //         minSortItem.Values[j] = SortItem.MinLimit;
            //         maxSortItem.Values[j] = SortItem.MaxLimit;
            //     }
            //
            //     long ruleTotal = 1;
            //     wf.AcceptedCombinations(minSortItem,maxSortItem,workflowDictionary);
            //     for (int k = 0; k < minSortItem.Values.Length; k++)
            //     {
            //         ruleTotal *= ((maxSortItem.Values[k] - minSortItem.Values[k]) + 1);
            //     }
            //
            //     total += ruleTotal;
            //     
            // }

            List<List<(Rule,bool)>> rulePaths = new List<List<(Rule,bool)>>();
            workflowDictionary["in"].BuildAcceptRules(rulePaths,null);


            foreach (List<(Rule,bool)> rules in rulePaths)
            {
                SortItem minSortItem = new SortItem();
                SortItem maxSortItem = new SortItem();

                for (int j = 0; j < minSortItem.Values.Length; j++)
                {
                    minSortItem.Values[j] = SortItem.MinLimit;
                    maxSortItem.Values[j] = SortItem.MaxLimit;
                }
                foreach (var r in rules)
                {
                    r.Item1.AcceptedCombinations(minSortItem, maxSortItem, workflowDictionary);
                }
                
                long ruleTotal = 1;
                 
                for (int k = 0; k < minSortItem.Values.Length; k++)
                {
                    ruleTotal *= ((maxSortItem.Values[k] - minSortItem.Values[k]) + 1);
                }

                total += ruleTotal; 
            }


            
            DebugOutput("Part 2 Total combinations : " + total);

        }
        else
        {
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

        
        
    }

    
    
    
    
    public record SortItem
    {
        public static int MinLimit = 1;
        public static int MaxLimit = 4000;
        
        // public int X;
        // public int M;
        // public int A;
        // public int S;

        public int[] Values = new int[4];

        public SortItem()
        {
        }

        public SortItem(string data)
        {
            data = data.Replace("}", "").Replace("}", "");
            string[] tokens = data.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            Values[0] = int.Parse(
                tokens[0].Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]);
            Values[1] = int.Parse(
                tokens[1].Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]);
            Values[2] = int.Parse(
                tokens[2].Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]);
            Values[3] = int.Parse(
                tokens[3].Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]);

        }

        public int Score
        {
            get
            {
                int total = 0;
                foreach (int val in Values)
                {
                    total += val;
                }

                return total;
            }
        }
    };



    public class WorkFlow
    {
        public string Id;
        private List<Rule> m_rules = new List<Rule>();
        private Dictionary<string, WorkFlow> m_workflowDictionary;
        private List<SortItem> m_acceptedList;

        public SortItem MinSortItem;
        public SortItem MaxSortItem;
        
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
                m_rules.Add(new Rule(token,this));
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

        public List<Rule> AcceptRules = new List<Rule>();

        public void BuildAcceptRules(List<List<(Rule,bool)>> rulePaths,List<(Rule,bool)> acceptRules)
        {
            List<(Rule,bool)> original = acceptRules;
           
            for (int i=0 ;i < m_rules.Count;++i)
            {
                Rule r = m_rules[i];
                if (r.Destination == "A")
                {
                    List<(Rule,bool)> newRules = new List<(Rule,bool)>();
                    if (acceptRules != null)
                    {
                        newRules.AddRange(acceptRules);
                    }
                    
                    if (r.LHS != null)
                    {
                        acceptRules.Add((r,true));    
                    }
                    // else
                    // {
                    //     acceptRules.Add((m_rules[i-1],false));
                    // }

                    
                    rulePaths.Add(acceptRules);

                    acceptRules = newRules;

                }
                else if (r.Destination == "R")
                {
                    
                }
                else
                {
                    List<(Rule,bool)> newRules = new List<(Rule,bool)>();
                    if (acceptRules != null)
                    {
                        newRules.AddRange(acceptRules);
                    }

                    if (r.LHS != null)
                    {
                        newRules.Add((r,true));    
                    }
                    else
                    {
                        newRules.Add((m_rules[i-1],false));
                    }
                    
                    
                    m_workflowDictionary[r.Destination].BuildAcceptRules(rulePaths,newRules);
                }
            }
            
        }
        



        public void AcceptedCombinations(SortItem minSortItem,SortItem maxSortItem,Dictionary<string,WorkFlow> workflows)
        {
            foreach (Rule rule in m_rules)
            {
                rule.AcceptedCombinations(minSortItem, maxSortItem,workflows);
            }
        }

        // public void AcceptRules(List<Rule> acceptRules)
        // {
        //     foreach (Rule rule in m_rules)
        //     {
        //         if (rule.Destination == "A")
        //         {
        //             acceptRules.Add(rule);
        //         }
        //     }
        // }
        
    }

    public class Rule
    {
        private string m_text;
        private string m_lhs;
        private int m_rhs;
        private string m_comparator;
        private string m_destination;
        private WorkFlow m_workflow;
        
        public string Destination => m_destination;
        public string LHS => m_lhs;
        public int RHS => m_rhs;

        public string COMP => m_comparator;
        
        public Rule(string ruleText,WorkFlow workFlow)
        {
            m_text = ruleText;
            m_workflow = workFlow;
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

        public override string ToString()
        {
            return m_text;
        }

        public void AcceptedCombinations(SortItem minSortItem,SortItem maxSortItem,Dictionary<string,WorkFlow> workflows)
        {
            int valueIndex = 0;
                
            if (m_lhs == "x")
            {
                valueIndex = 0;
            }
            else if (m_lhs == "m")
            {
                valueIndex = 1;
                    
            }
            else if (m_lhs == "a")
            {
                valueIndex = 2;
                    
            }
            else if (m_lhs == "s")
            {
                valueIndex = 3;
            }

            
            if (m_destination == "A")
            {
                if (m_comparator == "<")
                {
                    if (maxSortItem.Values[valueIndex] > m_rhs - 1 )
                    {
                        maxSortItem.Values[valueIndex] = m_rhs - 1;
                    }
                }
                else if (m_comparator == ">")
                {
                    if (minSortItem.Values[valueIndex] < m_rhs + 1)
                    {
                        minSortItem.Values[valueIndex] = m_rhs + 1;
                    }
                }
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
            if (m_lhs == "x") compareValue = sortItem.Values[0];
            if (m_lhs == "m") compareValue = sortItem.Values[1];
            if (m_lhs == "a") compareValue = sortItem.Values[2];
            if (m_lhs == "s") compareValue = sortItem.Values[3];


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