using System;
using System.Collections.Generic;
using System.Security.AccessControl;

public class Test8_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 8;
    }

    private int m_screenWidth = 50;
    private int m_screenHeight = 6;

    private bool[] m_screen = null;
    private bool[] m_tempRow = null;
    private bool[] m_tempColumn = null;
    
    public override void Execute()
    {
        m_screen = new bool[m_screenWidth * m_screenHeight];
        m_tempRow= new bool[m_screenWidth];
        m_tempColumn = new bool[m_screenHeight];

        foreach (string line in m_dataFileContents)
        {
            if (line.StartsWith("rect"))
            {
                string[] tokens = line.Split(new char[] { ' ', 'x' }, StringSplitOptions.RemoveEmptyEntries);
                int w = int.Parse(tokens[tokens.Length - 2]);
                int h = int.Parse(tokens[tokens.Length - 1]);
                Rect(w,h);
                
            }
            else if (line.StartsWith("rotate row"))
            {
                string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int row = int.Parse(tokens[2].Replace("y=",""));
                int amount = int.Parse(tokens.Last());
                RotateRow(row,amount);
            }
            else if (line.StartsWith("rotate column"))
            {
                string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int column = int.Parse(tokens[2].Replace("x=",""));
                int amount = int.Parse(tokens.Last());
                RotateColumn(column,amount);
            }
            //
            
        }
        
        DebugOutput(Helper.DrawGridHash(m_screen,m_screenWidth,m_screenHeight));

        int count = Array.FindAll(m_screen, x => x == true).Length;
        
        DebugOutput($"There are {count} illuminated screens.");
    }

    public void Rect(int width, int height)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                m_screen[(i * m_screenWidth) + j] = true;
            }
        }
    }

    
    public void RotateRow(int row, int shift)
    {
        for (int i = 0; i < m_screenWidth; i++)
        {
            m_tempRow[i] = m_screen[(row*m_screenWidth) + i];
        }

        for (int i = 0; i < m_screenWidth; i++)
        {
            int index = i + shift;
            if (index >= m_screenWidth)
            {
                index -= m_screenWidth;
            }
            m_screen[(row * m_screenWidth) + index] = m_tempRow[i];    
        }
    }

    public void RotateColumn(int column, int shift)
    {
        for (int i = 0; i < m_tempColumn.Length; i++)
        {
            m_tempColumn[i] = m_screen[(i * m_screenWidth) + column];
        }

        for (int i = 0; i < m_screenHeight; i++)
        {
            int index = i + shift;
            if (index >= m_screenHeight)
            {
                index -= m_screenHeight;
            }
            
            m_screen[(index * m_screenWidth) + column] = m_tempColumn[i];
        }
        
    }
    
    
}