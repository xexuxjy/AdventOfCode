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
using static Test21_2015;
using static Test8_2023;

public class Test22_2015 : BaseTest
{
    public override void Initialise()
    {
        Year = 2015;
        TestID = 22;
    }


    public Target BestPlayer=null;

    public override void Execute()
    {

        List<SpellEffect> spellEffects = new List<SpellEffect>();

        spellEffects.Add(new SpellEffect() { Name = SpellEffect.MAGICMISSILE, Cost = 53 });
        spellEffects.Add(new SpellEffect() { Name = SpellEffect.DRAIN, Cost = 73 });
        spellEffects.Add(new SpellEffect() { Name = SpellEffect.SHIELD, Cost = 113, Duration = 6 });
        spellEffects.Add(new SpellEffect() { Name = SpellEffect.POISON, Cost = 173, Duration = 6 });
        spellEffects.Add(new SpellEffect() { Name = SpellEffect.RECHARGE, Cost = 229, Duration = 5 });


        Target player = new Target();
        player.Name = "Player";
        player.Health = IsTestInput ? 10 : 50;
        player.Mana = IsTestInput ? 250 : 500;
        player.BaseTest = this;

        Target boss = new Target();
        boss.Name = "Boss";
        boss.Health = IsTestInput ? 14 : 58;
        boss.Damage = IsTestInput ? 8 : 9;
        boss.BaseTest = this;

        ChooseSpell(0, player, boss,spellEffects);

        //ApplySpell(SpellEffect.POISON, player, boss, spellEffects);
        //ApplySpell(SpellEffect.MAGICMISSILE, player, boss, spellEffects);



        //ApplySpell(SpellEffect.POISON, player, boss, spellEffects);
        //ApplySpell(SpellEffect.MAGICMISSILE, player, boss, spellEffects);
        //ApplySpell(SpellEffect.RECHARGE, player, boss, spellEffects);
        //ApplySpell(SpellEffect.POISON, player, boss, spellEffects);
        //ApplySpell(SpellEffect.MAGICMISSILE, player, boss, spellEffects);
        //ApplySpell(SpellEffect.SHIELD, player, boss, spellEffects);
        //ApplySpell(SpellEffect.POISON, player, boss, spellEffects);
        //ApplySpell(SpellEffect.DRAIN, player, boss, spellEffects);
        //ApplySpell(SpellEffect.SHIELD, player, boss, spellEffects);





        if (BestPlayer != null)
        {
            DebugOutput($"Best mana spent is : {BestPlayer.TotalManaSpent} with spells : {string.Join(',', BestPlayer.CastHistory)}");
        }
        else
        {
            DebugOutput("Didnt't find anything");
        }

    }

    public void ApplySpell(string spellName, Target playerTarget, Target bossTarget, List<SpellEffect> allSpells)
    {
        SpellEffect spellEffect = allSpells.Find(x => x.Name == spellName);

        DebugOutput("Start Player Round");
        playerTarget.StartRound(bossTarget);

        if (bossTarget.Health <= 0)
        {
            if (BestPlayer == null || playerTarget.TotalManaSpent < BestPlayer.TotalManaSpent)
            {
                BestPlayer = playerTarget;
            }
            return;
        }

        if(!playerTarget.CanCastSpell(spellEffect))
        {
            int ibreak = 0;
        }


        DebugOutput($"Player casting {spellEffect.Name}");
        playerTarget.CastSpell(spellEffect, bossTarget);
        if (bossTarget.Health <= 0)
        {
            if (BestPlayer == null || playerTarget.TotalManaSpent < BestPlayer.TotalManaSpent)
            {
                BestPlayer = playerTarget;
            }
            return;
        }

        DebugOutput("Start Boss Round");
        playerTarget.StartRound(bossTarget);

        if (bossTarget.Health <= 0)
        {
            if (BestPlayer == null || playerTarget.TotalManaSpent < BestPlayer.TotalManaSpent)
            {
                BestPlayer = playerTarget;
            }
            return;
        }


        playerTarget.TakeDamage(bossTarget.Damage);

        if (playerTarget.Health <= 0)
        {
            return;
        }

    }





