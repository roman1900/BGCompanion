using System.Collections.Generic;

namespace BGCompanion
{
    public class Card
    {
        private List<Effect> buffs;
        private int cHealth;
        private int cAttack;
        private int cMana;
        private Tribe cTribe;
        public Card(int health,int attack,int mana,Tribe tribe)
        {
            cHealth = health;
            cAttack = attack;
            cMana = mana;
            cTribe = tribe;
            buffs = new List<Effect>();
        }
        public void AddBuff(Effect buff)
        {
            buffs.Add(buff);
        }
    }
}