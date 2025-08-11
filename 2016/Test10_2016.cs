using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

public class Test10_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 10;
    }

    public static Dictionary<int, Bot> BotDictionary = new Dictionary<int, Bot>();

    public static Dictionary<int, int> OutputDictionary = new Dictionary<int, int>();
    public static Dictionary<int, Bot> OutputSourceDictionary = new Dictionary<int, Bot>();

    public static readonly string BOT_TARGET = "bot";
    public static readonly string OUTPUT_TARGET = "output";


    public override void Execute()
    {
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(' ');

            //value 5 goes to bot 2
            if (tokens[0] == "value")
            {
                int value = int.Parse(tokens[1]);
                string target = tokens[4];
                int targetId = int.Parse(tokens[5]);

                if (target == BOT_TARGET)
                {
                    if (!BotDictionary.TryGetValue(targetId, out Bot bot))
                    {
                        bot = new Bot();
                        bot.Id = targetId;
                        BotDictionary[targetId] = bot;
                    }

                    bot.WorkList.Add(value);
                }
                else
                {
                    Debug.Assert(false, "Unexpected input");
                }
            }
            // bot 2 gives low to bot 1 and high to bot 0
            // bot 1 gives low to output 1 and high to bot 0
            else if (tokens[0] == BOT_TARGET)
            {
                int botId = int.Parse(tokens[1]);

                if (!BotDictionary.TryGetValue(botId, out Bot bot))
                {
                    bot = new Bot();
                    bot.Id = botId;
                    BotDictionary.Add(bot.Id, bot);
                }

                bot.LowTarget = tokens[5];
                bot.LowTargetID = int.Parse(tokens[6]);

                bot.HighTarget = tokens[10];
                bot.HighTargetID = int.Parse(tokens[11]);
            }
            else
            {
                Debug.Assert(false, "Failed to parse input");
            }
        }

        foreach (int id in BotDictionary.Keys.Order())
        {
            DebugOutput(BotDictionary[id].DebugInfo());
        }

        bool keepGoing = true;

        List<int> keyValues = new List<int>();
        if (IsTestInput)
        {
            keyValues.Add(5);
            keyValues.Add(2);
        }
        else
        {
            keyValues.Add(61);
            keyValues.Add(17);
            
        }
        
       
        while (keepGoing)
        {
            keepGoing = false;
            foreach (Bot bot in BotDictionary.Values)
            {
                bot.ProcessValues();
                if (!bot.Processed)
                {
                    keepGoing = true;
                }
            }
        }

        Bot keyBot = null;
        foreach (Bot b in BotDictionary.Values)
        {
            if (b.WorkList.Intersect(keyValues).Count( )== keyValues.Count)
            {
                keyBot = b;
                break;
            }
        }
        
        foreach (int id in OutputDictionary.Keys.Order())
        {
            //DebugOutput($"Output [{id}] is : OutputDictionary[{OutputDictionary[id]}]");
            DebugOutput($"Output [{id}] is : {OutputSourceDictionary[id].Id}");
        }

        if (keyBot != null)
        {
            DebugOutput($"Bot containing [{string.Join(',',keyValues)}] is Bot {keyBot.Id}");
        }

        if (IsPart2)
        {
           DebugOutput($"Part 2 is {OutputDictionary[0] * OutputDictionary[1] * OutputDictionary[2]}"); 
            
        }
        
    }

    public class Bot
    {
        public int Id;
        public string LowTarget;
        public int LowTargetID;
        public string HighTarget;
        public int HighTargetID;

        public List<int> WorkList = new List<int>();


        public bool Processed;
        
        
        public void ProcessValues()
        {
            if (Processed)
            {
                return;
            }
        
            // only when it has 2 chips.
            if (WorkList.Count == 2)
            {
                int lowVal = Math.Min(WorkList[0], WorkList[1]);
                int highVal = Math.Max(WorkList[0], WorkList[1]);

                if (LowTarget == BOT_TARGET)
                {
                    Bot lowBot = BotDictionary[LowTargetID];
                    lowBot.WorkList.Add(lowVal);
                }
                else if (LowTarget == OUTPUT_TARGET)
                {
                    OutputDictionary[LowTargetID] = lowVal;
                    OutputSourceDictionary[LowTargetID] = this;

                }
                else
                {
                    Debug.Assert(false, "Unexpected input");
                }


                if (HighTarget == BOT_TARGET)
                {
                    Bot highBot = BotDictionary[HighTargetID];
                    highBot.WorkList.Add(highVal);
                }
                else if (HighTarget == OUTPUT_TARGET)
                {
                    OutputDictionary[HighTargetID] = highVal;
                    OutputSourceDictionary[HighTargetID] = this;
                }
                else
                {
                    Debug.Assert(false, "Unexpected input");
                }
                Processed = true;
            }
            // else
            // {
            //     Debug.Assert(false, "Unexpected input");
            // }
        }

        public string DebugInfo()
        {
            return
                $"Bot [{Id}]  Low : {LowTarget} {LowTargetID}    High : {HighTarget} {HighTargetID} ";
        }
    }
}