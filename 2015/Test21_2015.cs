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
using System.Xml.XPath;
using static Test21_2015;
using static Test8_2023;

public class Test21_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 21;
    }

    /*
     * Weapons:    Cost  Damage  Armor
Dagger        8     4       0
Shortsword   10     5       0
Warhammer    25     6       0
Longsword    40     7       0
Greataxe     74     8       0

Armor:      Cost  Damage  Armor
Leather      13     0       1
Chainmail    31     0       2
Splintmail   53     0       3
Bandedmail   75     0       4
Platemail   102     0       5

Rings:      Cost  Damage  Armor
Damage +1    25     1       0
Damage +2    50     2       0
Damage +3   100     3       0
Defense +1   20     0       1
Defense +2   40     0       2
Defense +3   80     0       3
    */


    public override void Execute()
    {
        List<Item> weapons = new List<Item>();
        weapons.Add(new Item() { Type = "Weapon", Name = "Dagger", Cost = 8, Damage = 4 });
        weapons.Add(new Item() { Type = "Weapon", Name = "Shortsword", Cost = 10, Damage = 5 });
        weapons.Add(new Item() { Type = "Weapon", Name = "Warhammer", Cost = 25, Damage = 6 });
        weapons.Add(new Item() { Type = "Weapon", Name = "Longsword", Cost = 40, Damage = 7 });
        weapons.Add(new Item() { Type = "Weapon", Name = "Greataxe", Cost = 74, Damage = 8 });


        List<Item> armours = new List<Item>();
        // allow for null option
        armours.Add(null);
        armours.Add(new Item() { Type = "Armour", Name = "Leather", Cost = 13, Armour = 1 });
        armours.Add(new Item() { Type = "Armour", Name = "Chainmail", Cost = 31, Armour = 2 });
        armours.Add(new Item() { Type = "Armour", Name = "Splintmail", Cost = 53, Armour = 3 });
        armours.Add(new Item() { Type = "Armour", Name = "Bandedmail", Cost = 75, Armour = 4 });
        armours.Add(new Item() { Type = "Armour", Name = "Platemail", Cost = 102, Armour = 5 });


        List<Item> rings = new List<Item>();
        rings.Add(new Item() { Type = "Ring", Name = "Damage+1", Cost = 25, Damage = 1 });
        rings.Add(new Item() { Type = "Ring", Name = "Damage+2", Cost = 50, Damage = 2 });
        rings.Add(new Item() { Type = "Ring", Name = "Damage+3", Cost = 100, Damage = 3 });
        rings.Add(new Item() { Type = "Ring", Name = "Defense+1", Cost = 20, Armour = 1 });
        rings.Add(new Item() { Type = "Ring", Name = "Defense+2", Cost = 40, Armour = 2 });
        rings.Add(new Item() { Type = "Ring", Name = "Defense+3", Cost = 80, Armour = 3 });


        List<List<Item>> ringPossibilities = new List<List<Item>>();

        // allow for no rings
        ringPossibilities.Add(new List<Item>());

        foreach (Item item in rings)
        {
            List<Item> itemList = new List<Item>();
            itemList.Add(item);
            ringPossibilities.Add(itemList);
        }

        //foreach (Item[] combo in Combinations.BuildOptions(2, rings.ToArray()))

        foreach (var combo in Combinations.GetPermutations(rings,2))
        {
            List<Item> itemList = new List<Item>();
            itemList.AddRange(combo);
            ringPossibilities.Add(itemList);
        }

        if (IsPart1)
        {
            int minimumGold = int.MaxValue;
            Target bestPlayer = null;
            List<Target> possiblePlayers = BuildPossiblePlayers(weapons, armours, ringPossibilities);

            foreach (Target player in possiblePlayers)
            {
                if (player.TotalCost < minimumGold)
                {

                    Target boss = new Target();
                    boss.Name = "Boss";
                    boss.Health = 104;
                    boss.Armour = 1;
                    boss.Damage = 8;


                    while (player.Health > 0 && boss.Health > 0)
                    {
                        ResolveAttack(player, boss);
                        if (boss.Health <= 0)
                        {
                            break;
                        }

                        ResolveAttack(boss, player);
                    }

                    if (player.Health > 0)
                    {
                        // player won
                        minimumGold = player.TotalCost;
                        bestPlayer = player;
                    }


                }
            }

            DebugOutput("The cheapest win was : " + bestPlayer.TotalCost);
            DebugOutput(bestPlayer.DebugInfo);
        }
        else
        {
            int maxGold = int.MinValue;
            Target bestPlayer = null;
            List<Target> possiblePlayers = BuildPossiblePlayers(weapons, armours, ringPossibilities);

            foreach (Target player in possiblePlayers)
            {
                if (player.TotalCost > maxGold)
                {

                    Target boss = new Target();
                    boss.Name = "Boss";
                    boss.Health = 104;
                    boss.Armour = 1;
                    boss.Damage = 8;


                    while (player.Health > 0 && boss.Health > 0)
                    {
                        ResolveAttack(player, boss);
                        if (boss.Health <= 0)
                        {
                            break;
                        }

                        ResolveAttack(boss, player);
                    }

                    if (player.Health <= 0)
                    {
                        // player won
                        maxGold= player.TotalCost;
                        bestPlayer = player;
                    }


                }
            }

            DebugOutput("The most expensive loss was : " + bestPlayer.TotalCost);
            DebugOutput(bestPlayer.DebugInfo);

        }

    }



    public void ResolveAttack(Target attacker, Target defender)
    {
        int damage = Math.Max(1, attacker.Damage - defender.Armour);
        defender.Health -= damage;
    }

    public List<Target> BuildPossiblePlayers(List<Item> weapons, List<Item> armours, List<List<Item>> ringPossibilities)
    {
        List<Target> players = new List<Target>();

        int count = 0;
        foreach (Item weapon in weapons)
        {
            foreach (Item armour in armours)
            {
                foreach (List<Item> ringItems in ringPossibilities)
                {
                    Target player = new Target();
                    players.Add(player);
                    player.Name = "Player-" + (count++);

                    player.AddItem(weapon);
                    player.AddItem(armour);
                    foreach (Item ring in ringItems)
                    {
                        player.AddItem(ring);
                    }
                }
            }
        }

        return players;


    }

    public class Item
    {
        public string Type;
        public string Name;
        public int Cost;
        public int Damage;
        public int Armour;
    }


    public class Target
    {
        public string Name;
        public int Armour;
        public int Damage;
        public int Health = 100;
        public int TotalCost = 0;

        public List<Item> Items = new List<Item>();
        public void AddItem(Item item)
        {
            if (item != null)
            {
                Items.Add(item);
                Damage = Items.Sum(x => x.Damage);
                Armour = Items.Sum(x => x.Armour);
                TotalCost = Items.Sum(x => x.Cost);
            }
        }

        public string DebugInfo
        {
            get
            {
                StringBuilder result = new StringBuilder();
                result.AppendLine($"{Name}   Damage[{Damage}]  Armour[{Armour}] Health [{Health}] TotalCost [{TotalCost}]");
                result.AppendLine("Items:");
                foreach (Item item in Items)
                {
                    result.AppendLine($"\t {item.Name}");
                }
                return result.ToString();
            }
        }
    }

}