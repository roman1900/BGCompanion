using System;
using System.Collections.Generic;
using System.Text;

namespace BGCompanion
{
    class Hand
    {
        const int maxHandSize = 7;
        private List<Card> slots;
        Hand()
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
    }
}
