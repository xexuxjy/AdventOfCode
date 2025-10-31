using System;
using System.Collections.Generic;
using System.Text;

public class Test18_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 18;
    }

    public override void Execute()
    {
        List<string> rows = new List<string>();
        rows.Add(m_dataFileContents[0]);
        int numRows = IsPart1?40:400000;
        numRows--;
        
        for (int i = 0; i < numRows; i++)
        {
            rows.Add(BuildLine(rows[i]));
        }

        int safeTiles = 0;
        foreach (string row in rows)
        {
            //DebugOutput(row);
            safeTiles += row.Count(x => x == SAFE);
        }

        DebugOutput($"There are {safeTiles} safe tiles.");
    }

    public char SAFE = '.';
    public char TRAP = '^';

    public string BuildLine(string previousLine)
    {
        char leftTile = ' ';
        char centerTile = ' ';
        char rightTile = ' ';


        StringBuilder sb = new StringBuilder();
        
        for (int i = 0; i < previousLine.Length; i++)
        {
            leftTile = i == 0 ? SAFE : previousLine[i - 1];
            centerTile = previousLine[i];
            rightTile = i == previousLine.Length - 1 ? SAFE : previousLine[i + 1];
            sb.Append(IsTrap(leftTile, centerTile, rightTile) ? TRAP : SAFE);
        }

        return sb.ToString();
    }

/*
 *
    Its left and center tiles are traps, but its right tile is not.
    Its center and right tiles are traps, but its left tile is not.
    Only its left tile is a trap.
    Only its right tile is a trap.

 */
    public bool IsTrap(char leftTile, char centerTile, char rightTile)
    {
        bool isTrap = false;
        if (leftTile == TRAP && centerTile == SAFE && rightTile == SAFE)
        {
            return true;
        }

        if (leftTile == SAFE && centerTile == SAFE && rightTile == TRAP)
        {
            return true;
        }

        if (leftTile == TRAP && centerTile == TRAP && rightTile == SAFE)
        {
            return true;
        }

        if (leftTile == SAFE && centerTile == TRAP && rightTile == TRAP)
        {
            return true;
        }

        return false;
    }
}