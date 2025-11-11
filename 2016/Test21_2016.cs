using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

public class Test21_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 21;
    }

    public override void Execute()
    {
        string originalStart = IsTestInput ? "abcde" : "abcdefgh";
        string start = originalStart;
        if (IsPart2)
        {
            start = IsTestInput ? "decab" : "fbgdceah";
        }

        List<char> sourceMessage = start.ToCharArray().ToList();

        List<ScrambleRule> rules = new List<ScrambleRule>();
        int count = 0;
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            switch (tokens[0])
            {
                case "swap":
                    if (tokens[1] == "position")
                    {
                        int index1 = Convert.ToInt32(tokens[2]);
                        int index2 = Convert.ToInt32(tokens[5]);

                        SwapPosition swapPosition = new SwapPosition();
                        swapPosition.X = IsPart1 ? index1 : index2;
                        swapPosition.Y = IsPart1 ? index2 : index1;
                        rules.Add(swapPosition);
                    }
                    else if (tokens[1] == "letter")
                    {
                        char c1 = Convert.ToChar(tokens[2]);
                        char c2 = Convert.ToChar(tokens[5]);

                        SwapLetter swapLetter = new SwapLetter();
                        swapLetter.X = IsPart1 ? c1 : c2;
                        swapLetter.Y = IsPart1 ? c2 : c1;
                        rules.Add(swapLetter);
                    }
                    else
                    {
                        DebugOutput("Invalid input : " + line);
                    }

                    break;
                case "reverse":
                    if (tokens[1] == "positions")
                    {
                        ReverseXY reverseXY = new ReverseXY();
                        reverseXY.X = Convert.ToInt32(tokens[2]);
                        reverseXY.Y = Convert.ToInt32(tokens[4]);
                        rules.Add(reverseXY);
                    }
                    else
                    {
                        DebugOutput("Invalid input : " + line);
                    }

                    break;
                case "rotate":
                    if (tokens[1] == "based")
                    {
                        RotateRelative rotateRelative = new RotateRelative();
                        rotateRelative.Character = Convert.ToChar(tokens[6]);
                        rules.Add(rotateRelative);
                    }
                    else if (tokens[1] == "right" || tokens[1] == "left")
                    {
                        Rotate rotate = new Rotate();
                        rotate.Amount = Convert.ToInt32(tokens[2]);
                        if (tokens[1] == "left")
                        {
                            rotate.Amount *= -1;
                        }


                        rules.Add(rotate);
                    }
                    else
                    {
                        DebugOutput("Invalid input : " + line);
                    }

                    break;
                case "move":
                    int index1a = Convert.ToInt32(tokens[2]);
                    int index2a = Convert.ToInt32(tokens[5]);

                    Move move = new Move();
                    move.X = IsPart1 ? index1a : index2a;
                    move.Y = IsPart1 ? index2a : index1a;

                    rules.Add(move);
                    break;

                default:
                    DebugOutput("Invalid input : " + tokens[0]);
                    break;
            }

            rules.Last().Id = count++;
        }

        DebugOutput("Total number of lines: " + m_dataFileContents.Count);
        DebugOutput("Total number of rules: " + rules.Count);

        if (IsPart2)
        {
            rules.Reverse();
        }

        foreach (var rule in rules)
        {
            rule.Test = this;
            rule.Apply(sourceMessage);
            DebugOutput("interim message : " + new string(sourceMessage.ToArray()));
        }

        string finalMessage = new string(sourceMessage.ToArray());
        if (IsPart1)
        {
            DebugOutput("Final message : " + finalMessage);
        }
        else
        {
            DebugOutput("Reverse message : " + start);
            DebugOutput("Original message : " + originalStart);
            DebugOutput("Final message : " + finalMessage);
        }
    }


    public abstract class ScrambleRule
    {
        public int Id;
        public BaseTest Test;
        protected List<char> inputCopy = new List<char>();


        public abstract void Apply(List<char> input);
    }


    public class SwapPosition : ScrambleRule
    {
        public int X;
        public int Y;

        public override void Apply(List<char> input)
        {
            char temp = input[X];
            input[X] = input[Y];
            input[Y] = temp;
        }
    }

    public class SwapLetter : ScrambleRule
    {
        public char X;
        public char Y;

        public override void Apply(List<char> input)
        {
            int xcount = input.Count(x => x == X);
            int ycount = input.Count(x => x == Y);

            Debug.Assert(xcount == 1 && ycount == 1);
            int xindex = input.IndexOf(X);
            int yindex = input.IndexOf(Y);
            char temp = input[xindex];
            input[xindex] = input[yindex];
            input[yindex] = temp;
        }
    }

    public class Rotate : ScrambleRule
    {
        public int Amount;

        public override void Apply(List<char> input)
        {
            inputCopy.Clear();
            inputCopy.AddRange(input);

            if (Test.IsPart2)
            {
                Amount *= -1;
            }

            for (int i = 0; i < input.Count; i++)
            {
                int newIndex = (i + Amount) % input.Count;
                if (newIndex < 0)
                {
                    newIndex += input.Count;
                }

                input[newIndex] = inputCopy[i];
            }
        }
    }

    public class RotateRelative : ScrambleRule
    {
        public char Character;
        public int Amount;

        public override void Apply(List<char> input)
        {
            inputCopy.Clear();
            inputCopy.AddRange(input);

            int index = inputCopy.IndexOf(Character);

            if (Test.IsPart1)
            {
                Amount = index;
                Amount += 1;
                if (inputCopy.IndexOf(Character) >= 4)
                {
                    Amount += 1;
                }
            }
            else
            {
/*

 From p_tsengs Ruby solution at : https://www.reddit.com/r/adventofcode/comments/5ji29h/2016_day_21_solutions/

 rotate_based needs the most work to undo.
 pos shift newpos
   0     1      1
   1     2      3
   2     3      5
   3     4      7
   4     6      2
   5     7      4
   6     8      6
   7     9      0
 all odds have a clear pattern, all evens have a clear pattern...
 except 0, which we'll just special-case.
 */
                Amount = index / 2 + (index % 2 == 1 || index == 0 ? 1 : 5);
                Amount *= -1;
            }

            for (int i = 0; i < input.Count; i++)
            {
                int newIndex = (i + Amount) % input.Count;
                if (newIndex < 0)
                {
                    newIndex += input.Count;
                }

                input[newIndex] = inputCopy[i];
            }
        }
    }

    public class ReverseXY : ScrambleRule
    {
        public int X;
        public int Y;

        public override void Apply(List<char> input)
        {
            inputCopy.Clear();
            inputCopy.AddRange(input);

            int span = (Y - X) + 1;
            for (int i = 0; i < span; i++)
            {
                input[X + i] = inputCopy[Y - i];
            }
        }
    }

    public class Move : ScrambleRule
    {
        public int X;
        public int Y;

        public override void Apply(List<char> input)
        {
            char temp = input[X];
            input.RemoveAt(X);
            input.Insert(Y, temp);
        }
    }
}