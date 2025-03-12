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

public class Test12_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 12;
    }


    public override void Execute()
    {
        foreach (string line in m_dataFileContents)
        {

            using (JsonTextReader reader = new JsonTextReader(new StringReader(line)))
            {
                int sum = 0;
                JObject rootObject = JObject.Parse(line);
                HandleNode(rootObject, ref sum);
                DebugOutput($"Sum  is {sum}");
            }
        }
    }


    public void HandleNode(JToken token, ref int sum)
    {
        bool objectHasRedProperty = false;
        if (IsPart2)
        {
            foreach (JToken childToken in token.Children())
            {
                if (childToken is JProperty)
                {
                    JProperty property = childToken as JProperty;
                    if (property.Value.Type == JTokenType.String)
                    {
                        //DebugOutput(property.Value.ToString());
                        if (((string)property.Value) == "red")
                        {
                            //redCount++;
                            objectHasRedProperty = true;
                        }
                    }
                }
            }
        }
        if (!objectHasRedProperty)
        {
            foreach (JToken childToken in token.Children())
            {

                if (childToken.Type == JTokenType.Integer)
                {
                    sum += childToken.Value<int>();
                }
                HandleNode(childToken, ref sum);
            }

        }

    }
}