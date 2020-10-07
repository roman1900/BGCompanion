using Accessibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BGCompanion
{
    public static class Battle
    {
        public static int tieCount;
        public static int winCount;
        public static int loseCount;
        public static int totalBattles;
        public static List<float[]> results = new List<float[]> { };
        public static float winPerc;
        public static float tiePerc;
        public static float losePerc;
        public enum Who
        {
            Player = 0,
            Enemy = 1
        }
        public enum CombatPhase
        {
            attack,
            whenever,
            reborn,
            deathrattle,
            startofcombat,
        }
        public struct CombatPosition
        {
            public int[] AttackQ;
            public int Attacker;
            public int Target;
            public int AttackCount;
            public int EffectDirectDamage;
            public int[] EffectQ;
            public int EffectHand;
            public CombatPhase Phase;
        }
        public static void Simulate(Hand mine, Hand enemy)
        {
            //Rules of Engagement
            //1. Person with the largest had goes first otherwise random
            //2. Minions attack in order from left to right 
            //3. If a minion summons multiple minions upon death the attack position is set to the first position of the summon minion
            // eg.   a b c d (b dies) summons 3 e board now looks like a e e e c d attack position remains 2 and doesn't increment to 3

            //1.

            Hand _mine = new Hand();
            _mine.slots = new List<Card>(mine.slots);
            Hand _enemy = new Hand();
            _enemy.slots = new List<Card>(enemy.slots);
            tieCount = winCount = loseCount = totalBattles = 0;
            results.Clear();
            if (_mine.slots.Count == 0)
            {
                if (_enemy.slots.Count == 0)
                {
                    tieCount++;
                }
                else
                {
                    loseCount++;
                }
            }
            else if (_enemy.slots.Count == 0)
            {
                winCount++;
            }
            else
            {
                bool startmine;
                bool startenemy;
                startmine = _mine.slots.FindAll(m => m.buffs.Exists(b => b.What == Buffs.startOfCombat)).Count > 0;
                startenemy = _enemy.slots.FindAll(m => m.buffs.Exists(b => b.What == Buffs.startOfCombat)).Count > 0;
                CombatPosition cp = new CombatPosition();
                cp.Target = -1;
                cp.AttackCount = 0;
                cp.AttackQ = new int[] { 0, 0 };
                cp.EffectQ = new int[] { 0, 0 };
                Console.WriteLine("The Player's hand has {0} cards. Enemy Hand has {1} cards.", _mine.slots.Count, _enemy.slots.Count);
                cp.Attacker = (_mine.slots.Count > _enemy.slots.Count) ? 0 : (_enemy.slots.Count > _mine.slots.Count) ? 1 : -1;
                cp.Phase = (startmine || startenemy) ? CombatPhase.startofcombat : CombatPhase.attack;
                bool alternateSOC = startenemy && startmine;
                int[] attackbounds = new int[] { cp.Attacker == -1 ? 0 : cp.Attacker, cp.Attacker == -1 ? 1 : cp.Attacker };

                for (int attackorder = attackbounds[0]; attackorder <= attackbounds[1]; attackorder++)
                {
                    Console.WriteLine("[BATTLE COMMENCEMENT] {0} is attacking.", (Who)attackorder);
                    cp.Attacker = attackorder;
                    for (int startofcombatorder = alternateSOC ? 0 : startenemy ? 1 : 0; startofcombatorder <= (startenemy ? 1 : 0); startofcombatorder++)
                    {
                        cp.EffectHand = startofcombatorder;

                        if (cp.Phase == CombatPhase.startofcombat) Console.WriteLine("[BATTLE START OF COMBAT] Start of Combat triggering for {0}", (Who)startofcombatorder);
                        _mine.slots = new List<Card>(mine.slots);
                        _enemy.slots = new List<Card>(enemy.slots);
                        Combat(new Hand[] { _mine, _enemy }, cp);

                        Console.WriteLine("Results Win:{0:P2} Tie:{1:P2} Lose:{2:P2}", (float)winCount / totalBattles, (float)tieCount / totalBattles, (float)loseCount / totalBattles);
                        int tb = winCount + tieCount + loseCount;
                        results.Add(new float[] { (float)winCount / tb, (float)tieCount / tb, (float)loseCount / tb });
                        winCount = 0;
                        tieCount = 0;
                        loseCount = 0;

                    }
                }
            }
            if (results.Count > 0)
            {
                winPerc = results.Average(x => x[0]);
                tiePerc = results.Average(x => x[1]);
                losePerc = results.Average(x => x[2]);
            }
            Console.WriteLine("Win: {0:P2} Tie: {1:P2} Lose: {2:P2}", winPerc, tiePerc, losePerc);

        }
        private static void DoEffect()
        {

        }
        private static void Combat(Hand[] Hands, CombatPosition combatPosition)//int[] attackQ, int combatPosition.Attacker, int target)
        {


            //First step copy the classes passed via parameter so we do not modify the originals 
            Hand[] _Hands = new Hand[] { new Hand(), new Hand() };
            _Hands[0].slots = Hands[0].slots.ConvertAll<Card>(m => new Card(m));
            _Hands[1].slots = Hands[1].slots.ConvertAll<Card>(m => new Card(m));

            List<Card>[] _Taunts = { new List<Card>(), new List<Card>() };
            _Taunts[0] = _Hands[0].slots.FindAll(m => m.Taunt);
            _Taunts[1] = _Hands[1].slots.FindAll(m => m.Taunt);
            List<Card>[] _Starters = { new List<Card>(), new List<Card>() };
            _Starters[0] = _Hands[0].slots.FindAll(m => m.buffs.Exists(b => b.What == Buffs.startOfCombat));
            _Starters[1] = _Hands[1].slots.FindAll(m => m.buffs.Exists(b => b.What == Buffs.startOfCombat));
            //We should also check at this point whether the following conditions exist:
            //A single unit defeats all of the opponents minions (Implemented in SoloMinion)
            PrintHand(_Hands, combatPosition);
            //Is this battle over?
            if (_Hands[0].slots.Count == 0 || _Hands[1].slots.Count == 0)
            {
                totalBattles++;
                if (_Hands[0].slots.Count == 0)
                {
                    if (_Hands[1].slots.Count == 0)
                    {
                        Console.WriteLine("We Tied");
                        tieCount++;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("We Lost");
                        loseCount++;
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("We Won");
                    winCount++;
                    return;
                }
            }
            else if (!(_Hands[0].slots.Exists(m => m.Poisonous || m.Reborn || m.DivineShield))
                && !(_Hands[1].slots.Exists(m => m.Poisonous || m.Reborn || m.DivineShield)))
            {
                if (SoloMinion(_Hands[0],_Hands[1]))
                {
                    Console.WriteLine("We Won");
                    totalBattles++;
                    winCount++;
                    return;
                }
                if (SoloMinion(_Hands[1],_Hands[0]))
                {
                    Console.WriteLine("We Lost");
                    totalBattles++;
                    loseCount++;
                    return;
                }
                
            }
            
            switch (combatPosition.Phase)
            {
                case CombatPhase.startofcombat:
                    if (combatPosition.EffectDirectDamage == 0) //We are in the setup phase
                    {
                        if (_Starters[combatPosition.EffectHand].Count > 0)
                        {
                            Card c = _Starters[combatPosition.EffectHand][0];
                            Console.WriteLine("{1} is doing Buff {0}", _Hands[combatPosition.EffectHand].slots.FindIndex(m => m.guid == c.guid), (Who)combatPosition.EffectHand);
                            Effect e = c.buffs.Find(m => m.What == Buffs.startOfCombat);
                            if (e.DamagePer)
                            {
                                if (e.Who.HasFlag(Tribe.friendly))
                                {
                                    int count = _Hands[combatPosition.EffectHand].slots.FindAll(m => (Tribe)m.Tribe == (e.Who ^ Tribe.friendly)).Count;
                                    combatPosition.EffectDirectDamage = count * e.Damage;
                                    if (e.Target.HasFlag(Tribe.random) && e.Target.HasFlag(Tribe.enemy))
                                    {
                                        for (int d = 0; d < _Hands[combatPosition.EffectHand ^ 1].slots.Count; d++)
                                        {
                                            c.buffs.Remove(e);
                                            combatPosition.Target = d;
                                            Console.WriteLine("Dealing damage to {0}", d);
                                            Combat(_Hands, combatPosition);
                                        }
                                        //return;
                                    }
                                }
                            }
                        }


                        else
                        {
                            combatPosition.Phase = CombatPhase.attack;
                            combatPosition.Target = -1;
                            combatPosition.EffectDirectDamage = 0;
                            Combat(_Hands, combatPosition);
                        }
                    }
                    else //We are doing the effect
                    {
                        Console.WriteLine("Start of Combat Effects");
                        //TODO(#19): Need to process potential triggers due to start of combat damage
                        UpdateHealth(_Hands[combatPosition.EffectHand ^ 1].slots[combatPosition.Target], combatPosition.EffectDirectDamage);
                        Console.WriteLine("{0} in position {1} new health is {2}", _Hands[combatPosition.EffectHand ^ 1].slots[combatPosition.Target].Name, combatPosition.Target, _Hands[combatPosition.EffectHand ^ 1].slots[combatPosition.Target].Health);
                        
                        if (_Hands[combatPosition.EffectHand ^ 1].slots[combatPosition.Target].Health <= 0)
                        {
                            if (combatPosition.AttackQ[combatPosition.EffectHand ^ 1] > combatPosition.Target)
                            {
                                combatPosition.AttackQ[combatPosition.EffectHand ^ 1]--;
                            }
                            //Reborn check 
                            //TODO(#24): Deathrattles trigger first so there may not be any room for the reborn minion

                            ProcessReborn(_Hands[combatPosition.EffectHand ^ 1], combatPosition.Target);
                        }
                        //combatPosition.Phase = CombatPhase.attack;
                        //combatPosition.Target = -1;
                        combatPosition.EffectDirectDamage = 0;
                        combatPosition.AttackCount++;
                        //combatPosition.EffectHand = _Starters[combatPosition.EffectHand ^ 1].Count > 0 ? combatPosition.EffectHand ^ 1 : combatPosition.EffectHand;
                        //One Hand performs all start of combat effects before the other
                        combatPosition.EffectHand = _Starters[combatPosition.EffectHand].Count > 0 ? combatPosition.EffectHand : combatPosition.EffectHand ^ 1;
                        Combat(_Hands, combatPosition);
                    }
                    break;
                case CombatPhase.attack:
                    if (combatPosition.Target == -1)
                    {
                        //TODO(#8): Zapp Slywick always hits lowest attack (bypass taunt)
                        if (_Taunts[combatPosition.Attacker ^ 1].Count > 0) //the non attacking hands taunts
                        {
                            for (int i = 0; i < _Taunts[combatPosition.Attacker ^ 1].Count; i++)
                            {
                                combatPosition.Target = _Hands[combatPosition.Attacker ^ 1].slots.FindIndex(m => m == _Taunts[combatPosition.Attacker ^ 1][i]);
                                Combat(_Hands, combatPosition);
                            }
                        }
                        else
                        {
                            for (int targetposition = 0; targetposition < _Hands[combatPosition.Attacker ^ 1].slots.Count; targetposition++)
                            {
                                //PrintHand(_Hands,combatPosition);
                                Console.WriteLine("{0}'s  {1} in position {2} is attacking position {3}", (Who)combatPosition.Attacker, _Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].Name, combatPosition.AttackQ[combatPosition.Attacker], targetposition);
                                combatPosition.Target = targetposition;
                                Combat(_Hands, combatPosition);
                            }
                        }
                    }
                    else
                    {
                        // Do the whenever phase if starting an attack


                        List<Card> wheneverAttacks = new List<Card>(_Hands[combatPosition.Attacker].slots.FindAll(m => m.buffs.Exists(b => b.What == Buffs.whenEver && b.Trigger == WheneverTrigger.attacks)));
                        for (int i = 0; i < wheneverAttacks.Count; i++)
                        {
                            //self buff attack
                            if (wheneverAttacks[i].buffs.Exists(m => m.Who == Tribe.self) && wheneverAttacks[i].guid == _Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].guid)
                            {
                                _Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].Attack += wheneverAttacks[i].buffs.Find(m => m.Who == Tribe.self).Attack;
                            }
                        }


                        //do some hitting
                        //TODO(#9): Cleave hits adjacent minions
                        //TODO(#10): Windfury e

                        //Calculate impact on attacker
                        UpdateHealth(_Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]], _Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target]);
                        //Calculate impact on defender
                        UpdateHealth(_Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target], _Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]]);
                        


                        //TODO(#12): Process deathrattles



                        if (_Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].Health <= 0)
                        {

                            _Hands[combatPosition.Attacker ^ 1].slots.FindAll(m => m.HasWhenever(WheneverTrigger.dies, Tribe.friendly))
                                .ForEach(d => ProcessWheneverDies(d, d.buffs.Find(b => b.What == Buffs.whenEver && b.Who.HasFlag(Tribe.friendly) && b.Trigger == WheneverTrigger.dies), _Hands[combatPosition.Attacker ^ 1], combatPosition.Target));
                            

                            ProcessReborn(_Hands[combatPosition.Attacker ^ 1], combatPosition.Target);
                            if (combatPosition.AttackQ[combatPosition.Attacker ^ 1] > combatPosition.Target)
                            {
                                combatPosition.AttackQ[combatPosition.Attacker ^ 1]--;
                            }
                            if ((combatPosition.AttackQ[combatPosition.Attacker ^ 1] > _Hands[combatPosition.Attacker ^ 1].slots.Count - 1))
                            {
                                combatPosition.AttackQ[combatPosition.Attacker ^ 1] = 0;
                            }
                            
                        }
                        if (_Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].Health <= 0)
                        {
                            _Hands[combatPosition.Attacker].slots.FindAll(m => m.buffs.Exists(b => b.What == Buffs.whenEver && b.Who.HasFlag(Tribe.friendly) && b.Trigger == WheneverTrigger.dies))
                                .ForEach(d => ProcessWheneverDies(d, d.buffs.Find(b => b.What == Buffs.whenEver && b.Who.HasFlag(Tribe.friendly) && b.Trigger == WheneverTrigger.dies), _Hands[combatPosition.Attacker], combatPosition.AttackQ[combatPosition.Attacker]));

                            ProcessReborn(_Hands[combatPosition.Attacker], combatPosition.AttackQ[combatPosition.Attacker]);
                            if (combatPosition.AttackQ[combatPosition.Attacker] >= _Hands[combatPosition.Attacker].slots.Count)
                            {
                                combatPosition.AttackQ[combatPosition.Attacker] = 0;
                            }
                        }
                        else
                        {
                            combatPosition.AttackQ[combatPosition.Attacker] = combatPosition.AttackQ[combatPosition.Attacker] == _Hands[combatPosition.Attacker].slots.Count - 1 ? 0 : combatPosition.AttackQ[combatPosition.Attacker] + 1;
                        }

                        combatPosition.Attacker ^= 1;
                        combatPosition.Target = -1;
                        combatPosition.Phase = CombatPhase.attack;
                        combatPosition.AttackCount++;
                        Combat(_Hands, combatPosition);

                    }
                    break;
                default:
                    break;
            }





        }
        private static bool SoloMinion(Hand Checking, Hand Against)
        {
            bool yep = false;
            if (!Against.slots.Exists(m => m.buffs.Count > 0))
            {
                //The opponent has no buffs left check for card which solo's
                
                Checking.slots.ForEach(delegate (Card c)
                {
                    if (c.Health > Against.slots.Sum(m => m.Attack) && c.Attack >= Against.slots.Max(m => m.Health))
                    {
                        yep = true;
                    }
                });
                
            }
            return yep;
        }
        private static void ProcessDeathRattle(Card deadCard, Effect e, Hand hand, int target)
        {

        }

        private static void UpdateHealth(Card toUpdate, Card damager)
        {
            if (toUpdate.DivineShield)
            {
                toUpdate.DivineShield = false;

            }
            else if (damager.Poisonous)
            {
                toUpdate.Health = -99;
            }
            else
            {
                toUpdate.Health -= damager.Attack;
            }
        }
        
        private static void UpdateHealth(Card toUpdate, int directDamage)
        {
            if (toUpdate.DivineShield)
            {
                toUpdate.DivineShield = false;

            }
            else
            {
                toUpdate.Health -= directDamage;
            }
        }

        private static void ProcessWheneverDies(Card buffCard, Effect e, Hand hand, int target)
        {
            //Whenever friendly x dies buff self
            if (((Tribe)hand.slots[target].Tribe & e.Who) > 0)
            {
                if (e.Target.HasFlag(Tribe.self))
                {
                    buffCard.Attack += e.Attack;
                    buffCard.Health += e.Health;
                }
            }
            
        }

        private static void ProcessReborn(Hand hand, int target)
        {
            if (hand.slots[target].Reborn)
            {
                var cardName = hand.slots[target].Name;
                hand.slots.RemoveAt(target);
                hand.slots.Insert(target, new Card(Deck.Cards.Find(m => m.Name == cardName)));
                hand.slots[target].Reborn = false;
                hand.slots[target].Health = 1;
            }
            else
            {
                hand.slots.RemoveAt(target);
            }
        }

        private static void PrintHand(Hand[] Hands, CombatPosition cp)
        {
            int ColWidth = 25;// Hands[0].slots.Max(m => m.Name.Length);
            //ColWidth = (Hands[1].slots.Max(m => m.Name.Length) > ColWidth ? Hands[1].slots.Max(m => m.Name.Length) : ColWidth) + 3;
            Console.WriteLine();
            Console.WriteLine("Stage of Combat: {0} ({1} will attack)", cp.Phase, (Who)cp.Attacker);
            Console.WriteLine();
            Console.WriteLine("Current Board:");

            for (int h = 1; h >= 0; h--)
            {
                Console.WriteLine("{0} ---", (Who)h);
                StringBuilder stats = new StringBuilder();
                StringBuilder names = new StringBuilder();
                for (int i = 0; i < Hands[h].slots.Count; i++)
                {
                    stats.AppendFormat("{0}{1}{2}\t", Hands[h].slots[i].Attack, Hands[h].slots[i].Tribe.ToString().CentreString(ColWidth - Hands[h].slots[i].Attack.ToString().Length - Hands[h].slots[i].Health.ToString().Length), Hands[h].slots[i].Health);
                    names.AppendFormat("{0}\t", Hands[h].slots[i].Name.CentreString(ColWidth));
                }
                Console.WriteLine(names.ToString());
                Console.WriteLine(stats.ToString());

                Console.WriteLine();
            }
        }
    }
}
