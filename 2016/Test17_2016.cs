using System;
using System.Collections.Generic;
using System.Security.Cryptography;

public class Test17_2016 : BaseTest
{
    public override void Initialise()
    {
        Year = 2016;
        TestID = 17;
    }

    public override void Execute()
    {
        MaxDepth = IsPart1 ? 100 : 10000;
        
        using (MD5 md5 = MD5.Create())
        {
            IntVector2 startLocation = new  IntVector2(0, 0);
            IntVector2 endLocation = new  IntVector2(3, 3);

            //Move("",startLocation,endLocation,md5,0,"DDRRRD");
            Move("",startLocation,endLocation,md5,0);
        }

        if (IsPart1)
        {
            DebugOutput($"Best movepath is {BestPath}");
        }
        else
        {
            DebugOutput($"Longest movepath is {BestPath.Length}");
        }

    }

    private int MaxDepth;
    
    private string BestPath = "";
    public void Move(string movePath,IntVector2 currentLocation,IntVector2 endLocation ,MD5 md5,int depth,string fixedPath=null)
    {
        if (depth > MaxDepth)
        {
            return;
        }

        if (IsPart1)
        {
            if (BestPath != "" && BestPath.Length < movePath.Length)
            {
                return;
            }
        }

        if (currentLocation == endLocation)
        {
            if (IsPart1)
            {
                if (BestPath == "" || movePath.Length < BestPath.Length)
                {
                    BestPath = movePath;
                }
            }
            else
            {
                if (BestPath == "" || movePath.Length > BestPath.Length)
                {
                    BestPath = movePath;
                }
            }
        }
        else
        if (currentLocation.X < 0 || currentLocation.Y < 0 || currentLocation.X > endLocation.X ||
            currentLocation.Y > endLocation.Y)
        {
            // out of bounds
        }
        else
        {
            string moveHash = Helper.FormattedHash(m_dataFileContents[0]+movePath, md5);

            if (fixedPath != null)
            {
                if (depth < fixedPath.Length)
                {
                    switch (fixedPath[depth])
                    {
                        case 'U':
                            Move(movePath + "U", currentLocation + IntVector2.Down, endLocation, md5, depth + 1,
                                fixedPath);
                            break;
                        case 'D':
                            Move(movePath + "D", currentLocation + IntVector2.Up, endLocation, md5, depth + 1,
                                fixedPath);
                            break;
                        case 'L':
                            Move(movePath + "L", currentLocation + IntVector2.Left, endLocation, md5, depth + 1,
                                fixedPath);
                            break;
                        case 'R':
                            Move(movePath + "R", currentLocation + IntVector2.Right, endLocation, md5, depth + 1,
                                fixedPath);
                            break;

                    }
                }
                else
                {
                    int ibreak = 0;
                }
            }
            else
            {
                if (IsOpen(moveHash[0]))
                {
                    Move(movePath + "U", currentLocation + IntVector2.Down, endLocation, md5, depth + 1,fixedPath);
                }

                if (IsOpen(moveHash[1]))
                {
                    Move(movePath + "D", currentLocation + IntVector2.Up, endLocation, md5, depth + 1,fixedPath);
                }

                if (IsOpen(moveHash[2]))
                {
                    Move(movePath + "L", currentLocation + IntVector2.Left, endLocation, md5, depth + 1,fixedPath);
                }

                if (IsOpen(moveHash[3]))
                {
                    Move(movePath + "R", currentLocation + IntVector2.Right, endLocation, md5, depth + 1,fixedPath);
                }
            }
        }

    }
    
    public bool IsOpen(char c)
    {
        return (c == 'b' || c == 'c' || c == 'd' || c == 'e' || c == 'f');
    }
    
    
    
}