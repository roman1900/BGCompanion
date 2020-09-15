using System;
using System.Collections.Generic;

namespace BGCompanion
{
    [Flags] public enum Attribute
    {
        none            = 0b00000000,
        taunt           = 0b00000001,
        divineShield    = 0b00000010,
        poisonous       = 0b00000100,
        reborn          = 0b00001000,
        whenEver        = 0b00010000,
        deathRattle     = 0b00100000,
        cleave          = 0b01000000,
        startofcombat   = 0b10000000,
    }
    [Flags] public enum Tribe
    {
        none = 0,
        self        = 0b000000000001,
        random      = 0b000000000010,
        friendly    = 0b000000000100,
        enemy       = 0b000000001000,
        Beast       = 0b000000010000,
        Demon       = 0b000000100000,
        Mech        = 0b000001000000,
        Murloc      = 0b000010000000,
        Neutral     = 0b000100000000,
        Pirate      = 0b001000000000,
        Dragon      = 0b010000000000,
        all         = 0b100000000000,
    }

    public class Effect
    {
        public Attribute What { get; set; }
        public Tribe Who { get; set; }
        public Tribe Target { get; set; }
        public List<Card> Summons { get; set; }
        public int Damage { get; set; }
        public int Attack { get; set; }
        public int Health { get; set; }
        public Attribute Give { get; set; }
    }
    
}