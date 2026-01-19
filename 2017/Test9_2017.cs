using System;
using System.Collections.Generic;

public class Test9_2017 : BaseTest
{
    public const char OPEN_GROUP = '{';
    public const char CLOSE_GROUP = '}';
    public const char OPEN_GARBAGE = '<';
    public const char CLOSE_GARBAGE = '>';
    public const char CANCEL_NEXT = '!';
    
    public override void Initialise()
    {
        Year = 2017;
        TestID = 9;
    }

    public override void Execute()
    {

        foreach (string line in m_dataFileContents)
        {

            Stack<ParseGroup> groupStack = new Stack<ParseGroup>();
            List<ParseGroup> groups = new List<ParseGroup>();
            ParseGroup currentGroup = new ParseGroup();
            bool inGarbage = false;
            bool cancelNext = false;
            
            int garbageCount = 0;
            
            foreach (char c in line)
            {
                if (cancelNext)
                {
                    cancelNext = false;
                }
                else if (c == CANCEL_NEXT)
                {
                    cancelNext = true;
                }
                else if (c == OPEN_GARBAGE)
                {
                    if (!inGarbage)
                    {
                        inGarbage = true;
                    }
                    else
                    {
                        garbageCount++;
                    }
                }
                else if (c == CLOSE_GARBAGE)
                {
                    if (inGarbage)
                    {
                        inGarbage = false;
                    }
                    else
                    {
                        garbageCount++;
                    }
                }
                else if (c == OPEN_GROUP)
                {
                    if (!inGarbage)
                    {
                        currentGroup = new ParseGroup();
                        if (groupStack.Count == 0)
                        {
                            currentGroup.Score = 1;
                        }
                        else
                        {
                            currentGroup.Score = groupStack.Peek().Score + 1;
                        }

                        groupStack.Push(currentGroup);
                        groups.Add(currentGroup);
                    }
                    else
                    {
                        garbageCount++;
                    }
                }
                else if (c == CLOSE_GROUP)
                {
                    if (!inGarbage)
                    {
                        groupStack.Pop();
                    }
                    else
                    {
                        garbageCount++;
                    }
                }
                else
                {
                    if (inGarbage)
                    {
                        garbageCount++;
                    }
                }

                
            }

            int total = 0;
            foreach (ParseGroup group in groups)
            {
                total += group.Score;
            }

            if (IsPart1)
            {
                DebugOutput($"The group score for {line} was {total}");
            }

            if (IsPart2)
            {
                DebugOutput($"The garbage count for {line } was {garbageCount}");
            }
            
        }
        
    }
}

public class ParseGroup
{
    public int Score = 0;

}