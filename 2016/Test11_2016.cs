using System;
using System.Collections.Generic;
using System.Xml;

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
    public static int[] SlotPositions = null;

    public static List<(List<int>, bool)> BestMoveList = new List<(List<int>, bool)>();

    public override void Execute()
    {
        int elevatorFloor = 0;

        if (IsTestInput)
        {
            int column = 0;

            SlotNames.Clear();
            SlotNames.Add("HG");
            SlotNames.Add("HM");
            SlotNames.Add("LG");
            SlotNames.Add("LM");

            SlotPositions = new int[SlotNames.Count];
            int count = 0;
            SlotPositions[count++] = 1;
            SlotPositions[count++] = 0;
            SlotPositions[count++] = 2;
            SlotPositions[count++] = 0;
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
            SlotPositions = new int[SlotNames.Count];
            int count = 0;
            SlotPositions[count++] = 0;
            SlotPositions[count++] = 1;
            SlotPositions[count++] = 0;
            SlotPositions[count++] = 0;
            SlotPositions[count++] = 0;
            SlotPositions[count++] = 1;
            SlotPositions[count++] = 0;
            SlotPositions[count++] = 0;
            SlotPositions[count++] = 0;
            SlotPositions[count++] = 0;
        }


        FloorState initialState = new FloorState(elevatorFloor, SlotPositions);

        var allUpCombos = BuildAllCombinations(SlotNames.Count,true);
        var allDownCombos = BuildAllCombinations(SlotNames.Count,false);
        
        List<(List<int>, bool)> moveList = new List<(List<int>, bool)>();

        //GenerateMovesDepth(initialState, allUpCombos,allDownCombos, 0, moveList);
        GenerateMovesBreadth(initialState, allUpCombos,allDownCombos, 0, moveList);

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
        foreach (var item in BestMoveList)
        {
            DebugOutput($"{string.Join(',', item.Item1)},{item.Item2}");
        }

        DebugOutput("Replaying Moves");
        DebugState(initialState);
        GenerateMovesPlanned(initialState, BestMoveList, 0);


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

        public void GenerateMovesBreadth(FloorState currentState, List<List<int>> upCombinations, List<List<int>> downCombinations,int depth,
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
            List<FloorState> newStates = new List<FloorState>();
            for (int i = 0; i < upCombinations.Count; i++)
            {
                FloorState newState = currentState.MoveItems(true, upCombinations[i]);

                if (newState != null && ValidState(newState))
                {
                    newStates.Add(newState);
                }
                newState = currentState.MoveItems(false, downCombinations[i]);
                if (newState != null && ValidState(newState))
                {
                    newStates.Add(newState);
                }
            }

            foreach (var state in newStates)
            {
                if (GoalState(state))
                {
                    if (depth < QuickestMove)
                    {
                        QuickestMove = depth;
                    }
                }
            }

            foreach (var state in newStates)
            {
                GenerateMovesBreadth(state, upCombinations,downCombinations, depth + 1, moveList);
            }
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


    public bool GoalState(FloorState state)
    {
        bool valid = true;
        foreach (int val in state.Floors)
        {
            if (val != NumFloors - 1)
            {
                valid = false;
                break;
            }
        }

        if (valid)
        {
            int ibreak = 0;
        }

        return valid;
    }


    HashSet<string> chipsOnFloor = new HashSet<string>();
    HashSet<string> generatorsOnFloor = new HashSet<string>();
    HashSet<string> chipsCopy = new HashSet<string>();
    List<string> sameFloors = new List<string>();

    public bool ValidState(FloorState state, bool currentFloorOnly = true)
    {
        return ValidState2(state, currentFloorOnly);
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

            chipsOnFloor.Clear();
            generatorsOnFloor.Clear();
            chipsCopy.Clear();
            sameFloors.Clear();


            for (int i = 0; i < SlotPositions.Length; i++)
            {
                if (state.Floors[i] == floorNum)
                {
                    sameFloors.Add(SlotNames[i]);
                }
            }

            foreach (var pair in sameFloors)
            {
                if (pair.EndsWith('M'))
                {
                    chipsOnFloor.Add("" + pair.Substring(0, pair.Length - 1));
                }
                else if (pair.EndsWith('G'))
                {
                    generatorsOnFloor.Add("" + pair.Substring(0, pair.Length - 1));
                }
            }

            foreach (string chip in chipsOnFloor)
            {
                chipsCopy.Add(chip);
            }

            foreach (string chip in chipsCopy)
            {
                if (generatorsOnFloor.Contains(chip))
                {
                    chipsOnFloor.Remove(chip);
                    generatorsOnFloor.Remove(chip);
                }
            }

            if (chipsOnFloor.Count != 0 && generatorsOnFloor.Count != 0)
            {
                return false;
            }
        }

        return valid;
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

            int unpairedChips = 0;
            int unpairedGenerators = 0;
            for (int i = 0; i < SlotPositions.Length; i += 2)
            {
                if (state.Floors[i] == floorNum && state.Floors[i + 1] != floorNum)
                {
                    unpairedGenerators++;
                }

                if (state.Floors[i + 1] == floorNum && state.Floors[i] != floorNum)
                {
                    unpairedChips++;
                }
            }

            if (unpairedChips > 0 && unpairedGenerators > 0)
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

            for (int i = 0; i < SlotPositions.Length; i++)
            {
                if (floorState.Floors[i] == floorNum)
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
        foreach (string info in results)
        {
            DebugOutput(info);
        }

        DebugOutput("");
    }


    public class FloorState
    {
        public int ElevatorFloor;

        //public List<List<string>> Floors = new List<List<string>>();
        //public List<int> Floors = new List<int>();
        public int[] Floors = null;

        public FloorState(int elevatorFloor, int[] floors)
        {
            ElevatorFloor = elevatorFloor;
            Floors = new int[floors.Length];
            Array.Copy(floors, Floors, floors.Length);
        }

        public FloorState Clone()
        {
            return new FloorState(ElevatorFloor, Floors);
        }

        public FloorState MoveItems(bool up, List<int> moveItems)
        {
            FloorState clone = null;

            // check and see if the moveItemes are on this floor.
            foreach (var item in moveItems)
            {
                // not on this floor.
                if (Floors[item] != ElevatorFloor)
                {
                    return clone;
                }
            }

            int newFloor = ElevatorFloor + (up ? 1 : -1);
            if (newFloor >= 0 && newFloor < NumFloors)
            {
                clone = this.Clone();

                foreach (int index in moveItems)
                {
                    clone.Floors[index] = newFloor;
                }

                clone.ElevatorFloor = newFloor;
            }

            return clone;
        }

        protected bool Equals(FloorState other)
        {
            bool equals = ElevatorFloor.Equals(other.ElevatorFloor);
            bool listsEqual = Floors.SequenceEqual(other.Floors);

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
            return HashCode.Combine(ElevatorFloor, Floors);
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