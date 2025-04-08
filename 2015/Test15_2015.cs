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
using static Test15_2015;
using static Test8_2023;

public class Test15_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 15;
    }

    public List<Ingredient> m_ingredients = new List<Ingredient>();

    public override void Execute()
    {
        foreach (string line in m_dataFileContents)
        {
            string[] tokens = line.Split(new char[] { ':', ' ', ',' });
            string name = tokens[0];
            int capacity = int.Parse(tokens[3]);
            int durability = int.Parse(tokens[6]);
            int flavour = int.Parse(tokens[9]);
            int texture = int.Parse(tokens[12]);
            int calories = int.Parse(tokens[15]);

            Ingredient ingredient = new Ingredient(name, capacity, durability, flavour, texture, calories);
            m_ingredients.Add(ingredient);
        }

        int numSpoons = 100;

        int highScore = 0;
        List<int> ingredientAmounts = new List<int>();
        for (int i = 0; i < m_ingredients.Count; ++i)
        {
            ingredientAmounts.Add(0);
        }

        CalcScore(m_ingredients, ingredientAmounts, 0, numSpoons, ref highScore,IsPart2?500:0);

        DebugOutput($"Highest Score is {highScore}");


    }

    public void CalcScore(List<Ingredient> ingredients, List<int> ingredientAmounts, int index, int numSpoons, ref int highScore, int caloriesGoal)
    {
        //ingredientAmounts[0] = 44;
        //ingredientAmounts[1] = 56;
        int sum = ingredientAmounts.Sum();
        if (sum == numSpoons)
        {
            Ingredient total = null;
            for (int i = 0; i < ingredientAmounts.Count; ++i)
            {
                if (ingredientAmounts[i] > 0)
                {
                    if (total == null)
                    {
                        total = ingredients[i].Multiply(ingredientAmounts[i]);
                    }
                    else
                    {
                        Ingredient mul = ingredients[i].Multiply(ingredientAmounts[i]);
                        total = total.Add(mul);
                    }
                }
            }

            if (caloriesGoal == 0 || total.Calories == caloriesGoal)
            {


                int totalScore = total.Score();
                if (totalScore > highScore)
                {
                    highScore = totalScore;
                    DebugOutput($"{highScore}  {string.Join(',', ingredientAmounts)}");
                }
            }
        }


        if (index < ingredients.Count)
        {
            for (int i = 0; i <= numSpoons; ++i)
            {
                ingredientAmounts[index] = i;
                CalcScore(ingredients, ingredientAmounts, index + 1, numSpoons, ref highScore,caloriesGoal);

            }
        }
    }



    public record Ingredient(string Name, int Capacity, int Durability, int Flavour, int Texture, int Calories)
    {
        public Ingredient Multiply(int multiplier)
        {
            return new Ingredient(Name, Capacity * multiplier, Durability * multiplier, Flavour * multiplier, Texture * multiplier, Calories * multiplier);
        }

        public Ingredient Add(Ingredient rhs)
        {
            Ingredient i = new Ingredient(Name, Capacity + rhs.Capacity, Durability + rhs.Durability, Flavour + rhs.Flavour, Texture + rhs.Texture, Calories + rhs.Calories);
            //Ingredient i2 = new Ingredient(Name, Math.Max(0, i.Capacity), Math.Max(0, i.Durability), Math.Max(0, i.Flavour), Math.Max(0, i.Texture), Math.Max(0, i.Calories));
            return i;
        }

        public int Score()
        {
            if (Capacity <= 0 || Durability <= 0 || Flavour <= 0 || Texture <= 0)
            {
                return 0;
            }

            return Capacity * Durability * Flavour * Texture;
        }

    }


}