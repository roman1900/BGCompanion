using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
        public Card(string name,int attack, int health, int mana, int tier, Race tribe)
        {
            Name = name;
            Health = health;
            Attack = attack;
            Mana = mana;
            Tier = tier;
            Tribe = tribe;
            buffs = new List<Effect>();
        }
        public void AddBuff(Effect buff)
        {
            buffs.Add(buff);
        }
    }
}