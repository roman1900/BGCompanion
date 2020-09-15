using System.Collections.Generic;

namespace BGCompanion
{
    public class Card
    {
        public List<Effect> buffs { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Mana { get; set; }
        public string Name { get; set; }
        public Tribe Tribe { get; set; }
        public Card(string name,int attack, int health, int mana,Tribe tribe)
        {
            Name = name;
            Health = health;
            Attack = attack;
            Mana = mana;
            Tribe = tribe;
            buffs = new List<Effect>();
        }
        public void AddBuff(Effect buff)
        {
            buffs.Add(buff);
        }
    }
}