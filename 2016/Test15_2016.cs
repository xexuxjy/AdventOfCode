using System;
using System.Collections.Generic;
using System.ComponentModel;

public class Test15_2016 : BaseTest
{
    public List<Disk> Disks = new List<Disk>();

    public override void Initialise()
    {
        Year = 2016;
        TestID = 15;
    }

    public override void Execute()
    {
        foreach (string line in m_dataFileContents)
        {
            //Disc #1 has 5 positions; at time=0, it is at position 4.
            string[] tokens = line.Split(' ');
            Disk disk = new Disk()
            {
                Id = tokens[1], Slots = int.Parse(tokens[3]), StartPosition = int.Parse(tokens[11].Replace(".", ""))
            };
            Disks.Add(disk);
        }

        if (IsPart2)
        {
            Disks.Add(new Disk(){Id="Extra",Slots=11,StartPosition=0});
        }
        
        int startTime = 1;

        while (startTime < 10000000)
        {
            int time = startTime;
            int tokenPosition;
            bool valid = true;
            foreach (Disk disk in Disks)
            {
                if (disk.Position(time) != 0)
                {
                    valid = false;
                    break;
                }

                time++;
            }

            if (valid)
            {
                DebugOutput($"Finished search : {startTime-1}");
                break;
            }

            startTime++;
        }

        long product = 1;
        foreach (Disk disk in Disks)
        {
            product *= disk.Slots;
        }
        DebugOutput($"Product : {product}");
        
    }

    public class Disk
    {
        public string Id;
        public int Slots;
        public int StartPosition;

        public int Position(int time)
        {
            return (time + StartPosition) % Slots;
        }
    }
}