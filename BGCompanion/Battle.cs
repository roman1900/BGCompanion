using System;
using System.Collections.Generic;
using System.Text;

namespace BGCompanion
{
    public static class Battle
    {
        public static int tieCount;
        public static int winCount;
        public static int loseCount;
        public static int totalBattles;
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
                if (_mine.slots.Count > _enemy.slots.Count)
                {
                    cp.Attacker = 0;
                }
                else if (_enemy.slots.Count > _mine.slots.Count)
                {
                    cp.Attacker = 1;
                }
                else
                {
                    cp.Attacker = -1;
                }


                cp.Phase = (startmine || startenemy) ? CombatPhase.startofcombat : CombatPhase.attack;

                if (cp.Phase.HasFlag(CombatPhase.startofcombat))
                {
                    if (startmine)
                    {
                        if (startenemy) //Both players have start of combat effects
                        {
                            cp.EffectHand = 0;
                            if (cp.Attacker == -1)
                            {
                                cp.Attacker = 0;
                                Combat(new Hand[] { _mine, _enemy }, cp);
                                cp.Attacker = 1;
                                _mine.slots = new List<Card>(mine.slots);
                                _enemy.slots = new List<Card>(enemy.slots);
                                Combat(new Hand[] { _mine, _enemy }, cp);
                            }
                            else
                            {
                                Combat(new Hand[] { _mine, _enemy }, cp);
                            }
                            cp.EffectHand = 1;
                            if (cp.Attacker == -1)
                            {
                                cp.Attacker = 0;
                                Combat(new Hand[] { _mine, _enemy }, cp);
                                cp.Attacker = 1;
                                _mine.slots = new List<Card>(mine.slots);
                                _enemy.slots = new List<Card>(enemy.slots);
                                Combat(new Hand[] { _mine, _enemy }, cp);
                            }
                            else
                            {
                                Combat(new Hand[] { _mine, _enemy }, cp);
                            }
                        }
                        else
                        {
                            cp.EffectHand = 0;
                            if (cp.Attacker == -1)
                            {
                                cp.Attacker = 0;
                                Combat(new Hand[] { _mine, _enemy }, cp);
                                cp.Attacker = 1;
                                _mine.slots = new List<Card>(mine.slots);
                                _enemy.slots = new List<Card>(enemy.slots);
                                Combat(new Hand[] { _mine, _enemy }, cp);
                            }
                            else
                            {
                                Combat(new Hand[] { _mine, _enemy }, cp);
                            }
                        }
                    }
                    else if (startenemy)
                    {
                        cp.EffectHand = 0;
                        if (cp.Attacker == -1)
                        {
                            cp.Attacker = 0;
                            Combat(new Hand[] { _mine, _enemy }, cp);
                            cp.Attacker = 1;
                            _mine.slots = new List<Card>(mine.slots);
                            _enemy.slots = new List<Card>(enemy.slots);
                            Combat(new Hand[] { _mine, _enemy }, cp);
                        }
                        else
                        {
                            Combat(new Hand[] { _mine, _enemy }, cp);
                        }
                    }

                }
                else
                {
                    if (cp.Attacker == -1)
                    {
                        cp.Attacker = 0;
                        Combat(new Hand[] { _mine, _enemy }, cp);
                        cp.Attacker = 1;
                        _mine.slots = new List<Card>(mine.slots);
                        _enemy.slots = new List<Card>(enemy.slots);
                        Combat(new Hand[] { _mine, _enemy }, cp);
                    }
                    else
                    {
                        Combat(new Hand[] { _mine, _enemy }, cp);
                    }
                }

            }


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
            switch (combatPosition.Phase)
            {
                case CombatPhase.startofcombat:
                    if (combatPosition.EffectDirectDamage == 0) //We are in the setup phase
                    {
                        if (_Starters[combatPosition.EffectHand].Count > 0)
                        {
                            Card c = _Starters[combatPosition.EffectHand][0];
                            Console.WriteLine("Doing Buff {0}", _Hands[combatPosition.EffectHand].slots.FindIndex(m => m.guid == c.guid));
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



            //if (combatPosition.Phase == CombatPhase.attack && combatPosition.Target == -1)
            //{

            //    //TODO(#8): Zapp Slywick always hits lowest attack (bypass taunt)
            //    if (_Taunts[combatPosition.Attacker ^ 1].Count > 0) //the non attacking hands taunts
            //    {
            //        for (int i = 0; i < _Taunts[combatPosition.Attacker ^ 1].Count; i++)
            //        {
            //            combatPosition.Target = _Hands[combatPosition.Attacker ^ 1].slots.FindIndex(m => m == _Taunts[combatPosition.Attacker ^ 1][i]);
            //            Combat(_Hands, combatPosition);
            //        }
            //    }
            //    else
            //    {
            //        for (int targetposition = 0; targetposition < _Hands[combatPosition.Attacker ^ 1].slots.Count; targetposition++)
            //        {
            //            Console.WriteLine("{0} is attacking {1}", combatPosition.Attacker, targetposition);
            //            combatPosition.Target = targetposition;
            //            Combat(_Hands, combatPosition);
            //        }
            //    }
            //}
            //else
            //{
            //    // Do the whenever phase if starting an attack
            //    int wheneverMode = 0;
            //    if (combatPosition.Phase == CombatPhase.attack)
            //    {
            //        combatPosition.Phase = CombatPhase.whenever;
            //        wheneverMode = 1;
            //    }

            //    if (combatPosition.Phase == CombatPhase.whenever)
            //    {
            //        if (wheneverMode == 1) //First time here this attack
            //        {
            //            List<Card> wheneverAttacks = new List<Card>(_Hands[combatPosition.Attacker].slots.FindAll(m => m.buffs.Exists(b => b.What == Buffs.whenEver && b.Trigger == WheneverTrigger.attacks)));
            //            for (int i = 0; i < wheneverAttacks.Count; i++)
            //            {
            //                //Combat(_Hands, combatPosition);
            //            }
            //        }
            //        else
            //        {

            //        }

            //    }

            //    //do some hitting
            //    //TODO(#9): Cleave hits adjacent minions
            //    //TODO(#10): Windfury 
            //    //TODO(#11): Process Whenever effects
            //    //Calculate impact on attacker
            //    //Console.WriteLine("{0} in postion {1} attacks {2} in position {3}", _Hands[combatPosition.Attacker].slots[attackQ[combatPosition.Attacker]].Name, attackQ[combatPosition.Attacker], _Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].Name, combatPosition.Target);
            //    if (_Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].DivineShield)
            //    {
            //        _Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].DivineShield = false;

            //    }
            //    else if (_Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].Poisonous)
            //    {
            //        _Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].Health = -99;
            //    }
            //    else
            //    {
            //        _Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].Health -= _Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].Attack;
            //        //Console.WriteLine("{0} in position {1} new health is {2}", _Hands[combatPosition.Attacker].slots[attackQ[combatPosition.Attacker]].Name, attackQ[combatPosition.Attacker], _Hands[combatPosition.Attacker].slots[attackQ[combatPosition.Attacker]].Health);
            //    }
            //    //Calculate impact on defender
            //    if (_Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].DivineShield)
            //    {
            //        _Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].DivineShield = false;
            //    }
            //    else if (_Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].Poisonous)
            //    {
            //        _Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].Health = -99;
            //    }
            //    else
            //    {
            //        _Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].Health -= _Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].Attack;
            //        //Console.WriteLine("{0} in position {1} new health is {2}", _Hands[combatPosition.Attacker ^ 1].slots[target].Name,target, _Hands[combatPosition.Attacker ^ 1].slots[target].Health);
            //    }


            //    //TODO(#12): Process deathrattles



            //    //TODO(#13): Process reborn
            //    if (_Hands[combatPosition.Attacker ^ 1].slots[combatPosition.Target].Health <= 0)
            //    {
            //        if (combatPosition.AttackQ[combatPosition.Attacker ^ 1] > combatPosition.Target)
            //        {
            //            combatPosition.AttackQ[combatPosition.Attacker ^ 1]--;
            //        }
            //        _Hands[combatPosition.Attacker ^ 1].slots.RemoveAt(combatPosition.Target);
            //    }
            //    if (_Hands[combatPosition.Attacker].slots[combatPosition.AttackQ[combatPosition.Attacker]].Health <= 0)
            //    {
            //        //Console.WriteLine("removing {0} at postion {1} from hand", _Hands[combatPosition.Attacker].slots[attackQ[combatPosition.Attacker]].Name, attackQ[combatPosition.Attacker]);
            //        _Hands[combatPosition.Attacker].slots.RemoveAt(combatPosition.AttackQ[combatPosition.Attacker]);
            //    }
            //    else
            //    {
            //        combatPosition.AttackQ[combatPosition.Attacker] = combatPosition.AttackQ[combatPosition.Attacker] == _Hands[combatPosition.Attacker].slots.Count - 1 ? 0 : combatPosition.AttackQ[combatPosition.Attacker] + 1;
            //    }
            //    if (_Hands[0].slots.Count == 0 || _Hands[1].slots.Count == 0)
            //    {
            //        totalBattles++;
            //        if (_Hands[0].slots.Count == 0)
            //        {
            //            if (_Hands[1].slots.Count == 0)
            //            {
            //                tieCount++;
            //            }
            //            else
            //            {
            //                loseCount++;
            //            }
            //        }
            //        else
            //        {
            //            Console.WriteLine("We Won");
            //            winCount++;
            //        }
            //    }
            //    else
            //    {
            //        combatPosition.Attacker = combatPosition.Attacker ^ 1;
            //        combatPosition.Target = -1;
            //        combatPosition.Phase = CombatPhase.attack;
            //        combatPosition.AttackCount++;
            //        Combat(_Hands, combatPosition);
            //    }
            //}

        }


    }
}
