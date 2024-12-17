using Spectre.Console;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Reflection.Emit;
using System.Xml.Linq;
using static Test17_2024;

public class Test17_2024 : BaseTest
{
    List<int> InstructionList = new List<int>();
    public int InstructionPointer = 0;


    public override void Initialise()
    {
        Year = 2024;
        TestID = 17;
    }

    public Dictionary<string, Register> RegisterMap = new Dictionary<string, Register>();

    public Dictionary<int, Func<int, int, int>> OpCodeMap = new Dictionary<int, Func<int, int, int>>();

    public List<int> OutputList = new List<int>();

    public bool IncrementInstructionPointer = true;

    public override void Execute()
    {
        RegisterMap["A"] = new Register() { Id = "A" };
        RegisterMap["B"] = new Register() { Id = "B" };
        RegisterMap["C"] = new Register() { Id = "C" };


        RegisterMap["A"].Value = int.Parse(m_dataFileContents[0].Split(':')[1]);
        RegisterMap["B"].Value = int.Parse(m_dataFileContents[1].Split(':')[1]);
        RegisterMap["C"].Value = int.Parse(m_dataFileContents[2].Split(':')[1]);

        OpCodeMap[0] = HandleADV;
        OpCodeMap[1] = HandleBXL;
        OpCodeMap[2] = HandleBST;
        OpCodeMap[3] = HandleJNZ;
        OpCodeMap[4] = HandleBXC;
        OpCodeMap[5] = HandleOUT;
        OpCodeMap[6] = HandleBDV;
        OpCodeMap[7] = HandleCDV;


        string program = m_dataFileContents[4].Split(':')[1];
        string[] tokens = program.Split(",");
        foreach (string token in tokens)
        {
            InstructionList.Add(int.Parse(token));
        }

        while (InstructionPointer < InstructionList.Count)
        {
            HandleOpCode(InstructionList[InstructionPointer], InstructionList[InstructionPointer + 1]);
            if (IncrementInstructionPointer)
            {
                InstructionPointer += 2;
            }
            IncrementInstructionPointer = true;


            // we went somewhere bad so halt.
            if (InstructionPointer > InstructionList.Count)
            {
                break;
            }
        }

        DebugOutput(string.Join(',', OutputList));

        int ibreak = 0;
    }


    public void HandleOpCode(int opCode, int operand)
    {
        int literalOperand = operand;
        int comboOperand = operand;
        // translate operand
        if (operand >= 0 && operand <= 3)
        {
        }
        else if (operand == 4)
        {
            comboOperand = RegisterMap["A"].Value;
        }
        else if (operand == 5)
        {
            comboOperand = RegisterMap["B"].Value;
        }
        else if (operand == 6)
        {
            comboOperand = RegisterMap["C"].Value;
        }
        else
        {
            Debug.Assert(false);
        }

        OpCodeMap[opCode](literalOperand, comboOperand);

    }


    /*
    The adv instruction (opcode 0) performs division. The numerator is the value in the A register. The denominator is found by raising 2 to the power of the instruction's combo operand. (So, an operand of 2 would divide A by 4 (2^2); an operand of 5 would divide A by 2^B.) The result of the division operation is truncated to an integer and then written to the A register.

The bxl instruction (opcode 1) calculates the bitwise XOR of register B and the instruction's literal operand, then stores the result in register B.

The bst instruction (opcode 2) calculates the value of its combo operand modulo 8 (thereby keeping only its lowest 3 bits), then writes that value to the B register.
    The jnz instruction (opcode 3) does nothing if the A register is 0. However, if the A register is not zero, it jumps by setting the instruction pointer to the value of its literal operand; if this instruction jumps, the instruction pointer is not increased by 2 after this instruction.
    The bxc instruction (opcode 4) calculates the bitwise XOR of register B and register C, then stores the result in register B. (For legacy reasons, this instruction reads an operand but ignores it.)

The out instruction (opcode 5) calculates the value of its combo operand modulo 8, then outputs that value. (If a program outputs multiple values, they are separated by commas.)

The bdv instruction (opcode 6) works exactly like the adv instruction except that the result is stored in the B register. (The numerator is still read from the A register.)

The cdv instruction (opcode 7) works exactly like the adv instruction except that the result is stored in the C register. (The numerator is still read from the A register.)

    */

