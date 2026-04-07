using System;
using System.Collections.Generic;

public class Test7_2018 : BaseTest
{
    Dictionary<string, SleighNode> NodeDictionary = new Dictionary<string, SleighNode>();

    public override void Initialise()
    {
        Year = 2018;
        TestID = 7;
    }

    public override void Execute()
    {

        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            SleighNode current = GetSleighNode(tokens[7]);
            SleighNode requires = GetSleighNode(tokens[1]);

            current.Requires.Add(requires);
        }

        // find the node that has no requirements.

        List<SleighNode> completeNodes = new List<SleighNode>();

        
        List<WorkerElf> workerElves = new List<WorkerElf>();
        int numElves = 5;
        for (int i = 0; i < numElves; i++)
        {
            WorkerElf workerElf = new WorkerElf();
            workerElf.Id = "Worker-" + i;
            workerElf.Available = true;
            workerElves.Add(workerElf);
        }

        int stepCount = 0;
        
        while (completeNodes.Count < NodeDictionary.Count)
        {
            if (IsPart1)
            {
                foreach (string key in NodeDictionary.Keys.OrderBy(k => k))
                {
                    SleighNode current = GetSleighNode(key);
                    if (!completeNodes.Contains(current))
                    {
                        bool allComplete = true;
                        foreach (SleighNode requires in current.Requires)
                        {
                            if (!completeNodes.Contains(requires))
                            {
                                allComplete = false;
                                break;
                            }
                        }

                        if (allComplete)
                        {
                            completeNodes.Add(current);
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (string key in NodeDictionary.Keys.OrderBy(k => k))
                {
                    SleighNode current = GetSleighNode(key);
                    if (!completeNodes.Contains(current) && current.AssignedTo == null)
                    {
                        bool allComplete = true;
                        foreach (SleighNode requires in current.Requires)
                        {
                            if (!completeNodes.Contains(requires))
                            {
                                allComplete = false;
                                break;
                            }
                        }

                        if (allComplete)
                        {
                            foreach (WorkerElf workerElf in workerElves)
                            {
                                if (workerElf.Available)
                                {
                                    workerElf.AssignJob(current);
                                    break;
                                }
                            }
                        }
                    }
                }

                foreach (WorkerElf workerElf in workerElves)
                {
                    workerElf.Tick(completeNodes);
                }
            }
            stepCount++;
        }

        int ibreak = 0;

        string joined = "";
        foreach (SleighNode node in completeNodes)
        {
            joined += node.ToString();
        }
        DebugOutput($"The order is {joined}");
        if (IsPart2)
        {
            DebugOutput($"Complete in {stepCount} steps");
        }

    }

    public SleighNode GetSleighNode(string id)
    {
        if (!NodeDictionary.TryGetValue(id, out SleighNode sleighNode))
        {
            sleighNode = new SleighNode();
            sleighNode.Id = id;
            NodeDictionary.Add(id, sleighNode);
        }
        return sleighNode;
    }
    
}

public class SleighNode
{
    public string Id;

    public WorkerElf AssignedTo;
    
    public List<SleighNode> Requires = new List<SleighNode>();

    public override string ToString()
    {
        return Id;
    }
    
}

public class WorkerElf
{
    public string Id;
    public SleighNode CurrentJob;
    public bool Available;
    private int Timer;

    public override string ToString()
    {
        return Id;
    }
    
    public void AssignJob(SleighNode job)
    {
        //Assert(CurrentJob== null);
        CurrentJob = job;
        CurrentJob.AssignedTo = this;
        Timer = (job.Id[0] - 'A')+1;
        Timer += 60;
        Available = false;
    }

    
    public void Tick(List<SleighNode> completedJobs)
    {
        if (Timer > 0)
        {
            Timer--;
        }

        if (Timer == 0)
        {
            if (CurrentJob != null)
            {
                
                completedJobs.Add(CurrentJob);
                CurrentJob = null;
            }
            Available = true;
        }
    }
    
}