    public void ChooseSpell(int depth, Target playerTarget, Target bossTarget, List<SpellEffect> allSpells)
    {
        if (depth > 50 || (BestPlayer != null && playerTarget.TotalManaSpent >= BestPlayer.TotalManaSpent))
        {
            return;
        }


        //List<SpellEffect> choices = new List<SpellEffect>();
        //foreach (SpellEffect spellEffect in allSpells)
        //{
        //    if (spellEffect.Cost < playerTarget.Mana)
        //    {
        //        if (!playerTarget.ActiveEffects.Exists(x => x.Name == spellEffect.Name))
        //        {
        //            choices.Add(spellEffect);
        //        }
        //    }
        //}

        foreach (SpellEffect spellEffect in allSpells)
        {

            Target newBoss = Target.Copy(bossTarget);
            Target newPlayer = Target.Copy(playerTarget);

            if(IsPart2)
            {
                newPlayer.Health -= 1;
                if (newPlayer.Health <= 0)
                {
                    continue;
                }
            }

            //DebugOutput("Start Player Round");
            newPlayer.StartRound(newBoss);

            if (newBoss.Health <= 0)
            {
                if (BestPlayer == null || newPlayer.TotalManaSpent < BestPlayer.TotalManaSpent)
                {
                    BestPlayer = newPlayer;
                }
                continue;
            }


            if(!newPlayer.CanCastSpell(spellEffect))
            {
                continue;
            }


            newPlayer.CastSpell(spellEffect, newBoss);
            if (newBoss.Health <= 0)
            {
                if (BestPlayer == null || newPlayer.TotalManaSpent < BestPlayer.TotalManaSpent)
                {
                    BestPlayer = newPlayer;
                }
                continue;
            }

            //DebugOutput("Start Boss Round");
            newPlayer.StartRound(newBoss);

            if (newBoss.Health <= 0)
            {
                if (BestPlayer == null || newPlayer.TotalManaSpent < BestPlayer.TotalManaSpent)
                {
                    BestPlayer = newPlayer;
                }
                continue;
            }


            newPlayer.TakeDamage(newBoss.Damage);

            if (newPlayer.Health <= 0)
            {
                continue;
            }

            ChooseSpell(depth + 1, newPlayer, newBoss, allSpells);
        }
    }



    public class Target
    {
        public string Name="";
        public int Armour;
        public int Damage;
        public int Health = 50;
        public int Mana = 500;

        public int TotalManaSpent;

        public List<SpellEffect> ActiveEffects = new List<SpellEffect>();
        public List<string> CastHistory = new List<string>();

        public BaseTest BaseTest;


        public void TakeDamage(int amount)
        {
            int damage = Math.Max(1, amount - Armour);
            Health -= damage;
            //BaseTest.DebugOutput($"{Name} takes {damage} damage.  Health now {Health}");


        }

        public bool CanCastSpell(SpellEffect spellEffect)
        {
            return  spellEffect.Cost <= Mana && (spellEffect.Duration == 0 || !ActiveEffects.Exists(x=>x.Name == spellEffect.Name));
        }


        public void StartRound(Target target)
        {
            foreach (SpellEffect spellEffect in ActiveEffects)
            {
                spellEffect.ApplyTurnEffect(this, target,BaseTest);
            }

            ActiveEffects.RemoveAll(x => x.CurrentDuration == 0);
        }



        public void CastSpell(SpellEffect spellEffect, Target target)
        {
            CastHistory.Add(spellEffect.Name);
            TotalManaSpent += spellEffect.Cost;

            spellEffect.ApplyImmediateEffect(this, target,BaseTest);
            if (spellEffect.Duration > 0)
            {
                ActiveEffects.Add(spellEffect);
            }
        }

        public static Target Copy(Target target)
        {
            Target newTarget = new Target();
            newTarget.BaseTest = target.BaseTest;
            newTarget.Name = target.Name;
            newTarget.Armour = target.Armour;
            newTarget.Damage = target.Damage;
            newTarget.Health = target.Health;
            newTarget.Mana = target.Mana;

            newTarget.TotalManaSpent = target.TotalManaSpent;
            newTarget.CastHistory.AddRange(target.CastHistory);

            foreach (SpellEffect spellEffect in target.ActiveEffects)
            {
                newTarget.ActiveEffects.Add(SpellEffect.Copy(spellEffect));
            }
            return newTarget;
        }

    }



    public class SpellEffect
    {
        public const string MAGICMISSILE = "MagicMissile";
        public const string DRAIN = "Drain";
        public const string SHIELD = "Shield";
        public const string POISON = "Poison";
        public const string RECHARGE = "Recharge";


        public string Name="";
        public int Duration;
        public int CurrentDuration;
        public int Cost;




        public void ApplyImmediateEffect(Target owner, Target target,BaseTest baseTest)
        {
            CurrentDuration = Duration;

            if (Name == MAGICMISSILE)
            {
                target.TakeDamage(4);
            }
            else if (Name == DRAIN)
            {
                target.TakeDamage(2);
                owner.Health += 2;
                //baseTest.DebugOutput($"{owner.Name} gains 2 health");
            }
            else if (Name == SHIELD)
            {
                owner.Armour += 7;
                //baseTest.DebugOutput($"{owner.Name} gains 7 armour");
            }

            owner.Mana -= Cost;

        }
        public void ApplyTurnEffect(Target owner, Target target,BaseTest baseTest)
        {

            if (Name == POISON)
            {
                //baseTest.DebugOutput($"POISON");
                target.TakeDamage(3);
            }
            else if (Name == RECHARGE)
            {
                //baseTest.DebugOutput($"{owner.Name} gets 101 mana");
                owner.Mana += 101;
            }

            CurrentDuration--;

            if (CurrentDuration == 0)
            {
                if (Name == SHIELD)
                {
                    owner.Armour -= 7;
                    //baseTest.DebugOutput($"{owner.Name} looses 7 armour");
                }
            }
        }

        public static SpellEffect Copy(SpellEffect effect)
        {
            SpellEffect newEffect = new SpellEffect();
            newEffect.Name = effect.Name;
            newEffect.Cost = effect.Cost;
            newEffect.Duration = effect.Duration;
            newEffect.CurrentDuration = effect.CurrentDuration;
            return newEffect;
        }

    }

}