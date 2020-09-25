using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace BGCompanion
{
    public class Card
    {
        public string Name { get; set; }
        public int Tier { get; set; }
        public int Attack { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public List<Effect> buffs { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Race Tribe { get; set; }
        public bool Taunt { get; set; }
        public bool DivineShield { get; set; }
        public bool Reborn { get; set; }
        public bool Cleave { get; set; }
        public bool Poisonous { get; set; }
        public Guid guid { get; set; }
        [JsonConstructor]
        public Card(string name,int attack, int health, int mana, int tier, bool taunt,bool divineshield,bool reborn,bool cleave, bool poisonous, Race tribe)
        {
            Name = name;
            Health = health;
            Attack = attack;
            Mana = mana;
            Tier = tier;
            Tribe = tribe;
            Taunt = taunt;
            DivineShield = divineshield;
            Reborn = reborn;
            Cleave = cleave;
            Poisonous = poisonous;
            buffs = new List<Effect>();
        }
        public Card(Card c)
        {
            Name = c.Name;
            Tier = c.Tier;
            Attack = c.Attack;
            Health = c.Health;
            Mana = c.Mana;
            buffs = c.buffs;
            Tribe = c.Tribe;
            Taunt = c.Taunt;
            DivineShield = c.DivineShield;
            Reborn = c.Reborn;
            Cleave = c.Cleave;
            Poisonous = c.Poisonous;
            guid = Guid.NewGuid();
        }

        
        public void AddBuff(Effect buff)
        {
            buffs.Add(buff);
        }
    }
}