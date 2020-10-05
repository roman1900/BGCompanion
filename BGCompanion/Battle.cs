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
                
                for (int attackorder = attackbounds[0]; attackorder <= attackbounds[1]; attackorder++  )
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
                        
                        Console.WriteLine("Results Win:{0:P2} Tie:{1:P2} Lose:{2:P2}", (float) winCount/totalBattles, (float) tieCount/totalBattles, (float) loseCount/totalBattles);
                        int tb = winCount + tieCount + loseCount;
                        results.Add(new float[] { (float)winCount/tb, (float)tieCount/tb, (float)loseCount/tb });
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
            Console.WriteLine("Win: {0:P2} Tie: {1:P2} Lose: {2:P2}",winPerc, tiePerc, losePerc);

        }
        private static void DoEffect()
        {

        }
        private static void Combat(Hand[] Hands, CombatPosition combatPosition)//int[] attackQ, int combatPosition.Attacker, int target)
        {
            //TODO(#15): Modify Combat system so a call to Combat can jump to a particular stage of the combat ie. Attack, Reborn, Whenever, DeathRattle etc


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
            //Is this battle over?
            if (_Hands[0].slots.Count == 0 || _Hands[1].slots.Count == 0)
            {
                totalBattles++;
                if (_Hands[0].slots.Count == 0)
                {
                    if (_Hands[1].slots.Count == 0)
                    {
                        tieCount++;
                        return;
                    }
                    else
                    {
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
            PrintHand(_Hands, combatPosition);
            switch (combatPosition.Phase)
            {
                case CombatPhase.startofcombat:
                    if (combatPosition.EffectDirectDamage == 0) //We are in the setup phase
                    {
                        if (_Starters[combatPosition.EffectHand].Count > 0)
                        {
                            Card c = _Starters[combatPosition.EffectHand][0];
                            Console.WriteLine("{1} is doing Buff {0}", _Hands[combatPosition.EffectHand].slots.FindIndex(m => m.guid == c.guid),(Who)combatPosition.EffectHand);
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
                    else //We are doing to effect
                    {
                        Console.WriteLine("Start of Combat Effects");
                        //TODO(#19): Need to process potential triggers due to start of combat damage
                        if (_Hands[combatPosition.EffectHand ^ 1].slots[combatPosition.Target].DivineShield)
                        {
                            _Hands[combatPosition.EffectHand ^ 1].slots[combatPosition.Target].DivineShield = false;
                        }
                        else
                        {
                            _Hands[combatPosition.EffectHand ^ 1].slots[combatPosition.Target].Health -= combatPosition.EffectDirectDamage;
                            Console.WriteLine("{0} in position {1} new health is {2}", _Hands[combatPosition.EffectHand ^ 1].slots[combatPosition.Target].Name, combatPosition.Target, _Hands[combatPosition.EffectHand ^ 1].slots[combatPosition.Target].Health);
                        }
                        if (_Hands[combatPosition.EffectHand ^ 1].slots[combatPosition.Target].Health <= 0)
                        {
                            if (combatPosition.AttackQ[combatPosition.EffectHand ^ 1] > combatPosition.Target)
                            {
                                combatPosition.AttackQ[combatPosition.EffectHand ^ 1]--;
                            }
                            _Hands[combatPosition.EffectHand ^ 1].slots.RemoveAt(combatPosition.Target);
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
                                PrintHand(_Hands,combatPosition);
                                Console.WriteLine("{0} is attacking {1}", combatPosition.Attacker, targetposition);
                                combatPosition.Target = targetposition;
                                Combat(_Hands, combatPosition);
                            }
                        }
                    }
                    else
                    {
                        // Do the whenever phase if starting an attack
                        int wheneverMode = 0;
                        if (combatPosition.Phase == CombatPhase.attack)
                        {
                            combatPosition.Phase = CombatPhase.whenever;
                            wheneverMode = 1;
                        }

                        if (combatPosition.Phase == CombatPhase.whenever)
                        {
                            if (wheneverMode == 1) //First time here this attack
                            {
                                List<Card> wheneverAttacks = new List<Card>(_Hands[combatPosition.Attacker].slots.FindAll(m => m.buffs.Exists(b => b.What == Buffs.whenEver && b.Trigger == WheneverTrigger.attacks)));
                                for (int i = 0; i < wheneverAttacks.Count; i++)
                                {
                                    //Combat(_Hands, combatPosition);
                                }
                            }
                            else
                            {

                            }

                        }

                        //do some hitting
                        //TODO(#9): Cleave hits adjacent minions
                        //TODO(#10): Windfury 
                        //TODO(#11): Process Whenever effects
                        //Calculate impact on attacker
                        //Console.WriteLine("{0} in postion {1} attacks {2} in position {3}", _Hands[combatPosition.Attacker].slots[attackQ[combatPosition.Attacker]].Name, attackQ[combatPosition.Attacker], _Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].Name, combatPosition.Target);
                        if (_Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].DivineShield)
                        {
                            _Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].DivineShield = false;

                        }
                        else if (_Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].Poisonous)
                        {
                            _Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].Health = -99;
                        }
                        else
                        {
                            _Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].Health -= _Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].Attack;
                            //Console.WriteLine("{0} in position {1} new health is {2}", _Hands[combatPosition.Attacker].slots[attackQ[combatPosition.Attacker]].Name, attackQ[combatPosition.Attacker], _Hands[combatPosition.Attacker].slots[attackQ[combatPosition.Attacker]].Health);
                        }
                        //Calculate impact on defender
                        if (_Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].DivineShield)
                        {
                            _Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].DivineShield = false;
                        }
                        else if (_Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].Poisonous)
                        {
                            _Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].Health = -99;
                        }
                        else
                        {
                            _Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].Health -= _Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].Attack;
                            //Console.WriteLine("{0} in position {1} new health is {2}", _Hands[combatPosition.Attacker ^ 1].slots[target].Name,target, _Hands[combatPosition.Attacker ^ 1].slots[target].Health);
                        }


                        //TODO(#12): Process deathrattles



                        //TODO(#13): Process reborn
                        if (_Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].Health <= 0)
                        {
                            if (combatPosition.AttackQ[combatPosition.Attacker ^ 1] > combatPosition.Target)
                            {
                                combatPosition.AttackQ[combatPosition.Attacker ^ 1]--;
                            }
                            _Hands[combatPosition.Attacker ^ 1].slots.RemoveAt(combatPosition.Target);
                        }
                        if (_Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].Health <= 0)
                        {
                            //Console.WriteLine("removing {0} at postion {1} from hand", _Hands[combatPosition.Attacker].slots[attackQ[combatPosition.Attacker]].Name, attackQ[combatPosition.Attacker]);
                            _Hands[combatPosition.Attacker].slots.RemoveAt(combatPosition.AttackQ[combatPosition.Attacker]);
                        }
                        else
                        {
                            combatPosition.AttackQ[combatPosition.Attacker] = combatPosition.AttackQ[combatPosition.Attacker] == _Hands[combatPosition.Attacker].slots.Count - 1 ? 0 : combatPosition.AttackQ[combatPosition.Attacker] + 1;
                        }
                        if (_Hands[0].slots.Count == 0 || _Hands[1].slots.Count == 0)
                        {
                            totalBattles++;
                            if (_Hands[0].slots.Count == 0)
                            {
                                if (_Hands[1].slots.Count == 0)
                                {
                                    tieCount++;
                                }
                                else
                                {
                                    loseCount++;
                                }
                            }
                            else
                            {
                                Console.WriteLine("We Won");
                                winCount++;
                            }
                        }
                        else
                        {
                            combatPosition.Attacker = combatPosition.Attacker ^ 1;
                            combatPosition.Target = -1;
                            combatPosition.Phase = CombatPhase.attack;
                            combatPosition.AttackCount++;
                            Combat(_Hands, combatPosition);
                        }
                    }
                    break;
                default:
                    break;
            }



            

        }

        private static void PrintHand(Hand[] Hands, CombatPosition cp)
        {
            int ColWidth = 25;// Hands[0].slots.Max(m => m.Name.Length);
            //ColWidth = (Hands[1].slots.Max(m => m.Name.Length) > ColWidth ? Hands[1].slots.Max(m => m.Name.Length) : ColWidth) + 3;
            Console.WriteLine();
            Console.WriteLine("Stage of Combat: {0} ({1} will attack)", cp.Phase,(Who)cp.Attacker);
            Console.WriteLine();
            Console.WriteLine("Current Board:");
            
            for (int h = 1; h >= 0; h--)
            {
                Console.WriteLine("{0} ---",(Who)h);
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
