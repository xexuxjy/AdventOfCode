using AdventOfCode.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Test8_2023;

public class Test20_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 20;
    }


    public override void Execute()
    {

        int targetScore = IsTestInput ? 70 : 29000000;

        int lowest = int.MaxValue;


        int perHouseDelivery = IsPart2 ? 11 : 10;

        int deliveryLimit = 50;

        int numIterations = targetScore / 10;

        Parallel.For(0, numIterations, counter =>
        {
            int score = 0;

            List<int> factorList = new List<int>();

            int max = (int)Math.Sqrt(counter);  // Round down

            for (int factor = 1; factor <= max; factor++)
            {

                if (counter % factor == 0)
                {
                    if (!IsPart2 || counter <= (factor * deliveryLimit))
                    {

                        factorList.Add(factor);
                        score += (factorList.Last() * perHouseDelivery);
                    }

                    if (factor != counter / factor)
                    {
                        if (!IsPart2 || counter <= ((counter/factor) * deliveryLimit))
                        {
                            // Don't add the square root twice!  Thanks Jon
                            factorList.Add(counter / factor);
                            score += (factorList.Last() * perHouseDelivery);

                        }
                    }

                    if (score >= targetScore)
                    {
                        if (counter < lowest)
                        {
                            lowest = counter;
                            break;
                        }
                    }
                }
            }

        });

        DebugOutput($"Reached target at {lowest}");


    }


}