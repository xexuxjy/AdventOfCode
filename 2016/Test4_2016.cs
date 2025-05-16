using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
public class Test4_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 4;
    }
    public override void Execute()
    {
        int numRealRooms = 0;
        int sectorSum = 0;

        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split('-');
            string idAndChecksum = tokens[tokens.Length - 1];
            List<string> encryptedName = new List<string>();
            for (int i = 0; i < tokens.Length - 1; i++)
            {
                encryptedName.Add(tokens[i]);
            }

            int startBracketIndex = idAndChecksum.IndexOf("[");
            int endBracketIndex = idAndChecksum.IndexOf("]");

            int sectorId = int.Parse(idAndChecksum.Substring(0, startBracketIndex));
            string checksum = idAndChecksum.Substring(startBracketIndex + 1, endBracketIndex - startBracketIndex - 1);

            if (IsRoomReal(encryptedName, checksum))
            {
                numRealRooms++;
                sectorSum += sectorId;
            }

            if(IsPart2)
            {
                List<string> decryptedName = new List<string>();
                foreach(string name in encryptedName)
                {
                    string result = "";
                    foreach(char c in name)
                    {
                        result += RotateChar(c,sectorId);
                    }
                    decryptedName.Add(result);
                }

                //DebugOutput($"Sector {sectorId} encrypted : {string.Join(" ", encryptedName)}  decrypted : {string.Join(" ",decryptedName)}");
            }

            //DebugOutput($"encName : {string.Join(", ", encryptedName)}   sectorId {sectorId}   checkSum {checksum}");
        }


        DebugOutput($"Number of real rooms is {numRealRooms} / {m_dataFileContents.Count}  Sector sum : {sectorSum} ");
    }

    //A room is real (not a decoy) if the checksum is the five most common letters in the encrypted name, in order, with ties broken by alphabetization. For example:
    public bool IsRoomReal(List<string> encryptedName, string checksum)
    {
        TopList topList = new TopList();

        foreach (string word in encryptedName)
        {
            foreach (char c in word)
            {
                topList.Add(c);
            }
        }

        List<(char, int)> results = topList.FindTop(5);
        int score = 0;
        foreach (var result in results)
        {
            if (checksum.Contains(result.Item1))
            {
                score++;
            }
        }

        return score == checksum.Length;
    }

    public char RotateChar(char c, int numTimes)
    {
        char d = char.IsUpper(c) ? 'A' : 'a';  
        char rotated = (char)((((c + numTimes) - d) % 26) + d);  

        return rotated;
    }



    public class TopList
    {
        private List<(char, int)> values = new List<(char, int)>();
        public void Add(char c)
        {
            int index = values.FindIndex(x => x.Item1 == c);
            if (index < 0)
            {
                values.Add((c, 1));
            }
            else
            {
                var pair = values[index];
                values[index] = (pair.Item1, pair.Item2 + 1);
            }
        }

        public List<(char, int)> FindTop(int num)
        {
            List<(char, int)> results = new List<(char, int)>();
            int count = 0;
            foreach (var pair in values.OrderByDescending(x => x.Item2).ThenBy(x => x.Item1))
            {

                results.Add(pair);
                count++;
                if (count >= num)
                {
                    break;
                }
            }

            return results;
        }

    }
}
