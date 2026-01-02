using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.Z3;
using Spectre.Console.Cli;

public class Test10_2025 : BaseTest
{
    public override void Initialise()
    {
        Year = 2025;
        TestID = 10;
    }

    public override void Execute()
    {
        List<Machine> machines = new List<Machine>();
        foreach (string line in m_dataFileContents)
        {
            Machine machine = Machine.FromLine(line, this);
            machines.Add(machine);
        }

        int total = 0;
        if (IsPart1)
        {
            foreach (Machine machine in machines)
            {
                total += machine.FindOptimum();
                break;
            }
        }
        else
        {
            List<JoltageState> visitedTest = new List<JoltageState>();
            visitedTest.Add(new JoltageState(new int[] { 1, 2, 3, 4, 5 }, 0));
            visitedTest.Add(new JoltageState(new int[] { 1, 2, 5 }, 2));

            bool contains1 = visitedTest.Contains(new JoltageState(new int[] { 1, 2, 3, 4, 5 }, 0));
            bool contains2 = visitedTest.Contains(new JoltageState(new int[] { 1, 2, 3, 4, 5 }, 2));
            bool contains3 = visitedTest.Contains(new JoltageState(new int[] { 1, 2, 4 }, 2));
            bool contains4 = visitedTest.Contains(new JoltageState(new int[] { 1, 2, 5 }, 2));

            int ibreak2 = 0;


        
            
            
            foreach (Machine machine in machines)
            {
                int[] joltage = new int[machine.Joltage.Count];
                

                PriorityQueue<JoltageState, int> workQueue = new PriorityQueue<JoltageState, int>();
                List<JoltageState> visited = new List<JoltageState>();
                workQueue.Enqueue(new JoltageState(new int[machine.Joltage.Count], 0), 1);
                machine.FindPart2(workQueue,visited);                

                total += machine.BestDepth;
                DebugOutput($"Found value : {machine.BestDepth}");
            }
        }

        DebugOutput($"Final result is {total}");

        int ibreak = 0;
    }

    public class Machine
    {
        public char[] LightsMatch = null;
        public char[] LightsCurrent = null;

        public List<int> Joltage = new List<int>();
        public List<int> CurrentJoltage = new List<int>();

        public List<List<int>> Wiring = new List<List<int>>();

        public BaseTest Test;

        public void Reset()
        {
            Array.Fill(LightsCurrent, '.');
            CurrentJoltage.Clear();
            foreach (int number in Joltage)
            {
                CurrentJoltage.Add(0);
            }
        }

        public void PushButton(int buttonIndex)
        {
            foreach (int number in Wiring[buttonIndex])
            {
                if (Test.IsPart1)
                {
                    ToggleLight(number);
                }
                else
                {
                    IncrementJoltage(number);
                }
            }
        }

        public void PushButton(int buttonIndex, int[] joltage)
        {
            foreach (int number in Wiring[buttonIndex])
            {
                joltage[number] += 1;
            }
        }

        public void IncrementJoltage(int number)
        {
            Joltage[number] += 1;
        }


        public void IncrementJoltage(int number, int[] joltage)
        {
            joltage[number] += 1;
        }


        public bool Matches()
        {
            if (Test.IsPart1)
            {
                return LightsCurrent.SequenceEqual(LightsMatch);
            }

            return CurrentJoltage.SequenceEqual(Joltage);
        }

        public void TestLinear()
        {
            Context context = new Context();
            
            List<IntExpr> buttons = new List<IntExpr>();
            for (int i = 0; i < Wiring.Count; i++)
            {
                buttons.Add(context.MkIntConst("B"+i));
            }

            for (int i = 0; i < Joltage.Count; i++)
            {
                for (int j = 0; j < Wiring[i].Count; j++)
                {
                    if (Wiring[i].Contains(i))
                    {
                        //context. Sum(buttons[j]);
                    }
                }
            }
            
            Optimize optimiser = context.MkOptimize();

            foreach (var button in buttons)
            {
                optimiser.Add(button >= 0);
            }

            
            //Sum()
            //optimiser.MkMinimize(z3.Sum(bs))
            //Debug.Assert(optimiser.Check() == Status.SATISFIABLE);

            //Model model = optimiser.Model();

        }
        
        
        
