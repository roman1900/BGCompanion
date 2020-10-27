using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
                slots.Add(new Card(c));
            else
                throw new NotImplementedException();
            RefreshPositions();
        }
        public void Pop(Card c)
        {
            slots.Remove(c);
            RefreshPositions();
        }
        public void Insert(int index, Card c)
        {
            slots.Insert(index, new Card(c));
            RefreshPositions();
        }
        public bool Full()
        {
            return slots.Count == 7;
        }
        public void RefreshPositions()
        {
            slots.ForEach(m => m.Position = slots.IndexOf(m));
        }
        public byte[] GetHash()
        {
            byte[] hand_hashes = new byte[] { };
            slots.ForEach(delegate (Card c) { hand_hashes = Combine(hand_hashes, c.GetHash()); });
            return SHA256.Create().ComputeHash(hand_hashes);
        }
        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }
        public static string HashedString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "");
        }
        
    }
}
