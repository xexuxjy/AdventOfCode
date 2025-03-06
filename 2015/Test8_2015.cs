using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test8_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 8;
    }


    public override void Execute()
    {
        int codeCharacters = 0;
        int memoryCharacters = 0;

        int reencodeCharacters = 0;


        foreach(string line in m_dataFileContents)
        {
            int lineCodeCharacters = line.Length;
            int lineMemoryCharacters = 0;
            int lineReencodeCharacters = 0;

            lineReencodeCharacters+= 3; // encode inital doublequote

            for(int i=0;i<line.Length; i++)
            {
                if(line[i] == '\\')
                {
                    if(i+1 <line.Length)
                    {

                        if(line[i+1] == 'x')
                        {
                            lineMemoryCharacters++;
                            i+=3;
                            lineReencodeCharacters+=5;
                        }
                        else
                        {
                            if(line[i+1] == '\\' || line[i+1] == '\"')
                            {
                                lineReencodeCharacters+=4;
                            }

                            lineMemoryCharacters++;
                            i+=1;
                        }
                    }
                }
                else 
                {
                    if(i != 0 && i != line.Length-1)
                    {
                        lineMemoryCharacters++;
                        lineReencodeCharacters++;
                    }
                }
            }

            lineReencodeCharacters+= 3; // encode final doublequote


            codeCharacters += lineCodeCharacters;
            memoryCharacters += lineMemoryCharacters;
            reencodeCharacters += lineReencodeCharacters;

            if(IsPart2)
            {
                DebugOutput($"Line {line}   rc: {lineReencodeCharacters}   cc: {lineCodeCharacters}   mc : {lineMemoryCharacters}");
            }
            else
            {
                DebugOutput($"Line {line}   cc: {lineCodeCharacters}   mc : {lineMemoryCharacters}");
            }
        }


        
        if(IsPart2)
        {
            int diff = reencodeCharacters-codeCharacters;
            DebugOutput($"The difference is {diff}   (cc : {reencodeCharacters}   mc: {codeCharacters}");
        }
        else
        {
            int diff = codeCharacters-memoryCharacters;
            DebugOutput($"The difference is {diff}   (cc : {codeCharacters}   mc: {memoryCharacters}");
        }


    }
}