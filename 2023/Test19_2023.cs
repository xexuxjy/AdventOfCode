public class Test19_2023 : BaseTest
{
    public override void Initialise()
    {
        Year = 2023;
        TestID = 19;
        IsTestInput = false;
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

            workflows.Add(new WorkFlow(m_dataFileContents[i], workflowDictionary, acceptList));
        }

        if (IsPart2)
        {
            long total = 0;

            BinaryNode<Rule> rootNode = WorkFlow.BuildTree(workflowDictionary["in"], 0, workflowDictionary);


            List<(Rule, bool)> start = null;
            List<List<(Rule, bool)>> rulePaths = new List<List<(Rule, bool)>>();

            WorkFlow.BuildAcceptRulesTree(rootNode, rulePaths, null);

            foreach (List<(Rule, bool)> rules in rulePaths)
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
                    UpdateCombinations(r, minSortItem, maxSortItem);
                }

                long ruleTotal = 1;

                for (int k = 0; k < minSortItem.Values.Length; k++)
                {
                    ruleTotal *= ((maxSortItem.Values[k] - minSortItem.Values[k]) + 1);

                }
                total += ruleTotal;
                //string rangeInfo = $"Min(X : {minSortItem.Values[0]}  M : {minSortItem.Values[1]}  A : {minSortItem.Values[2]}  S : {minSortItem.Values[3]})  Max(X : {maxSortItem.Values[0]}  M : {maxSortItem.Values[1]}  A : {maxSortItem.Values[2]}  S : {maxSortItem.Values[3]})";
                //DebugOutput($"rangeProduct {ruleTotal}   sum {total} "+rangeInfo);
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


    public void UpdateCombinations((Rule, bool) ruleData, SortItem minItem, SortItem maxItem)
    {
        if (ruleData.Item1.LHS != null)
        {
            int valueIndex = Rule.GetValueIndex(ruleData.Item1.LHS);

            if (ruleData.Item2)
            {
                if (ruleData.Item1.COMP == "<")
                {
                    maxItem.Values[valueIndex] = Int32.Min(maxItem.Values[valueIndex], ruleData.Item1.RHS - 1);
                }
                else if (ruleData.Item1.COMP == ">")
                {
                    minItem.Values[valueIndex] = Int32.Max(minItem.Values[valueIndex], ruleData.Item1.RHS + 1);
                }
            }
            else
            {
                if (ruleData.Item1.COMP == "<")
                {
                    minItem.Values[valueIndex] = ruleData.Item1.RHS;
                }
                else if (ruleData.Item1.COMP == ">")
                {
                    maxItem.Values[valueIndex] = ruleData.Item1.RHS;
                }
            }

        }
    }

    public record SortItem
    {
        public static int MinLimit = 1;
        public static int MaxLimit = 4000;

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

        public WorkFlow(string data, Dictionary<string, WorkFlow> workflowDictionary, List<SortItem> acceptedList)
        {
            Id = data.Substring(0, data.IndexOf("{"));
            workflowDictionary[Id] = this;
            m_workflowDictionary = workflowDictionary;
            m_acceptedList = acceptedList;

            string ruleText = data.Substring(data.IndexOf("{") + 1);
            ruleText = ruleText.Substring(0, ruleText.IndexOf("}"));
            string[] tokens =
                ruleText.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            foreach (string token in tokens)
            {
                m_rules.Add(new Rule(token, this));
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


        private static BinaryNode<Rule> acceptRule = new BinaryNode<Rule>() { Data = new Rule("A", null) };
        private static BinaryNode<Rule> rejectRule = new BinaryNode<Rule>() { Data = new Rule("R", null) };

        public static BinaryNode<Rule> BuildTree(WorkFlow wf, int ruleIndex,
            Dictionary<string, WorkFlow> workflowDictionary)
        {
            BinaryNode<Rule> newNode = new BinaryNode<Rule>()
            {
                Data = wf.m_rules[ruleIndex]
            };


            if (newNode.Data.Destination == "A")
            {
                newNode.Left = acceptRule;
            }
            else if (newNode.Data.Destination == "R")
            {
                newNode.Left = rejectRule;
            }
            else
            {
                newNode.Left = BuildTree(workflowDictionary[newNode.Data.Destination], 0, workflowDictionary);
            }


            if (ruleIndex < wf.m_rules.Count - 1)
            {
                var nextRule = wf.m_rules[ruleIndex + 1];
                newNode.Right = BuildTree(wf, ruleIndex + 1, workflowDictionary);
            }

            return newNode;
        }


        public static void
            BuildAcceptRulesTree(BinaryNode<Rule> node, List<List<(Rule, bool)>> rulePaths,
                List<(Rule, bool)> acceptRules)
        {
            if (acceptRules == null)
            {
                acceptRules = new List<(Rule, bool)>();
            }

            if (node == acceptRule)
            {
                rulePaths.Add(acceptRules);
            }
            else if (node == rejectRule)
            {
            }
            else
            {
                if (node.Left != null)
                {
                    List<(Rule, bool)> lhs = new List<(Rule, bool)>();
                    lhs.AddRange(acceptRules);
                    lhs.Add((node.Data, true));
                    BuildAcceptRulesTree(node.Left, rulePaths, lhs);
                }

                if (node.Right != null)
                {
                    List<(Rule, bool)> rhs = new List<(Rule, bool)>();
                    rhs.AddRange(acceptRules);
                    rhs.Add((node.Data, false));
                    BuildAcceptRulesTree(node.Right, rulePaths, rhs);
                }
            }
        }


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

        public Rule(string ruleText, WorkFlow workFlow)
        {
            m_text = ruleText;
            m_workflow = workFlow;
            int index = ruleText.IndexOf("<");
            if (index == -1)
            {
                index = ruleText.IndexOf(">");
            }

            if (index != -1)
            {
                m_comparator = ruleText.Substring(index, 1);
                m_lhs = ruleText.Substring(0, index);


                string temp = ruleText.Substring(index + 1);
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

        public static int GetValueIndex(string val)
        {
            if (val == "x")
            {
                return 0;
            }
            else if (val == "m")
            {
                return 1;
            }
            else if (val == "a")
            {
                return 2;
            }
            else if (val == "s")
            {
                return 3;
            }

            return 0;
        }

        public void AcceptedCombinations(SortItem minSortItem, SortItem maxSortItem,
            Dictionary<string, WorkFlow> workflows)
        {
            int valueIndex = GetValueIndex(m_lhs);

            if (m_destination == "A")
            {
                if (m_comparator == "<")
                {
                    if (maxSortItem.Values[valueIndex] > m_rhs - 1)
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


        public bool Process(SortItem sortItem, out string destination)
        {
            // default / last case.
            if (m_lhs == null)
            {
                destination = m_destination;
                return true;
            }


            int compareValue = 0;
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