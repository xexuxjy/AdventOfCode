using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class Test14_2016 : BaseTest
{
    public Dictionary<string,string> StretchedCache = new Dictionary<string, string>();
    
    public override void Initialise()
    {
        Year = 2016;
        TestID = 14;
    }

    public string FormattedHash(string message, MD5 md5)
    {
        byte[] input = Encoding.ASCII.GetBytes(message);
        byte[] hash = md5.ComputeHash(input);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++) 
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return  sb.ToString().ToLower();
    }

    public string StretchHash(string originalMessage, MD5 md5, int num)
    {
        if (!StretchedCache.TryGetValue(originalMessage, out string message))
        {
            message = originalMessage;
            for (int i = 0; i < num; i++)
            {
                message = FormattedHash(message, md5);
            }
            StretchedCache[originalMessage] = message;        
        }
        return message;
    }
    

    public override void Execute()
    {
        using (MD5 md5 = MD5.Create())
        {
            int index = 0;
            int keyCount = 0;
            while (index < 100000)
            {
                string message = m_dataFileContents[0] + index;
                string formattedHash = StretchHash(message, md5,IsPart1?1:2017);

                if (index == 7858)
                {
                    int ibreak = 0;
                }
                
                if (CheckHash1(formattedHash,out char matchVal))
                {
                    //for (int j = 1; j < 1001; j++)
                    for (int j = 1; j <= 1000; j++)
                    {
                        message = m_dataFileContents[0] + (index+j);
                        string formattedHash2 = StretchHash(message, md5,IsPart1?1:2017);
                        if (CheckHash2(formattedHash2,matchVal))
                        {
                            keyCount++;
                            //DebugOutput(""+index);
                            if (keyCount == 64)
                            {
                                DebugOutput($"Found all keys by index {index}");
                                goto EndOfSearch;
                            }

                            break;
                        }
                    }
                }

                index++;
            }
            EndOfSearch:
            DebugOutput("Complete");
        }
    }

    public bool CheckHash1(string data,out char val)
    {
        int length = 2;
     
        for (int i = 0; i < data.Length - length; i++)
        {
            if (data[i] == data[i + 1] && data[i] == data[i + 2])
            {
                val = data[i];
                return true;
            }
        }
        val = ' ';
        return false;
    }
    
    public bool CheckHash2(string data,char match)
    {
        int length = 4;
        for (int i = 0; i < data.Length - length; i++)
        {
            if (data[i] == match && data[i + 1] == match && data[i + 2] == match && data[i + 3] == match &&
                data[i + 4] == match)
            {
                return true;
            }
        }

        return false;
    }
}