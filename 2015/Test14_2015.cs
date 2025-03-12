using AdventOfCode.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Test8_2023;

public class Test14_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 14;
    }


    public override void Execute()
    {
        //Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.
        List<Reindeer> allReindeer = new List<Reindeer>();
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(' ');
            string name = tokens[0];
            int speed = int.Parse(tokens[3]);
            int duration = int.Parse(tokens[6]);
            int rest = int.Parse(tokens[13]);

            //DebugOutput($"{name}  {speed}  {duration}  {rest}");
            Reindeer reindeer = new Reindeer(name, speed, duration, rest);
            allReindeer.Add(reindeer);
        }


        int stepCount = IsTestInput?1000:2503;
        for(int i=0;i<stepCount; i++)
        {
            foreach(Reindeer r in allReindeer)
            {
                r.Step();
            }

            if(IsPart2)
            {
                foreach(var r in allReindeer.FindAll(x=>x.Distance==allReindeer.Max(x=>x.Distance)))
                {
                    r.Score += 1;
                }
            }
        }


        if(IsPart2)
        {
            Reindeer highestScore = allReindeer.Find(x=>x.Score == allReindeer.Max( x=>x.Score));
            DebugOutput($"The winner is {highestScore.Name} with {highestScore.Score}");
        }
        else
        {
            Reindeer furthestReindeer = allReindeer.Find(x=>x.Distance== allReindeer.Max( x=>x.Distance));
            DebugOutput($"The winner is {furthestReindeer.Name} with {furthestReindeer.Distance}");
        }
    }

    private class Reindeer
    {
        public string Name;
        public int Speed;
        public int Duration;
        public int Rest;

        public int Distance;
        
        public bool Resting;

        public int Count;

        public int Score;

        public Reindeer(string name,int speed,int duration,int rest)
        {
            Name = name;
            Speed = speed;
            Duration = duration;
            Rest = rest;
        }

        public void Reset()
        {
            Count = 0;
            Distance = 0;
            Resting = false;
        }

        public void Step()
        {
            if(!Resting)
            {
                if(Count < Duration)
                {
                    Distance+= Speed;
                    Count++;
                }
                if(Count == Duration)
                {
                    Resting = true;
                    Count = 0;
                }
            }
            else
            {
                Count++;
                if(Count == Rest)
                {
                    Resting = false;
                    Count = 0;
                }
            }
        }

    }


}