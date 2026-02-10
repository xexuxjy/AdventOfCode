using System;
using System.Collections.Generic;
using System.Numerics;

public class Test20_2017 : BaseTest
{
    public override void Initialise()
    {
        Year = 2017;
        TestID = 20;
    }

    public override void Execute()
    {
        List<Particle> particles = new List<Particle>();
        int particleCount = 0;
        foreach (string line in m_dataFileContents)
        {
            string[] components = line.Split(", ");
            Particle particle = new Particle();
            particle.Id = particleCount++;
            
            particle.Position = ReadIV3(components[0]);
            particle.Velocity = ReadIV3(components[1]);
            particle.Acceleration = ReadIV3(components[2]);
            particles.Add(particle);
        }


        if (IsPart1)
        {
            int iterations = 10000;

            for (int i = 0; i < iterations; i++)
            {
                foreach (Particle particle in particles)
                {
                    particle.Velocity += particle.Acceleration;
                    particle.Position += particle.Velocity;
                    particle.Distances += particle.Position.ManhattanDistance(IntVector3.Zero);
                }
            }

            Particle closestParticle = particles.OrderBy(p => p.Distances).First();

            DebugOutput($"The closest particle on average is {closestParticle.Id}");
        }
        else
        {
            int iterations = 1000;

            Dictionary<IntVector3, List<Particle>> particleCounts = new Dictionary<IntVector3, List<Particle>>();
            for (int i = 0; i < iterations; i++)
            {
                particleCounts.Clear();

                foreach (Particle particle in particles)
                {
                    particle.Velocity += particle.Acceleration;
                    particle.Position += particle.Velocity;

                    if (!particleCounts.TryGetValue(particle.Position, out var list))
                    {
                        list = new List<Particle>();
                        particleCounts[particle.Position] = list;
                    }

                    list.Add(particle);
                }

                foreach (List<Particle> list in particleCounts.Values)
                {
                    if (list.Count > 1)
                    {
                        foreach (Particle particle in list)
                        {
                            particles.Remove(particle);
                        }
                    }
                }
            }
            DebugOutput($"After {iterations} iterations there are {particles.Count} particles left");
        }
    }

    public IntVector3 ReadIV3(string value)
    {
        int openIndex = value.IndexOf('<');
        int closeIndex = value.IndexOf('>');
        string values =  value.Substring(openIndex + 1, closeIndex - openIndex - 1);
        string[] components = values.Split(','); 
        IntVector3 result = new IntVector3();
        result.X = int.Parse(components[0]);
        result.Y = int.Parse(components[1]);
        result.Z = int.Parse(components[2]);
        return result;
    }
}

public class Particle
{
    public int Id;
    public IntVector3 Position;
    public IntVector3 Velocity;
    public IntVector3 Acceleration;
    
    public long Distances; 
}