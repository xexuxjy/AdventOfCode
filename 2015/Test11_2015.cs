using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Test8_2023;

public class Test11_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 11;
    }


    public override void Execute()
    {
        foreach (string line in m_dataFileContents)
        {
            int meetsRequirement = MeetsRequirement(line.ToCharArray());

            DebugOutput($"{line} is valid {meetsRequirement}");

            char[] charArray = line.ToCharArray();

            do
            {
                IncrementPassword(charArray);
            }
            while (MeetsRequirement(charArray) != 0);

            if (IsPart2)
            {
                do
                {
                    IncrementPassword(charArray);
                }
                while (MeetsRequirement(charArray) != 0);


            }


            DebugOutput($"Next password after {line} is {new string(charArray)}");
        }

    }

    public void IncrementPassword(char[] password)
    {
        int index = password.Length - 1;
        bool hasIncremented = false;
        while (!hasIncremented && index >= 0)
        {

            if (password[index] < 'z')
            {
                password[index] = (char)(password[index] + 1);
                hasIncremented = true;
            }
            else
            {
                password[index] = 'a';
                index--;
            }
        }

    }


    public int MeetsRequirement(char[] password)
    {

        bool found3InARow = false;
        for (int i = 0; i < password.Length - 3; ++i)
        {
            if ((password[i + 1] - password[i] == 1) && (password[i + 2] - password[i + 1] == 1))
            {
                found3InARow = true;
                break;
            }
        }

        if (!found3InARow)
        {
            return 1;
        }

        if (password.Contains('l') || password.Contains('o') || password.Contains('i'))
        {
            return 2;
        }


        int pairCount = 0;
        for (int i = 0; i < password.Length - 1; ++i)
        {
            if (password[i] == password[i + 1])
            {
                pairCount++;
                // step past pair
                i += 1;
            }
        }

        if (pairCount < 2)
        {
            return 3;
        }


        return 0;

    }
}