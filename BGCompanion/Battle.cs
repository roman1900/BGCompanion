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
            else if (_mine.slots.Count>_enemy.slots.Count)
            {
                Combat(new Hand[] { _mine, _enemy }, new int[] { 0, 0 }, 0, -1); 
            }
            else if (_enemy.slots.Count>_mine.slots.Count)
            {
                Combat(new Hand[] { _mine, _enemy }, new int[] { 0, 0 }, 1, -1); 
            }
            else
            {
                Combat(new Hand[] { _mine, _enemy }, new int[] { 0, 0 }, 0, -1);
                _mine.slots = new List<Card>(mine.slots);
                _enemy.slots = new List<Card>(enemy.slots);
                Combat(new Hand[] { _mine, _enemy }, new int[] { 0, 0 }, 1, -1);
            }



        }
        private static void Combat(Hand[] Hands,int[] attackQ, int currentAttacker, int target)
        {
            Hand[] _Hands = new Hand[] { new Hand(), new Hand()};
            _Hands[0].slots = Hands[0].slots.ConvertAll<Card>(m => new Card(m));
            _Hands[1].slots = Hands[1].slots.ConvertAll<Card>(m => new Card(m));
            List<Card>[] Taunts = new List<Card>[] { _Hands[0].slots.FindAll(m => m.Taunt), _Hands[1].slots.FindAll(m => m.Taunt) };
            
            if (target == -1)
            {
                //TODO: Start of Combat effects
                //TODO: Zapp Slywick always hits lowest attack (bypass taunt)
                if (Taunts[currentAttacker ^ 1].Count > 0) //the non attacking hands taunts
                {
                    for (int i = 0; i < Taunts[currentAttacker ^ 1].Count; i++)
                    {
                        target = _Hands[currentAttacker ^ 1].slots.FindIndex(m => m == Taunts[currentAttacker ^ 1][i]);
                        Combat(_Hands, attackQ, currentAttacker, target);

                    }
                }
                else
                {
                    for (int i = 0; i < _Hands[currentAttacker ^ 1].slots.Count; i++ )
                    {
                        Combat(_Hands, attackQ, currentAttacker, i);
                    }
                }
            }
            else
            {
                //do some hitting
                //TODO: Cleave hits adjacent minions
                //TODO: Windfury 
                //TODO: Process Whenever effects
                //Calculate impact on attacker
                Console.WriteLine("{0} in postion {1} attacks {2} in position {3}", _Hands[currentAttacker].slots[attackQ[currentAttacker]].Name, attackQ[currentAttacker], _Hands[currentAttacker ^ 1].slots[target].Name, target);
                if (_Hands[currentAttacker].slots[attackQ[currentAttacker]].DivineShield)
                {
                    _Hands[currentAttacker].slots[attackQ[currentAttacker]].DivineShield = false;

                }
                else if (_Hands[currentAttacker ^ 1].slots[target].Poisonous)
                {
                    _Hands[currentAttacker].slots[attackQ[currentAttacker]].Health = -99;
                }
                else
                {
                    _Hands[currentAttacker].slots[attackQ[currentAttacker]].Health -= _Hands[currentAttacker ^ 1].slots[target].Attack;
                    //Console.WriteLine("{0} in position {1} new health is {2}", _Hands[currentAttacker].slots[attackQ[currentAttacker]].Name, attackQ[currentAttacker], _Hands[currentAttacker].slots[attackQ[currentAttacker]].Health);
                }
                //Calculate impact on defender
                if (_Hands[currentAttacker ^ 1].slots[target].DivineShield)
                {
                    _Hands[currentAttacker ^ 1].slots[target].DivineShield = false;
                }
                else if (_Hands[currentAttacker].slots[attackQ[currentAttacker]].Poisonous)
                {
                    _Hands[currentAttacker ^ 1].slots[target].Health = -99;
                }
                else
                {
                    _Hands[currentAttacker ^ 1].slots[target].Health -= _Hands[currentAttacker].slots[attackQ[currentAttacker]].Attack;
                    //Console.WriteLine("{0} in position {1} new health is {2}", _Hands[currentAttacker ^ 1].slots[target].Name,target, _Hands[currentAttacker ^ 1].slots[target].Health);
                }


                //TODO: Process deathrattles
                //TODO: Process reborn
                if (_Hands[currentAttacker ^ 1].slots[target].Health <= 0)
                {
                    if (attackQ[currentAttacker ^ 1] > target)
                    {
                        attackQ[currentAttacker ^ 1]--;
                    }
                    _Hands[currentAttacker ^ 1].slots.RemoveAt(target);
                }
                if (_Hands[currentAttacker].slots[attackQ[currentAttacker]].Health <= 0)
                {
                    //Console.WriteLine("removing {0} at postion {1} from hand", _Hands[currentAttacker].slots[attackQ[currentAttacker]].Name, attackQ[currentAttacker]);
                    _Hands[currentAttacker].slots.RemoveAt(attackQ[currentAttacker]);
                }
                else
                {
                    attackQ[currentAttacker] = attackQ[currentAttacker] == _Hands[currentAttacker].slots.Count - 1 ? 0 : attackQ[currentAttacker] + 1;
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
                        winCount++;
                    }
                }
                else
                {

                    Combat(_Hands, attackQ, currentAttacker ^ 1, -1);
                }
            }
        }
        
        
    }
}
