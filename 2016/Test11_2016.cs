using System;
using System.Collections.Generic;
using System.Xml;
using CompactPack.Packers;
using Newtonsoft.Json.Linq;

public class Test11_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 11;
    }

    public static HashSet<CacheState> Cache = new HashSet<CacheState>();

    public readonly static string EMPTY = ". ";
    public readonly static int NumFloors = 4;
    public static int MaxDepth = 12;

    public static int QuickestMove = int.MaxValue;

    public static List<string> SlotNames = new List<string>();

    public static List<(List<int>, bool)> BestMoveList = new List<(List<int>, bool)>();

    public static Bit64Packer FloorPacker = new Bit64Packer();

    
    public override void Execute()
    {
        int elevatorFloor = 0;

        if (IsTestInput)
        {
            SlotNames.Clear();
            SlotNames.Add("HG");
            SlotNames.Add("HM");
            SlotNames.Add("LG");
            SlotNames.Add("LM");

            FloorPacker = new Bit64Packer();
            for (int i = 0; i < SlotNames.Count; i++)
            {
                FloorPacker.AddField("Slot"+i, 2);
            }

            int count = 0;
            FloorPacker.SetValue("Slot" + (count++), 1);
            FloorPacker.SetValue("Slot" + (count++), 0);
            FloorPacker.SetValue("Slot" + (count++), 2);
            FloorPacker.SetValue("Slot" + (count++), 0);
        }
        else
        {
            MaxDepth = 30;
            SlotNames.Clear();
            SlotNames.Add("PoG");
            SlotNames.Add("PoM");
            SlotNames.Add("ThG");
            SlotNames.Add("ThM");
            SlotNames.Add("PrG");
            SlotNames.Add("PrM");
            SlotNames.Add("RuG");
            SlotNames.Add("RuM");
            SlotNames.Add("CoG");
            SlotNames.Add("CoM");

            if (IsPart2)
            {
                SlotNames.Add("EG");
                SlotNames.Add("EM");
                SlotNames.Add("DG");
                SlotNames.Add("DM");
            }
            
            
            FloorPacker = new Bit64Packer();
            for (int i = 0; i < SlotNames.Count; i++)
            {
                FloorPacker.AddField("Slot"+i, 2);
            }

            
            // SlotPositions = new int[SlotNames.Count];
            // int count = 0;
            // SlotPositions[count++] = 0;
            // SlotPositions[count++] = 1;
            // SlotPositions[count++] = 0;
            // SlotPositions[count++] = 0;
            // SlotPositions[count++] = 0;
            // SlotPositions[count++] = 1;
            // SlotPositions[count++] = 0;
            // SlotPositions[count++] = 0;
            // SlotPositions[count++] = 0;
            // SlotPositions[count++] = 0;
            
            int count = 0;
            
            FloorPacker.SetValue("Slot" + (count++), 0);
            FloorPacker.SetValue("Slot" + (count++), 1);
            FloorPacker.SetValue("Slot" + (count++), 0);
            FloorPacker.SetValue("Slot" + (count++), 0);
            FloorPacker.SetValue("Slot" + (count++), 0);
            FloorPacker.SetValue("Slot" + (count++), 1);
            FloorPacker.SetValue("Slot" + (count++), 0);
            FloorPacker.SetValue("Slot" + (count++), 0);
            FloorPacker.SetValue("Slot" + (count++), 0);
            FloorPacker.SetValue("Slot" + (count++), 0);

            
        }


        
        FloorState initialState = new FloorState(elevatorFloor, FloorPacker.Pack());

        var allUpCombos = BuildAllCombinations(SlotNames.Count,true);
        var allDownCombos = BuildAllCombinations(SlotNames.Count,false);
        
        List<(List<int>, bool)> moveList = new List<(List<int>, bool)>();

        //GenerateMovesDepth(initialState, allUpCombos,allDownCombos, 0, moveList);
        //GenerateMovesBreadth(initialState, allUpCombos,allDownCombos, 0, moveList);
        PriorityQueue<FloorState,int> workQueue = new PriorityQueue<FloorState,int>();
        List<FloorState> visitedList = new List<FloorState>();
        
        workQueue.Enqueue(initialState, 0);
        TestBF(workQueue, visitedList ,allUpCombos, allDownCombos,0,moveList);
        
        // FloorState currentState = initialState;
        // currentState = currentState.MoveItems(true, new int[] { 1 }.ToList());
        // DebugState(currentState);
        // currentState = currentState.MoveItems(true, new int[] { 0,1 }.ToList());
        // DebugState(currentState);
        // currentState = currentState.MoveItems(false, new int[] { 1 }.ToList());
        // DebugState(currentState);
        // currentState = currentState.MoveItems(false, new int[] { 1 }.ToList());
        // DebugState(currentState);
        // currentState = currentState.MoveItems(true, new int[] { 1,3 }.ToList());
        // DebugState(currentState);
        // currentState = currentState.MoveItems(true, new int[] { 1,3 }.ToList());
        // DebugState(currentState);
        // currentState = currentState.MoveItems(true, new int[] { 1,3 }.ToList());
        // DebugState(currentState);
        // currentState = currentState.MoveItems(false, new int[] { 1 }.ToList());
        // DebugState(currentState);
        // currentState = currentState.MoveItems(true, new int[] { 0,2 }.ToList());
        // DebugState(currentState);
        // currentState = currentState.MoveItems(false, new int[] { 3 }.ToList());
        // DebugState(currentState);
        // currentState = currentState.MoveItems(true, new int[] { 1,3 }.ToList());
        // DebugState(currentState);
        //
        //
        //
        // DebugOutput($"Final state valid is : {ValidState(currentState)} Goal {GoalState(currentState)}");
        // List<List<int>> moves = new List<List<int>>();
        // List<bool> directions = new List<bool>();

        // moves.Add((new int[] { 1 }.ToList()));
        // moves.Add((new int[] { 0, 1 }.ToList()));
        // moves.Add((new int[] { 1 }.ToList()));
        // moves.Add((new int[] { 1 }.ToList()));
        // moves.Add((new int[] { 1, 3 }.ToList()));
        // moves.Add((new int[] { 1, 3 }.ToList()));
        // moves.Add((new int[] { 1, 3 }.ToList()));
        // moves.Add((new int[] { 1 }.ToList()));
        // moves.Add((new int[] { 0, 2 }.ToList()));
        // moves.Add((new int[] { 3 }.ToList()));
        // moves.Add((new int[] { 1, 3 }.ToList()));
        //
        // directions.Add(true);
        // directions.Add(true);
        // directions.Add(false);
        // directions.Add(false);
        // directions.Add(true);
        // directions.Add(true);
        // directions.Add(true);
        // directions.Add(false);
        // directions.Add(true);
        // directions.Add(false);
        // directions.Add(true);

        DebugState(initialState);


        DebugOutput($"Quickest move was {QuickestMove}");
        // foreach (var item in BestMoveList)
        // {
        //     DebugOutput($"{string.Join(',', item.Item1)},{item.Item2}");
        // }
        // //
        //  DebugOutput("Replaying Moves");
        // // DebugState(initialState);
        //  GenerateMovesPlanned(initialState, BestMoveList, 0);


        int ibreak2 = 0;
        
        
    }

    public void GenerateMovesPlanned(FloorState currentState, List<(List<int>, bool)> moveList, int depth)
    {
        if (depth >= moveList.Count)
        {
            return;
        }

        FloorState newState = currentState.MoveItems(moveList[depth].Item2, moveList[depth].Item1);
        if (newState != null)
        {
            if (ValidState(newState))
            {
                DebugState(newState);
                GenerateMovesPlanned(newState, moveList, depth + 1);
            }
        }
        else
        {
            DebugOutput(
                $"Invalid state on planned move : {string.Join(',', moveList[depth].Item1)} ,{moveList[depth].Item2}");
            DebugState(currentState);
        }
    }


    public void GenerateMovesDepth(FloorState currentState, List<List<int>> upCombinations,List<List<int>> downCombinations, int depth,
        List<(List<int>, bool)> moveList)
    {
        if (depth > MaxDepth || QuickestMove < depth)
        {
            return;
        }

        CacheState cacheState = new CacheState(currentState);
        
        if (!Cache.Contains(cacheState))
        {
            Cache.Add(cacheState);
            for (int i = 0; i < upCombinations.Count; i++)
            {
                FloorState newState = currentState.MoveItems(true, upCombinations[i]);

                if (newState != null && ValidState(newState))
                {
                    if (GoalState(newState))
                    {
                        if (depth < QuickestMove)
                        {
                            QuickestMove = depth;
                            BestMoveList.Clear();
                            BestMoveList.AddRange(moveList);
                            BestMoveList.Add((upCombinations[i], true));

                            // DebugOutput("Quickest Move : "+QuickestMove);
                            // DebugState(newState);
                            // foreach (var item in BestMoveList)
                            // {
                            //     DebugOutput($"{string.Join(',',item.Item1)},{item.Item2}");
                            // }
                        }
                    }
                    else
                    {
                        moveList.Add((upCombinations[i], true));
                        GenerateMovesDepth(newState, upCombinations,downCombinations, depth + 1, moveList);
                        moveList.RemoveAt(moveList.Count - 1);
                    }
                }

                if (QuickestMove < depth)
                {
                    return;
                }

                newState = currentState.MoveItems(false, downCombinations[i]);
                if (newState != null && ValidState(newState))
                {
                    if (GoalState(newState))
                    {
                        if (depth < QuickestMove)
                        {
                            QuickestMove = depth;
                            BestMoveList.Clear();
                            BestMoveList.AddRange(moveList);
                            BestMoveList.Add((downCombinations[i], false));
                            // DebugOutput("Quickest Move : "+QuickestMove);
                            // DebugState(newState);
                            // foreach (var item in BestMoveList)
                            // {
                            //     DebugOutput($"{string.Join(',',item.Item1)},{item.Item2}");
                            // }
                        }
                    }
                    else
                    {
                        moveList.Add((downCombinations[i], false));
                        GenerateMovesDepth(newState, upCombinations,downCombinations, depth + 1, moveList);
                        moveList.RemoveAt(moveList.Count - 1);
                    }
                }

                if (QuickestMove < depth)
                {
                    return;
                }
            }
        }
    }

    public void TestBF(PriorityQueue<FloorState,int> workQueue, List<FloorState> visitedStates, List<List<int>> upCombinations, List<List<int>> downCombinations,
        int depth, List<(List<int>, bool)> moveList)
    {
        int debugLimit = 1000;
        int iterCount = debugLimit;
        while(workQueue.Count > 0)
        {
            FloorState currentState = workQueue.Dequeue();
            visitedStates.Add(currentState);
            
            iterCount--;
            if (iterCount == 0)
            {
                iterCount = debugLimit;
                DebugOutput($"Work queue size {+workQueue.Count} quickest {QuickestMove}");
                
            }
            
            
            if (GoalState(currentState) && currentState.MoveCount < QuickestMove)
            {
                //DebugOutput("Moves to reach goal state is  " + currentState.MoveCount);
                DebugState(currentState);
                QuickestMove = currentState.MoveCount;
                return;
            }
            else
            {
                for (int i = 0; i < upCombinations.Count; i++)
                {
                    FloorState newState = currentState.MoveItems(true, upCombinations[i]);

                    if (newState != null && newState.MoveCount < QuickestMove && ValidState(newState) && !visitedStates.Contains(newState))
                    {
                        // calc score for state,
                        int score = FloorStateScore(newState);
                        workQueue.Enqueue(newState, score);
                    }

                    newState = currentState.MoveItems(false, downCombinations[i]);
                    if (newState != null && newState.MoveCount < QuickestMove && ValidState(newState) && !visitedStates.Contains(newState))
                    {
                        // calc score for state,
                        int score = FloorStateScore(newState);
                        workQueue.Enqueue(newState, score);
                    }
                }
            }
            //TestBF(workQueue,visitedStates,upCombinations,downCombinations,depth,moveList);
        }
    }
    
    
    
    public List<List<int>> BuildAllCombinations(int slots,bool favourUp)
    {
        List<int> t = new List<int>();
        List<List<int>> resultList = new List<List<int>>();
        // single moves
        for (int i = 0; i < slots; i++)
        {
            t.Add(i);
        }

        if (favourUp)
        {

            foreach (var combo in Combinations.GetPermutations(t, 2))
            {
                resultList.Add(combo.ToList());
            }

            foreach (var combo in Combinations.GetPermutations(t, 1))
            {
                resultList.Add(combo.ToList());
            }
        }
        else
        {
            foreach (var combo in Combinations.GetPermutations(t, 1))
            {
                resultList.Add(combo.ToList());
            }            
            foreach (var combo in Combinations.GetPermutations(t, 2))
            {
                resultList.Add(combo.ToList());
            }
        }


        return resultList;
    }

    public int FloorStateScore(FloorState state)
    {
        int score = 0;
        for (int i = 0; i < SlotNames.Count; i++)
        {
            int slotScore = FloorPacker.GetValueAsInt("Slot" + i); 
            score += (NumFloors - 1) - slotScore;
        }

        // weight things on position then move.
        // score *= 10;
        // score += state.MoveCount;
        
        return score;
    }

    public bool GoalState(FloorState state)
    {
        FloorPacker.Unpack(state.FloorsUL);
        bool valid = true;

        for (int i = 0; i < SlotNames.Count; i++)
        {
            if (FloorPacker.GetValueAsInt("Slot" + i) != NumFloors - 1)
            {
                valid = false;
                break;
            }
        }

        return valid;
    }

    public bool ValidState(FloorState state, bool currentFloorOnly = true)
    {
        return ValidState2(state, currentFloorOnly);
    }

    public bool ValidState2(FloorState state, bool currentFloorOnly = true)
    {
        if (state == null)
        {
            return false;
        }

        bool valid = true;


        for (int floorNum = 0; floorNum < NumFloors; floorNum++)
        {
            if (currentFloorOnly && floorNum != state.ElevatorFloor)
            {
                continue;
            }

            int chipsOnFloor = 0;
            int generatorsOnFloor = 0;
            
            int pairedChips = 0;
            int pairedGenerators = 0;

            FloorPacker.Unpack(state.FloorsUL);
            
            for (int i = 0; i < SlotNames.Count; i += 2)
            {
                int genPos = FloorPacker.GetValueAsInt("Slot" + i);
                int chipPos = FloorPacker.GetValueAsInt("Slot" + (i+1));
                
                if(genPos == floorNum)
                {
                    generatorsOnFloor++;
                }
                if (chipPos == floorNum)
                {
                    chipsOnFloor++;
                }
                
                if (genPos == floorNum && chipPos == floorNum)
                {
                    pairedGenerators++;
                    pairedChips++;
                }

            }

            if(chipsOnFloor > 0 &&  generatorsOnFloor > 0 && pairedChips != chipsOnFloor)
            {
                return false;
            }
        }

        return valid;
    }


    public void DebugState(FloorState floorState)
    {
        if (floorState == null)
        {
            return;
        }

        List<string> results = new List<string>();

        for (int floorNum = 0; floorNum < NumFloors; floorNum++)
        {
            string info = "F" + (floorNum + 1) + " ";
            info += (floorState.ElevatorFloor == floorNum) ? "E" : ".";
            info += " ";

            FloorPacker.Unpack(floorState.FloorsUL);

            bool valid = true;
        
            for (int i = 0; i < SlotNames.Count; i++)
            {
                if (FloorPacker.GetValueAsInt("Slot" + i) == floorNum)
                {
                    info += SlotNames[i];
                }
                else
                {
                    info += EMPTY;
                }

                info += " ";
            }

            results.Add(info);
        }

        results.Reverse();
        foreach (string info2 in results)
        {
            DebugOutput(info2);
        }

        DebugOutput("");
    }


    public class FloorState
    {
        public int MoveCount;
        public int ElevatorFloor;
        public ulong FloorsUL = 0;
        
        
        public FloorState(int elevatorFloor, ulong floorsUL)
        {
            ElevatorFloor = elevatorFloor;
            FloorsUL = floorsUL;
        }

        public FloorState Clone()
        {
            return new FloorState(ElevatorFloor, FloorsUL);
        }

        public FloorState MoveItems(bool up, List<int> moveItems)
        {
            FloorState clone = null;

            FloorPacker.Unpack(this.FloorsUL);
            
            
            // check and see if the moveItemes are on this floor.
            foreach (var item in moveItems)
            {
                // not on this floor.
                if (FloorPacker.GetValueAsInt("Slot"+item) != ElevatorFloor)
                {
                    return clone;
                }
            }

            int newFloor = ElevatorFloor + (up ? 1 : -1);
            if (newFloor >= 0 && newFloor < NumFloors)
            {
                clone = this.Clone();

                foreach (var slot in moveItems)
                {
                    FloorPacker.SetValue("Slot"+slot,newFloor);
                }

                clone.FloorsUL = FloorPacker.Pack();
                
                clone.ElevatorFloor = newFloor;
                clone.MoveCount = this.MoveCount+1;
            }

            return clone;
        }

        protected bool Equals(FloorState other)
        {
            bool equals = ElevatorFloor.Equals(other.ElevatorFloor);
            bool listsEqual = FloorsUL.Equals(other.FloorsUL);
            
            equals &= listsEqual;
            
            return equals;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals(obj as FloorState);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ElevatorFloor, FloorsUL);
        }
    }

    public class CacheState
    {
        protected bool Equals(CacheState other)
        {
            //return Floor.Equals(other.Floor) && Combination == other.Combination && Up == other.Up;
            return FromFloor.Equals(other.FromFloor);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CacheState)obj);
        }

        public override int GetHashCode()
        {
            //return HashCode.Combine(Floor, Combination, Up);
            return FromFloor.GetHashCode();
        }

        public readonly FloorState FromFloor;
        // public readonly FloorState ToFloor;

        public CacheState(FloorState fromFloor)
        {
            FromFloor = fromFloor;
            // ToFloor = toFloor;
        }
    }
}