    bool DebugOpcodes = true;

    public int HandleADV(int literalOperand, int comboOperand)
    {
        if (DebugOpcodes)
        {
            DebugOutput($"ADV {RegisterMap["A"].Value}  / {(Helper.IntPow(2, (uint)comboOperand))} = {(RegisterMap["A"].Value / Helper.IntPow(2, (uint)comboOperand))}");
        }

        RegisterMap["A"].Value = RegisterMap["A"].Value / (Helper.IntPow(2, (uint)comboOperand));
        return 0;
    }

    public int HandleBXL(int literalOperand, int comboOperand)
    {
        if (DebugOpcodes)
        {
            DebugOutput($"BXL {RegisterMap["B"].Value}  ^ {literalOperand} = {RegisterMap["B"].Value ^ literalOperand}");
        }

        int val = RegisterMap["B"].Value;
        val = val ^ literalOperand;
        RegisterMap["B"].Value = val;
        return 0;
    }
    public int HandleBST(int literalOperand, int comboOperand)
    {
        if (DebugOpcodes)
        {
            DebugOutput($"BST  {comboOperand} % 8= {comboOperand % 8}");
        }


        int val = comboOperand % 8;
        RegisterMap["B"].Value = val;
        return 0;
    }
    public int HandleJNZ(int literalOperand, int comboOperand)
    {
        if (DebugOpcodes)
        {
            DebugOutput($"JNZ   A[{RegisterMap["A"].Value}]  {literalOperand}");
        }

        if (RegisterMap["A"].Value != 0)
        {
            InstructionPointer = literalOperand;
            IncrementInstructionPointer = false;
        }
        return 0;
    }
    public int HandleBXC(int literalOperand, int comboOperand)
    {
        if (DebugOpcodes)
        {
            DebugOutput($"BXC {RegisterMap["B"].Value}  ^ {RegisterMap["C"].Value} = {RegisterMap["B"].Value ^ RegisterMap["C"].Value}");
        }

        int valb = RegisterMap["B"].Value;
        int valc = RegisterMap["C"].Value;
        int val = valb ^ valc;
        RegisterMap["B"].Value = val;
        return 0;
    }
    public int HandleOUT(int literalOperand, int comboOperand)
    {
        if (DebugOpcodes)
        {
            DebugOutput($"OUT {comboOperand}");
        }
        OutputList.Add(comboOperand % 8);
        return 0;
    }
    public int HandleBDV(int literalOperand, int comboOperand)
    {
        if (DebugOpcodes)
        {
            DebugOutput($"BDV {RegisterMap["A"].Value}  / {(Helper.IntPow(2, (uint)comboOperand))} = {(RegisterMap["A"].Value / Helper.IntPow(2, (uint)comboOperand))}");
        }

        RegisterMap["B"].Value = RegisterMap["A"].Value / (Helper.IntPow(2, (uint)comboOperand));
        return 0;
    }
    public int HandleCDV(int literalOperand, int comboOperand)
    {
        if (DebugOpcodes)
        {
            DebugOutput($"CDV {RegisterMap["A"].Value}  / {(Helper.IntPow(2, (uint)comboOperand))} = {(RegisterMap["A"].Value / Helper.IntPow(2, (uint)comboOperand))}");
        }

        RegisterMap["C"].Value = RegisterMap["A"].Value / (Helper.IntPow(2, (uint)comboOperand));
        return 0;
    }








    public class Register
    {
        public string Id;

        private int m_value;
        public int Value
        {
            get { return m_value; }
            set
            {
                m_value = value;
            }
        }
    }
}