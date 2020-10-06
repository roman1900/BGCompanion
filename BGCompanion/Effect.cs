using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace BGCompanion
{
    //TODO(#7): Add Windfury
    [Flags] public enum Attribute
    {
        none            = 0b00000000,
        taunt           = 0b00000001,
        divineShield    = 0b00000010,
        poisonous       = 0b00000100,
        reborn          = 0b00001000,
        
    }
    [Flags] public enum Buffs
    {
        none            = 0b00000000,
        whenEver        = 0b00000001,
        deathRattle     = 0b00000010,
        startOfCombat   = 0b00000100
    }
    [Flags] public enum Tribe
    {
        none = 0,
        self        = 0b0000000000001,
        random      = 0b0000000000010,
        friendly    = 0b0000000000100,
        enemy       = 0b0000000001000,
        Beast       = 0b0000000010000,
        Demon       = 0b0000000100000,
        Mech        = 0b0000001000000,
        Murloc      = 0b0000010000000,
        Neutral     = 0b0000100000000,
        Pirate      = 0b0001000000000,
        Dragon      = 0b0010000000000,
        Elemental   = 0b0100000000000,
        all         = 0b1000000000000,
    }
    [Flags] public enum Race
    {
        Beast = 0b000000010000,
        Demon = 0b000000100000,
        Mech = 0b000001000000,
        Murloc = 0b000010000000,
        Neutral = 0b000100000000,
        Pirate = 0b001000000000,
        Dragon = 0b010000000000,
        Elemental = 0b100000000000,
    }
    [Flags] public enum WheneverTrigger
    {
        none    = 0b000,
        dies    = 0b001,
        attacks = 0b010,
        per     = 0b100,
    }
    public class Effect
    {
       
        public Effect()
        {

        }
        public Effect(Effect m)
        {
            What = m.What;
            Who = m.Who;
            Trigger = m.Trigger;
            Target = m.Target;
            if (m.Summons != null) { m.Summons.ConvertAll<Card>(s => new Card(s)); }
            Damage = m.Damage;
            DamagePer = m.DamagePer;
            Attack = m.Attack;
            Health = m.Health;
            Give = m.Give;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public Buffs What { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Tribe Who { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public WheneverTrigger Trigger { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Tribe Target { get; set; }
        public List<Card> Summons { get; set; }
        public int Damage { get; set; }
        public bool DamagePer { get; set; }
        public int Attack { get; set; }
        public int Health { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Attribute Give { get; set; }
        
    }
    
}