        public int FindOptimum()
        {
            int[] possibilities = new int[Wiring.Count];
            for (int i = 0; i < possibilities.Length; i++)
            {
                possibilities[i] = i;
            }

            int bestCount = Int32.MaxValue;
            int numPresses = 1;
            int maxPresses = 1000;

            for (int i = 0; i < maxPresses; i++)
            {
                foreach (var combination in Combinations.GetPermutations(possibilities, numPresses))
                {
                    if (numPresses == 10)
                    {
                        int ibreak = 0;
                    }

                    Reset();
                    int count = 0;
                    foreach (int button in combination)
                    {
                        count++;
                        if (count > bestCount)
                        {
                            break;
                        }

                        PushButton(button);
                        if (ShouldStop())
                        {
                            //break;
                        }
                    }


                    if (Matches())
                    {
                        if (count < bestCount)
                        {
                            bestCount = count;
                        }
                    }
                }

                numPresses++;
            }

            Test.DebugOutput($"Got optimal at {bestCount} presses");
            return bestCount;
        }


        public int BestDepth = Int32.MaxValue;

        public void FindPart2(PriorityQueue<JoltageState, int> workQueue, List<JoltageState> visitedStates)
        {
            int debugLimit = 100000;
            int iterCount = debugLimit;
            while (workQueue.Count > 0)
            {
                var currentState = workQueue.Dequeue();
                visitedStates.Add(currentState);
                iterCount--;
                if (iterCount == 0)
                {
                    iterCount = debugLimit;
                    Test.DebugOutput($"Work queue size {+workQueue.Count} quickest {BestDepth}");
                    return;
                }
                else
                {
                    for (int i = 0; i < Wiring.Count; i++)
                    {
                        int[] joltageCopy = new int[Joltage.Count];
                        Array.Copy(currentState.Joltage, joltageCopy, Joltage.Count);
                        PushButton(i, joltageCopy);
                        var newState = new JoltageState(joltageCopy, currentState.Depth + 1);

                        if (joltageCopy.SequenceEqual(Joltage))
                        {
                            if (newState.Depth < BestDepth)
                            {
                                BestDepth = newState.Depth;
                                return;
                            }
                        }
                        else
                        {

                            if (newState.Depth < BestDepth && !ExceedsJoltage(joltageCopy) &&
                                !visitedStates.Contains(newState))
                            {
                                // calc score for state,
                                int score = CalculateJoltageScore(joltageCopy);
                                workQueue.Enqueue(newState, score);
                            }
                        }
                    }
                }
            }
        }

        public int CalculateJoltageScore(int[] joltage)
        {
            if (ExceedsJoltage(joltage))
            {
                return int.MinValue;
            }

            int diff = 0;
            for (int i = 0; i < Joltage.Count; i++)
            {
                diff +=  100 - (Joltage[i]-joltage[i]);
                //diff += Joltage[i] - joltage[i];
            }

            return diff;
        }


        public bool ExceedsJoltage(int[] joltage)
        {
            for (int i = 0; i < Joltage.Count; i++)
            {
                if (joltage[i] > Joltage[i])
                {
                    return true;
                }
            }

            return false;
        }


        public bool ShouldStop()
        {
            if (Test.IsPart1)
            {
                return false;
            }

            for (int i = 0; i < Joltage.Count; i++)
            {
                if (CurrentJoltage[i] > Joltage[i])
                {
                    return true;
                }
            }

            return false;
        }

        public void ToggleLight(int index)
        {
            if (LightsCurrent[index] == '.')
            {
                LightsCurrent[index] = '#';
            }
            else
            {
                LightsCurrent[index] = '.';
            }
        }

        public static Machine FromLine(string line, BaseTest test)
        {
            Machine machine = new Machine();
            machine.Test = test;
            string lights = Helper.SubstringBetween(line, "[", "]");
            machine.LightsMatch = lights.ToCharArray();
            machine.LightsCurrent = new char[lights.Length];
            machine.Reset();


            string wiring = line.Substring(line.IndexOf('('), (line.LastIndexOf(')') - line.IndexOf('(')) + 1);

            string[] tokens = wiring.Split(' ');
            foreach (string token in tokens)
            {
                string[] numbers = token.Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                List<int> wires = new List<int>();
                foreach (string number in numbers)
                {
                    wires.Add(int.Parse(number));
                }

                machine.Wiring.Add(wires);
            }


            string joltage = Helper.SubstringBetween(line, "{", "}");

            tokens = joltage.Split(',');
            foreach (string token in tokens)
            {
                machine.Joltage.Add(int.Parse(token));
            }

            return machine;
        }
    }

}


public class JoltageState : IEquatable<JoltageState>
{
    public JoltageState(int[] joltage, int depth)
    {
        Joltage = joltage;
        Depth = depth;
    }

    public bool Equals(JoltageState? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Joltage.SequenceEqual(other.Joltage) && Depth == other.Depth;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((JoltageState)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Joltage, Depth);
    }

    public int[] Joltage;
    public int Depth;
}