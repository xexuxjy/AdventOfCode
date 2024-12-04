using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Test4_2024 : BaseTest
{
    public override void Initialise()
    {
        Year = 2024;
        TestID = 4;
    }

    public override void Execute()
    {
        int width = 0;
        int height = 0;

        char[] dataGrid = Helper.GetCharGrid(m_dataFileContents, ref width, ref height);


        char[] searchTerm = ['X', 'M', 'A', 'S'];
        char[] searchTermPart2 = ['M', 'A', 'S'];

        int matches = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(IsPart2)
                {
                    matches += TestTermPart2(new IntVector2(x, y), searchTermPart2, dataGrid, width, height);
                }
                else
                {
                    matches += TestTerm(new IntVector2(x, y), searchTerm, dataGrid, width, height);
                }
            }
        }

        DebugOutput($"Have found {matches} matches");

    }


    public int TestTerm(IntVector2 position, char[] searchWord, char[] dataGrid, int width, int height)
    {
        if (dataGrid[(position.Y * width) + position.X] != searchWord[0])
        {
            return 0;
        }
        int validCount = 0;
        foreach (IntVector2 direction in IntVector2.AllDirections)
        {
            bool valid = true;

            for (int i = 1; i < searchWord.Length; i++)
            {
                IntVector2 newPosition = position + (direction * i);
                if (!BoundsCheck(newPosition, width, height))
                {
                    valid = false;
                    break;
                }

                if (dataGrid[(newPosition.Y * width) + newPosition.X] != searchWord[i])
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                validCount++;

            }
        }

        return validCount;

    }

    public bool BoundsCheck(IntVector2 position, int width, int height)
    {
        return !(position.X < 0 || position.Y < 0 || position.X >= width || position.Y >= height);
    }

    public int TestTermPart2(IntVector2 position, char[] searchWord, char[] dataGrid, int width, int height)
    {
        int validCount = 0;
        if (searchWord.Length % 2 == 1)
        {
            int midPoint = searchWord.Length / 2;
            int length = midPoint;

            if (dataGrid[(position.Y * width) + position.X] == searchWord[midPoint])
            {
                bool valid = true;
                for (int i = 1; i <= length; i++)
                {
                    bool result1 = TestDiagonal(position,new IntVector2(1,1),midPoint,i,searchWord,dataGrid,width,height);
                    bool result2 = TestDiagonal(position,new IntVector2(-1,1),midPoint,i,searchWord,dataGrid,width,height);
                    if(!(result1 && result2))
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    validCount++;
                }
            }
        }

        return validCount;
    }


    public bool TestDiagonal(IntVector2 position, IntVector2 direction,int midPoint,int positionOffset,char[] searchWord, char[] dataGrid, int width, int height)
    {
        IntVector2 offset = direction * positionOffset;

        IntVector2 diag1 = position + offset;
        IntVector2 diag2 = position - offset;
        if (BoundsCheck(diag1, width, height) && BoundsCheck(diag2, width, height))
        {
            char diag1Char = dataGrid[(diag1.Y * width) + diag1.X];
            char diag2Char = dataGrid[(diag2.Y * width) + diag2.X];
            bool matches = ((searchWord[midPoint - positionOffset] == diag1Char && searchWord[midPoint + positionOffset] == diag2Char) || searchWord[midPoint - positionOffset] == diag2Char && searchWord[midPoint + positionOffset] == diag1Char);
            return matches;
        }
        return false;
    }


}