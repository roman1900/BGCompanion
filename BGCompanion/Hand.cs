using System;
using System.Collections.Generic;
using System.Text;

namespace BGCompanion
{
    public class Hand
    {
        public const int maxHandSize = 7;
        public List<Card> slots { get; set; }
        public Hand()
        {
            slots = new List<Card>(maxHandSize);
        }
        public void Push(Card c)
        {
            if (slots.Count < 7)
                slots.Add(c);
            else
                throw new NotImplementedException();

        }
        public void Pop(Card c)
        {
            slots.Remove(c);
        }
        public bool Full()
        {
            return slots.Count == 7;
        }
    }
}
