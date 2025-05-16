using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

public class Test9_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 9;
    }

    public override void Execute()
    {
        foreach (string line in m_dataFileContents)
        {
            if (line.StartsWith('#'))
            {
                continue;
            }

            bool inToken = false;
            //StringBuilder finalMessage = new StringBuilder();
            long finalMessageLength = IsPart1?CalculatePart1(line) : CalculatePart2(line);


            //DebugOutput($"Original message: {line}");
            //DebugOutput($"Final message {finalMessage.ToString()}");
            //DebugOutput($"Original Length {line.Length}  Final length {finalMessage.ToString().Length} ");
            //DebugOutput($"Decompressed length : {finalMessage.Length}");
            DebugOutput($"Decompressed length : {finalMessageLength}");
        }
    }

    public long CalculatePart1(string line)
    {
        long finalMessageLength = 0;
        for (int i = 0; i < line.Length;)
        {
            if (line[i] == '(')
            {
                string numCharsStr = "";
                i += 1;
                while (line[i] != 'x')
                {
                    numCharsStr += line[i++];
                }

                // skip x
                i++;

                string numRepeatsStr = "";
                while (line[i] != ')')
                {
                    numRepeatsStr += line[i++];
                }

                // skip )
                i++;

                int numChars = int.Parse(numCharsStr);
                int numRepeats = int.Parse(numRepeatsStr);


                for (int j = 0; j < numRepeats; ++j)
                {
                    finalMessageLength += numChars;
                }

                i += numChars;
            }
            else
            {
                finalMessageLength++;
            }

        }
        return finalMessageLength;
    }

    public long CalculatePart2(string line,int depth=0)
    {
        int index = 0;
        long total = 0;

        if (!line.Contains('('))
        {
            return line.Length;
        }
        
        
        while(index < line.Length)
        {
            int multiplier = 1;
            char c = line[index];
            if (line[index] == '(')
            {
                if (TryParseBlock(line, index,out var block))
                {
                    multiplier = block.Item2;
                    int numChars = block.Item1;
                    int blockSize = block.Item3;
                    string segment = line.Substring(index+blockSize, numChars);
                    total += CalculatePart2(segment,depth+1) * multiplier;
                    index += (blockSize + numChars);
                }
            }
            else
            {
                total++;
                index++;
            }
        }

        return total;
    }

    public bool TryParseBlock(string line, int index,out (int, int,int) block)
    {
        if (line == null || line.Length == 0)
        {
            block = (-1,-1,-1);
            return false;
        }

        int indexCopy = index;
        
        if (line[index] == '(')
        {
            string numCharsStr = "";
            index += 1;
            while (line[index] != 'x')
            {
                numCharsStr += line[index++];
            }

            // skip x
            index++;

            string numRepeatsStr = "";
            while (line[index] != ')')
            {
                numRepeatsStr += line[index++];
            }

            // skip )
            index++;

            int numChars = int.Parse(numCharsStr);
            int numRepeats = int.Parse(numRepeatsStr);
            int blockSize = index - indexCopy; 
            block = (numChars, numRepeats,blockSize);
            return true;
        }
        block = (-1,-1,-1);
        return false;
    }

    
    
}